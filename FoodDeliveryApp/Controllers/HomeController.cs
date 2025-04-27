using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace FoodDeliveryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRepository<RestaurantCategory> _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IAddressRepository _addressRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IRestaurantRepository restaurantRepository,
            IRepository<RestaurantCategory> categoryRepository,
            IWebHostEnvironment webHostEnvironment,
            IUnitOfWork unitOfWork,
            IEmailSender emailSender,
            IMenuItemRepository menuItemRepository,
            IPromotionRepository promotionRepository,
            IAddressRepository addressRepository)
        {
            _logger = logger;
            _restaurantRepository = restaurantRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _menuItemRepository = menuItemRepository;
            _promotionRepository = promotionRepository;
            _addressRepository = addressRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var topRatedRestaurants = await _restaurantRepository.GetTopRatedRestaurantsAsync(5);
                var popularDishes = await _menuItemRepository.GetPopularDishesAsync(10);
                var promotions = await _promotionRepository.GetAllAsync();

                string currentLocation = "Cairo";
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    var addresses = await _addressRepository.GetUserAddressesAsync(userId);
                    currentLocation = addresses.FirstOrDefault()?.City ?? currentLocation;
                }

                var model = new HomeViewModel
                {
                    Categories = categories.Select(c => new CategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ImageUrl = c.ImageUrl ?? "/images/placeholder-category.jpg",
                        Description = c.Description
                    }).ToList(),
                    PopularRestaurants = topRatedRestaurants.Select(r => new RestaurantHomeViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ImageUrl = r.ImageUrl ?? "/images/placeholder-restaurant.jpg",
                        CategoryId = r.CategoryId,
                        Category = r.Category?.Name ?? "Unknown",
                        Rating = r.Rating,
                        DeliveryTime = "30-45 min",
                        Distance = r.Address != null ? CalculateDistance(r.Address) : 0.0,
                        PriceLevel = r.DeliveryFee > 10 ? 3 : r.DeliveryFee > 5 ? 2 : 1
                    }).ToList(),
                    PopularDishes = popularDishes.Select(d => new DishViewModel
                    {
                        Id = d.Id,
                        Name = d.Name,
                        ImageUrl = d.ImageUrl ?? "/images/placeholder-dish.jpg",
                        RestaurantId = d.RestaurantId,
                        RestaurantName = d.Restaurant?.Name ?? "Unknown",
                        Price = d.Price,
                        Description = d.Description
                    }).ToList(),
                    Testimonials = new List<TestimonialViewModel>
                    {
                        new TestimonialViewModel
                        {
                            Quote = "Great food and fast delivery!",
                            Name = "John Doe",
                            Location = "New York",
                            AvatarUrl = "/images/avatar1.jpg"
                        },
                        new TestimonialViewModel
                        {
                            Quote = "I love the variety of options available.",
                            Name = "Jane Smith",
                            Location = "Los Angeles",
                            AvatarUrl = "/images/avatar2.jpg"
                        }
                    },
                    Offers = promotions.Select(o => new OfferViewModel
                    {
                        Id = o.Id,
                        Title = o.Code,
                        Description = o.Description,
                        ValidUntil = o.EndDate,
                        RestaurantName = o.Restaurant?.Name ?? "Unknown"
                    }).ToList(),
                    Faqs = new List<FaqViewModel>
                    {
                        new FaqViewModel
                        {
                            Question = "How do I place an order?",
                            Answer = "Select your items and proceed to checkout."
                        },
                        new FaqViewModel
                        {
                            Question = "What payment methods are accepted?",
                            Answer = "We accept credit cards, PayPal, and cash on delivery."
                        },
                        new FaqViewModel
                        {
                            Question = "Can I track my order?",
                            Answer = "Yes, you can track your order in real-time."
                        },

                    },
                    Search = new SearchViewModel
                    {
                        Query = string.Empty,
                        Location = currentLocation
                    },
                    CurrentLocation = currentLocation,
                    Email = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading home page for user {UserId}", User.Identity.Name);
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please provide a valid search query.";
                return RedirectToAction("Index");
            }

            try
            {
                // Redirect to Restaurants controller with search parameters
                return RedirectToAction("Index", "Restaurants", new
                {
                    query = model.Query,
                    location = model.Location
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing search for query {Query}", model.Query);
                TempData["ErrorMessage"] = "An error occurred while processing your search.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLocation(double latitude, double longitude)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    var existingAddress = await _addressRepository.GetUserAddressesAsync(userId);
                    var address = new Address
                    {
                        City = "New York", // Placeholder, should be replaced with actual city from geolocation
                        State = "NY", // Placeholder, should be replaced with actual state from geolocation
                        Country = "USA" // Placeholder, should be replaced with actual country from geolocation
                    };
                    await _addressRepository.AddAsync(address);
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "User not authenticated" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating location for user {UserId}", User.Identity.Name);
                return Json(new { success = false, message = "Error updating location" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(string email)
        {
            try
            {
                // TODO: Save email to a newsletter subscription database or service
                await _emailSender.SendEmailAsync(email, "Welcome to FoodFast Pro!", "Thank you for subscribing to our newsletter!");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing email {Email}", email);
                return Json(new { success = false, message = "Error subscribing to newsletter" });
            }
        }

        // Placeholder for distance calculation
        private double CalculateDistance(string address)
        {
            // TODO: Implement geolocation service (e.g., Google Maps API) to calculate distance
            // Example: Use address and user's current location (from model.CurrentLocation or geolocation)
            return 0.0;
        }

        [Route("/Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("/AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("/Help")]
        public IActionResult Help()
        {
            return View();
        }

        [Route("/Terms")]
        public IActionResult Terms()
        {
            return View();
        }

        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}
