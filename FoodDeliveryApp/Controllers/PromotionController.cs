using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Promotion;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services.Interfaces;

namespace FoodDeliveryApp.Controllers
{
    public class PromotionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<PromotionController> _logger;

        public PromotionController(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            ILogger<PromotionController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index(PromotionListViewModel model)
        {
            try
            {
                IEnumerable<Promotion> promotions;
                if (model.RestaurantId.HasValue)
                {
                    promotions = await _unitOfWork.Promotions.GetByRestaurantIdAsync(model.RestaurantId.Value);
                }
                else
                {
                    promotions = await _unitOfWork.Promotions.GetAllAsync();
                }

                var viewModel = new PromotionListViewModel
                {
                    Promotions = promotions.Select(p => new PromotionViewModel
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Description = p.Description,
                        StartDate = DateTime.UtcNow,
                        EndDate = p.ValidUntil,
                        RestaurantId = p.RestaurantId,
                        RestaurantName = p.Restaurant?.Name ?? "Unknown",
                        DiscountValue = p.DiscountAmount,
                        IsPercentage = p.IsPercentage,
                        MinimumOrderAmount = p.MinimumOrderAmount,
                        UsageLimit = null,
                        UsageCount = 0
                    }).ToList(),
                    ActiveOnly = model.ActiveOnly,
                    CurrentOnly = model.CurrentOnly,
                    RestaurantId = model.RestaurantId
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving promotions");
                TempData["Error"] = "Failed to retrieve promotions. Please try again.";
                return View(new PromotionListViewModel());
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Create()
        {
            var restaurants = await _unitOfWork.Restaurants.GetAllAsync();
            var restaurantList = restaurants.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();

            // Add "All Restaurants" option with value 0
            restaurantList.Insert(0, new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = "0",
                Text = "All Restaurants"
            });

            ViewBag.Restaurants = restaurantList;

            var model = new PromotionCreateViewModel
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30)
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromotionCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var promotion = new Promotion
                {
                    Title = viewModel.Code,
                    Code = viewModel.Code,
                    Description = viewModel.Description,
                    DiscountAmount = viewModel.DiscountValue,
                    MinimumOrderAmount = viewModel.MinimumOrderAmount,
                    IsPercentage = viewModel.IsPercentage,
                    ValidUntil = viewModel.EndDate,
                    IsActive = true,
                    RestaurantId = viewModel.RestaurantId ?? 0
                };

                await _unitOfWork.Promotions.AddAsync(promotion);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Promotion created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating promotion");
                ModelState.AddModelError("", "Failed to create promotion. Please try again.");
                return View(viewModel);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var promotion = await _unitOfWork.Promotions.GetByIdAsync(id);
                if (promotion == null)
                {
                    return NotFound();
                }

                var restaurants = await _unitOfWork.Restaurants.GetAllAsync();
                var restaurantList = restaurants.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                }).ToList();

