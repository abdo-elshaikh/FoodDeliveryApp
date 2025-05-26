using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels.Address;
using FoodDeliveryApp.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        private readonly IUnitOfWork _unitOfWork;

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
            IUnitOfWork unitOfWork,
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
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Challenge();

                var cart = await _cartService.GetCartAsync(user.Id);
                if (cart == null || !cart.Items.Any())
                {
                    TempData["ErrorMessage"] = "Your cart is empty.";
                    return RedirectToAction("Index", "Cart");
                }

                var addresses = await _addressRepository.GetUserAddressesAsync(user.Id);
                var paymentMethods = await _paymentMethodRepository.GetAllAsync();
                var restaurantId = cart.Items.First().MenuItem.RestaurantId;
                var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);

                var cartItemsViewModel = cart.Items.Select(item => new OrderItemViewModel
                {
                    Id = item.Id,
                    MenuItemId = item.MenuItemId,
                    Name = item.MenuItem.Name,
                    Price = item.MenuItem.Price,
                    Quantity = item.Quantity,
                    ImageUrl = item.MenuItem.ImageUrl ?? "/images/placeholder-dish.jpg",
                    Customizations = item.Customizations?.Select(c => new OrderCustomizationViewModel
                    {
                        Id = c.Id,
                        OptionId = c.OptionId,
                        ChoiceId = c.ChoiceId,
                        Name = c.Option?.Name ?? "N/A",
                        Choice = c.Choice?.Name ?? "N/A",
                        Price = c.Price
                    }).ToList() ?? new List<OrderCustomizationViewModel>()
                }).ToList();

                decimal subtotal = cartItemsViewModel.Sum(item => item.Price * item.Quantity + item.Customizations.Sum(c => c.Price * item.Quantity));
                decimal deliveryFee = restaurant?.DeliveryFee ?? 5.0m;
                decimal taxRate = 0.1m;
                decimal tax = subtotal * taxRate;
                decimal discount = 0; // TODO: Implement promo code logic
                decimal total = subtotal + deliveryFee + tax - discount;

                var viewModel = new OrderCreateViewModel
                {
                    RestaurantId = restaurantId,
                    RestaurantName = restaurant?.Name ?? "N/A",
                    CartItems = cartItemsViewModel,
                    Subtotal = subtotal,
                    Tax = tax,
                    DeliveryFee = deliveryFee,
                    Discount = discount,
                    Total = total,
                    AddressOptions = addresses.Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = $"{a.Street}, {a.City}, {a.State} {a.PostalCode}"
                    }),
                    PaymentMethodOptions = paymentMethods.Select(pm => new SelectListItem
                    {
                        Value = pm.Id.ToString(),
                        Text = pm.Provider.ToString()
                    })
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading checkout page for user {UserId}", User.Identity?.Name);
                TempData["ErrorMessage"] = "An error occurred while loading checkout.";
                return RedirectToAction("Index", "Cart");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderCreateViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!ModelState.IsValid)
            {
                var addresses = await _addressRepository.GetUserAddressesAsync(user.Id);
                var paymentMethods = await _paymentMethodRepository.GetAllAsync();
                model.AddressOptions = addresses.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Street}, {a.City}, {a.State} {a.PostalCode}"
                });
                model.PaymentMethodOptions = paymentMethods.Select(pm => new SelectListItem
                {
                    Value = pm.Id.ToString(),
                    Text = pm.Provider.ToString()
                });
                var cart = await _cartService.GetCartAsync(user.Id);
                if (cart != null && cart.Items.Any())
                {
                    model.CartItems = cart.Items.Select(item => new OrderItemViewModel
                    {
                        Id = item.Id,
                        MenuItemId = item.MenuItemId,
                        Name = item.MenuItem.Name,
                        Price = item.MenuItem.Price,
                        Quantity = item.Quantity,
                        ImageUrl = item.MenuItem.ImageUrl ?? "/images/placeholder-dish.jpg",
                        Customizations = item.Customizations?.Select(c => new OrderCustomizationViewModel
                        {
                            Id = c.Id,
                            OptionId = c.OptionId,
                            ChoiceId = c.ChoiceId,
                            Name = c.Option?.Name ?? "N/A",
                            Choice = c.Choice?.Name ?? "N/A",
                            Price = c.Price
                        }).ToList() ?? new List<OrderCustomizationViewModel>()
                    }).ToList();

                    decimal subtotal = model.CartItems.Sum(item => item.Price * item.Quantity + item.Customizations.Sum(c => c.Price * item.Quantity));
                    var restaurant = await _restaurantRepository.GetByIdAsync(model.RestaurantId);
                    decimal deliveryFee = restaurant?.DeliveryFee ?? 5.0m;
                    decimal taxRate = 0.1m;
                    decimal tax = subtotal * taxRate;
                    decimal discount = 0; // TODO: Implement promo code logic
                    model.Subtotal = subtotal;
                    model.Tax = tax;
                    model.DeliveryFee = deliveryFee;
                    model.Discount = discount;
                    model.Total = subtotal + deliveryFee + tax - discount;
                    model.RestaurantName = restaurant?.Name ?? "N/A";
                }
                return View(model);
            }

            try
            {
                var cart = await _cartService.GetCartAsync(user.Id);
                if (cart == null || !cart.Items.Any())
                {
                    TempData["ErrorMessage"] = "Your cart is empty or session expired.";
                    return RedirectToAction("Index", "Cart");
                }

                var restaurant = await _restaurantRepository.GetByIdAsync(model.RestaurantId);
                var deliveryAddress = await _addressRepository.GetByIdAsync(model.AddressId);
                var paymentMethod = await _paymentMethodRepository.GetByIdAsync(model.PaymentMethodId);

                if (restaurant == null || deliveryAddress == null || paymentMethod == null)
                {
                    TempData["ErrorMessage"] = "Invalid restaurant, address, or payment method selected.";
                    return View(model);
                }

                var order = new Order
                {
                    UserId = user.Id,
                    RestaurantId = model.RestaurantId,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Placed,
                    Subtotal = model.Subtotal,
                    DeliveryFee = model.DeliveryFee,
                    Tax = model.Tax,
                    Total = model.Total,
                    DeliveryAddressId = model.AddressId,
                    SpecialInstructions = model.SpecialRequests,
                    PaymentMethodType = paymentMethod.Type,
                    PaymentDetails = paymentMethod.Type == PaymentMethodType.CashOnDelivery
                        ? "Cash on Delivery"
                        : $"{paymentMethod.Provider} ending in {paymentMethod.AccountNumberMasked?.Substring(paymentMethod.AccountNumberMasked.Length - 4)}"
                };

                foreach (var cartItem in cart.Items)
                {
                    var orderItem = new OrderItem
                    {
                        MenuItemId = cartItem.MenuItemId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.MenuItem.Price,
                        RestaurantId = cartItem.MenuItem.RestaurantId,
                        Customizations = cartItem.Customizations?.Select(cc => new OrderCustomization
                        {
                            OptionId = cc.OptionId,
                            ChoiceId = cc.ChoiceId,
                            Price = cc.Price
                        }).ToList() ?? new List<OrderCustomization>()
                    };
                    order.OrderItems.Add(orderItem);
                }

                var payment = new Payment
                {
                    OrderId = order.Id,
                    Amount = order.Total,
                    PaymentDate = DateTime.UtcNow,
                    Status = PaymentStatus.Pending,
                    PaymentMethodId = model.PaymentMethodId,
                    UserId = user.Id
                };

                if (paymentMethod.Type == PaymentMethodType.CashOnDelivery)
                {
                    payment.Status = PaymentStatus.Pending;
                    payment.TransactionId = Guid.NewGuid().ToString();
                }
                else
                {
                    var paymentResult = await _paymentService.ProcessPayment(model.PaymentMethodId, order.Total, $"Order {order.Id}");
                    if (!paymentResult.Success)
                    {
                        TempData["ErrorMessage"] = $"Payment failed: {paymentResult.ErrorMessage}";
                        return View(model);
                    }
                    payment.Status = PaymentStatus.Paid;
                    payment.TransactionId = paymentResult.TransactionId;
                }

                order.Payment = payment;
                await _orderRepository.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();
                await _cartService.ClearCartAsync(user.Id);

                TempData["SuccessMessage"] = "Order placed successfully!";
                return RedirectToAction("Details", new { id = order.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error placing order for user {UserId}", User.Identity?.Name);
                TempData["ErrorMessage"] = "An error occurred while placing your order.";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrderWithDetailsAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found.", id);
                    return NotFound();
                }

                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null) return Challenge();

                bool isOwner = order.UserId == currentUser.Id;
                bool isAdmin = User.IsInRole("Admin");
                bool isRestaurantOwner = order.Restaurant?.OwnerId == currentUser.Id;

                if (!isOwner && !isAdmin && !isRestaurantOwner)
                {
                    _logger.LogWarning("User {UserId} forbidden from viewing order {OrderId}.", currentUser.Id, id);
                    return Forbid();
                }

                var viewModel = new OrderDetailsViewModel
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    Address = order.Address != null ? new AddressViewModel
                    {
                        Id = order.Address.Id,
                    } : null,
                    SpecialInstructions = order.SpecialInstructions,
                    PaymentDetails = order.PaymentDetails,
                    PaymentStatus = order.Payment?.Status ?? PaymentStatus.Pending,
                    RestaurantName = order.Restaurant?.Name ?? "N/A",
                    Items = order.OrderItems.Select(oi => new OrderItemViewModel
                    {
                        Id = oi.Id,
                        MenuItemId = oi.MenuItemId,
                        Name = oi.MenuItem?.Name ?? "N/A",
                        ImageUrl = oi.MenuItem?.ImageUrl ?? "/images/placeholder-dish.jpg",
                        Price = oi.Price,
                        Quantity = oi.Quantity,
                        SpecialInstructions = oi.SpecialInstructions,
                        Customizations = oi.Customizations.Select(oc => new OrderCustomizationViewModel
                        {
                            Id = oc.Id,
                            OptionId = oc.OptionId,
                            ChoiceId = oc.ChoiceId,
                            Name = oc.OrderItem?.MenuItem.Name ?? "N/A",
                            //Choice = oc?.Name ?? "N/A",
                            Price = oc.Price
                        }).ToList()
                    }).ToList(),
                    Subtotal = order.Subtotal,
                    DeliveryFee = order.DeliveryFee,
                    Tax = order.Tax,
                    Total = order.Total
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order details for order {OrderId}, user {UserId}", id, User.Identity?.Name);
                TempData["ErrorMessage"] = "An error occurred while loading order details.";
                return RedirectToAction("History");
            }
        }

        [HttpGet]
        public async Task<IActionResult> History(int page = 1, int pageSize = 10, string sortBy = "OrderDate", string sortOrder = "desc")
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Challenge();

                var orders = await _orderRepository.GetUserOrdersAsync(user.Id, o => o.Restaurant, o => o.OrderItems, o => o.Payment);
                if (sortOrder?.ToLower() == "desc")
                {
                    orders = sortBy switch
                    {
                        "Total" => orders.OrderByDescending(o => o.Total),
                        _ => orders.OrderByDescending(o => o.OrderDate)
                    };
                }
                else
                {
                    orders = sortBy switch
                    {
                        "Total" => orders.OrderBy(o => o.Total),
                        _ => orders.OrderBy(o => o.OrderDate)
                    };
                }

                var totalCount = orders.Count();
                var pagedOrders = orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var viewModel = new OrderListViewModel
                {
                    Orders = pagedOrders.Select(o => new OrderViewModel
                    {
                        Id = o.Id,
                        OrderNumber = o.Id.ToString(),
                        CustomerName = o.User?.CustomerProfile != null
                            ? $"{o.User.CustomerProfile.FirstName} {o.User.CustomerProfile.LastName}".Trim()
                            : o.User?.UserName ?? "N/A",
                        CustomerPhone = o.User?.CustomerProfile?.PhoneNumber ?? "N/A",
                        RestaurantName = o.Restaurant?.Name ?? "N/A",
                        RestaurantPhone = o.Restaurant?.PhoneNumber ?? "N/A",
                        Status = o.Status,
                        OrderDate = o.OrderDate,
                        DeliveryDate = o.DeliveryDate,
                        EstimatedDeliveryTime = o.EstimatedDeliveryTime,
                        DeliveryAddress = o.Address != null
                            ? $"{o.Address.Street}, {o.Address.City}, {o.Address.State} {o.Address.PostalCode}"
                            : "N/A",
                        PaymentMethod = o.PaymentDetails ?? "N/A",
                        PaymentStatus = o.Payment?.Status ?? PaymentStatus.Pending,
                        Subtotal = o.Subtotal,
                        Tax = o.Tax,
                        DeliveryFee = o.DeliveryFee,
                        Discount = o.Discount,
                        Total = o.Total,
                        DeliveryInstructions = o.DeliveryInstructions,
                        SpecialRequests = o.SpecialInstructions,
                        TrackingUrl = o.TrackingUrl,
                        Items = o.OrderItems.Select(oi => new OrderItemViewModel
                        {
                            Id = oi.Id,
                            MenuItemId = oi.MenuItemId,
                            Name = oi.MenuItem?.Name ?? "N/A",
                            ImageUrl = oi.MenuItem?.ImageUrl ?? "/images/placeholder-dish.jpg",
                            Price = oi.Price,
                            Quantity = oi.Quantity,
                            SpecialInstructions = oi.SpecialInstructions,
                            Customizations = oi.Customizations.Select(oc => new OrderCustomizationViewModel
                            {
                                Id = oc.Id,
                                OptionId = oc.OptionId,
                                ChoiceId = oc.ChoiceId,
                                Name = oc.CustomizationOption?.Name ?? "N/A",
                                Choice = oc.CustomizationChoice?.Name ?? "N/A",
                                Price = oc.Price
                            }).ToList()
                        }).ToList()
                    }).ToList(),
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    SortBy = sortBy,
                    SortOrder = sortOrder
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order history for user {UserId}", User.Identity?.Name);
                TempData["ErrorMessage"] = "An error occurred while loading your order history.";
                return View(new OrderListViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);
                if (order.UserId != user.Id && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                if (order.Status != OrderStatus.Placed)
                {
                    TempData["ErrorMessage"] = "Only orders in 'Placed' status can be cancelled.";
                    return RedirectToAction("Details", new { id });
                }

                if (order.Payment != null && order.Payment.Status == PaymentStatus.Paid)
                {
                    var refundResult = await _paymentService.ProcessRefund(order.Payment.TransactionId, order.Total, $"Order {id} cancelled");
                    if (!refundResult.Success)
                    {
                        TempData["ErrorMessage"] = $"Could not process refund: {refundResult.ErrorMessage}";
                        return RedirectToAction("Details", new { id });
                    }
                    order.Payment.Status = PaymentStatus.Refunded;
                }

                order.Status = OrderStatus.Canceled;
                await _orderRepository.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = "Order cancelled successfully.";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId} for user {UserId}", id, User.Identity?.Name);
                TempData["ErrorMessage"] = "An error occurred while cancelling the order.";
                return RedirectToAction("Details", new { id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Track(int orderId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);

                if (order == null || order.UserId != user.Id)
                {
                    TempData["ErrorMessage"] = "Order not found or you do not have permission to view it.";
                    return RedirectToAction("Index", "Home");
                }

                var viewModel = new OrderTrackViewModel
                {
                    OrderId = order.Id,
                    Status = order.Status,
                    EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                    DeliveryAddress = new AddressViewModel
                    {
                        Id = order.Address?.Id ?? 0,
                    },
                    Items = order.OrderItems.Select(item => new OrderItemViewModel
                    {
                        Id = item.Id,
                        MenuItemId = item.MenuItemId,
                        Name = item.MenuItem?.Name ?? "N/A",
                        Quantity = item.Quantity,
                        Price = item.Price,
                        ImageUrl = item.MenuItem?.ImageUrl ?? "/images/placeholder-dish.jpg",
                        Customizations = item.Customizations.Select(c => new OrderCustomizationViewModel
                        {
                            Id = c.Id,
                            OptionId = c.OptionId,
                            ChoiceId = c.ChoiceId,
                            Name = c.CustomizationOption?.Name ?? "N/A",
                            Choice = c.CustomizationChoice?.Name ?? "N/A",
                            Price = c.Price
                        }).ToList()
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while tracking order with ID {OrderId}", orderId);
                TempData["ErrorMessage"] = "An error occurred while trying to track your order.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Manage(string status = "All", int? restaurantId = null, int page = 1, int pageSize = 10, string sortBy = "OrderDate", string sortOrder = "desc", string searchQuery = null)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Challenge();

                var query = await _orderRepository.GetAllAsync();
                if (User.IsInRole("Owner"))
                {
                    query = query.Where(o => o.Restaurant.OwnerId == user.Id);
                }

                if (status != "All")
                {
                    if (Enum.TryParse<OrderStatus>(status, out var statusEnum))
                    {
                        query = query.Where(o => o.Status == statusEnum);
                    }
                }

                if (restaurantId.HasValue)
                {
                    query = query.Where(o => o.RestaurantId == restaurantId.Value);
                }

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(o => o.Id.ToString().Contains(searchQuery) ||
                                            o.User.CustomerProfile.FirstName.Contains(searchQuery) ||
                                            o.User.CustomerProfile.LastName.Contains(searchQuery));
                }

                query = sortBy switch
                {
                    "Id" => sortOrder == "asc" ? query.OrderBy(o => o.Id) : query.OrderByDescending(o => o.Id),
                    "Total" => sortOrder == "asc" ? query.OrderBy(o => o.Total) : query.OrderByDescending(o => o.Total),
                    _ => sortOrder == "asc" ? query.OrderBy(o => o.OrderDate) : query.OrderByDescending(o => o.OrderDate)
                };

                var totalCount = query.Count();
                var orders = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var restaurantName = restaurantId.HasValue ? (await _restaurantRepository.GetByIdAsync(restaurantId.Value))?.Name : null;

                var viewModel = new OrderManagementListViewModel
                {
                    Orders = orders.Select(o => new OrderManagementViewModel
                    {
                        Id = o.Id,
                        OrderDate = o.OrderDate,
                        CustomerName = o.User?.CustomerProfile != null
                            ? $"{o.User.CustomerProfile.FirstName} {o.User.CustomerProfile.LastName}".Trim()
                            : o.User?.UserName ?? "N/A",
                        RestaurantCount = o.OrderItems.Select(oi => oi.RestaurantId).Distinct().Count(),
                        ItemsCount = o.OrderItems.Sum(oi => oi.Quantity),
                        Status = o.Status,
                        PaymentStatus = o.Payment?.Status ?? PaymentStatus.Pending,
                        Total = o.Total
                    }).ToList(),
                    TotalCount = totalCount,
                    CurrentPage = page,
                    PageSize = pageSize,
                    StatusFilter = status,
                    RestaurantId = restaurantId,
                    RestaurantName = restaurantName,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    SearchQuery = searchQuery
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order management for user {UserId}", User.Identity?.Name);
                TempData["ErrorMessage"] = "An error occurred while loading orders.";
                return View(new OrderManagementListViewModel());
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            try
            {
                var order = await _orderRepository.GetOrderWithDetailsAsync(id);
                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found.";
                    return RedirectToAction("Manage");
                }

                var user = await _userManager.GetUserAsync(User);
                if (User.IsInRole("Owner") && order.Restaurant?.OwnerId != user.Id)
                {
                    return Forbid();
                }

                if (!Enum.TryParse<OrderStatus>(status, out var newStatus))
                {
                    TempData["ErrorMessage"] = "Invalid status.";
                    return RedirectToAction("Manage");
                }

                // Validate status transition
                var validNextStatuses = newStatus switch
                {
                    OrderStatus.Confirmed => new[] { OrderStatus.Placed },
                    OrderStatus.InPreparation => new[] { OrderStatus.Confirmed },
                    OrderStatus.ReadyForPickup => new[] { OrderStatus.InPreparation },
                    OrderStatus.OutForDelivery => new[] { OrderStatus.ReadyForPickup },
                    OrderStatus.Delivered => new[] { OrderStatus.OutForDelivery },
                    OrderStatus.Canceled => new[] { OrderStatus.Placed, OrderStatus.Confirmed, OrderStatus.InPreparation, OrderStatus.ReadyForPickup, OrderStatus.OutForDelivery },
                    _ => Array.Empty<OrderStatus>()
                };

                if (!validNextStatuses.Contains(order.Status))
                {
                    TempData["ErrorMessage"] = $"Cannot change status from {order.Status} to {newStatus}.";
                    return RedirectToAction("Manage");
                }

                if (newStatus == OrderStatus.Canceled && order.Payment?.Status == PaymentStatus.Paid)
                {
                    var refundResult = await _paymentService.ProcessRefund(order.Payment.TransactionId, order.Total, $"Order {id} cancelled");
                    if (!refundResult.Success)
                    {
                        TempData["ErrorMessage"] = $"Could not process refund: {refundResult.ErrorMessage}";
                        return RedirectToAction("Manage");
                    }
                    order.Payment.Status = PaymentStatus.Refunded;
                }

                order.Status = newStatus;
                await _orderRepository.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Order #{id} updated to {newStatus}.";
                return RedirectToAction("Manage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for order {OrderId}", id);
                TempData["ErrorMessage"] = "An error occurred while updating the order status.";
                return RedirectToAction("Manage");
            }
        }
    }
}