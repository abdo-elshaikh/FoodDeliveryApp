using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.Cart;
using FoodDeliveryApp.ViewModels.MenuItems;
using FoodDeliveryApp.ViewModels.RestaurantViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Diagnostics;

namespace FoodDeliveryApp.Controllers
{
    [Route("menu-items")]
    public class MenuItemsController : Controller
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartService _cartService;
        private readonly IRepository<RestaurantCategory> _categoryRepository;
        private readonly ILogger<MenuItemsController> _logger;

        public MenuItemsController(
            IMenuItemRepository menuItemRepository,
            IRestaurantRepository restaurantRepository,
            IOrderRepository orderRepository,
            ICartService cartService,
            UserManager<ApplicationUser> userManager,
            IRepository<RestaurantCategory> categoryRepository,
            ILogger<MenuItemsController> logger)
        {
            _menuItemRepository = menuItemRepository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
            _cartService = cartService;
            _userManager = userManager;
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        // GET: Index (Get All Items)
        [HttpGet]
        [Route("")] // Default route for menu items
        public async Task<IActionResult> Index(int? restaurantId , int? categoryId, int pageNumber = 1, int pageSize = 12)
        {
            try
            {
                IEnumerable<MenuItem> menuItems;
                if (categoryId != null)
                {
                    menuItems = await _menuItemRepository.GetByRestaurantCategoryAsync(categoryId.Value);
                }
                else if (restaurantId != null)
                {
                    menuItems = await _menuItemRepository.GetByRestaurantAsync(restaurantId.Value);
                }
                else
                {
                    menuItems = await _menuItemRepository.GetAllAsync();
                }

                if (menuItems == null || !menuItems.Any())
                {
                    _logger.LogWarning("No menu items found");
                    return NotFound("No menu items found.");
                }
                var categories = await _categoryRepository.GetAllAsync();
                var restaurants = await _restaurantRepository.GetAllAsync();
                var model = new MenuItemListViewModel
                {
                    MenuItems = menuItems.Select(m => new MenuItemViewModel
                    {
                        Id = m.Id,
                        RestaurantId = m.RestaurantId,
                        Name = m.Name ?? "Unnamed Item",
                        Description = m.Description ?? "",
                        Price = m.Price,
                        ImageUrl = m.ImageUrl ?? "/images/food-placeholder.jpg",
                        CreatedAt = m.CreatedAt,
                        UpdatedAt = m.UpdatedAt,
                        IsAvailable = m.IsAvailable
                    }).ToList(),
                    Categories = categories.Select(c => new MenuItemCategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name ?? "Unknown Category",
                        
                    }).ToList(),
                    Restaurants = restaurants.Select(r => new MenuItemRestaurantViewModel
                    {
                        Id = r.Id,
                        Name = r.Name ?? "Unknown Restaurant",
                        ImageUrl = r.ImageUrl ?? "/images/restaurant-placeholder.jpg",
                        Rating = r.Rating,
                        MenuItemCount = r.MenuItems.Count(m => m.IsAvailable),
                    }).ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = menuItems.Count()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading menu items");
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "An error occurred while loading menu items. Please try again later."
                });
            }
        }

        // GET: MenuItems By Restaurant
        [HttpGet]
        [Route("restaurants/{restaurantId}/menu-items")]
        public async Task<IActionResult> ByRestaurant(int restaurantId)
        {
            try
            {
                var menuItems = await _menuItemRepository.GetAvailableItemsByRestaurantAsync(restaurantId);
                if (menuItems == null || !menuItems.Any())
                {
                    _logger.LogWarning("No menu items found for restaurant ID {RestaurantId}", restaurantId);
                    return NotFound("No menu items found for this restaurant.");
                }

                var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);
                if (restaurant == null)
                {
                    _logger.LogWarning("Restaurant ID {RestaurantId} not found", restaurantId);
                    return NotFound("Restaurant not found.");
                }

                var categories = await _categoryRepository.GetAllAsync();

                var model = new MenuItemListViewModel
                {
                    MenuItems = menuItems.Select(m => new MenuItemViewModel
                    {
                        Id = m.Id,
                        RestaurantId = m.RestaurantId,
                        Name = m.Name ?? "Unnamed Item",
                        Description = m.Description ?? "",
                        Price = m.Price,
                        ImageUrl = m.ImageUrl ?? "/images/food-placeholder.jpg",
                        CreatedAt = m.CreatedAt,
                        UpdatedAt = m.UpdatedAt,
                        IsAvailable = m.IsAvailable
                    }).ToList(),
                    Categories = categories.Select(c => new MenuItemCategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name ?? "Unknown Category"
                    }).ToList(),
                    RestaurantId = restaurant.Id,
                    RestaurantName = restaurant.Name ?? "Unknown Restaurant",
                    RestaurantImageUrl = restaurant.ImageUrl ?? "/images/restaurant-placeholder.jpg"
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading menu items for restaurant ID {RestaurantId}", restaurantId);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "An error occurred while loading menu items. Please try again later."
                });
            }
        }

        // GET: MenuItem Details
        [HttpGet]
        [Route("menu-items/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item ID {MenuItemId} not found", id);
                    return NotFound("Menu item not found.");
                }

                var relatedItems = await _menuItemRepository.GetRelatedItemsAsync(menuItem.Id, menuItem.RestaurantId, 5);
                var customizationOptions = await _menuItemRepository.GetCustomizationOptionsAsync(menuItem.Id);
                var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(menuItem.RestaurantId);
                if (restaurant == null)
                {
                    _logger.LogWarning("Restaurant ID {RestaurantId} not found for menu item ID {MenuItemId}", menuItem.RestaurantId, id);
                    return NotFound("Restaurant not found.");
                }

                var model = new MenuItemDetailsViewModel
                {
                    MenuItem = new MenuItemViewModel
                    {
                        Id = menuItem.Id,
                        RestaurantId = menuItem.RestaurantId,
                        Name = menuItem.Name ?? "Unnamed Item",
                        Description = menuItem.Description ?? "",
                        Price = menuItem.Price,
                        ImageUrl = menuItem.ImageUrl ?? "/images/food-placeholder.jpg",
                        CreatedAt = menuItem.CreatedAt,
                        UpdatedAt = menuItem.UpdatedAt,
                        IsAvailable = menuItem.IsAvailable
                    },
                    RelatedItems = relatedItems.Select(r => new RelatedItemViewModel
                    {
                        Id = r.Id,
                        RestaurantId = r.RestaurantId,
                        Name = r.Name ?? "Unnamed Item",
                        Description = r.Description ?? "",
                        Price = r.Price,
                        ImageUrl = r.ImageUrl ?? "/images/food-placeholder.jpg"
                    }).ToList(),
                    CustomizationOptions = customizationOptions.Select(o => new CustomizationOptionViewModel
                    {
                        Id = o.Id,
                        Name = o.Name ?? "Option",
                        Choices = o.Choices.Select(c => new CustomizationChoiceViewModel
                        {
                            Id = c.Id,
                            Name = c.Name ?? "Choice",
                            Price = c.Price
                        }).ToList()
                    }).ToList(),
                    Restaurant = new RestaurantViewModel
                    {
                        Id = restaurant.Id,
                        Name = restaurant.Name ?? "Unknown Restaurant",
                        ImageUrl = restaurant.ImageUrl ?? "/images/restaurant-placeholder.jpg",
                        Address = restaurant.Address ?? "Address not available",
                        City = restaurant.City ?? "",
                        State = restaurant.State ?? "",
                        PostalCode = restaurant.PostalCode ?? "",
                        OpeningTime = restaurant.OpeningTime,
                        ClosingTime = restaurant.ClosingTime
                    }
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading details for menu item ID {MenuItemId}", id);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "An error occurred while loading menu item details. Please try again later."
                });
            }
        }

        // POST: Add to Cart
        [HttpPost]
        [Route("menu-items/{id}/add-to-cart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id, AddToCartViewModel model)
        {
            if (model.Quantity <= 0)
            {
                _logger.LogWarning("Invalid quantity {Quantity} for menu item ID {MenuItemId}", model.Quantity, id);
                TempData["ErrorMessage"] = "Quantity must be at least 1.";
                return RedirectToAction("Details", new { id });
            }

            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item ID {MenuItemId} not found", id);
                    TempData["ErrorMessage"] = "Menu item not found.";
                    return RedirectToAction("ByRestaurant", new { restaurantId = model.RestaurantId });
                }

                if (!menuItem.IsAvailable)
                {
                    _logger.LogWarning("Attempted to add unavailable item {MenuItemId}: {MenuItemName}", id, menuItem.Name);
                    TempData["ErrorMessage"] = $"{menuItem.Name} is currently unavailable.";
                    return RedirectToAction("Details", new { id });
                }

                // Validate customizations
                if (model.Customizations?.Any() == true)
                {
                    var validOptions = await _menuItemRepository.GetCustomizationOptionsAsync(id);
                    var validOptionIds = validOptions.Select(o => o.Id).ToHashSet();
                    var validChoices = validOptions
                        .SelectMany(o => o.Choices)
                        .ToDictionary(c => c.Id, c => c.Price);

                    foreach (var customization in model.Customizations)
                    {
                        if (!validOptionIds.Contains(customization.OptionId) || !validChoices.ContainsKey(customization.ChoiceId))
                        {
                            _logger.LogWarning("Invalid customization for item {MenuItemId}: OptionId={OptionId}, ChoiceId={ChoiceId}",
                                id, customization.OptionId, customization.ChoiceId);
                            TempData["ErrorMessage"] = "Invalid customization selected.";
                            return RedirectToAction("Details", new { id });
                        }
                        customization.Price = validChoices[customization.ChoiceId];
                    }
                }

                var cartItem = new CartItemViewModel
                {
                    MenuItemId = id,
                    Quantity = model.Quantity,
                    Name = menuItem.Name ?? "Unnamed Item",
                    ImageUrl = menuItem.ImageUrl ?? "/images/food-placeholder.jpg",
                    Price = menuItem.Price,
                    RestaurantId = menuItem.RestaurantId,
                    RestaurantName = menuItem.Restaurant?.Name ?? "Unknown Restaurant",
                    Customizations = model.Customizations ?? new List<CustomizationViewModel>()
                };

                var user = await _userManager.GetUserAsync(User);
                await _cartService.AddItemToCartAsync(user.Id, cartItem);
                _logger.LogInformation("Added {Quantity} of {MenuItemName} to cart for user {UserId}", model.Quantity, menuItem.Name, user.Id);

                TempData["SuccessMessage"] = $"{model.Quantity} {menuItem.Name} added to cart.";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding menu item ID {MenuItemId} to cart", id);
                TempData["ErrorMessage"] = "Failed to add item to cart.";
                return RedirectToAction("Details", new { id });
            }
        }

        // GET: Create MenuItem
        [HttpGet]
        [Route("restaurants/{restaurantId}/menu-items/create")]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> Create(int restaurantId)
        {
            try
            {
                var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);
                if (restaurant == null)
                {
                    _logger.LogWarning("Restaurant ID {RestaurantId} not found", restaurantId);
                    return NotFound("Restaurant not found.");
                }

                var model = new MenuItemCreateViewModel
                {
                    RestaurantId = restaurant.Id,
                    IsAvailable = true
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create page for restaurant ID {RestaurantId}", restaurantId);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "An error occurred while loading the create page. Please try again later."
                });
            }
        }

        // POST: Create MenuItem
        [HttpPost]
        [Route("restaurants/{restaurantId}/menu-items/create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> Create(int restaurantId, MenuItemCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating menu item for restaurant ID {RestaurantId}", restaurantId);
                return View(model);
            }

            try
            {
                var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);
                if (restaurant == null)
                {
                    _logger.LogWarning("Restaurant ID {RestaurantId} not found", restaurantId);
                    return NotFound("Restaurant not found.");
                }

                var menuItem = new MenuItem
                {
                    RestaurantId = restaurantId,
                    Name = model.Name?.Trim(),
                    Description = model.Description?.Trim(),
                    Price = model.Price,
                    ImageUrl = model.ImageUrl,
                    CreatedAt = DateTime.UtcNow,
                    IsAvailable = model.IsAvailable,
                    UpdatedAt = DateTime.UtcNow
                };

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    menuItem.ImageUrl = SaveImage(model.ImageFile);
                }

                await _menuItemRepository.AddAsync(menuItem);
                _logger.LogInformation("Created menu item {MenuItemName} for restaurant ID {RestaurantId}", menuItem.Name, restaurantId);
                TempData["SuccessMessage"] = "Menu item created successfully.";
                return RedirectToAction("ByRestaurant", new { restaurantId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu item for restaurant ID {RestaurantId}", restaurantId);
                ModelState.AddModelError("", "An error occurred while creating the menu item. Please try again.");
                return View(model);
            }
        }

        // GET: Edit MenuItem
        [HttpGet]
        [Route("menu-items/{id}/edit")]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item ID {MenuItemId} not found", id);
                    return NotFound("Menu item not found.");
                }

                var model = new MenuItemEditViewModel
                {
                    Id = menuItem.Id,
                    RestaurantId = menuItem.RestaurantId,
                    Name = menuItem.Name,
                    Description = menuItem.Description,
                    Price = menuItem.Price,
                    ImageUrl = menuItem.ImageUrl,
                    IsAvailable = menuItem.IsAvailable
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit page for menu item ID {MenuItemId}", id);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "An error occurred while loading the edit page. Please try again later."
                });
            }
        }

        // POST: Edit MenuItem
        [HttpPost]
        [Route("menu-items/{id}/edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> Edit(int id, MenuItemEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for editing menu item ID {MenuItemId}", id);
                return View(model);
            }

            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item ID {MenuItemId} not found", id);
                    return NotFound("Menu item not found.");
                }

                menuItem.Name = model.Name?.Trim();
                menuItem.Description = model.Description?.Trim();
                menuItem.Price = model.Price;
                menuItem.IsAvailable = model.IsAvailable;
                menuItem.UpdatedAt = DateTime.UtcNow;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    menuItem.ImageUrl = SaveImage(model.ImageFile);
                }

                await _menuItemRepository.UpdateAsync(menuItem);
                _logger.LogInformation("Updated menu item ID {MenuItemId}: {MenuItemName}", id, menuItem.Name);
                TempData["SuccessMessage"] = "Menu item updated successfully.";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu item ID {MenuItemId}", id);
                ModelState.AddModelError("", "An error occurred while updating the menu item. Please try again.");
                return View(model);
            }
        }

        // GET: Delete MenuItem
        [HttpGet]
        [Route("menu-items/{id}/delete")]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item ID {MenuItemId} not found", id);
                    return NotFound("Menu item not found.");
                }

                var model = new MenuItemViewModel
                {
                    Id = menuItem.Id,
                    Name = menuItem.Name ?? "Unnamed Item",
                    Description = menuItem.Description ?? "",
                    Price = menuItem.Price,
                    ImageUrl = menuItem.ImageUrl ?? "/images/food-placeholder.jpg",
                    IsAvailable = menuItem.IsAvailable
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete page for menu item ID {MenuItemId}", id);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "An error occurred while loading the delete page. Please try again later."
                });
            }
        }

        // POST: Delete MenuItem
        [HttpPost, ActionName("Delete")]
        [Route("menu-items/{id}/delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item ID {MenuItemId} not found", id);
                    return NotFound("Menu item not found.");
                }

                await _menuItemRepository.RemoveAsync(menuItem);
                _logger.LogInformation("Deleted menu item ID {MenuItemId}: {MenuItemName}", id, menuItem.Name);
                TempData["SuccessMessage"] = "Menu item deleted successfully.";
                return RedirectToAction("ByRestaurant", new { restaurantId = menuItem.RestaurantId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu item ID {MenuItemId}", id);
                TempData["ErrorMessage"] = "Failed to delete menu item.";
                return RedirectToAction("ByRestaurant", new { restaurantId = (await _menuItemRepository.GetByIdAsync(id))?.RestaurantId });
            }
        }

        // GET: MenuItems By Category
        [HttpGet]
        [Route("menu-items/category/{categoryId:int}")]
        public async Task<IActionResult> ByCategory(int categoryId)
        {
            try
            {
                var menuItems = await _menuItemRepository.GetByRestaurantCategoryAsync(categoryId);
                if (menuItems == null || !menuItems.Any())
                {
                    _logger.LogWarning("No menu items found for category ID {CategoryId}", categoryId);
                    return NotFound("No menu items found for this category.");
                }

                var restaurants = await _restaurantRepository.GetRestaurantsByCategoryAsync(categoryId);
                if (restaurants == null || !restaurants.Any())
                {
                    _logger.LogWarning("No restaurants found for category ID {CategoryId}", categoryId);
                    return NotFound("No restaurants found for this category.");
                }

                var category = await _categoryRepository.GetByIdAsync(categoryId);

                var model = new MenuItemsByCategoryViewModel
                {
                    CategoryId = categoryId,
                    CategoryName = category.Name,
                    Restaurants = restaurants.Select(r => new RestaurantViewModel
                    {
                        Id = r.Id,
                        Name = r.Name ?? "Unknown Restaurant",
                        ImageUrl = r.ImageUrl ?? "/images/restaurant-placeholder.jpg",
                        Address = r.Address ?? "Address not available",
                        City = r.City ?? "",
                        State = r.State ?? "",
                        PostalCode = r.PostalCode ?? "",
                        OpeningTime = r.OpeningTime,
                        ClosingTime = r.ClosingTime
                    }).ToList(),
                    MenuItems = menuItems.Select(m => new MenuItemViewModel
                    {
                        Id = m.Id,
                        RestaurantId = m.RestaurantId,
                        Name = m.Name ?? "Unnamed Item",
                        Description = m.Description ?? "",
                        Price = m.Price,
                        ImageUrl = m.ImageUrl ?? "/images/food-placeholder.jpg",
                        CreatedAt = m.CreatedAt,
                        UpdatedAt = m.UpdatedAt,
                        IsAvailable = m.IsAvailable
                    }).ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading menu items for category ID {CategoryId}", categoryId);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "An error occurred while loading menu items for the selected category. Please try again later."
                });
            }
        }

        [HttpGet]
        [Route("menu-items/{id}/customizations")]
        public async Task<IActionResult> GetCustomizations(int id)
        {
            try
            {
                var customizationOptions = await _menuItemRepository.GetCustomizationOptionsAsync(id);
                var model = customizationOptions.Select(o => new CustomizationOptionViewModel
                {
                    Id = o.Id,
                    Name = o.Name ?? "Option",
                    Choices = o.Choices.Select(c => new CustomizationChoiceViewModel
                    {
                        Id = c.Id,
                        Name = c.Name ?? "Choice",
                        Price = c.Price
                    }).ToList()
                }).ToList();
                return PartialView("_Customizations", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customizations for menu item ID {MenuItemId}", id);
                return StatusCode(500, "Failed to load customizations.");
            }
        }

        // GET: Search Menu Items
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string query, int? restaurantId, int? categoryId, int pageNumber = 1, int pageSize = 12)
        {
            try
            {
                query = query?.Trim() ?? string.Empty;
                pageNumber = Math.Max(1, pageNumber);
                pageSize = Math.Clamp(pageSize, 1, 100);

                var searchResults = await _menuItemRepository.SearchMenuItemsAsync(query, restaurantId, categoryId, pageNumber, pageSize);
                var categories = await _menuItemRepository.GetByRestaurantCategoryAsync(restaurantId ?? 0);
                var restaurants = await _restaurantRepository.GetAllAsync();

                var model = new MenuItemListViewModel
                {
                    MenuItems = searchResults.Select(m => new MenuItemViewModel
                    {
                        Id = m.Id,
                        RestaurantId = m.RestaurantId,
                        Name = m.Name ?? "Unnamed Item",
                        Description = m.Description ?? "",
                        Price = m.Price,
                        ImageUrl = m.ImageUrl ?? "/images/food-placeholder.jpg",
                        CreatedAt = m.CreatedAt,
                        UpdatedAt = m.UpdatedAt,
                        IsAvailable = m.IsAvailable,
                        CategoryId = m.Restaurant.Category.Id
                    }).ToList(),
                    Categories = categories.Select(c => new MenuItemCategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name ?? "Unnamed Category"
                    }).ToList(),
                    Restaurants = restaurants.Select(r => new MenuItemRestaurantViewModel
                    {
                        Id = r.Id,
                        Name = r.Name ?? "Unknown Restaurant"
                    }).ToList(),
                    SearchQuery = query,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = searchResults.Count(),
                    SelectedCategoryId = categoryId,
                    SelectedRestaurantId = restaurantId,
                    RestaurantId = restaurantId ?? 0,
                    RestaurantName = restaurantId.HasValue ? (await _restaurantRepository.GetRestaurantByIdAsync(restaurantId.Value))?.Name ?? "All Restaurants" : "All Restaurants"
                };

                if (!model.MenuItems.Any() && !string.IsNullOrEmpty(query))
                {
                    _logger.LogInformation("No menu items found for search query '{Query}'", query);
                    TempData["InfoMessage"] = "No items found matching your search.";
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching menu items with query '{Query}'", query);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "An error occurred while searching menu items. Please try again later."
                });
            }
        }
        // Save image to server
        private string SaveImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return "/images/food-placeholder.jpg";
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "menu-items");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return "/images/menu-items/" + fileName;
        }
    }
}