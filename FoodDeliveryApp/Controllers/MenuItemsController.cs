using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels.MenuItems;
using FoodDeliveryApp.ViewModels.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.ViewModels.Review;

namespace FoodDeliveryApp.Controllers
{
    
    public class MenuItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartService _cartService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<MenuItemsController> _logger;

        public MenuItemsController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ICartService cartService,
            IWebHostEnvironment webHostEnvironment,
            ILogger<MenuItemsController> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _cartService = cartService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: MenuItems
        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = "", int? restaurantId = null, int? categoryId = null, 
            string sortBy = "Name", string sortOrder = "asc", int page = 1, int pageSize = 12)
        {
            try
            {
                var menuItemsQuery = _unitOfWork.MenuItems.GetAllAsync(
                    m => m.Restaurant, 
                    m => m.Reviews.AsQueryable().Include(r => r.CustomerProfile).AsEnumerable(),
                    m => m.Category,
                    m => m.CustomizationOptions.AsQueryable().Include(co => co.Choices).AsEnumerable()
                );
                var menuItems = await menuItemsQuery;
                
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    menuItems = menuItems.Where(m => 
                        m.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        (m.Description != null && m.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    );
                }
                
                if (restaurantId.HasValue)
                {
                    menuItems = menuItems.Where(m => m.RestaurantId == restaurantId.Value);
                }
                
                if (categoryId.HasValue)
                {
                    menuItems = menuItems.Where(m => m.CategoryId == categoryId.Value);
                }
                
                menuItems = ApplySorting(menuItems, sortBy, sortOrder);
                
                var totalCount = menuItems.Count();
                
                var pagedMenuItems = menuItems
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                
                var categories = await _unitOfWork.RestaurantCategories.GetAllAsync();
                var restaurants = await _unitOfWork.Restaurants.GetAllAsync();
                
                var viewModel = new MenuItemListViewModel
                {
                    MenuItems = pagedMenuItems.Select(m => MapToViewModel(m)).ToList(),
                    SearchQuery = searchTerm,
                    SelectedRestaurantId = restaurantId,
                    SelectedCategoryId = categoryId,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    TotalItems = totalCount,
                    PageNumber = page,
                    PageSize = pageSize,
                    RestaurantOptions = restaurants.Select(r => new SelectListItem 
                    { 
                        Value = r.Id.ToString(), 
                        Text = r.Name,
                        Selected = restaurantId.HasValue && r.Id == restaurantId.Value
                    }).ToList(),
                    CategoryOptions = categories.Select(c => new SelectListItem 
                    { 
                        Value = c.Id.ToString(), 
                        Text = c.Name,
                        Selected = categoryId.HasValue && c.Id == categoryId.Value
                    }).ToList()
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading menu items");
                TempData["ErrorMessage"] = "An error occurred while loading menu items. Please try again later.";
                return View(new MenuItemListViewModel());
            }
        }

        // GET: MenuItems/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var menuItem = await _unitOfWork.MenuItems.FirstOrDefaultAsync(
                                                          m => m.Id == id,
                                                          m => m.Restaurant,
                                                          m => m.Category,
                                                          m => m.Reviews.AsQueryable().Include(r => r.CustomerProfile).AsEnumerable(),
                                                          m => m.CustomizationOptions.AsQueryable().Include(co => co.Choices).AsEnumerable()
                                                          );

                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item with ID {MenuItemId} not found.", id);
                    return NotFound();
                }

                if (menuItem.Restaurant == null)
                {
                    _logger.LogError("Restaurant not found for menu item ID: {MenuItemId}, RestaurantId: {RestaurantId}", id, menuItem.RestaurantId);
                     TempData["ErrorMessage"] = "Restaurant data for this menu item is missing.";
                }
                
                var relatedItems = await _unitOfWork.MenuItems.GetRelatedItemsAsync(menuItem.Id, menuItem.RestaurantId, 5);
                
                var reviews = menuItem.Restaurant.Reviews
                    .Where(r => r.MenuItemId == id)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToList();

                bool isInUserCart = false;
                int? cartItemQuantity = null;
                
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var cart = await _cartService.GetCartAsync(userId);
                        var cartItem = cart?.Items?.FirstOrDefault(i => i.MenuItemId == id);
                        isInUserCart = cartItem != null;
                        cartItemQuantity = cartItem?.Quantity;
                    }
                }
                
                var viewModel = new MenuItemDetailsViewModel
                {
                    MenuItem = MapToViewModel(menuItem),
                    RelatedItems = relatedItems.Select(ri => new RelatedItemViewModel {
                        Id = ri.Id,
                        Name = ri.Name,
                        Description = ri.Description ?? string.Empty,
                        Price = ri.Price,
                        ImageUrl = ri.ImageUrl,
                        RestaurantId = ri.RestaurantId
                    }).ToList(),
                    Restaurant = menuItem.Restaurant != null ? new RestaurantViewModel { 
                        Id = menuItem.Restaurant.Id, 
                        Name = menuItem.Restaurant.Name,
                        CoverImageUrl = menuItem.Restaurant.ImageUrl,
                    } : new RestaurantViewModel { Name = "N/A" },
                    CustomizationOptions = menuItem.CustomizationOptions?.Select(co => new CustomizationOptionViewModel {
                        Id = co.Id,
                        Name = co.Name,
                        IsRequired = co.IsRequired,
                        AllowMultiple = co.AllowMultiple,
                        Choices = co.Choices?.Select(ch => new CustomizationChoiceViewModel {
                            Id = ch.Id,
                            Name = ch.Name,
                            Price = ch.Price,
                            IsDefault = ch.IsDefault
                        }).ToList() ?? new List<CustomizationChoiceViewModel>()
                    }).ToList() ?? new List<CustomizationOptionViewModel>(),
                    Reviews = reviews.Select(r => new ReviewViewModel
                    {
                        Id = r.Id,
                        Rating = (int)r.Rating,
                        Comment = r.Comment ?? string.Empty,
                        CustomerName = (r.CustomerProfile?.FirstName ?? "") + " " + (r.CustomerProfile?.LastName ?? "").Trim(),
                        CreatedAt = r.CreatedAt,
                        MenuItemId = r.MenuItemId ?? 0,
                    }).ToList(),
                    IsInUserCart = isInUserCart,
                    CartItemQuantity = cartItemQuantity
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu item details for ID: {MenuItemId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving menu item details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: MenuItems/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Create(int? restaurantId = null)
        {
            try
            {
                var restaurants = await _unitOfWork.Restaurants.GetAllAsync();
                var categories = await _unitOfWork.RestaurantCategories.GetAllAsync();
                
                var viewModel = new MenuItemCreateViewModel
                {
                    RestaurantId = restaurantId ?? 0,
                    IsAvailable = true,
                    RestaurantOptions = restaurants.Select(r => new SelectListItem 
                    { 
                        Value = r.Id.ToString(), 
                        Text = r.Name,
                        Selected = restaurantId.HasValue && r.Id == restaurantId.Value
                    }).ToList(),
                    CategoryOptions = categories.Select(c => new SelectListItem 
                    { 
                        Value = c.Id.ToString(), 
                        Text = c.Name 
                    }).ToList()
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create menu item form");
                TempData["ErrorMessage"] = "An error occurred while loading the form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: MenuItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Create(MenuItemCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }

            try
            {
                if (!await HasMenuItemAccess(viewModel.RestaurantId))
                {
                    TempData["ErrorMessage"] = "You do not have permission to add menu items to this restaurant.";
                    await PopulateDropdowns(viewModel);
                    return View(viewModel);
                }

                string? uniqueFileName = null;
                if (viewModel.ImageFile != null)
                {
                    uniqueFileName = await SaveImageAsync(viewModel.ImageFile);
                }

                var menuItem = new MenuItem
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    ImageUrl = uniqueFileName ?? viewModel.ImageUrl,
                    IsAvailable = viewModel.IsAvailable,
                    RestaurantId = viewModel.RestaurantId,
                    CategoryId = viewModel.CategoryId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.MenuItems.AddAsync(menuItem);
                await _unitOfWork.SaveChangesAsync();
                TempData["SuccessMessage"] = "Menu item created successfully.";
                return RedirectToAction(nameof(Index), new { restaurantId = menuItem.RestaurantId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu item");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the menu item.");
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }
        }

        // GET: MenuItems/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item with ID {MenuItemId} not found for edit.", id);
                    return NotFound();
                }

                if (!await HasMenuItemAccess(menuItem.RestaurantId))
                {
                     TempData["ErrorMessage"] = "You do not have permission to edit menu items for this restaurant.";
                    return RedirectToAction(nameof(Index));
                }
                
                var restaurants = await _unitOfWork.Restaurants.GetAllAsync();
                var categories = await _unitOfWork.RestaurantCategories.GetAllAsync();

                var viewModel = new MenuItemEditViewModel
                {
                    Id = menuItem.Id,
                    Name = menuItem.Name,
                    Description = menuItem.Description,
                    Price = menuItem.Price,
                    CurrentImageUrl = menuItem.ImageUrl,
                    IsAvailable = menuItem.IsAvailable,
                    RestaurantId = menuItem.RestaurantId,
                    CategoryId = menuItem.CategoryId,
                    CreatedAt = menuItem.CreatedAt,
                    RestaurantOptions = restaurants.Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = r.Name,
                        Selected = r.Id == menuItem.RestaurantId
                    }).ToList(),
                    CategoryOptions = categories.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                        Selected = c.Id == menuItem.CategoryId
                    }).ToList()
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit form for menu item ID: {MenuItemId}", id);
                TempData["ErrorMessage"] = "An error occurred while loading the edit form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: MenuItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Edit(int id, MenuItemEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }

            try
            {
                var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item with ID {MenuItemId} not found for update.", id);
                    return NotFound();
                }

                if (!await HasMenuItemAccess(menuItem.RestaurantId))
                {
                    TempData["ErrorMessage"] = "You do not have permission to edit this menu item.";
                    await PopulateDropdowns(viewModel);
                    return View(viewModel);
                }

                string? uniqueFileName = viewModel.CurrentImageUrl;
                if (viewModel.ImageFile != null)
                {
                    uniqueFileName = await SaveImageAsync(viewModel.ImageFile);
                }

                menuItem.Name = viewModel.Name;
                menuItem.Description = viewModel.Description;
                menuItem.Price = viewModel.Price;
                menuItem.ImageUrl = uniqueFileName;
                menuItem.IsAvailable = viewModel.IsAvailable;
                menuItem.RestaurantId = viewModel.RestaurantId;
                menuItem.CategoryId = viewModel.CategoryId;
                menuItem.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.MenuItems.UpdateAsync(menuItem);
                await _unitOfWork.SaveChangesAsync();
                TempData["SuccessMessage"] = "Menu item updated successfully.";
                return RedirectToAction(nameof(Details), new { id = menuItem.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu item ID: {MenuItemId}", id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the menu item.");
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }
        }

        // GET: MenuItems/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
                if (menuItem == null)
                {
                    return NotFound();
                }

                if (!await HasMenuItemAccess(menuItem.RestaurantId))
                {
                    return Forbid();
                }
                
                var viewModel = MapToViewModel(menuItem);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete confirmation for menu item ID: {MenuItemId}", id);
                TempData["ErrorMessage"] = "An error occurred while loading the delete confirmation.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item with ID {MenuItemId} not found for deletion.", id);
                    return NotFound();
                }

                if (!await HasMenuItemAccess(menuItem.RestaurantId))
                {
                    TempData["ErrorMessage"] = "You do not have permission to delete this menu item.";
                    return RedirectToAction(nameof(Index));
                }

                await _unitOfWork.MenuItems.RemoveAsync(menuItem);
                await _unitOfWork.SaveChangesAsync();
                TempData["SuccessMessage"] = "Menu item deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu item ID: {MenuItemId}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the menu item.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper methods
        private async Task<bool> HasMenuItemAccess(int restaurantId)
        {
            if (User.IsInRole("Admin"))
            {
                return true;
            }
            
            if (User.IsInRole("Owner"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }
                
                var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(restaurantId);
                return restaurant != null && restaurant.OwnerId == userId;
            }
            
            return false;
        }
        
        private MenuItemViewModel MapToViewModel(FoodDeliveryApp.Models.MenuItem menuItem)
        {
            if (menuItem == null) 
            {
                _logger.LogWarning("Attempted to map a null MenuItem to MenuItemViewModel.");
                return new MenuItemViewModel();
            }

            var viewModel = new MenuItemViewModel
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                ImageUrl = menuItem.ImageUrl,
                IsAvailable = menuItem.IsAvailable,
                RestaurantId = menuItem.RestaurantId,
                RestaurantName = menuItem.Restaurant?.Name ?? "N/A",
                CategoryId = menuItem.CategoryId,
                Category = menuItem.Category != null ? new MenuItemCategoryViewModel { Id = menuItem.Category.Id, Name = menuItem.Category.Name } : new MenuItemCategoryViewModel { Name = "N/A"},
                CreatedAt = menuItem.CreatedAt,
                UpdatedAt = menuItem.UpdatedAt,
                Rating = menuItem.Reviews != null && menuItem.Reviews.Any() ? menuItem.Reviews.Average(r => r.Rating) : 0,
                ReviewCount = menuItem.Reviews?.Count ?? 0,
                Reviews = menuItem.Reviews?.Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Rating = (int)r.Rating,
                    Comment = r.Comment ?? string.Empty,
                    CustomerName = (r.CustomerProfile?.FirstName ?? "") + " " + (r.CustomerProfile?.LastName ?? "").Trim(),
                    CreatedAt = r.CreatedAt,
                    MenuItemId = r.MenuItemId ?? 0,
                }).ToList() ?? new List<ReviewViewModel>(),
                CustomizationOptions = menuItem.CustomizationOptions?.Select(co => new CustomizationOptionViewModel
                {
                    Id = co.Id,
                    Name = co.Name,
                    IsRequired = co.IsRequired,
                    AllowMultiple = co.AllowMultiple,
                    Choices = co.Choices?.Select(ch => new CustomizationChoiceViewModel
                    {
                        Id = ch.Id,
                        Name = ch.Name,
                        Price = ch.Price,
                        IsDefault = ch.IsDefault
                    }).ToList() ?? new List<CustomizationChoiceViewModel>()
                }).ToList() ?? new List<CustomizationOptionViewModel>()
            };
            return viewModel;
        }
        
        private async Task PopulateDropdowns(MenuItemCreateViewModel viewModel)
        {
            var restaurants = await _unitOfWork.Restaurants.GetAllAsync();
            var categories = await _unitOfWork.RestaurantCategories.GetAllAsync();
            
            viewModel.RestaurantOptions = restaurants.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name,
                Selected = r.Id == viewModel.RestaurantId
            }).ToList();
            
            viewModel.CategoryOptions = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == viewModel.CategoryId
            }).ToList();
        }
        
        private IEnumerable<FoodDeliveryApp.Models.MenuItem> ApplySorting(IEnumerable<FoodDeliveryApp.Models.MenuItem> menuItems, string sortBy, string sortOrder)
        {
            if (menuItems == null)
            {
                return Enumerable.Empty<FoodDeliveryApp.Models.MenuItem>();
            }

            switch (sortBy?.ToLower())
            {
                case "price":
                    menuItems = sortOrder?.ToLower() == "desc" ? menuItems.OrderByDescending(m => m.Price) : menuItems.OrderBy(m => m.Price);
                    break;
                case "createdat":
                    menuItems = sortOrder?.ToLower() == "desc" ? menuItems.OrderByDescending(m => m.CreatedAt) : menuItems.OrderBy(m => m.CreatedAt);
                    break;
                default:
                    menuItems = sortOrder?.ToLower() == "desc" ? menuItems.OrderByDescending(m => m.Name) : menuItems.OrderBy(m => m.Name);
                    break;
            }
            return menuItems;
        }
        
        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "menu-items");
            
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/menu-items/{uniqueFileName}";
        }
    }
}