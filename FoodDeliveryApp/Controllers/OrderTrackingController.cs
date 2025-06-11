using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.ViewModels.Order;
using FoodDeliveryApp.ViewModels.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Controllers
{
    /// <summary>
    /// Controller responsible for tracking food orders and providing real-time status updates
    /// </summary>
    [Authorize]
    [Route("order-tracking")]
    public class OrderTrackingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        // user service for managing addresses
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OrderTrackingController> _logger;
        private readonly IMemoryCache _cache;
        private const string OrderTrackingCacheKey = "OrderTracking_";

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderTrackingController"/> class.
        /// </summary>
        public OrderTrackingController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            IMemoryCache cache,
            ILogger<OrderTrackingController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // --- Private helpers for DRY and best practice ---
        /// <summary>
        /// Gets the current user's ID
        /// </summary>
        private string GetCurrentUserId() => _currentUserService.GetCurrentUserId();
        
        /// <summary>
        /// Gets the current user
        /// </summary>
        private ApplicationUser GetCurrentUser() => _currentUserService.GetCurrentUser();
        
        /// <summary>
        /// Checks if the current user is an admin
        /// </summary>
        private bool IsAdmin() => User.IsInRole("Admin");
        
        /// <summary>
        /// Checks if the current user is a driver
        /// </summary>
        private bool IsDriver() => User.IsInRole("Driver");
        
        /// <summary>
        /// Checks if the current user is the owner of the order
        /// </summary>
        private bool IsOwner(string userId) => GetCurrentUserId() == userId;

        /// <summary>
        /// Displays tracking information for a specific order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order tracking view</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Track(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid order ID requested for tracking: {OrderId}", id);
                return BadRequest("Invalid order ID.");
            }

            try
            {
                string cacheKey = $"{OrderTrackingCacheKey}{id}";
                if (!_cache.TryGetValue(cacheKey, out OrderTrackViewModel viewModel))
                {
                    var order = await _unitOfWork.Orders.GetByIdAsync(id);
                    if (order == null)
                    {
                        _logger.LogWarning("Order not found for tracking. ID: {OrderId}", id);
                        return NotFound("Order not found");
                    }

                    var currentUser = GetCurrentUser();
                    if (currentUser == null)
                    {
                        _logger.LogWarning("Unauthenticated user attempted to track order");
                        return Challenge();
                    }

                    if (!IsOwner(order.UserId) && !IsAdmin() && !IsDriver())
                    {
                        _logger.LogWarning("User {UserId} forbidden from tracking order {OrderId}", currentUser.Id, id);
                        return Forbid();
                    }

                    var trackingHistory = await _unitOfWork.OrderTracking.GetOrderTrackingHistoryAsync(id);
                    var latestTracking = trackingHistory.OrderByDescending(t => t.Timestamp).FirstOrDefault();

                    TimeSpan? remainingTime = null;
                    if (order.EstimatedDeliveryTime.HasValue && order.Status != OrderStatus.Delivered && order.Status != OrderStatus.Canceled)
                    {
                        remainingTime = order.EstimatedDeliveryTime.Value - DateTime.UtcNow;
                        if (remainingTime.Value.TotalMinutes < 0)
                        {
                            remainingTime = TimeSpan.Zero;
                        }
                    }

                    viewModel = new OrderTrackViewModel
                    {
                        OrderId = order.Id,
                        OrderNumber = order.OrderNumber,
                        OrderDate = order.OrderDate,
                        Status = order.Status,
                        EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                        RemainingTime = remainingTime,
                        DeliveryAddress = new AddressViewModel
                        {
                            StreetAddress = order.DeliveryAddress?.StreetAddress,
                            City = order.DeliveryAddress?.City,
                            State = order.DeliveryAddress?.State,
                            PostalCode = order.DeliveryAddress?.PostalCode
                        },
                        Items = order.OrderItems.Select(oi => new OrderItemViewModel
                        {
                            MenuItemId = oi.MenuItemId,
                            Name = oi.MenuItem?.Name ?? "N/A",
                            ImageUrl = oi.MenuItem?.ImageUrl ?? "/images/placeholder-dish.jpg",
                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            Subtotal = oi.Price * oi.Quantity,
                            SpecialInstructions = oi.SpecialInstructions,
                            Status = oi.Status
                        }).ToList(),
                        LatestTracking = latestTracking != null ? new OrderTrackingViewModel
                        {
                            OrderId = latestTracking.OrderId,
                            Status = latestTracking.Status,
                            Description = latestTracking.Status.ToString(),
                            Timestamp = latestTracking.Timestamp,
                            Location = latestTracking.Location,
                            Latitude = latestTracking.Latitude,
                            Longitude = latestTracking.Longitude,
                            DriverId = latestTracking.Driver?.Id.ToString(),
                            DriverName = latestTracking.Driver?.User.FullName,
                            DriverPhone = latestTracking.Driver?.User.PhoneNumber,
                            DriverPhotoUrl = latestTracking.Driver?.User.ProfilePictureUrl,
                            IsDelayed = latestTracking.IsDelayed,
                            DelayReason = latestTracking.DelayReason
                        } : null,
                        TrackingHistory = trackingHistory.OrderByDescending(t => t.Timestamp)
                            .Select(t => new OrderTrackingViewModel
                            {
                                OrderId = t.Id,
                                Status = t.Status,
                                Description = t.Status.ToString(),
                                Timestamp = t.Timestamp,
                                Location = t.Location,
                                Latitude = t.Latitude,
                                Longitude = t.Longitude,
                                DriverId = t.Driver?.Id.ToString(),
                                DriverName = t.Driver?.User.FullName,
                                DriverPhone = t.Driver?.User.PhoneNumber,
                                DriverPhotoUrl = t.Driver?.User.ProfilePictureUrl,
                                IsDelayed = t.IsDelayed,
                                DelayReason = t.DelayReason
                            })
                            .ToList(),
                        ShowMap = order.Status == OrderStatus.OutForDelivery || order.Status == OrderStatus.ReadyForPickup,
                        IsDelivered = order.Status == OrderStatus.Delivered,
                        IsCancelled = order.Status == OrderStatus.Canceled
                    };

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    
                    _cache.Set(cacheKey, viewModel, cacheOptions);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tracking information for order {OrderId}", id);
                TempData["Error"] = "An error occurred while retrieving order tracking information.";
                return RedirectToAction("Index", "Order");
            }
        }
        
        /// <summary>
        /// Gets the CSS class for the status badge
        /// </summary>
        private string GetStatusBadgeClass(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "badge bg-warning",
                OrderStatus.Confirmed => "badge bg-info",
                OrderStatus.InPreparation => "badge bg-primary",
                OrderStatus.ReadyForPickup => "badge bg-secondary",
                OrderStatus.OutForDelivery => "badge bg-info",
                OrderStatus.Delivered => "badge bg-success",
                OrderStatus.Canceled => "badge bg-danger",
                _ => "badge bg-secondary"
            };
        }

        /// <summary>
        /// Gets the latest status of an order for real-time updates
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>JSON with latest status information</returns>
        [HttpGet("{id:int}/status")]
        public async Task<IActionResult> GetLatestStatus(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid order ID requested for status update: {OrderId}", id);
                return BadRequest("Invalid order ID.");
            }

            try
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Order not found for status update. ID: {OrderId}", id);
                    return NotFound("Order not found");
                }

                var currentUser = GetCurrentUser();
                if (currentUser == null)
                {
                    _logger.LogWarning("Unauthenticated user attempted to get order status");
                    return Challenge();
                }

                if (!IsOwner(order.UserId) && !IsAdmin() && !IsDriver())
                {
                    _logger.LogWarning("User {UserId} forbidden from getting status of order {OrderId}", currentUser.Id, id);
                    return Forbid();
                }

                var latestTracking = await _unitOfWork.OrderTracking.GetLatestTrackingAsync(id);

                TimeSpan? remainingTime = null;
                string remainingTimeFormatted = null;
                if (order.EstimatedDeliveryTime.HasValue && order.Status != OrderStatus.Delivered && order.Status != OrderStatus.Canceled)
                {
                    remainingTime = order.EstimatedDeliveryTime.Value - DateTime.UtcNow;
                    if (remainingTime.Value.TotalMinutes > 0)
                    {
                        remainingTimeFormatted = $"{remainingTime.Value.Hours}h {remainingTime.Value.Minutes}m";
                    }
                    else
                    {
                        remainingTimeFormatted = "Arriving soon";
                    }
                }

                return Json(new
                {
                    status = latestTracking.Status.ToString(),
                    statusClass = GetStatusBadgeClass(latestTracking.Status),
                    driverLocation = latestTracking.Location ?? "N/A",
                    driverName = latestTracking.Driver?.User.FullName ?? "N/A",
                    driverPhone = latestTracking.Driver?.User.PhoneNumber ?? "N/A",
                    driverImageUrl = latestTracking.Driver?.User.ProfilePictureUrl ?? "/images/default-avatar.png",
                    estimatedDeliveryTime = order.EstimatedDeliveryTime?.ToShortTimeString(),
                    remainingTime = remainingTimeFormatted,
                    lastUpdated = latestTracking.Timestamp.ToString("g"),
                    notes = latestTracking.Notes ?? string.Empty,
                    isDelivered = order.Status == OrderStatus.Delivered,
                    isCancelled = order.Status == OrderStatus.Canceled
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest tracking status for order {OrderId}", id);
                return StatusCode(500, new { error = "An error occurred while retrieving tracking status." });
            }
        }
        /// <summary>
        /// Updates the driver's location for an order (Driver only)
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="model">Location update data</param>
        /// <returns>JSON with update result</returns>
        [HttpPost("{id:int}/update-location")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> UpdateDriverLocation(int id, [FromBody] UpdateDriverLocationViewModel model)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid order ID for location update: {OrderId}", id);
                return BadRequest("Invalid order ID.");
            }

            try
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Order not found for location update. ID: {OrderId}", id);
                    return NotFound("Order not found");
                }

                var currentUser = GetCurrentUser();
                if (currentUser == null)
                {
                    _logger.LogWarning("Unauthenticated user attempted to update location");
                    return Challenge();
                }

                if (!IsDriver())
                {
                    _logger.LogWarning("Non-driver user {UserId} attempted to update location for order {OrderId}", currentUser.Id, id);
                    return Forbid();
                }

                var tracking = new OrderTracking
                {
                    OrderId = id,
                    DriverId = int.Parse(currentUser.Id),
                    Status = order.Status,
                    Location = model.Location,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Timestamp = DateTime.UtcNow,
                    UpdatedBy = currentUser.UserName
                };

                await _unitOfWork.OrderTracking.AddTrackingUpdateAsync(tracking);
                await _unitOfWork.SaveChangesAsync();

                _cache.Remove($"{OrderTrackingCacheKey}{id}");

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating driver location for order {OrderId}", id);
                return StatusCode(500, new { error = "An error occurred while updating location." });
            }
        }
    }
    
    /// <summary>
    /// View model for updating driver location
    /// </summary>
    public class UpdateDriverLocationViewModel
    {
        /// <summary>
        /// Latitude coordinate
        /// </summary>
        [Required]
        public double Latitude { get; set; }
        
        /// <summary>
        /// Longitude coordinate
        /// </summary>
        [Required]
        public double Longitude { get; set; }
        
        /// <summary>
        /// Current address (optional)
        /// </summary>
        public string Location { get; set; }
    }
}
