using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;
        private readonly IPaymentService _paymentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderController> _logger;
        private readonly IAddressRepository _addressRepository;

        public OrderController(
            IOrderRepository orderRepository,
            ICartService cartService,
            IMenuItemRepository menuItemRepository,
            IRestaurantRepository restaurantRepository,
            IRepository<PaymentMethod> paymentMethodRepository,
            IPaymentService paymentService,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IAddressRepository addressRepository,
            ILogger<OrderController> logger)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
            _menuItemRepository = menuItemRepository;
            _restaurantRepository = restaurantRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _paymentService = paymentService;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _addressRepository = addressRepository;
        }

        #region Customer Actions

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var cart = await _cartService.GetCartAsync(user.Id);
                var addresses = await _addressRepository.GetUserAddressesAsync(user.Id);
                if (!cart.Items.Any())
                {
                    TempData["ErrorMessage"] = "Your cart is empty.";
                    return RedirectToAction("Index", "Cart");
                }

                var viewModel = new OrderCreateViewModel
                {
                    UserId = user.Id,
                    Addresses = addresses.Select(a => new AddressViewModel
                    {
                        Id = a.Id,
                        Street = a.Street,
                        City = a.City,
                        State = a.State,
                        PostalCode = a.PostalCode,
                        Country = a.Country
                    }).ToList(),
                    Items = cart.Items.Select(item => new OrderItemViewModel
                    {
                        MenuItemId = item.MenuItemId,
                        Name = item.MenuItem.Name,
                        Quantity = item.Quantity,
                        Price = item.MenuItem.Price + item.Customizations.Sum(c => c.Price),
                        ImageUrl = item.MenuItem.ImageUrl ?? "/images/placeholder-dish.jpg",
                        RestaurantId = item.MenuItem.RestaurantId,
                        RestaurantName = item.MenuItem.Restaurant.Name,
                        Customizations = item.Customizations.Select(c => new OrderCustomizationViewModel
                        {
                            OptionId = c.OptionId,
                            ChoiceId = c.ChoiceId,
                            Price = c.Price
                        }).ToList()
                    }).ToList(),
                    AvailablePaymentMethods = await GetUserPaymentMethods(user.Id),
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading checkout page for user {UserId}", User.Identity.Name);
                TempData["ErrorMessage"] = "An error occurred while loading checkout.";
                return RedirectToAction("Index", "Cart");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailablePaymentMethods = await GetUserPaymentMethods(model.UserId);
                return View(model);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                var cart = await _cartService.GetCartAsync(user.Id);
                if (!cart.Items.Any())
                {
                    TempData["ErrorMessage"] = "Your cart is empty.";
                    return RedirectToAction("Index", "Cart");
                }

                // Group cart items by restaurant
                var restaurantGroups = cart.Items.GroupBy(i => i.MenuItem.RestaurantId).ToList();
                var orders = new List<Order>();

                foreach (var group in restaurantGroups)
                {
                    var restaurantId = group.Key;
                    var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
                    var cartItems = group.ToList();

                    // Calculate totals for this restaurant
                    var (subtotal, deliveryFee, tax, total) = await CalculateOrderTotals(cartItems);

                    // Create order for this restaurant
                    var order = new Order
                    {
                        UserId = user.Id,
                        RestaurantId = restaurantId,
                        OrderDate = DateTime.UtcNow,
                        Status = OrderStatus.Pending,
                        Subtotal = subtotal,
                        DeliveryFee = deliveryFee,
                        Tax = tax,
                        Total = total,
                        DeliveryAddressId = model.SelectedAddressId,
                        SpecialInstructions = model.SpecialInstructions,
                        PaymentMethodType = model.SelectedPaymentMethod,
                        PaymentDetails = await GetPaymentDetails(model)
                    };

                    // Process payment if not cash on delivery
                    if (model.SelectedPaymentMethod != PaymentMethodType.CashOnDelivery)
                    {
                        var paymentResult = await ProcessPayment(order, model.SelectedPaymentMethodId);
                        if (!paymentResult.Success)
                        {
                            ModelState.AddModelError("", paymentResult.ErrorMessage);
                            model.AvailablePaymentMethods = await GetUserPaymentMethods(model.UserId);
                            return View(model);
                        }

                        order.Payment = new Payment
                        {
                            Amount = order.Total,
                            Status = PaymentStatus.Completed,
                            PaymentDate = DateTime.UtcNow,
                            TransactionId = paymentResult.TransactionId,
                            PaymentMethodId = model.SelectedPaymentMethodId
                        };
                    }

                    // Add order items
                    foreach (var item in cartItems)
                    {
                        order.OrderItems.Add(new OrderItem
                        {
                            MenuItemId = item.MenuItemId,
                            RestaurantId = item.MenuItem.RestaurantId,
                            Quantity = item.Quantity,
                            Price = item.MenuItem.Price + item.Customizations.Sum(c => c.Price),
                            Customizations = item.Customizations.Select(c => new OrderCustomization
                            {
                                OptionId = c.OptionId,
                                ChoiceId = c.ChoiceId,
                                Price = c.Price
                            }).ToList()
                        });
                    }

                    orders.Add(order);
                }

                // Save all orders
                foreach (var order in orders)
                {
                    await _orderRepository.AddAsync(order);
                }

                // Clear cart
                await _cartService.ClearCartAsync(user.Id);

                TempData["SuccessMessage"] = $"Order{(orders.Count > 1 ? "s" : "")} placed successfully!";
                return RedirectToAction("History");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error placing order for user {UserId}", User.Identity.Name);
                ModelState.AddModelError("", "An error occurred while placing your order.");
                model.AvailablePaymentMethods = await GetUserPaymentMethods(model.UserId);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);

                if (order == null)
                {
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);
                if (order.UserId != user.Id && !User.IsInRole("Admin") && !User.IsInRole("RestaurantOwner"))
                {
                    return Forbid();
                }

                var viewModel = MapToOrderViewModel(order);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order details for order {OrderId}, user {UserId}", id, User.Identity.Name);
                TempData["ErrorMessage"] = "An error occurred while loading order details.";
                return RedirectToAction("History");
            }
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var orders = await _orderRepository.GetUserOrdersAsync(user.Id);

                var viewModel = orders.Select(order => new OrderHistoryViewModel
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    Total = order.Total,
                    PaymentStatus = order.Payment?.Status ?? PaymentStatus.Pending,
                    Items = order.OrderItems.Take(3).Select(item => new OrderItemViewModel
                    {
                        Name = item.MenuItem.Name,
                        RestaurantName = item.Restaurant.Name,
                        Quantity = item.Quantity
                    }).ToList(),
                    RestaurantName = order.Restaurant.Name
                }).OrderByDescending(o => o.OrderDate).ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order history for user {UserId}", User.Identity.Name);
                TempData["ErrorMessage"] = "An error occurred while loading your order history.";
                return View(new List<OrderHistoryViewModel>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(string id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);
                if (order.UserId != user.Id && !User.IsInRole("Admin") && !User.IsInRole("RestaurantOwner"))
                {
                    return Forbid();
                }

                if (order.Status != OrderStatus.Pending)
                {
                    TempData["ErrorMessage"] = "Only pending orders can be cancelled.";
                    return RedirectToAction("Details", new { id });
                }

                if (order.Payment != null && order.Payment.Status == PaymentStatus.Completed)
                {
                    var refundResult = await _paymentService.ProcessRefund(order.Payment.TransactionId, order.Total, $"Order {id} cancelled");
                    if (!refundResult.Success)
                    {
                        TempData["ErrorMessage"] = $"Could not process refund: {refundResult.ErrorMessage}";
                        return RedirectToAction("Details", new { id });
                    }

                    order.Payment.Status = PaymentStatus.Refunded;
                }

                order.Status = OrderStatus.Cancelled;
                await _orderRepository.UpdateAsync(order);

                TempData["SuccessMessage"] = "Order cancelled successfully.";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId} for user {UserId}", id, User.Identity.Name);
                TempData["ErrorMessage"] = "An error occurred while cancelling the order.";
                return RedirectToAction("Details", new { id });
            }
        }

        #endregion

        #region Restaurant/Admin Actions

        [Authorize(Roles = "Admin,RestaurantOwner")]
        [HttpGet]
        public async Task<IActionResult> Manage(string status = "all", int restaurantId = 0)
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync();
                var user = await _userManager.GetUserAsync(User);

                // Filter for RestaurantOwner
                if (User.IsInRole("RestaurantOwner"))
                {
                    // Assume ApplicationUser has a RestaurantId property or similar
                    var ownerRestaurants = await _restaurantRepository.FindAsync(r => r.OwnerId == user.Id);
                    var restaurantIds = ownerRestaurants.Select(r => r.Id).ToList();
                    orders = orders.Where(o => restaurantIds.Contains(o.RestaurantId)).ToList();
                }

                // Apply filters
                if (status != "all" && Enum.TryParse<OrderStatus>(status, true, out var statusFilter))
                {
                    orders = orders.Where(o => o.Status == statusFilter).ToList();
                }

                if (restaurantId != 0 && User.IsInRole("RestaurantOwner"))
                {
                    orders = orders.Where(o => o.RestaurantId == restaurantId).ToList();
                }

                var viewModel = orders.Select(order => new OrderManagementViewModel
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    Total = order.Total,
                    CustomerName = order.User.UserName,
                    RestaurantName = order.Restaurant.Name,
                    ItemsCount = order.OrderItems.Count,
                    PaymentStatus = order.Payment?.Status ?? PaymentStatus.Pending
                }).OrderByDescending(o => o.OrderDate).ToList();

                ViewBag.StatusFilter = status;
                ViewBag.RestaurantId = restaurantId;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order management for user {UserId}", User.Identity.Name);
                TempData["ErrorMessage"] = "An error occurred while loading orders.";
                return View(new List<OrderManagementViewModel>());
            }
        }

        [Authorize(Roles = "Admin,RestaurantOwner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(string id, OrderStatus status, string restaurantId = null)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                // Restrict RestaurantOwner to their own restaurant
                if (User.IsInRole("RestaurantOwner"))
                {
                    var user = await _userManager.GetUserAsync(User);
                    var ownerRestaurants = await _restaurantRepository.FindAsync(r => r.OwnerId == user.Id);
                    if (!ownerRestaurants.Any(r => r.Id == order.RestaurantId))
                    {
                        return Forbid();
                    }
                }

                order.Status = status;
                await _orderRepository.UpdateAsync(order);

                TempData["SuccessMessage"] = $"Order status updated to {status}.";
                return RedirectToAction("Manage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order {OrderId} status for user {UserId}", id, User.Identity.Name);
                TempData["ErrorMessage"] = "An error occurred while updating order status.";
                return RedirectToAction("Manage");
            }
        }

        #endregion

        #region Private Helper Methods

        private async Task<(decimal subtotal, decimal deliveryFee, decimal tax, decimal total)> CalculateOrderTotals(List<CartItem> cartItems)
        {
            var subtotal = cartItems.Sum(item => (item.MenuItem.Price + item.Customizations.Sum(c => c.Price)) * item.Quantity);
            var deliveryFee = CalculateDeliveryFee(1); // Per restaurant
            var tax = CalculateTax(subtotal);
            var total = subtotal + deliveryFee + tax;

            return (subtotal, deliveryFee, tax, total);
        }

        private decimal CalculateDeliveryFee(int restaurantCount)
        {
            var baseFee = _configuration.GetValue<decimal>("CartSettings:BaseDeliveryFeePerRestaurant", 5.00m);
            return baseFee * restaurantCount;
        }

        private decimal CalculateTax(decimal subtotal)
        {
            var taxRate = _configuration.GetValue<decimal>("CartSettings:TaxRate", 0.07m);
            return subtotal * taxRate;
        }

        private async Task<List<PaymentMethodViewModel>> GetUserPaymentMethods(string userId)
        {
            var methods = await _paymentMethodRepository.FindAsync(m => m.UserId == userId);
            return methods.Select(m => new PaymentMethodViewModel
            {
                Id = m.Id,
                Type = m.Type,
                DisplayName = GetPaymentMethodDisplayName(m),
                LastFourDigits = m.AccountNumberMasked?.Length >= 4
                    ? m.AccountNumberMasked.Substring(m.AccountNumberMasked.Length - 4)
                    : null
            }).ToList();
        }

        private string GetPaymentMethodDisplayName(PaymentMethod method)
        {
            return method.Type switch
            {
                PaymentMethodType.CreditCard => $"{method.Provider} ending in {method.AccountNumberMasked?.Substring(method.AccountNumberMasked.Length - 4)}",
                PaymentMethodType.DebitCard => $"{method.Provider} ending in {method.AccountNumberMasked?.Substring(method.AccountNumberMasked.Length - 4)}",
                _ => method.Type.ToString()
            };
        }

        private async Task<string> GetPaymentDetails(OrderCreateViewModel model)
        {
            if (model.SelectedPaymentMethod == PaymentMethodType.CashOnDelivery)
            {
                return "Cash on Delivery";
            }

            var method = await _paymentMethodRepository.GetByIdAsync(model.SelectedPaymentMethodId);
            return method?.Type switch
            {
                PaymentMethodType.CreditCard or PaymentMethodType.DebitCard
                    => $"{method.Provider} ending in {method.AccountNumberMasked?.Substring(method.AccountNumberMasked.Length - 4)}",
                _ => method?.Type.ToString() ?? "Unknown"
            };
        }

        private async Task<PaymentResult> ProcessPayment(Order order, int paymentMethodId)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(paymentMethodId);
            if (paymentMethod == null)
            {
                return new PaymentResult { Success = false, ErrorMessage = "Payment method not found." };
            }
            if (paymentMethod.Type == PaymentMethodType.CashOnDelivery)
            {
                return new PaymentResult { Success = true, TransactionId = Guid.NewGuid().ToString() };
            }
            if (paymentMethod.Type == PaymentMethodType.CreditCard || paymentMethod.Type == PaymentMethodType.DebitCard)
            {
                var result = await _paymentService.ProcessPayment(paymentMethodId, order.Total, $"Order {order.Id}");
                if (result.Success)
                {
                    return result;
                }
                return new PaymentResult { Success = false, ErrorMessage = result.ErrorMessage };
            }
            return new PaymentResult { Success = false, ErrorMessage = "Unsupported payment method." };
        }

        private OrderViewModel MapToOrderViewModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Subtotal = order.Subtotal,
                DeliveryFee = order.DeliveryFee,
                Tax = order.Tax,
                Total = order.Total,
                DeliveryAddressId = order.DeliveryAddressId,
                SpecialInstructions = order.SpecialInstructions,
                PaymentMethodType = order.PaymentMethodType,
                PaymentDetails = order.PaymentDetails,
                PaymentStatus = order.Payment?.Status ?? PaymentStatus.Pending,
                Items = order.OrderItems.Select(item => new OrderItemViewModel
                {
                    MenuItemId = item.MenuItemId,
                    Name = item.MenuItem.Name,
                    RestaurantId = item.RestaurantId,
                    RestaurantName = item.Restaurant.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    ImageUrl = item.MenuItem.ImageUrl ?? "/images/placeholder-dish.jpg",
                    Customizations = item.Customizations.Select(c => new OrderCustomizationViewModel
                    {
                        OptionId = c.OptionId,
                        ChoiceId = c.ChoiceId,
                        Price = c.Price
                    }).ToList()
                }).ToList(),
                Address = order.Address != null ? new AddressViewModel
                {
                    Id = order.Address.Id,
                    Street = order.Address.Street,
                    City = order.Address.City,
                    State = order.Address.State,
                    PostalCode = order.Address.PostalCode,
                    Country = order.Address.Country
                } : null
            };
        }

        #endregion

        // GET: Order/GetCartCount
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            try
            {
                var userId = User.Identity.Name;
                var count = await _cartService.GetCartItemCountAsync(userId);
                return Json(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart count for user {UserId}", User.Identity.Name);
                return Json(0);
            }
        }
    }
}