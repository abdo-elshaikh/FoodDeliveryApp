using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.ViewModels.Order;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.Json;

namespace FoodDeliveryApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderService> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailSender _emailSender;
        private readonly IPaymentService _paymentService;
        private readonly ICartService _cartService;

        public OrderService(
            ApplicationDbContext context,
            ILogger<OrderService> logger,
            ICurrentUserService currentUserService,
            IEmailSender emailSender,
            IPaymentService paymentService,
            ICartService cartService)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
            _emailSender = emailSender;
            _paymentService = paymentService;
            _cartService = cartService;
        }

        public async Task<PaginatedList<Order>> SearchOrdersAsync(
            string searchTerm = null,
            string statusFilter = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int page = 1,
            int pageSize = 10,
            string sortBy = "OrderDate",
            string sortOrder = "desc")
        {
            try
            {
                var user = _currentUserService.GetCurrentUser();
                var query = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(i => i.MenuItem)
                    .Include(o => o.OrderItems)
                        .ThenInclude(i => i.Restaurant)
                    .Include(o => o.DeliveryAddress)
                    .Include(o => o.Driver)
                        .ThenInclude(d => d.User)
                    .Include(o => o.TrackingHistory)
                    .AsQueryable();

                // Apply authorization filters
                if (user.Role != UserType.Admin && user.Role != UserType.Driver)
                {
                    query = query.Where(o => o.UserId == user.Id);
                }
                else if (user.Role == UserType.Owner)
                {
                    var restaurantId = await _context.Restaurants.Where(r => r.OwnerId == user.Id).Select(r => r.Id).FirstOrDefaultAsync();
                    query = query.Where(o => o.OrderItems.Any(i => i.RestaurantId == restaurantId));
                }

                // Apply search filters
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(o => 
                        o.OrderNumber.Contains(searchTerm) ||
                        o.User.FullName.Contains(searchTerm) ||
                        o.User.Email.Contains(searchTerm));
                }

                if (!string.IsNullOrEmpty(statusFilter))
                {
                    if (Enum.TryParse<OrderStatus>(statusFilter, out var status))
                    {
                        query = query.Where(o => o.Status == status);
                    }
                }

                if (startDate.HasValue)
                {
                    query = query.Where(o => o.OrderDate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(o => o.OrderDate <= endDate.Value);
                }

                // Apply sorting
                query = sortBy.ToLower() switch
                {
                    "orderdate" => sortOrder.ToLower() == "asc" 
                        ? query.OrderBy(o => o.OrderDate)
                        : query.OrderByDescending(o => o.OrderDate),
                    "status" => sortOrder.ToLower() == "asc"
                        ? query.OrderBy(o => o.Status)
                        : query.OrderByDescending(o => o.Status),
                    "total" => sortOrder.ToLower() == "asc"
                        ? query.OrderBy(o => o.Total)
                        : query.OrderByDescending(o => o.Total),
                    _ => query.OrderByDescending(o => o.OrderDate)
                };

                // Apply pagination
                query = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                return await PaginatedList<Order>.CreateAsync(query, page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching orders");
                throw new InvalidOperationException("Error searching orders", ex);
            }
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(i => i.MenuItem)
                    .Include(o => o.OrderItems)
                        .ThenInclude(i => i.Restaurant)
                    .Include(o => o.DeliveryAddress)
                    .Include(o => o.Driver)
                        .ThenInclude(d => d.User)
                    .Include(o => o.TrackingHistory)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {id} not found");
                }

                // Check authorization
                var user = _currentUserService.GetCurrentUser();
                if (user == null || user.Id != order.UserId)
                {
                    throw new UnauthorizedAccessException("Unauthorized to view this order");
                }

                return order;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException && ex is not UnauthorizedAccessException)
            {
                _logger.LogError(ex, "Error retrieving order {OrderId}", id);
                throw new InvalidOperationException($"Error retrieving order {id}", ex);
            }
        }

        public async Task<Order> CreateOrderAsync(OrderCreateViewModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                if (!model.IsValid)
                {
                    throw new InvalidOperationException("Invalid order data");
                }

                var user = _currentUserService.GetCurrentUser();
                var deliveryAddress = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.Id == model.DeliveryAddressId && a.UserId == user.Id);

                if (deliveryAddress == null)
                {
                    throw new InvalidOperationException("Invalid delivery address");
                }

                var order = new Order
                {
                    UserId = user.Id,
                    OrderNumber = GenerateOrderNumber(),
                    Status = OrderStatus.Pending,
                    OrderDate = DateTime.UtcNow,
                    DeliveryAddressId = model.DeliveryAddressId,
                    Subtotal = model.Subtotal,
                    Tax = model.Tax,
                    DeliveryFee = model.DeliveryFee,
                    Discount = model.Discount,
                    Total = model.Total,
                    PaymentMethod = model.PaymentMethod,
                    PaymentStatus = PaymentStatus.Pending,
                    Notes = model.DeliveryAddresses.FirstOrDefault()?.Notes ?? string.Empty,
                    CustomFields = new Dictionary<string, string>
                    {
                        { "PromotionCode", model.PromoCode ?? string.Empty }
                    }
                };

                // Create order items
                order.OrderItems = model.Items.Select(item => new OrderItem
                {
                    MenuItemId = item.MenuItemId,
                    RestaurantId = item.RestaurantId,
                    Quantity = item.Quantity,
                    Price = item.UnitPrice,
                    SpecialInstructions = item.SpecialInstructions,
                    Modifiers = JsonSerializer.Serialize(item.Modifiers),
                    Status = OrderItemStatus.Pending
                }).ToList();

                // Add initial tracking entry
                order.TrackingHistory.Add(new OrderTracking
                {
                    OrderId = order.Id,
                    Status = OrderStatus.Pending,
                    Timestamp = DateTime.UtcNow,
                    Notes = "Order created"
                });

                _context.Orders.Add(order);

                // Process payment
                var paymentResult = await _paymentService.ProcessPaymentAsync(order);
                if (!paymentResult.Success)
                {
                    throw new InvalidOperationException(paymentResult.ErrorMessage);
                }

                order.PaymentStatus = PaymentStatus.Pending;

                // Notify restaurants
                foreach (var restaurantGroup in order.OrderItems.GroupBy(i => i.RestaurantId))
                {
                    var restaurant = await _context.Restaurants
                        .Include(r => r.Owner)
                        .FirstOrDefaultAsync(r => r.Id == restaurantGroup.Key);

                    if (restaurant != null)
                    {
                        await _emailSender.SendOrderConfirmationAsync(
                            restaurant.Owner.Email,
                            order.OrderNumber,
                            $"You have received a new order with {restaurantGroup.Count()} items");
                    }
                }

                // Notify customer
                await _emailSender.SendOrderConfirmationAsync(
                    user.Email,
                    order.OrderNumber,
                    "Your order has been placed and is being processed");

                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                throw new InvalidOperationException("Error creating order", ex);
            }
        }

        public async Task<Order> UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            try
            {
                var order = await GetOrderByIdAsync(id);
                var user = _currentUserService.GetCurrentUser();

                // Check authorization
                if (user == null || user.Id != order.UserId)
                {
                    throw new UnauthorizedAccessException("Unauthorized to update this order");
                }
                
                // Validate status transition
                if (!IsValidStatusTransition(order.Status, status))
                {
                    throw new InvalidOperationException($"Cannot transition order from {order.Status} to {status}");
                }

                order.Status = status;
                order.TrackingHistory.Add(new OrderTracking
                {
                    OrderId = order.Id,
                    Status = status,
                    Timestamp = DateTime.UtcNow,
                    UpdatedBy = user.Id,
                    Notes = $"Order status updated to {status}"
                });

                // Handle status-specific actions
                switch (status)
                {
                    case OrderStatus.InPreparation:
                        await HandleOrderPreparationAsync(order);
                        break;
                    case OrderStatus.ReadyForPickup:
                        await HandleOrderReadyAsync(order);
                        break;
                    case OrderStatus.OutForDelivery:
                        await HandleOrderOutForDeliveryAsync(order);
                        break;
                    case OrderStatus.Delivered:
                        await HandleOrderDeliveredAsync(order);
                        break;
                    case OrderStatus.Canceled:
                        await HandleOrderCancellationAsync(order);
                        break;
                }

                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex) when (ex is not UnauthorizedAccessException)
            {
                _logger.LogError(ex, "Error updating order {OrderId} status to {Status}", id, status);
                throw new InvalidOperationException($"Error updating order status", ex);
            }
        }

        public async Task<Order> CancelOrderAsync(int id)
        {
            try
            {
                var order = await GetOrderByIdAsync(id);
                var user = _currentUserService.GetCurrentUser();

                // Check authorization
                if (user.Role == UserType.Admin || user.Role == UserType.Owner)
                {
                    throw new UnauthorizedAccessException("Unauthorized to cancel this order");
                }

                if (user == null || user.Id != order.UserId)
                {
                    throw new UnauthorizedAccessException("Unauthorized to cancel this order");
                }

                // Handle status-specific actions
                await HandleOrderCancellationAsync(order);

                // Check if order can be cancelled
                if (!CanCancelOrder(order))
                {
                    throw new InvalidOperationException("This order cannot be cancelled");
                }

                return await UpdateOrderStatusAsync(id, OrderStatus.Canceled);
            }
            catch (Exception ex) when (ex is not UnauthorizedAccessException)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId}", id);
                throw new InvalidOperationException("Error cancelling order", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders for user {UserId}", userId);
                throw new InvalidOperationException("Error retrieving user orders", ex);
            }
        }

        public async Task<Order> AssignDriverAsync(int orderId, int driverId)
        {
            try
            {
                var order = await GetOrderByIdAsync(orderId);
                var driver = await _context.Drivers.FindAsync(driverId);

                if (driver == null)
                {
                    throw new KeyNotFoundException("Driver not found");
                }

                order.DriverId = driverId;
                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning driver {DriverId} to order {OrderId}", driverId, orderId);
                throw new InvalidOperationException("Error assigning driver to order", ex);
            }
        }

        public async Task<Order> UpdateOrderTrackingAsync(int orderId, string status, string notes)
        {
            try
            {
                var order = await GetOrderByIdAsync(orderId);
                order.TrackingHistory.Add(new OrderTracking
                {
                    OrderId = orderId,
                    Status = Enum.Parse<OrderStatus>(status),
                    Timestamp = DateTime.UtcNow,
                    Notes = notes
                });

                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tracking for order {OrderId}", orderId);
                throw new InvalidOperationException("Error updating order tracking", ex);
            }
        }

        public async Task<decimal> CalculateOrderTotalAsync(OrderCreateViewModel model)
        {
            try
            {
                if (model == null || !model.IsValid)
                {
                    throw new ArgumentException("Invalid order data");
                }

                decimal subtotal = 0;
                decimal totalTax = 0;
                decimal totalDeliveryFee = 0;

                foreach (var item in model.Items)
                {
                    var menuItem = await _context.MenuItems.FindAsync(item.MenuItemId);
                    if (menuItem == null)
                    {
                        throw new KeyNotFoundException($"Menu item {item.MenuItemId} not found");
                    }

                    subtotal += menuItem.Price * item.Quantity;
                    totalTax += menuItem.Price * item.Quantity * 0.1m; // 10% tax
                }

                // Calculate delivery fee based on distance or other factors
                totalDeliveryFee = 5.0m; // Example fixed delivery fee

                model.Subtotal = subtotal;
                model.Tax = totalTax;
                model.DeliveryFee = totalDeliveryFee;
                model.Discount = 0;

                // Apply promotion if exists
                if (!string.IsNullOrEmpty(model.PromoCode))
                {
                    var promotion = await _context.Promotions
                        .FirstOrDefaultAsync(p => p.Code == model.PromoCode && p.IsActive);

                    if (promotion != null)
                    {
                        model.Discount = promotion.DiscountType == DiscountType.Percentage
                            ? subtotal * (promotion.DiscountValue / 100)
                            : promotion.DiscountValue;
                    }
                }

                model.Total = model.Subtotal + model.Tax + model.DeliveryFee - model.Discount;
                return model.Total;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating order total");
                throw new InvalidOperationException("Error calculating order total", ex);
            }
        }

        public async Task<IEnumerable<OrderTrackingViewModel>> GetOrderTrackingHistoryAsync(int orderId)
        {
            try
            {
                var order = await GetOrderByIdAsync(orderId);
                return order.TrackingHistory.Select(t => new OrderTrackingViewModel
                {
                    OrderId = t.OrderId,
                    Status = t.Status,
                    Timestamp = t.Timestamp,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tracking history for order {OrderId}", orderId);
                throw new InvalidOperationException("Error retrieving order tracking history", ex);
            }
        }

        public async Task<Order> SetDeliveryTimeAsync(int orderId, DateTime deliveryTime)
        {
            try
            {
                var order = await GetOrderByIdAsync(orderId);
                order.EstimatedDeliveryTime = deliveryTime;
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting delivery time for order {OrderId}", orderId);
                throw new InvalidOperationException("Error setting delivery time", ex);
            }
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order {OrderId}", order.Id);
                throw new InvalidOperationException("Error updating order", ex);
            }
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        private bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                (OrderStatus.Pending, OrderStatus.InPreparation) => true,
                (OrderStatus.InPreparation, OrderStatus.ReadyForPickup) => true,
                (OrderStatus.ReadyForPickup, OrderStatus.OutForDelivery) => true,
                (OrderStatus.OutForDelivery, OrderStatus.Delivered) => true,
                (_, OrderStatus.Canceled) => true,
                _ => false
            };
        }

        private bool CanCancelOrder(Order order)
        {
            return order.Status != OrderStatus.Delivered &&
                   order.Status != OrderStatus.Canceled &&
                   (DateTime.UtcNow - order.OrderDate).TotalMinutes <= 30;
        }

        private async Task HandleOrderPreparationAsync(Order order)
        {
            // Notify restaurants
            foreach (var restaurantGroup in order.OrderItems.GroupBy(i => i.RestaurantId))
            {
                var restaurant = await _context.Restaurants
                    .Include(r => r.Owner)
                    .FirstOrDefaultAsync(r => r.Id == restaurantGroup.Key);

                if (restaurant != null)
                {
                    await _emailSender.SendOrderStatusUpdateAsync(
                        restaurant.Owner.Email,
                        order.OrderNumber,
                        "In Preparation",
                        $"Please prepare {restaurantGroup.Count()} items");
                }
            }
        }

        private async Task HandleOrderReadyAsync(Order order)
        {
            var user = await _context.Users.FindAsync(order.UserId);
            if (user != null)
            {
                await _emailSender.SendOrderStatusUpdateAsync(
                    user.Email,
                    order.OrderNumber,
                    "Ready for Pickup/Delivery",
                    "Your order is ready for pickup/delivery");
            }

            // Notify available drivers
            var availableDrivers = await _context.Drivers
                .Include(d => d.User)
                .Where(d => d.IsAvailable)
                .ToListAsync();

            foreach (var driver in availableDrivers)
            {
                await _emailSender.SendOrderStatusUpdateAsync(
                    driver.User.Email,
                    order.OrderNumber,
                    "New Delivery Available",
                    "A new delivery is available for pickup");
            }
        }

        private async Task HandleOrderOutForDeliveryAsync(Order order)
        {
            var user = await _context.Users.FindAsync(order.UserId);
            if (user != null)
            {
                await _emailSender.SendOrderStatusUpdateAsync(
                    user.Email,
                    order.OrderNumber,
                    "Out for Delivery",
                    "Your order is on its way");
            }
        }

        private async Task HandleOrderDeliveredAsync(Order order)
        {
            var user = await _context.Users.FindAsync(order.UserId);
            if (user != null)
            {
                await _emailSender.SendOrderStatusUpdateAsync(
                    user.Email,
                    order.OrderNumber,
                    "Delivered",
                    "Your order has been delivered");
            }
        }

        private async Task HandleOrderCancellationAsync(Order order)
        {
            var user = await _context.Users.FindAsync(order.UserId);
            if (user != null)
            {
                await _emailSender.SendOrderStatusUpdateAsync(
                    user.Email,
                    order.OrderNumber,
                    "Cancelled",
                    "Your order has been cancelled");
            }
        }

        public async Task<bool> IsRestaurantOwnerAsync(int restaurantId, string userId)
        {
            try
            {
                return await _context.Restaurants
                    .AnyAsync(r => r.Id == restaurantId && r.OwnerId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking restaurant ownership for restaurant {RestaurantId} and user {UserId}", restaurantId, userId);
                throw new InvalidOperationException("Error checking restaurant ownership", ex);
            }
        }

        public async Task<Order> UpdateOrderItemStatusAsync(int orderId, int itemId, OrderItemStatus status)
        {
            try
            {
                var order = await GetOrderByIdAsync(orderId);
                var item = order.OrderItems.FirstOrDefault(i => i.Id == itemId);
                
                if (item == null)
                {
                    throw new KeyNotFoundException($"Order item {itemId} not found in order {orderId}");
                }

                item.Status = status;
                item.PreparedAt = status == OrderItemStatus.Delivered ? DateTime.UtcNow : null;

                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order item status for order {OrderId}, item {ItemId}", orderId, itemId);
                throw new InvalidOperationException("Error updating order item status", ex);
            }
        }

        public async Task<byte[]> ExportOrdersAsync(
            string searchTerm = null,
            string statusFilter = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var orders = await SearchOrdersAsync(
                    searchTerm: searchTerm,
                    statusFilter: statusFilter,
                    startDate: startDate,
                    endDate: endDate,
                    page: 1,
                    pageSize: int.MaxValue);

                using var ms = new MemoryStream();
                using var writer = new StreamWriter(ms);

                // Write CSV header
                writer.WriteLine("Order Number,Order Date,Customer Name,Status,Total,Payment Method,Payment Status,Delivery Address,Restaurant(s),Items");

                // Write data
                foreach (var order in orders.Items)
                {
                    var restaurantNames = string.Join("; ", order.OrderItems
                        .Select(i => i.Restaurant?.Name)
                        .Distinct()
                        .Where(n => !string.IsNullOrEmpty(n)));

                    var items = string.Join("; ", order.OrderItems
                        .Select(i => $"{i.Quantity}x {i.MenuItem?.Name}"));

                    var fields = new[]
                    {
                        order.OrderNumber,
                        order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        order.User?.FullName ?? "N/A",
                        order.Status.ToString(),
                        order.Total.ToString("C"),
                        order.PaymentMethod.ToString(),
                        order.PaymentStatus.ToString(),
                        order.DeliveryAddress?.ToString() ?? "N/A",
                        restaurantNames,
                        items
                    };

                    // Escape fields that contain commas
                    var escapedFields = fields.Select(f => f.Contains(",") ? $"\"{f}\"" : f);
                    writer.WriteLine(string.Join(",", escapedFields));
                }

                writer.Flush();
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting orders");
                throw new InvalidOperationException("Error exporting orders", ex);
            }
        }

        public async Task<byte[]> GenerateOrderPdfAsync(OrderDetailsViewModel order)
        {
            try
            {
                using var ms = new MemoryStream();
                using var document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Add logo
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
                if (File.Exists(logoPath))
                {
                    var logo = Image.GetInstance(logoPath);
                    logo.ScaleToFit(100f, 100f);
                    logo.SetAbsolutePosition(25f, document.PageSize.Height - 100f);
                    document.Add(logo);
                }

                // Add header
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var header = new Paragraph($"Order #{order.OrderNumber}", headerFont);
                header.Alignment = Element.ALIGN_CENTER;
                header.SpacingAfter = 20f;
                document.Add(header);

                // Add order details
                var detailsFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var detailsTable = new PdfPTable(2);
                detailsTable.WidthPercentage = 100;
                detailsTable.SetWidths(new float[] { 1, 2 });

                AddTableRow(detailsTable, "Order Date:", order.CreatedAt.ToString("MMM dd, yyyy HH:mm"), detailsFont);
                AddTableRow(detailsTable, "Status:", order.Status.ToString(), detailsFont);
                AddTableRow(detailsTable, "Customer:", order.CustomerName, detailsFont);
                AddTableRow(detailsTable, "Email:", order.CustomerEmail, detailsFont);
                AddTableRow(detailsTable, "Payment Method:", order.PaymentMethod.ToString(), detailsFont);
                AddTableRow(detailsTable, "Payment Status:", order.PaymentStatus.ToString(), detailsFont);

                var address = $"{order.DeliveryAddress.StreetAddress}\n" +
                            $"{order.DeliveryAddress.City}, {order.DeliveryAddress.State} {order.DeliveryAddress.PostalCode}\n" +
                            $"{order.DeliveryAddress.Country}";
                AddTableRow(detailsTable, "Delivery Address:", address, detailsFont);

                document.Add(detailsTable);
                document.Add(new Paragraph("\n"));

                // Add order items
                var itemsFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var itemsTable = new PdfPTable(5);
                itemsTable.WidthPercentage = 100;
                itemsTable.SetWidths(new float[] { 3, 1, 1, 1, 1 });

                // Add headers
                headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                AddTableRow(itemsTable, "Item", "Price", "Qty", "Subtotal", "Status", headerFont);

                // Add items
                foreach (var group in order.RestaurantGroups)
                {
                    // Add restaurant header
                    var restaurantFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                    var restaurantCell = new PdfPCell(new Phrase(group.RestaurantName, restaurantFont));
                    restaurantCell.Colspan = 5;
                    restaurantCell.BackgroundColor = BaseColor.LightGray;
                    restaurantCell.Padding = 5f;
                    itemsTable.AddCell(restaurantCell);

                    foreach (var item in group.Items)
                    {
                        AddTableRow(itemsTable,
                            item.Name,
                            item.Price.ToString("C"),
                            item.Quantity.ToString(),
                            item.Subtotal.ToString("C"),
                            item.Status.ToString(),
                            itemsFont);
                    }
                }

                document.Add(itemsTable);
                document.Add(new Paragraph("\n"));

                // Add totals
                var totalsTable = new PdfPTable(2);
                totalsTable.WidthPercentage = 50;
                totalsTable.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalsTable.SetWidths(new float[] { 1, 1 });

                AddTableRow(totalsTable, "Subtotal:", order.Subtotal.ToString("C"), itemsFont);
                AddTableRow(totalsTable, "Tax:", order.Tax.ToString("C"), itemsFont);
                AddTableRow(totalsTable, "Delivery Fee:", order.DeliveryFee.ToString("C"), itemsFont);
                if (order.Discount > 0)
                {
                    AddTableRow(totalsTable, "Discount:", (-order.Discount).ToString("C"), itemsFont);
                }
                AddTableRow(totalsTable, "Total:", order.TotalAmount.ToString("C"), itemsFont);

                document.Add(totalsTable);

                // Add tracking history
                if (order.TrackingHistory.Any())
                {
                    document.Add(new Paragraph("\n"));
                    document.Add(new Paragraph("Order Timeline", headerFont));
                    document.Add(new Paragraph("\n"));

                    var timelineTable = new PdfPTable(3);
                    timelineTable.WidthPercentage = 100;
                    timelineTable.SetWidths(new float[] { 1, 2, 3 });

                    AddTableRow(timelineTable, "Date", "Status", "Notes", headerFont);

                    foreach (var tracking in order.TrackingHistory.OrderByDescending(t => t.Timestamp))
                    {
                        AddTableRow(timelineTable,
                            tracking.Timestamp.ToString("MMM dd, HH:mm"),
                            tracking.Status.ToString(),
                            tracking.DriverName ?? string.Empty,
                            itemsFont);
                    }

                    document.Add(timelineTable);
                }

                document.Close();
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF for order {OrderId}", order.OrderId);
                throw new InvalidOperationException("Error generating order PDF", ex);
            }
        }

        private void AddTableRow(PdfPTable table, string label, string value, Font font)
        {
            table.AddCell(new PdfPCell(new Phrase(label, font)) { Padding = 5f });
            table.AddCell(new PdfPCell(new Phrase(value, font)) { Padding = 5f });
        }

        private void AddTableRow(PdfPTable table, string col1, string col2, string col3, string col4, string col5, Font font)
        {
            table.AddCell(new PdfPCell(new Phrase(col1, font)) { Padding = 5f });
            table.AddCell(new PdfPCell(new Phrase(col2, font)) { Padding = 5f, HorizontalAlignment = Element.ALIGN_RIGHT });
            table.AddCell(new PdfPCell(new Phrase(col3, font)) { Padding = 5f, HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase(col4, font)) { Padding = 5f, HorizontalAlignment = Element.ALIGN_RIGHT });
            table.AddCell(new PdfPCell(new Phrase(col5, font)) { Padding = 5f, HorizontalAlignment = Element.ALIGN_CENTER });
        }

        private void AddTableRow(PdfPTable table, string col1, string col2, string col3, Font font)
        {
            table.AddCell(new PdfPCell(new Phrase(col1, font)) { Padding = 5f });
            table.AddCell(new PdfPCell(new Phrase(col2, font)) { Padding = 5f });
            table.AddCell(new PdfPCell(new Phrase(col3, font)) { Padding = 5f });
        }
    }
}
