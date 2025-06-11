using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.ViewModels.Review;
using FoodDeliveryApp.Services.Interfaces;


namespace FoodDeliveryApp.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReviewController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public ReviewController(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILogger<ReviewController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var reviews = await _unitOfWork.Reviews.GetAllAsync();
                var restaurantReviews = reviews.Where(r => r.RestaurantId.HasValue).ToList();
                var menuItemReviews = reviews.Where(r => r.MenuItemId.HasValue).ToList();

                var restaurantReviewViewModels = restaurantReviews.Select(r => new RestaurantReviewViewModel
                {
                    Id = r.Id,
                    Content = r.Content ?? "N/A",
                    Rating = (double)r.Rating,
                    UserId = r.UserId,
                    RestaurantId = r.RestaurantId ?? 0,
                    RestaurantName = r.Restaurant?.Name ?? "Unknown",
                    UserName = r.User?.UserName ?? "Anonymous"
                }).ToList();

                var menuItemReviewViewModels = menuItemReviews.Select(r => new MenuItemReviewViewModel
                {
                    Id = r.Id,
                    Content = r.Content ?? "N/A",
                    Rating = (double)r.Rating,
                    UserId = r.UserId,
                    MenuItemId = r.MenuItemId ?? 0,
                    MenuItemName = r.MenuItem?.Name ?? "Unknown",
                    UserName = r.User?.UserName ?? "Anonymous"
                }).ToList();

                var combinedViewModel = new ReviewListViewModel
                {
                    restaurantReviewListViewModel = new RestaurantReviewListViewModel { Items = restaurantReviewViewModels },
                    menuItemReviewListViewModel = new MenuItemReviewListViewModel { Items = menuItemReviewViewModels }
                };

                return View(combinedViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reviews");
                TempData["Error"] = "Failed to retrieve reviews. Please try again.";
                return View(new ReviewListViewModel());
            }
        }

        [HttpGet]
        public IActionResult CreateRestaurantReview()
        {
            var model = new RestaurantReviewCreateViewModel();
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateMenuItemReview()
        {
            var model = new MenuItemReviewCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRestaurantReview(RestaurantReviewCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var review = new Review
                {
                    Content = viewModel.Content,
                    Rating = (decimal)viewModel.Rating,
                    RestaurantId = viewModel.RestaurantId,
                    UserId = _currentUserService.GetCurrentUserId()
                };

                await _unitOfWork.Reviews.AddAsync(review);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Review created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review");
                ModelState.AddModelError("", "Failed to create review. Please try again.");
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMenuItemReview(MenuItemReviewCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var review = new Review
                {
                    Content = viewModel.Content,
                    Rating = (decimal)viewModel.Rating,
                    MenuItemId = viewModel.MenuItemId,
                    UserId = _currentUserService.GetCurrentUserId()
                };

                await _unitOfWork.Reviews.AddAsync(review);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Menu item review created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu item review");
                ModelState.AddModelError("", "Failed to create menu item review. Please try again.");
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRestaurantReview(int id)
        {
            try
            {
                var review = await _unitOfWork.Reviews.GetByIdAsync(id);
                if (review == null)
                {
                    return NotFound();
                }

                var model = new RestaurantReviewUpdateViewModel
                {
                    Id = review.Id,
                    Content = review.Content,
                    Rating = (double)review.Rating,
                    RestaurantId = review.RestaurantId ?? 0
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving review for edit");
                TempData["Error"] = "Failed to retrieve review. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditMenuItemReview(int id)
        {
            try
            {
                var review = await _unitOfWork.Reviews.GetByIdAsync(id);
                if (review == null)
                {
                    return NotFound();
                }

                var model = new MenuItemReviewUpdateViewModel
                {
                    Id = review.Id,
                    Content = review.Content,
                    Rating = (double)review.Rating,
                    MenuItemId = review.MenuItemId ?? 0
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu item review for edit");
                TempData["Error"] = "Failed to retrieve menu item review. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRestaurantReview(RestaurantReviewUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var review = await _unitOfWork.Reviews.GetByIdAsync(viewModel.Id);
                if (review == null)
                {
                    return NotFound();
                }

                review.Content = viewModel.Content;
                review.Rating = (decimal)viewModel.Rating;
                review.RestaurantId = viewModel.RestaurantId;

                await _unitOfWork.Reviews.UpdateAsync(review);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Review updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review");
                ModelState.AddModelError("", "Failed to update review. Please try again.");
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMenuItemReview(MenuItemReviewUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var review = await _unitOfWork.Reviews.GetByIdAsync(viewModel.Id);
                if (review == null)
                {
                    return NotFound();
                }

                review.Content = viewModel.Content;
                review.Rating = (decimal)viewModel.Rating;
                review.MenuItemId = viewModel.MenuItemId;

                await _unitOfWork.Reviews.UpdateAsync(review);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Menu item review updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu item review");
                ModelState.AddModelError("", "Failed to update menu item review. Please try again.");
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var review = await _unitOfWork.Reviews.GetByIdAsync(id);
                if (review == null)
                {
                    return NotFound();
                }

                await _unitOfWork.Reviews.DeleteAsync(review);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Review deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review");
                TempData["Error"] = "Failed to delete review. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
