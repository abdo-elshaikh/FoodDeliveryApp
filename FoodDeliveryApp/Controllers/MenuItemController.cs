using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.ViewModels.Restaurant;
using FoodDeliveryApp.ViewModels.MenuItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FoodDeliveryApp.Controllers
{
    [Route("Menu")]
    public class MenuItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MenuItemController> _logger;
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUserService;

        public MenuItemController(
            IUnitOfWork unitOfWork,
            ILogger<MenuItemController> logger,
            IFileService fileService,
            ICurrentUserService currentUserService
            )
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int? restaurantId = null, int? categoryId = null, string searchQuery = "", string sortBy = "Name", string sortOrder = "asc", int pageNumber = 1, int pageSize = 12)
        {
            try
            {
                var query = await _unitOfWork.MenuItems.GetAllAsync();

                // Apply filters
                if (restaurantId.HasValue)
                {
                    query = query.Where(m => m.RestaurantId == restaurantId.Value);
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(m => m.CategoryId == categoryId.Value);
                }

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    searchQuery = searchQuery.ToLower();
                    query = query.Where(m =>
                        m.Name.ToLower().Contains(searchQuery) ||
                        m.Description.ToLower().Contains(searchQuery));
                }

                // Apply sorting
                query = sortBy.ToLower() switch
                {
                    "price" => sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(m => m.Price)
                        : query.OrderBy(m => m.Price),
                    "name" => sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(m => m.Name)
                        : query.OrderBy(m => m.Name),
                    _ => query.OrderBy(m => m.Name)
                };

                // Get total count for pagination
                var totalItems = query.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

                // Apply pagination
                var menuItems = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync();
                var restaurants = await _unitOfWork.Restaurants.GetAllAsync();

                // Get current user's favorites
                var currentUser = _currentUserService.GetCurrentUser();
                var favorites = currentUser != null
                    ? await _unitOfWork.Favorites.GetUserFavoritesAsync(currentUser.Id)
                    : new List<MenuItem>();

                var viewModel = new MenuItemListViewModel
                {
                    MenuItems = menuItems.Select(m => new MenuItemViewModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Price = m.Price,
                        ImageUrl = m.ImageUrl,
                        IsAvailable = m.IsAvailable,
                        CategoryName = m.Category?.Name,
                        RestaurantName = m.Restaurant?.Name,
                        RestaurantId = m.RestaurantId,
                        CategoryId = m.CategoryId,
                        IsFavorite = favorites.Any(f => f.Id == m.Id)
                    }).ToList(),
                    Categories = categories.Select(c => new MenuItemCategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        MenuItemCount = c.MenuItems?.Count ?? 0
                    }).ToList(),
                    Restaurants = restaurants.Select(r => new RestaurantViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        ImageUrl = r.ImageUrl
                    }).ToList(),
                    SelectedCategoryId = categoryId,
                    SelectedRestaurantId = restaurantId,
                    SearchQuery = searchQuery,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu items");
                TempData["Error"] = "An error occurred while retrieving menu items.";
                return View(new MenuItemListViewModel());
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item not found. ID: {MenuItemId}", id);
                    return NotFound();
                }

                var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(menuItem.RestaurantId);
                var relatedItems = await _unitOfWork.MenuItems.GetByCategoryIdAsync(menuItem.CategoryId ?? 0);
                relatedItems = relatedItems.Where(m => m.Id != id).Take(4).ToList();

                var viewModel = new MenuItemDetailsViewModel
                {
                    Id = menuItem.Id,
                    Name = menuItem.Name,
                    Description = menuItem.Description ?? "No description provided.",
                    Price = menuItem.Price,
                    ImageUrl = menuItem.ImageUrl,
                    IsAvailable = menuItem.IsAvailable,
                    CategoryName = menuItem.Category?.Name ?? "No category provided.",
                    RestaurantName = restaurant?.Name ?? "No restaurant provided.",
                    RestaurantId = menuItem.RestaurantId,
                    MenuItem = new MenuItemViewModel
                    {
                        Id = menuItem.Id,
                        Name = menuItem.Name,
                        Description = menuItem.Description ?? "No description provided.",
                        Price = menuItem.Price,
                        ImageUrl = menuItem.ImageUrl ?? "No image provided.",
                        IsAvailable = menuItem.IsAvailable,
                        CategoryName = menuItem.Category?.Name ?? "No category provided.",
                        RestaurantName = restaurant?.Name ?? "No restaurant provided."
                    },
                    RelatedItems = relatedItems.Select(m => new MenuItemViewModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description ?? "No description provided.",
                        Price = m.Price,
                        ImageUrl = m.ImageUrl ?? "No image provided.",
                        IsAvailable = m.IsAvailable,
                        CategoryName = m.Category?.Name ?? "No category provided.",
                        RestaurantName = m.Restaurant?.Name ?? "No restaurant provided.",
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu item details. ID: {MenuItemId}", id);
                TempData["Error"] = "An error occurred while retrieving menu item details.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet("Create/{restaurantId}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Create(int restaurantId)
        {
            try
            {
                var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(restaurantId);
                if (restaurant == null)
                {
                    _logger.LogWarning("Restaurant not found. ID: {RestaurantId}", restaurantId);
                    return NotFound();
                }

                // Check if current user is authorized (Admin or restaurant owner)
                var currentUserId = _currentUserService.GetCurrentUserId();
                if (!User.IsInRole("Admin") && !restaurant.IsOwner(currentUserId))
                {
                    return Forbid();
                }

                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync();
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });

                var viewModel = new MenuItemCreateViewModel
                {
                    RestaurantId = restaurantId,
                    RestaurantName = restaurant.Name,
                    Categories = categories.Select(c => new MenuItemCategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing menu item creation form");
                TempData["Error"] = "An error occurred while preparing the form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Create(MenuItemCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync();
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
                return View(viewModel);
            }

            try
            {
                var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(viewModel.RestaurantId);
                if (restaurant == null)
                {
                    _logger.LogWarning("Restaurant not found. ID: {RestaurantId}", viewModel.RestaurantId);
                    return NotFound();
                }

                // Check if current user is authorized (Admin or restaurant owner)
                var currentUserId = _currentUserService.GetCurrentUserId();
                if (!User.IsInRole("Admin") && !restaurant.IsOwner(currentUserId))
                {
                    return Forbid();
                }

                var menuItem = new MenuItem
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    IsAvailable = viewModel.IsAvailable,
                    RestaurantId = viewModel.RestaurantId,
                    CategoryId = viewModel.CategoryId,
                };

                if (viewModel.ImageFile != null)
                {
                    // TODO: Implement image upload logic
                    var (fileName, filePath) = await _fileService.SaveFileAsync(viewModel.ImageFile, "MenuItems");
                    menuItem.ImageUrl = filePath;
                }

                await _unitOfWork.MenuItems.AddAsync(menuItem);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Menu item created successfully. ID: {MenuItemId}", menuItem.Id);
                TempData["Success"] = "Menu item created successfully.";
                return RedirectToAction(nameof(Index), new { restaurantId = viewModel.RestaurantId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu item");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the menu item.");
                return View(viewModel);
            }
        }

        [HttpGet("Edit/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item not found. ID: {MenuItemId}", id);
                    return NotFound();
                }

                // Check if current user is authorized (Admin or restaurant owner)
                var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(menuItem.RestaurantId);
                var currentUserId = _currentUserService.GetCurrentUserId();

                if (!User.IsInRole("Admin") && !restaurant.IsOwner(currentUserId))
                {
                    return Forbid();
                }

                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync();
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == menuItem.CategoryId
                });

                var viewModel = new MenuItemEditViewModel
                {
                    Id = menuItem.Id,
                    Name = menuItem.Name,
                    Description = menuItem.Description,
                    Price = menuItem.Price,
                    IsAvailable = menuItem.IsAvailable,
                    RestaurantId = menuItem.RestaurantId,
                    CategoryId = menuItem.CategoryId,
                    CurrentImageUrl = menuItem.ImageUrl
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu item for edit. ID: {MenuItemId}", id);
                TempData["Error"] = "An error occurred while retrieving the menu item.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Edit(int id, MenuItemEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            // Check if current user is authorized (Admin or restaurant owner)
            var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(viewModel.RestaurantId);
            var currentUserId = _currentUserService.GetCurrentUserId();
            if (!User.IsInRole("Admin") && !restaurant.IsOwner(currentUserId))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync();
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == viewModel.CategoryId
                });
                return View(viewModel);
            }

            try
            {
                var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item not found. ID: {MenuItemId}", id);
                    return NotFound();
                }

                menuItem.Name = viewModel.Name;
                menuItem.Description = viewModel.Description;
                menuItem.Price = viewModel.Price;
                menuItem.IsAvailable = viewModel.IsAvailable;
                menuItem.CategoryId = viewModel.CategoryId;

                if (viewModel.NewImageFile != null)
                {
                    await _fileService.DeleteFileAsync(menuItem.ImageUrl.Split('/').Last(), "menu-items");
                   var (fileName, filePath) = await _fileService.SaveFileAsync(viewModel.NewImageFile, "menu-items");
                    menuItem.ImageUrl = filePath;
                }

                await _unitOfWork.MenuItems.UpdateAsync(menuItem);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Menu item updated successfully. ID: {MenuItemId}", id);
                TempData["Success"] = "Menu item updated successfully.";
                return RedirectToAction(nameof(Index), new { restaurantId = viewModel.RestaurantId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu item. ID: {MenuItemId}", id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the menu item.");
                return View(viewModel);
            }
        }

        [HttpGet("Delete/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item not found. ID: {MenuItemId}", id);
                    return NotFound();
                }

                // Check if current user is authorized (Admin or restaurant owner)
                var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(menuItem.RestaurantId);

                var currentUserId = _currentUserService.GetCurrentUserId();
                if (!User.IsInRole("Admin") && !restaurant.IsOwner(currentUserId))
                {
                    return Forbid();
                }

                var viewModel = new MenuItemViewModel
                {
                    Id = menuItem.Id,
                    Name = menuItem.Name,
                    Description = menuItem.Description,
                    Price = menuItem.Price,
                    ImageUrl = menuItem.ImageUrl,
                    IsAvailable = menuItem.IsAvailable,
                    CategoryName = menuItem.Category?.Name,
                    RestaurantName = menuItem.Restaurant?.Name
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu item for deletion. ID: {MenuItemId}", id);
                TempData["Error"] = "An error occurred while retrieving the menu item.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("Delete/{id}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item not found. ID: {MenuItemId}", id);
                    return NotFound();
                }

                await _unitOfWork.MenuItems.DeleteAsync(menuItem);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Menu item deleted successfully. ID: {MenuItemId}", id);
                TempData["Success"] = "Menu item deleted successfully.";
                return RedirectToAction(nameof(Index), new { restaurantId = menuItem.RestaurantId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu item. ID: {MenuItemId}", id);
                TempData["Error"] = "An error occurred while deleting the menu item.";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
