using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.Restaurant;
using FoodDeliveryApp.ViewModels.MenuItem;
using FoodDeliveryApp.ViewModels.Review;
using FoodDeliveryApp.ViewModels.Promotion;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using FoodDeliveryApp.Services.Interfaces;

namespace FoodDeliveryApp.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RestaurantController> _logger;

        public RestaurantController(
            IUnitOfWork unitOfWork,
            IFileService fileService,
            UserManager<ApplicationUser> userManager,
            ILogger<RestaurantController> logger,
            IRestaurantRepository @object)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("Restaurants")]
        public async Task<IActionResult> Index(string searchTerm = "", int? categoryId = null, string sortBy = "name", int pageNumber = 1)
        {
            try
            {
                var pageSize = 12;
                var restaurants = await _unitOfWork.Restaurants.GetAllAsync();

                // Apply search filter
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    restaurants = restaurants.Where(r =>
                        r.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        (r.Description != null && r.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                // Apply category filter
                if (categoryId.HasValue)
                {
                    restaurants = restaurants.Where(r => r.Categories != null && r.Categories.Any(c => c.Id == categoryId.Value)).ToList();
                }

                // Apply sorting
                restaurants = sortBy.ToLower() switch
                {
                    "rating" => restaurants.OrderByDescending(r => r.Reviews != null && r.Reviews.Any() ? r.Reviews.Average(review => review.Rating) : 0).ToList(),
                    "deliverytime" => restaurants.OrderBy(r => r.DeliveryTime).ToList(),
                    _ => restaurants.OrderBy(r => r.Name).ToList()
                };

                // Calculate pagination
                var totalItems = restaurants.Count();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));
                var pagedRestaurants = restaurants
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var currentUser = await _userManager.GetUserAsync(User);
                var favorites = currentUser != null
                    ? await _unitOfWork.Favorites.GetUserFavoritesAsync(currentUser.Id)
                    : new List<MenuItem>();

                var viewModel = new RestaurantListViewModel
                {
                    Restaurants = pagedRestaurants.Select(r => new RestaurantViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        PhoneNumber = r.PhoneNumber ?? "",
                        ImageUrl = r.ImageUrl ?? "",
                        Rating = r.Reviews != null && r.Reviews.Any() ? (double)r.Reviews.Average(review => review.Rating) : 0,
                        ReviewCount = r.Reviews?.Count ?? 0,
                        DeliveryTime = r.DeliveryTime ?? "30-45 min",
                        Categories = r.Categories != null ? r.Categories.Select(c => c.Name).ToArray() : new string[0],
                        Website = r.WebsiteUrl ?? "",
                        DeliveryFee = r.DeliveryFee,
                        IsOpen = IsRestaurantOpen(r.OpeningTime, r.ClosingTime),
                        IsActive = r.IsActive,
                        OpeningTime = r.OpeningTime ?? TimeSpan.FromHours(10),
                        ClosingTime = r.ClosingTime ?? TimeSpan.FromHours(22),
                        Address = new RestaurantAddressViewModel
                        {
                            Street = r.Address ?? "",
                            City = r.City ?? "",
                            State = r.State ?? "",
                            PostalCode = r.PostalCode ?? ""
                        },
                        TaxRate = r.TaxRate,
                        IsAdminOrOwner = User.IsInRole("Admin") || r.OwnerId == currentUser?.Id
                    }),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    TotalItems = totalItems,
                    CurrentPage = pageNumber,
                    SearchTerm = searchTerm,
                    CategoryId = categoryId,
                    SortBy = sortBy
                };

                // Add categories to ViewBag for dropdown
                ViewBag.Categories = await _unitOfWork.MenuItemCategories.GetAllAsync();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving restaurants");
                TempData["Error"] = "An error occurred while retrieving restaurants.";
                return View(new RestaurantListViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(id);
                if (restaurant == null)
                {
                    _logger.LogWarning($"Restaurant with id {id} not found.");
                    return NotFound();
                }
                //var MenuItems = await _unitOfWork.MenuItems.GetByRestaurantIdAsync(id);
                
                // Get categories for the restaurant menu items
                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync(includes: m => m.MenuItems);
                var restaurantCategories = categories.Where(c => c.MenuItems.Any(m => m.CategoryId == c.Id)).ToList();
                var restaurantMenuItems = categories.SelectMany(c => c.MenuItems)
                    .Where(m => m.RestaurantId == id)
                    .ToList();

                // Fetch promotions for this restaurant
                var restaurantPromotions = await _unitOfWork.Promotions.GetByRestaurantIdAsync(id);

                // Fetch reviews for this restaurant
                var reviews = await _unitOfWork.Reviews.GetByRestaurantAsync(id);

                


                var viewModel = new RestaurantDetailViewModel
                {
                    Restaurant = new RestaurantViewModel
                    {
                        Id = restaurant.Id,
                        Name = restaurant.Name,
                        Description = restaurant.Description,
                        PhoneNumber = restaurant.PhoneNumber ?? "",
                        ImageUrl = restaurant.ImageUrl ?? "",
                        Rating = reviews != null && reviews.Any() ? (double)reviews.Average(review => review.Rating) : 0,
                        ReviewCount = reviews?.Count() ?? 0,
                        DeliveryTime = restaurant.DeliveryTime ?? "30-45 min",
                        Categories = restaurant.Categories != null ? restaurant.Categories.Select(c => c.Name).ToArray() : new string[0],
                        Website = restaurant.WebsiteUrl ?? "",
                        DeliveryFee = restaurant.DeliveryFee,
                        IsOpen = IsRestaurantOpen(restaurant.OpeningTime, restaurant.ClosingTime),
                        IsActive = restaurant.IsActive,
                        OpeningTime = restaurant.OpeningTime ?? TimeSpan.FromHours(10),
                        ClosingTime = restaurant.ClosingTime ?? TimeSpan.FromHours(22),
                        TaxRate = restaurant.TaxRate,
                        Address = new RestaurantAddressViewModel
                        {
                            Street = restaurant.Address ?? "",
                            City = restaurant.City ?? "",
                            State = restaurant.State ?? "",
                            PostalCode = restaurant.PostalCode ?? ""
                        },
                        RatingDistribution = reviews?.GroupBy(r => (int)r.Rating)
                            .ToDictionary(g => g.Key, g => g.Count()) ?? new Dictionary<int, int>()
                    },
                    MenuItems = restaurantMenuItems.Select(mi => new MenuItemViewModel
                    {
                        Id = mi.Id,
                        Name = mi.Name,
                        Description = mi.Description ?? "No description provided.",
                        Price = mi.Price,
                        ImageUrl = mi.ImageUrl ?? "",
                        IsAvailable = mi.IsAvailable,
                        CategoryId = mi.CategoryId,
                        CategoryName = mi.Category?.Name ?? "No category provided.",
                        RestaurantId = mi.RestaurantId,
                        Calories = mi.Calories,
                        SpiceLevel = mi.SpiceLevel,
                        IsFavorite = false,
                        IsVegetarian = mi.IsVegetarian,
                        IsVegan = mi.IsVegan,
                    }).ToList(),
                    Reviews = reviews.Select(r => new RestaurantReviewViewModel
                    {
                        Id = r.Id,
                        UserName = r.User?.UserName ?? "Anonymous",
                        UserImageUrl = r.User?.ImageUrl ?? "",
                        Rating = (double)r.Rating,
                        Content = r.Content ?? "No content provided.",
                        CreatedAt = r.CreatedAt,
                        IsVerified = false,
                        RestaurantId = r.RestaurantId ?? 0,
                        RestaurantName = restaurant.Name,
                        UserId = r.UserId,
                        Images = new List<string>()
                    }).ToList(),
                    Promotions = restaurantPromotions.Select(p => new PromotionViewModel
                    {
                        Id = p.Id,
                        Description = p.Description,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        Code = p.Code,
                        IsActive = p.IsActive,
                        RestaurantId = p.RestaurantId ?? 0,
                        UsageLimit = p.UsageLimit,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        IsPercentage = p.IsPercentage,
                        MinimumOrderAmount = p.MinimumOrderAmount,
                        ImageUrl = p.ImageUrl ?? "",
                        DiscountValue = p.DiscountValue,
                        UsageCount = p.UsageCount,
                        CreatedBy = p.CreatedBy,
                        RestaurantName = p.Restaurant?.Name ?? "Food Delivery App",
                        Notes = $"{p.Code} ({p.UsageCount}/{p.UsageLimit})",
                    }).ToList(),
                    Categories = restaurantCategories.Select(c => new RestaurantCategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        ImageUrl = c.ImageUrl,
                        ItemCount = restaurantMenuItems.Count,
                        MenuItems = restaurantMenuItems
                            .Select(m => new RestaurantMenuItemViewModel
                            {
                                Id = m.Id,
                                Name = m.Name,
                                Description = m.Description ?? "No description provided.",
                                Price = m.Price,
                                OriginalPrice = m.Price,
                                Discount = 0,
                                ImageUrl = m.ImageUrl ?? "",
                                IsAvailable = m.IsAvailable,
                                IsPopular = m.IsPopular(),
                                IsSpicy = m.IsSpicy(),
                                IsVegetarian = m.IsVegetarian,
                                IsVegan = m.IsVegan,
                                Rating = m.Rating,
                                ReviewCount = m.Reviews?.Count ?? 0,
                                CategoryId = m.CategoryId ?? 0,
                                CategoryName = m.Category?.Name ?? "Uncategorized",
                                RestaurantId = m.RestaurantId,
                                RestaurantName = m.Restaurant?.Name ?? string.Empty
                            }).ToList()
                    }).ToList(),
                    IsAdminOrOwner = User.IsInRole("Admin") || restaurant.OwnerId == _userManager.GetUserId(User)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return View(new RestaurantDetailViewModel());
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var categories = _unitOfWork.MenuItemCategories.GetAllAsync().Result;
                ViewBag.Categories = new SelectList(categories, "Id", "Name");

                var viewModel = new RestaurantCreateViewModel
                {
                    Address = new RestaurantAddressViewModel(),
                    OpeningTime = TimeSpan.FromHours(10),
                    ClosingTime = TimeSpan.FromHours(22),
                    DeliveryFee = 5,
                    TaxRate = 10,
                    DeliveryTime = "30 - 45 minutes"
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create restaurant form");
                TempData["Error"] = "An error occurred while loading the create form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RestaurantCreateViewModel viewModel, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                viewModel.Address ??= new RestaurantAddressViewModel();
                return View(viewModel);
            }

            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Unauthorized();
                }

                var restaurant = new Restaurant
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    PhoneNumber = viewModel.PhoneNumber,
                    DeliveryTime = viewModel.DeliveryTime,
                    WebsiteUrl = viewModel.Website,
                    LocationUrl = viewModel.LocationUrl,
                    DeliveryFee = viewModel.DeliveryFee,
                    IsActive = true,
                    OpeningTime = viewModel.OpeningTime,
                    ClosingTime = viewModel.ClosingTime,
                    Address = viewModel.Address?.Street ?? "",
                    City = viewModel.Address?.City ?? "",
                    State = viewModel.Address?.State ?? "",
                    PostalCode = viewModel.Address?.PostalCode ?? "",
                    TaxRate = viewModel.TaxRate,
                    OwnerId = currentUser.Id
                };

                if (imageFile != null)
                {
                    var (fileName, filePath) = await _fileService.SaveFileAsync(imageFile, "Restaurants");
                    restaurant.ImageUrl = filePath;
                }

                await _unitOfWork.Restaurants.AddAsync(restaurant);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Restaurant created successfully.";
                return RedirectToAction(nameof(Details), new { id = restaurant.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating restaurant");
                ModelState.AddModelError("", "An error occurred while creating the restaurant.");
                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                viewModel.Address ??= new RestaurantAddressViewModel();
                return View(viewModel);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var restaurant = await _unitOfWork.Restaurants.GetRestaurantByIdAsync(id.Value);
                if (restaurant == null)
                {
                    return NotFound();
                }

                // Check if user is authorized to edit this restaurant
                var currentUser = await _userManager.GetUserAsync(User);
                if (!User.IsInRole("Admin") && restaurant.OwnerId != currentUser.Id)
                {
                    return Forbid();
                }

                var viewModel = new RestaurantEditViewModel
                {
                    Id = restaurant.Id,
                    Name = restaurant.Name,
                    Description = restaurant.Description,
                    PhoneNumber = restaurant.PhoneNumber ?? "",
                    CurrentImageUrl = restaurant.ImageUrl ?? "",
                    EstimatedTime = restaurant.DeliveryTime ?? "30-45 min",
                    Website = restaurant.WebsiteUrl ?? "",
                    LocationUrl = restaurant.LocationUrl ?? "",
                    DeliveryFee = restaurant.DeliveryFee,
                    IsActive = restaurant.IsActive,
                    OpeningTime = restaurant.OpeningTime ?? TimeSpan.FromHours(10),
                    ClosingTime = restaurant.ClosingTime ?? TimeSpan.FromHours(22),
                    CategoryId = restaurant.CategoryId,
                    TaxRate = restaurant.TaxRate,

                    Address = new RestaurantAddressViewModel
                    {
                        Street = restaurant.Address ?? "",
                        City = restaurant.City ?? "",
                        State = restaurant.State ?? "",
                        Country = "Egypt",
                        PostalCode = restaurant.PostalCode ?? ""
                    }
                };

                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", viewModel.CategoryId);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading restaurant edit form for ID: {Id}", id);
                TempData["Error"] = "An error occurred while loading the edit form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RestaurantEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", viewModel.CategoryId);
                viewModel.Address ??= new RestaurantAddressViewModel();
                TempData["Error"] = "Invalid input.";
                return View(viewModel);
            }

            try
            {
                var restaurant = await _unitOfWork.Restaurants.GetRestaurantByIdAsync(id);
                if (restaurant == null)
                {
                    TempData["Error"] = "Restaurant not found.";
                    return NotFound();
                }

                // Check if user is authorized to edit this restaurant
                var currentUser = await _userManager.GetUserAsync(User);
                if (!User.IsInRole("Admin") && restaurant.OwnerId != currentUser?.Id)
                {
                    TempData["Error"] = "Unauthorized to edit this restaurant.";
                    return Forbid();
                }

                restaurant.Name = viewModel.Name;
                restaurant.Description = viewModel.Description;
                restaurant.PhoneNumber = viewModel.PhoneNumber;
                restaurant.DeliveryTime = viewModel.EstimatedTime;
                restaurant.WebsiteUrl = viewModel.Website;
                restaurant.DeliveryFee = viewModel.DeliveryFee;
                restaurant.IsActive = viewModel.IsActive;
                restaurant.OpeningTime = viewModel.OpeningTime;
                restaurant.ClosingTime = viewModel.ClosingTime;
                restaurant.Address = viewModel.Address?.Street ?? "";
                restaurant.City = viewModel.Address?.City ?? "";
                restaurant.State = viewModel.Address?.State ?? "";
                restaurant.PostalCode = viewModel.Address?.PostalCode ?? "";
                restaurant.TaxRate = viewModel.TaxRate;
                restaurant.LocationUrl = viewModel.LocationUrl;
                restaurant.CategoryId = viewModel.CategoryId;


                if (viewModel.NewImageFile != null)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(restaurant.ImageUrl))
                    {
                        await _fileService.DeleteFileAsync(restaurant.ImageUrl);
                    }
                    var (fileName, filePath) = await _fileService.SaveFileAsync(viewModel.NewImageFile, "Restaurants");
                    restaurant.ImageUrl = filePath;
                }

                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Restaurant updated successfully.";
                return RedirectToAction(nameof(Details), new { id = restaurant.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating restaurant with ID: {Id}", id);
                ModelState.AddModelError("", "An error occurred while updating the restaurant.");
                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                viewModel.Address ??= new RestaurantAddressViewModel();
                TempData["Error"] = "Invalid input . Please try again.";
                return View(viewModel);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var restaurant = await _unitOfWork.Restaurants.GetRestaurantByIdAsync(id.Value);
                if (restaurant == null)
                {
                    return NotFound();
                }

                // Check if user is authorized to delete this restaurant
                var currentUser = await _userManager.GetUserAsync(User);
                if (!User.IsInRole("Admin") && restaurant.OwnerId != currentUser?.Id)
                {
                    return Forbid();
                }

                return View(restaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading restaurant delete confirmation for ID: {Id}", id);
                TempData["Error"] = "An error occurred while loading the delete confirmation.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var restaurant = await _unitOfWork.Restaurants.GetRestaurantByIdAsync(id);
                if (restaurant == null)
                {
                    return NotFound();
                }

                // Check if user is authorized to delete this restaurant
                var currentUser = await _userManager.GetUserAsync(User);
                if (!User.IsInRole("Admin") && restaurant.OwnerId != currentUser.Id)
                {
                    return Forbid();
                }

                // Delete restaurant image if exists
                if (!string.IsNullOrEmpty(restaurant.ImageUrl))
                {
                    await _fileService.DeleteFileAsync(restaurant.ImageUrl);
                }

                await _unitOfWork.Restaurants.DeleteAsync(restaurant);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Restaurant deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting restaurant with ID: {Id}", id);
                TempData["Error"] = "An error occurred while deleting the restaurant.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool IsRestaurantOpen(TimeSpan? openingTime, TimeSpan? closingTime)
        {
            if (!openingTime.HasValue || !closingTime.HasValue)
                return true;

            var currentTime = DateTime.Now.TimeOfDay;

            // Handle cases where restaurant closes after midnight
            if (closingTime < openingTime)
            {
                return currentTime >= openingTime || currentTime <= closingTime;
            }

            return currentTime >= openingTime && currentTime <= closingTime;
        }
    }
}
