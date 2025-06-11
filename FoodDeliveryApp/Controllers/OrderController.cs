using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.ViewModels.Order;
using FoodDeliveryApp.ViewModels.Address;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace FoodDeliveryApp.Controllers
{
    [Authorize]
    [Route("orders")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IAddressService _addressService;
        private readonly ICartService _cartService;
        private const int CACHE_DURATION_MINUTES = 5;

        public OrderController(
            IOrderService orderService,
            ILogger<OrderController> logger,
            IMemoryCache cache,
            IAddressService addressService,
            ICartService cartService)
        {
            _orderService = orderService;
            _logger = logger;
            _cache = cache;
            _addressService = addressService;
            _cartService = cartService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index(
            string searchTerm = null!,
            OrderStatus? status = null,
            DateTime? date = null,
            string sortBy = "OrderDate",
            string sortOrder = "desc",
            int page = 1)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Admin");
                var isRestaurantOwner = User.IsInRole("Owner");

                var cacheKey = $"orders_{userId}_{searchTerm}_{status}_{date}_{sortBy}_{sortOrder}_{page}";
                if (!_cache.TryGetValue(cacheKey, out PaginatedList<Order> orders))
                {
                    orders = await _orderService.SearchOrdersAsync(
                        searchTerm: searchTerm,
                        statusFilter: status?.ToString(),
                        startDate: date,
                        endDate: date?.AddDays(1),
                        page: page,
                        pageSize: 10,
                        sortBy: sortBy,
                        sortOrder: sortOrder);

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                    _cache.Set(cacheKey, orders, cacheOptions);
                }

                var viewModel = new OrderListViewModel
                {
                    Orders = orders.Items.Select(o => new OrderSummaryViewModel
                    {
                        OrderId = o.Id,
                        OrderNumber = o.OrderNumber,
                        CreatedAt = o.OrderDate,
                        Status = o.Status,
                        CustomerName = o.User.FullName,
                        CustomerEmail = o.User.Email,
                        TotalAmount = o.Total,
                        PaymentMethod = o.PaymentMethod,
                        CanBeCancelled = o.Status != OrderStatus.Delivered && 
                                       o.Status != OrderStatus.Canceled &&
                                       (DateTime.UtcNow - o.OrderDate).TotalMinutes <= 30,
                        RestaurantGroups = o.OrderItems
                            .GroupBy(i => i.RestaurantId)
                            .Select(g => new RestaurantOrderGroup
                            {
                                RestaurantId = g.Key,
                                RestaurantName = g.First().Restaurant.Name,
                                Items = g.Select(i => new OrderItemViewModel
                                {
                                    MenuItemId = i.MenuItemId,
                                    Name = i.MenuItem.Name,
                                    ImageUrl = i.MenuItem.ImageUrl,
                                    Quantity = i.Quantity,
                                    Price = i.Price,
                                    Subtotal = i.Price * i.Quantity,
                                    SpecialInstructions = i.SpecialInstructions,
                                    Status = i.Status
                                }).ToList(),
                                Subtotal = g.Sum(i => i.Price * i.Quantity),
                                Status = g.All(i => i.Status == OrderItemStatus.Delivered) 
                                    ? OrderItemStatus.Delivered 
                                    : g.Any(i => i.Status == OrderItemStatus.Preparing) 
                                        ? OrderItemStatus.Preparing 
                                        : OrderItemStatus.Pending
                            }).ToList()
                    }).ToList(),
                    SearchTerm = searchTerm,
                    Status = status,
                    Date = date,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    CurrentPage = page,
                    TotalPages = orders.TotalPages,
                    IsAdmin = isAdmin,
                    IsRestaurantOwner = isRestaurantOwner
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders");
                TempData["Error"] = "An error occurred while retrieving orders.";
                return View(new OrderListViewModel());
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var cacheKey = $"order_details_{id}";
                if (!_cache.TryGetValue(cacheKey, out OrderDetailsViewModel viewModel))
                {
                    var order = await _orderService.GetOrderByIdAsync(id);
                    if (order == null)
                    {
                        _logger.LogWarning("Order {OrderId} not found", id);
                        return NotFound();
                    }

                    // Check authorization
                    if (!User.IsInRole("Admin") && 
                        !User.IsInRole("RestaurantOwner") && 
                        order.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                    {
                        _logger.LogWarning("User {UserId} attempted to access order {OrderId} without authorization", 
                            User.FindFirstValue(ClaimTypes.NameIdentifier), id);
                        return Forbid();
                    }

                    viewModel = new OrderDetailsViewModel
                    {
                        OrderId = order.Id,
                        OrderNumber = order.OrderNumber,
                        Status = order.Status,
                        CreatedAt = order.OrderDate,
                        EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                        ActualDeliveryTime = order.ActualDeliveryTime,
                        IsDelayed = order.IsDelayed,
                        DelayReason = order.DelayReason,
                        CustomerName = order.User.FullName,
                        CustomerEmail = order.User.Email,
                        DeliveryAddress = new AddressViewModel
                        {
                            StreetAddress = order.DeliveryAddress.StreetAddress,
                            City = order.DeliveryAddress.City,
                            State = order.DeliveryAddress.State,
                            PostalCode = order.DeliveryAddress.PostalCode,
                            Country = order.DeliveryAddress.Country
                        },
                        PaymentMethod = order.PaymentMethod,
                        PaymentStatus = order.PaymentStatus,
                        RestaurantGroups = order.OrderItems
                            .GroupBy(i => i.RestaurantId)
                            .Select(g => new RestaurantOrderGroup
                            {
                                RestaurantId = g.Key,
                                RestaurantName = g.First().Restaurant.Name,
                                RestaurantPhone = g.First().Restaurant.PhoneNumber,
                                ImageUrl = g.First().Restaurant.ImageUrl,
                                RestaurantAddress = new AddressViewModel
                                {
                                    StreetAddress = g.First().Restaurant.Address,
                                    City = g.First().Restaurant.City,
                                    State = g.First().Restaurant.State,
                                    PostalCode = g.First().Restaurant.PostalCode,
                                    Country = "Egypt"
                                },
                                Items = g.Select(i => new OrderItemViewModel
                                {
                                    MenuItemId = i.MenuItemId,
                                    Name = i.MenuItem.Name,
                                    ImageUrl = i.MenuItem.ImageUrl,
                                    Quantity = i.Quantity,
                                    Price = i.Price,
                                    Subtotal = i.Price * i.Quantity,
                                    SpecialInstructions = i.SpecialInstructions,
                                    Status = i.Status
                                }).ToList(),
                                Subtotal = g.Sum(i => i.Price * i.Quantity),
                                Status = g.All(i => i.Status == OrderItemStatus.Delivered) 
                                    ? OrderItemStatus.Delivered 
                                    : g.Any(i => i.Status == OrderItemStatus.Preparing) 
                                        ? OrderItemStatus.Preparing 
                                        : OrderItemStatus.Pending
                            }).ToList(),
                        Subtotal = order.Subtotal,
                        Tax = order.Tax,
                        DeliveryFee = order.DeliveryFee,
                        Discount = order.Discount,
                        TotalAmount = order.Total,
                        Driver = order.Driver != null ? new DriverViewModel
                        {
                            DriverId = order.Driver.Id,
                            Name = order.Driver.User.FullName,
                            PhoneNumber = order.Driver.User.PhoneNumber,
                            ProfilePictureUrl = order.Driver.User.ProfilePictureUrl,
                            VehicleType = order.Driver.VehicleInfo,
                            LicensePlate = order.Driver.LicenseNumber
                        } : null,
                        TrackingHistory = order.TrackingHistory.Select(t => new OrderTrackingViewModel
                        {
                            OrderId = t.OrderId,
                            OrderNumber = order.OrderNumber,
                            Status = t.Status,
                            Location = t.Location,
                        }).ToList(),
                    };

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                    _cache.Set(cacheKey, viewModel, cacheOptions);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order details for order {OrderId}", id);
                TempData["Error"] = "An error occurred while retrieving order details.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                // Check if user is authorized to cancel the order
                if (!User.IsInRole("Admin") && 
                    !User.IsInRole("RestaurantOwner") && 
                    order.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    return Forbid();
                }

                // Check if order can be cancelled
                if (order.Status == OrderStatus.Delivered || 
                    order.Status == OrderStatus.Canceled ||
                    (DateTime.UtcNow - order.OrderDate).TotalMinutes > 30)
                {
                    TempData["Error"] = "This order cannot be cancelled.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                await _orderService.CancelOrderAsync(id);
                TempData["Success"] = "Order has been cancelled successfully.";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId}", id);
                TempData["Error"] = "An error occurred while cancelling the order.";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        [Authorize(Roles = "Admin,RestaurantOwner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateItemStatus(int orderId, int itemId, OrderItemStatus status)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound();
                }

                // Check if restaurant owner is authorized to update this order
                if (User.IsInRole("RestaurantOwner"))
                {
                    var restaurantId = order.OrderItems.FirstOrDefault(i => i.Id == itemId)?.RestaurantId;
                    if (!restaurantId.HasValue || !await _orderService.IsRestaurantOwnerAsync(restaurantId.Value, User.FindFirstValue(ClaimTypes.NameIdentifier)))
                    {
                        return Forbid();
                    }
                }

                await _orderService.UpdateOrderItemStatusAsync(orderId, itemId, status);

                // Update order status based on all items
                var newOrderStatus = CalculateOrderStatus(order.OrderItems);
                if (newOrderStatus != order.Status)
                {
                    await _orderService.UpdateOrderStatusAsync(orderId, newOrderStatus);
                }

                TempData["Success"] = "Order item status updated successfully.";
                return RedirectToAction(nameof(Details), new { id = orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order item status for order {OrderId}, item {ItemId}", orderId, itemId);
                TempData["Error"] = "An error occurred while updating the order item status.";
                return RedirectToAction(nameof(Details), new { id = orderId });
            }
        }

        private OrderStatus CalculateOrderStatus(ICollection<OrderItem> items)
        {
            if (items.All(i => i.Status == OrderItemStatus.Delivered))
                return OrderStatus.Delivered;
            if (items.Any(i => i.Status == OrderItemStatus.Preparing))
                return OrderStatus.InPreparation;
            if (items.All(i => i.Status == OrderItemStatus.Ready))
                return OrderStatus.ReadyForPickup;
            return OrderStatus.Confirmed;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Export(
            string searchTerm = null,
            OrderStatus? status = null,
            DateTime? date = null)
        {
            try
            {
                var orders = await _orderService.ExportOrdersAsync(
                    searchTerm: searchTerm,
                    statusFilter: status?.ToString(),
                    startDate: date,
                    endDate: date?.AddDays(1));

                var fileName = $"orders_export_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
                return File(orders, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting orders");
                TempData["Error"] = "An error occurred while exporting orders.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet("Export/{id:int}")]
        public async Task<IActionResult> ExportOrder(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                // Check authorization
                if (!User.IsInRole("Admin") && 
                    !User.IsInRole("RestaurantOwner") && 
                    order.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    return Forbid();
                }

                var viewModel = new OrderDetailsViewModel
                {
                    OrderId = order.Id,
                    OrderNumber = order.OrderNumber,
                    Status = order.Status,
                    CreatedAt = order.OrderDate,
                    EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                    ActualDeliveryTime = order.ActualDeliveryTime,
                    IsDelayed = order.IsDelayed,
                    DelayReason = order.DelayReason,
                    CustomerName = order.User.FullName,
                    CustomerEmail = order.User.Email,
                    DeliveryAddress = new AddressViewModel
                    {
                        StreetAddress = order.DeliveryAddress.StreetAddress,
                        City = order.DeliveryAddress.City,
                        State = order.DeliveryAddress.State,
                        PostalCode = order.DeliveryAddress.PostalCode,
                        Country = order.DeliveryAddress.Country
                    },
                    PaymentMethod = order.PaymentMethod,
                    PaymentStatus = order.PaymentStatus,
                    RestaurantGroups = order.OrderItems
                        .GroupBy(i => i.RestaurantId)
                        .Select(g => new RestaurantOrderGroup
                        {
                            RestaurantId = g.Key,
                            RestaurantName = g.First().Restaurant.Name,
                            RestaurantPhone = g.First().Restaurant.PhoneNumber,
                            RestaurantAddress = new AddressViewModel
                            {
                                StreetAddress = g.First().Restaurant.Address,
                                City = g.First().Restaurant.City,
                                State = g.First().Restaurant.State,
                                PostalCode = g.First().Restaurant.PostalCode,
                                Country = "Egypt"
                            },
                            Items = g.Select(i => new OrderItemViewModel
                            {
                                MenuItemId = i.MenuItemId,
                                Name = i.MenuItem.Name,
                                ImageUrl = i.MenuItem.ImageUrl,
                                Quantity = i.Quantity,
                                Price = i.Price,
                                Subtotal = i.Price * i.Quantity,
                                SpecialInstructions = i.SpecialInstructions,
                                Status = i.Status
                            }).ToList(),
                            Subtotal = g.Sum(i => i.Price * i.Quantity),
                            Status = g.All(i => i.Status == OrderItemStatus.Delivered) 
                                ? OrderItemStatus.Delivered 
                                : g.Any(i => i.Status == OrderItemStatus.Preparing) 
                                    ? OrderItemStatus.Preparing 
                                    : OrderItemStatus.Pending
                        }).ToList(),
                    Subtotal = order.Subtotal,
                    Tax = order.Tax,
                    DeliveryFee = order.DeliveryFee,
                    Discount = order.Discount,
                    TotalAmount = order.Total,
                    Driver = order.Driver != null ? new DriverViewModel
                    {
                        DriverId = order.Driver.Id,
                        Name = order.Driver.User.FullName,
                        PhoneNumber = order.Driver.User.PhoneNumber,
                        ProfilePictureUrl = order.Driver.User.ProfilePictureUrl,
                        VehicleType = order.Driver.VehicleInfo,
                        LicensePlate = order.Driver.LicenseNumber
                    } : null,
                    TrackingHistory = order.TrackingHistory.Select(t => new OrderTrackingViewModel
                    {
                        OrderId = t.OrderId,
                        OrderNumber = order.OrderNumber,
                        Status = t.Status,
                        Location = t.Location,
                        DriverName = t.Driver?.User?.FullName,
                        DriverPhone = t.Driver?.User?.PhoneNumber,
                        DriverPhotoUrl = t.Driver?.User?.ProfilePictureUrl,
                        Timestamp = t.Timestamp,
                    }).ToList()
                };

                // Generate PDF using a view
                var pdfBytes = await _orderService.GenerateOrderPdfAsync(viewModel);
                var fileName = $"order_{order.OrderNumber}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting order {OrderId}", id);
                TempData["Error"] = "An error occurred while exporting the order.";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var addresses = await _addressService.GetUserAddressesAsync(userId);
                ViewBag.DeliveryAddresses = addresses;

                var viewModel = new OrderCreateViewModel
                {
                    Items = new List<OrderItemCreateViewModel>(),
                    PaymentMethod = PaymentMethod.CashOnDelivery
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing order creation form");
                TempData["Error"] = "An error occurred while preparing the order form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var addresses = await _addressService.GetUserAddressesAsync(currentUserId);
                    ViewBag.DeliveryAddresses = addresses;
                    return View(model);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var order = await _orderService.CreateOrderAsync(model);

                if (order == null)
                {
                    TempData["Error"] = "Failed to create the order. Please try again.";
                    return View(model);
                }

                return RedirectToAction(nameof(Details), new { id = order.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                TempData["Error"] = "An error occurred while creating your order.";
                return View(model);
            }
        }

        [HttpGet]
        [Route("Checkout")]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["Error"] = "User not authenticated.";
                    return RedirectToAction("Login", "Account");
                }

                var cart = await _cartService.GetCartAsync(userId);
                if (cart == null)
                {
                    TempData["Error"] = "Cart not found. Please try again.";
                    return RedirectToAction("Index", "Cart");
                }

                if (cart.IsEmpty)
                {
                    TempData["Error"] = "Your cart is empty. Please add items before checkout.";
                    return RedirectToAction("Index", "Cart");
                }

                var addresses = await _addressService.GetUserAddressesAsync(userId);
                if (!addresses.Any())
                {
                    TempData["Warning"] = "Please add a delivery address before proceeding with checkout.";
                    return RedirectToAction("Create", "Address", new { returnUrl = Url.Action("Checkout", "Order") });
                }

                var cartItems = await _cartService.GetCartItemsAsync(userId);
                if (cartItems == null || !cartItems.Any())
                {
                    TempData["Error"] = "No items found in cart. Please try again.";
                    return RedirectToAction("Index", "Cart");
                }

                var viewModel = new OrderCreateViewModel
                {
                    Items = cartItems.Select(item => new OrderItemCreateViewModel
                    {
                        MenuItemId = item.MenuItemId,
                        RestaurantId = item.RestaurantId,
                        Quantity = item.Quantity,
                        SpecialInstructions = item.SpecialInstructions ?? "",
                        MenuItemName = item.MenuItem?.Name ?? "Unknown Item",
                        RestaurantName = item.MenuItem?.Restaurant?.Name ?? "Unknown Restaurant",
                        ImageUrl = item.MenuItem?.ImageUrl ?? "/images/placeholder-dish.jpg",
                        UnitPrice = item.MenuItem?.Price ?? 0,
                        Modifiers = new List<string>(),
                        
                    }).ToList(),
                    DeliveryAddresses = addresses.Select(a => new AddressViewModel
                    {
                        Id = a.Id,
                        StreetAddress = a.StreetAddress,
                        City = a.City,
                        State = a.State,
                        PostalCode = a.PostalCode,
                        Country = a.Country,
                        IsDefault = a.IsDefault
                    }).ToList(),
                    Subtotal = cart.Subtotal,
                    Tax = cart.Tax,
                    DeliveryFee = cart.DeliveryFee,
                    Discount = cart.PromotionDiscountAmount,
                    Total = cart.Total,
                    PaymentMethod = PaymentMethod.CashOnDelivery
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing checkout form");
                TempData["Error"] = "An error occurred while preparing the checkout form.";
                return RedirectToAction("Index", "Cart");
            }
        }

        [HttpPost]
        [Route("Checkout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderCreateViewModel model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["Error"] = "User not authenticated.";
                    return RedirectToAction("Login", "Account");
                }

                // Always ensure DeliveryAddresses is populated
                var addresses = await _addressService.GetUserAddressesAsync(userId);
                model.DeliveryAddresses = addresses.Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    StreetAddress = a.StreetAddress,
                    City = a.City,
                    State = a.State,
                    PostalCode = a.PostalCode,
                    Country = a.Country,
                    IsDefault = a.IsDefault
                }).ToList();

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var order = await _orderService.CreateOrderAsync(model);

                if (order == null)
                {
                    TempData["Error"] = "Failed to create the order. Please try again.";
                    return View(model);
                }

                // Clear the cart after successful order creation
                await _cartService.ClearCartAsync(userId);

                return RedirectToAction(nameof(Details), new { id = order.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing checkout");
                TempData["Error"] = "An error occurred while processing your order.";
                return View(model);
            }
        }
    }
}
