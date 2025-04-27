using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.MenuItems;
using FoodDeliveryApp.ViewModels.RestaurantViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IDistributedCache _cache;
        private readonly ICartService _cartService;
        private readonly ILogger<MenuItemsController> _logger;

        public MenuItemsController(
            IMenuItemRepository menuItemRepository,
            IRestaurantRepository restaurantRepository,
            IOrderRepository orderRepository,
            IDistributedCache cache,
            ILogger<MenuItemsController> logger,
            ICartService cartService,
            UserManager<ApplicationUser> userManager)
        {
            _menuItemRepository = menuItemRepository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _cache = cache;
            _logger = logger;
            _cartService = cartService;
        }

        // GET: MenuItems By Restaurant
        [HttpGet]
        [Route("restaurants/{restaurantId}/menu-items")]
        public async Task<IActionResult> ByRestaurant(int restaurantId)
        {
            var menuItems = await _menuItemRepository.GetAvailableItemsByRestaurantAsync(restaurantId);
            if (menuItems == null || !menuItems.Any())
            {
                return NotFound("No menu items found for this restaurant.");
            }
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }
            var model = new MenuItemListViewModel
            {
                MenuItems = menuItems.Select(m => new MenuItemViewModel
                {
                    Id = m.Id,
                    RestaurantId = m.RestaurantId,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    ImageUrl = m.ImageUrl,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt,
                    IsAvailable = m.IsAvailable
                }),
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name,
                RestaurantImageUrl = restaurant.ImageUrl
            };
            return View(model);
        }

        [HttpGet]
        [Route("menu-items/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound("Menu item not found.");
            }
            var relatedItems = await _menuItemRepository.GetRelatedItemsAsync(menuItem.Id, menuItem.RestaurantId, 5);
            var customizationOptions = await _menuItemRepository.GetCustomizationOptionsAsync(menuItem.Id);
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(menuItem.RestaurantId);
            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            var model = new MenuItemDetailsViewModel
            {
                MenuItem = new MenuItemViewModel
                {
                    Id = menuItem.Id,
                    RestaurantId = menuItem.RestaurantId,
                    Name = menuItem.Name,
                    Description = menuItem.Description,
                    Price = menuItem.Price,
                    ImageUrl = menuItem.ImageUrl,
                    CreatedAt = menuItem.CreatedAt,
                    UpdatedAt = menuItem.UpdatedAt,
                    IsAvailable = menuItem.IsAvailable
                },
                RelatedItems = relatedItems.Select(r => new RelatedItemViewModel
                {
                    Id = r.Id,
                    RestaurantId = r.RestaurantId,
                    Name = r.Name,
                    Description = r.Description,
                    Price = r.Price,
                    ImageUrl = r.ImageUrl,
                }).ToList(),
                CustomizationOptions = customizationOptions.Select(o => new CustomizationOptionViewModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    Choices = o.Choices.Select(c => new CustomizationChoiceViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Price = c.Price
                    }).ToList()
                }).ToList(),
                Restaurant = new RestaurantViewModel
                {
                    Id = restaurant.Id,
                    Name = restaurant.Name,
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

        [HttpPost]
        [Route("menu-items/{id}/add-to-cart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            if (quantity <= 0)
            {
                TempData["ErrorMessage"] = "Quantity must be at least 1.";
                return RedirectToAction("Details", new { id });
            }
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
            {
                TempData["ErrorMessage"] = "Menu item not found.";
                return RedirectToAction("Index");
            }
            // Add to cart logic here
            TempData["SuccessMessage"] = $"{menuItem.Name} added to cart.";
            return RedirectToAction("Details", new { id });
        }

        // GET: Create MenuItem
        [HttpGet]
        [Route("restaurants/{restaurantId}/menu-items/create")]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> Create(int restaurantId)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }
            var model = new MenuItemCreateViewModel
            {
                RestaurantId = restaurant.Id,
            };
            return View(model);
        }

        // POST: Create MenuItem
        [HttpPost]
        [Route("restaurants/{restaurantId}/menu-items/create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> Create(int restaurantId, MenuItemCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please correct the errors and try again.");
                return View(model);
            }
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }
            var menuItem = new MenuItem
            {
                RestaurantId = restaurantId,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                IsAvailable = model.IsAvailable,
            };
            if (model.ImageFile != null)
            {
                menuItem.ImageUrl = SaveImage(model.ImageFile);
            }
            await _menuItemRepository.AddAsync(menuItem);
            TempData["SuccessMessage"] = "Menu item created successfully.";
            return RedirectToAction("ByRestaurant", new { restaurantId });
        }

        // GET: Edit MenuItem
        [HttpGet]
        [Route("menu-items/{id}/edit")]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
            {
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

        // POST: Edit MenuItem
        [HttpPost]
        [Route("menu-items/{id}/edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> Edit(int id, MenuItemEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please correct the errors and try again.");
                return View(model);
            }
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound("Menu item not found.");
            }
            menuItem.Name = model.Name;
            menuItem.Description = model.Description;
            menuItem.Price = model.Price;
            menuItem.IsAvailable = model.IsAvailable;
            if (model.ImageFile != null)
            {
                menuItem.ImageUrl = SaveImage(model.ImageFile);
            }
            await _menuItemRepository.UpdateAsync(menuItem);
            TempData["SuccessMessage"] = "Menu item updated successfully.";
            return RedirectToAction("Details", new { id });
        }

        // GET: Delete MenuItem
        [HttpGet]
        [Route("menu-items/{id}/delete")]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound("Menu item not found.");
            }
            var model = new ViewModels.MenuItems.MenuItemViewModel
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                ImageUrl = menuItem.ImageUrl,
                IsAvailable = menuItem.IsAvailable
            };
            return View(model);
        }

        // POST: Delete MenuItem
        [HttpPost, ActionName("Delete")]
        [Route("menu-items/{id}/delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound("Menu item not found.");
            }
            await _menuItemRepository.RemoveAsync(menuItem);
            TempData["SuccessMessage"] = "Menu item deleted successfully.";
            return RedirectToAction("ByRestaurant", new { restaurantId = menuItem.RestaurantId });
        }

        [HttpGet]
        [Route("menu-items/category/{categoryId:int}")]
        public async Task<IActionResult> ByCategory(int categoryId)
        {
            try
            {
                // Get menu items by category
                var menuItems = await _menuItemRepository.GetByRestaurantCategoryAsync(categoryId);
                if (menuItems == null || !menuItems.Any())
                {
                    return NotFound("No menu items found for this category.");
                }

                // Get restaurants that have this category
                var restaurants = await _restaurantRepository.GetRestaurantsByCategoryAsync(categoryId);
                if (restaurants == null || !restaurants.Any())
                {
                    return NotFound("No restaurants found for this category.");
                }

                // Get category name from the first restaurant (assuming all have the same category)
                var categoryName = restaurants.First().Category?.Name ?? "Unknown Category";

                var model = new MenuItemsByCategoryViewModel
                {
                    CategoryId = categoryId,
                    CategoryName = categoryName,
                    Restaurants = restaurants.Select(r => new RestaurantViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ImageUrl = r.ImageUrl ?? "/images/restaurant-placeholder.jpg",
                        Address = r.Address ?? "Address not available",
                        City = r.City ?? "",
                        State = r.State ?? "",
                        PostalCode = r.PostalCode ?? "",
                        OpeningTime = r.OpeningTime,
                        ClosingTime = r.ClosingTime
                    }).ToList(),
                    MenuItems = menuItems.Select(m => new ViewModels.MenuItems.MenuItemViewModel
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
                //_logger.LogError(ex, "Error loading menu items for category {CategoryId}", categoryId);
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "An error occurred while loading menu items for the selected category. Please try again later."
                });
            }
        }


        // save image to server
        private string SaveImage(IFormFile imageFile)
        {
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