                // Add "All Restaurants" option with value 0
                restaurantList.Insert(0, new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = "0",
                    Text = "All Restaurants"
                });

                ViewBag.Restaurants = restaurantList;

                var model = new PromotionEditViewModel
                {
                    Id = promotion.Id,
                    Code = promotion.Code,
                    Description = promotion.Description,
                    StartDate = DateTime.UtcNow,
                    EndDate = promotion.ValidUntil,
                    RestaurantId = promotion.RestaurantId,
                    DiscountValue = promotion.DiscountAmount,
                    IsPercentage = promotion.IsPercentage,
                    MinimumOrderAmount = promotion.MinimumOrderAmount
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving promotion for edit");
                TempData["Error"] = "Failed to retrieve promotion. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PromotionEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var promotion = await _unitOfWork.Promotions.GetByIdAsync(viewModel.Id);
                if (promotion == null)
                {
                    return NotFound();
                }

                promotion.Code = viewModel.Code;
                promotion.Description = viewModel.Description;
                promotion.DiscountAmount = viewModel.DiscountValue;
                promotion.MinimumOrderAmount = viewModel.MinimumOrderAmount;
                promotion.IsPercentage = viewModel.IsPercentage;
                promotion.ValidUntil = viewModel.EndDate;
                promotion.RestaurantId = viewModel.RestaurantId ?? 0;


                await _unitOfWork.Promotions.UpdateAsync(promotion);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Promotion updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating promotion");
                ModelState.AddModelError("", "Failed to update promotion. Please try again.");
                return View(viewModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var promotion = await _unitOfWork.Promotions.GetByIdAsync(id);
                if (promotion == null)
                {
                    return NotFound();
                }

                await _unitOfWork.Promotions.DeleteAsync(promotion);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Promotion deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting promotion");
                TempData["Error"] = "Failed to delete promotion. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var promotion = await _unitOfWork.Promotions.GetByIdAsync(id);
                if (promotion == null)
                {
                    return NotFound();
                }

                promotion.IsActive = !promotion.IsActive;
                await _unitOfWork.Promotions.UpdateAsync(promotion);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = $"Promotion {(promotion.IsActive ? "activated" : "deactivated")} successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling promotion status");
                TempData["Error"] = "Failed to update promotion status. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Validate(string code, decimal orderTotal)
        {
            try
            {
                var promotion = await _unitOfWork.Promotions.GetByCodeAsync(code);
                if (promotion == null)
                {
                    return Json(new { isValid = false, message = "Invalid promotion code." });
                }

                if (!promotion.IsActive)
                {
                    return Json(new { isValid = false, message = "This promotion is no longer active." });
                }

                if (promotion.ValidUntil < DateTime.UtcNow)
                {
                    return Json(new { isValid = false, message = "This promotion has expired." });
                }

                if (orderTotal < promotion.MinimumOrderAmount)
                {
                    return Json(new { isValid = false, message = $"Minimum order amount for this promotion is {promotion.MinimumOrderAmount:C}." });
                }

                var discountAmount = promotion.CalculateDiscountAmount(orderTotal);

                return Json(new
                {
                    isValid = true,
                    discountAmount = discountAmount,
                    finalTotal = orderTotal - discountAmount,
                    message = "Promotion applied successfully!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating promotion");
                return Json(new { isValid = false, message = "An error occurred while validating the promotion." });
            }
        }

        // Apply promotion to selected cart items
        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        [Route("apply-to-cart")]
        public async Task<IActionResult> ApplyToCart(string promotionCode)
        {
            try
            {
                var userId = _currentUser.GetCurrentUserId();
                if (userId == null) return Challenge();

                var promotion = await _unitOfWork.Promotions.GetByCodeAsync(promotionCode);
                if (promotion == null)
                {
                    TempData["Error"] = "Invalid promotion code.";
                    return RedirectToAction("Index", "Cart");
                }

                var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
                if (cart == null)
                {
                    TempData["Error"] = "Your cart is empty.";
                    return RedirectToAction("Index", "Cart");
                }

                var uniqeRestaurants =
                    cart.Items
                    .Select(x => x.Restaurant)
                    .Distinct()
                    .ToList();

                bool isValid = false;
                string errorMessage = string.Empty;
                decimal discountAmount = 0;
                decimal totalAmount = cart.Items.Sum(x => x.Subtotal);

                if (uniqeRestaurants.Count > 1)
                {
                    foreach (var restaurant in uniqeRestaurants)
                    {
                        var cartItems = cart.Items.Where(x => x.RestaurantId == restaurant.Id).ToList();
                        (bool valid, string message) = await _unitOfWork.Promotions.IsPromotionValidAsync(promotionCode, cartItems.Sum(x => x.Subtotal), restaurant.Id);
                        if (!valid)
                        {
                            isValid = false;
                            errorMessage = message;
                            break;
                        }
                        discountAmount += promotion.CalculateDiscountAmount(cartItems.Sum(x => x.Subtotal));
                    }
                }
                else if (uniqeRestaurants.Count == 1)
                {
                    (isValid, errorMessage) = await _unitOfWork.Promotions.IsPromotionValidAsync(promotionCode, totalAmount, uniqeRestaurants[0].Id);
                    discountAmount = promotion.CalculateDiscountAmount(totalAmount);
                }
                else
                {
                    (isValid, errorMessage) = await _unitOfWork.Promotions.IsPromotionValidAsync(promotionCode, totalAmount);
                    discountAmount = promotion.CalculateDiscountAmount(totalAmount);
                }

                if (!isValid)
                {
                    TempData["Error"] = errorMessage;
                    return RedirectToAction("Index", "Cart");
                }

                cart.PromotionCode = promotionCode;
                cart.IsPromotionApplied = true;
                cart.PromotionCodeExpiration = promotion.ValidUntil;
                cart.PromotionDiscountAmount = discountAmount;
                cart.TotalWithDiscount = cart.Items.Sum(x => x.Subtotal) - discountAmount;

                await _unitOfWork.Carts.UpdateAsync(cart);
                await _unitOfWork.SaveChangesAsync();
                TempData["Success"] = "Promotion applied successfully!";
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving promotion for cart");
                TempData["Error"] = "Failed to retrieve promotion. Please try again.";
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}
