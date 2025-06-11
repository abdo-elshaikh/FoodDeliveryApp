using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels.Home;
using Newtonsoft.Json;

namespace FoodDeliveryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRestaurantService _restaurantService;
        private readonly ICategoryService _categoryService;
        private readonly IPromotionService _promotionService;
        private readonly IReviewService _reviewService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IUnitOfWork unitOfWork,
            IRestaurantService restaurantService,
            ICategoryService categoryService,
            IPromotionService promotionService,
            IReviewService reviewService,
            IMemoryCache cache,
            ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _restaurantService = restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _promotionService = promotionService ?? throw new ArgumentNullException(nameof(promotionService));
            _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            // Get user location from session or TempData
            var userLocationString = HttpContext.Session.GetString("UserLocation");
            var userLocation = userLocationString != null ? JsonConvert.DeserializeObject<UserLocationViewModel>(userLocationString) : null;
            if (userLocation == null)
            {
                var tempData = TempData["SearchLocation"];
                userLocation = tempData != null ? JsonConvert.DeserializeObject<UserLocationViewModel>(tempData.ToString()) : null;
            }

            // Normalize cache key to avoid empty or invalid keys
            var locationKey = string.IsNullOrWhiteSpace(userLocation?.Address) ? "default" : userLocation.Address.Replace(" ", "_").ToLowerInvariant();
            var cacheKey = $"home_page_data_{locationKey}";

            if (!_cache.TryGetValue(cacheKey, out HomeViewModel model))
            {
                _logger.LogInformation("Cache miss for home page data. Fetching fresh data.");

                try
                {
                    // Get featured restaurants based on rating and popularity
                    var featuredRestaurants = await _restaurantService.GetFeaturedRestaurantsAsync();
                    var popularCategories = await _categoryService.GetPopularCategoriesAsync();

                    // Limit promotions and testimonials to avoid large data sets
                    var promotions = (await _unitOfWork.Promotions.GetAllAsync()).Take(10).ToList();
                    var Reviews = (await _unitOfWork.Reviews.GetAllAsync()).Take(10).ToList();

                    model = new HomeViewModel
                    {
                        UserLocation = userLocation,
                        FeaturedRestaurants = featuredRestaurants?.Select(r => new RestaurantHomeViewModel
                        {
                            Id = r.Id,
                            Name = r.Name,
                            ImageUrl = r.ImageUrl,
                            Rating = r.Rating,
                            DeliveryTime = int.Parse(r.DeliveryTime?.Split('-')[0] ?? "30"),
                            DeliveryFee = r.DeliveryFee,
                            CuisineType = r.CategoryName
                        }).ToList() ?? new List<RestaurantHomeViewModel>(),
                        PopularCategories = popularCategories?.Select(c => new CategoryHomeViewModel
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Icon = c.ImageUrl,
                            RestaurantCount = c.RestaurantCount
                        }).ToList() ?? new List<CategoryHomeViewModel>(),
                        ActivePromotions = promotions?.Select(p => new PromotionHomeViewModel
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Description = p.Description,
                            Code = p.Code,
                            DiscountAmount = p.DiscountAmount,
                            DiscountType = p.DiscountType.ToString(),
                            ExpiryDate = p.ValidUntil,
                            IsActive = p.IsActive,
                            ImageUrl = p.ImageUrl ?? "",
                        }).ToList() ?? new List<PromotionHomeViewModel>(),
                        RecentReviews = Reviews?.Select(t => new ReviewHomeViewModel
                        {
                            Id = t.Id,
                            UserName = t.User?.UserName ?? "Anonymous",
                            UserAvatar = t.User?.ProfilePictureUrl ?? "",
                            Comment = t.Content ?? "N/A",
                            Rating = (int)t.Rating,
                            RestaurantName = t.Restaurant?.Name ?? "N/A",
                            CreatedAt = t.CreatedAt,
                            Images = new List<string>()
                        }).ToList() ?? new List<ReviewHomeViewModel>()
                    };

                    // Get additional data
                    model.TrendingRestaurants = (await _restaurantService.GetFeaturedRestaurantsAsync())?
                        .Select(r => new RestaurantHomeViewModel
                        {
                            Id = r.Id,
                            Name = r.Name,
                            ImageUrl = r.ImageUrl,
                            Rating = r.Rating,
                            DeliveryTime = int.Parse(r.DeliveryTime?.Split('-')[0] ?? "30"),
                            DeliveryFee = r.DeliveryFee,
                            CuisineType = r.CategoryName
                        }).ToList() ?? new List<RestaurantHomeViewModel>();

                    model.FeaturedCategories = (await _categoryService.GetPopularCategoriesAsync())?
                        .Select(c => new CategoryHomeViewModel
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Icon = c.ImageUrl,
                            RestaurantCount = c.RestaurantCount
                        }).ToList() ?? new List<CategoryHomeViewModel>();

                    // Cache the data for 15 minutes
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                        .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(cacheKey, model, cacheOptions);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error fetching home page data. Returning partial data.");

                    model = new HomeViewModel
                    {
                        UserLocation = userLocation,
                        FeaturedRestaurants = new List<RestaurantHomeViewModel>(),
                        PopularCategories = new List<CategoryHomeViewModel>(),
                        ActivePromotions = new List<PromotionHomeViewModel>(),
                        RecentReviews = new List<ReviewHomeViewModel>(),
                        TrendingRestaurants = new List<RestaurantHomeViewModel>(),
                        FeaturedCategories = new List<CategoryHomeViewModel>()
                    };
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult SearchLocation(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                TempData["Error"] = "Please enter a valid location.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Store the location in TempData for the next request
                TempData["SearchLocation"] = new UserLocationViewModel
                {
                    Address = location
                };
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing location search");
                TempData["Error"] = "An error occurred while processing your location. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLocation([FromBody] UserLocationViewModel location)
        {
            try
            {
                if (location == null)
                {
                    return Json(new { success = false, message = "Invalid location data" });
                }

                // Validate coordinates: allow 0 if valid, check for valid latitude and longitude ranges
                if (location.Latitude < -90 || location.Latitude > 90 ||
                    location.Longitude < -180 || location.Longitude > 180)
                {
                    return Json(new { success = false, message = "Invalid coordinates" });
                }
                // Store location in session
                var locationString = JsonConvert.SerializeObject(location);
                HttpContext.Session.SetString("UserLocation", locationString);

                return Json(new { success = true, location });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating location");
                return Json(new { success = false, message = "An error occurred while updating location" });
            }
        }

        [Route("/error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
