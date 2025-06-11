using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.ViewModels.MenuItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FoodDeliveryApp.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    [Route("Menu/Categories")]
    public class MenuItemCategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MenuItemCategoriesController> _logger;
        private readonly IFileService _fileService;

        public MenuItemCategoriesController(
            IUnitOfWork unitOfWork,
            ILogger<MenuItemCategoriesController> logger,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> Index(
            string searchTerm = "",
            string sortBy = "Name",
            string sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 12)
        {
            try
            {
                var categories = await _unitOfWork.MenuItemCategories.GetAllAsync();

                // Apply search filter
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    categories = categories.Where(c =>
                        c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        (c.Description != null && c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                // Apply sorting
                categories = sortBy.ToLower() switch
                {
                    "name" => sortOrder.ToLower() == "asc"
                        ? categories.OrderBy(c => c.Name).ToList()
                        : categories.OrderByDescending(c => c.Name).ToList(),
                    "menuitemcount" => sortOrder.ToLower() == "asc"
                        ? categories.OrderBy(c => c.MenuItems?.Count ?? 0).ToList()
                        : categories.OrderByDescending(c => c.MenuItems?.Count ?? 0).ToList(),
                    _ => categories.OrderBy(c => c.Name).ToList()
                };

                // Apply pagination
                var totalItems = categories.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

                var pagedCategories = categories
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModel = new MenuItemCategoryListViewModel
                {
                    Categories = pagedCategories.Select(c => new MenuItemCategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        MenuItemCount = c.MenuItems?.Count ?? 0,
                        ImageUrl = c.ImageUrl
                    }).ToList(),
                    SearchTerm = searchTerm,
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
                _logger.LogError(ex, "Error retrieving menu item categories");
                TempData["Error"] = "An error occurred while retrieving categories.";
                return View(new MenuItemCategoryListViewModel());
            }
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View(new MenuItemCategoryCreateViewModel());
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItemCategoryCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                string imageUrl = null;
                if (viewModel.ImageFile != null)
                {
                    var (fileName, filePath) = await _fileService.SaveFileAsync(viewModel.ImageFile);
                    imageUrl = _fileService.GetFileUrl(fileName);
                }

                var category = new MenuItemCategory
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    ImageUrl = imageUrl
                };

                await _unitOfWork.MenuItemCategories.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Menu item category created successfully. ID: {CategoryId}", category.Id);
                TempData["Success"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu item category");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the category.");
                return View(viewModel);
            }
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _unitOfWork.MenuItemCategories.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Menu item category not found. ID: {CategoryId}", id);
                    return NotFound();
                }

                var viewModel = new MenuItemCategoryEditViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    MenuItemCount = category.MenuItems?.Count ?? 0,
                    CurrentImageUrl = category.ImageUrl
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu item category for edit. ID: {CategoryId}", id);
                TempData["Error"] = "An error occurred while retrieving the category.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItemCategoryEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var category = await _unitOfWork.MenuItemCategories.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Menu item category not found. ID: {CategoryId}", id);
                    return NotFound();
                }

                if (viewModel.ImageFile != null)
                {
                    if (!string.IsNullOrEmpty(category.ImageUrl))
                    {
                        var fileNameToDelete = category.ImageUrl.Split('/').Last();
                        await _fileService.DeleteFileAsync(fileNameToDelete);
                    }
                    var (fileName, filePath) = await _fileService.SaveFileAsync(viewModel.ImageFile);
                    category.ImageUrl = _fileService.GetFileUrl(fileName);
                }

                category.Name = viewModel.Name;
                category.Description = viewModel.Description;

                await _unitOfWork.MenuItemCategories.UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Menu item category updated successfully. ID: {CategoryId}", id);
                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu item category. ID: {CategoryId}", id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the category.");
                return View(viewModel);
            }
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _unitOfWork.MenuItemCategories.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Menu item category not found. ID: {CategoryId}", id);
                    return NotFound();
                }

                var viewModel = new MenuItemCategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    MenuItemCount = category.MenuItems?.Count ?? 0,
                    ImageUrl = category.ImageUrl
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu item category for deletion. ID: {CategoryId}", id);
                TempData["Error"] = "An error occurred while retrieving the category.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var category = await _unitOfWork.MenuItemCategories.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Menu item category not found. ID: {CategoryId}", id);
                    return NotFound();
                }

                if (category.MenuItems?.Any() == true)
                {
                    _logger.LogWarning("Cannot delete category with associated menu items. ID: {CategoryId}", id);
                    TempData["Error"] = "Cannot delete category that has associated menu items.";
                    return RedirectToAction(nameof(Index));
                }

                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    var fileNameToDelete = category.ImageUrl.Split('/').Last();
                    await _fileService.DeleteFileAsync(fileNameToDelete);
                }

                await _unitOfWork.MenuItemCategories.DeleteAsync(category);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Menu item category deleted successfully. ID: {CategoryId}", id);
                TempData["Success"] = "Category deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu item category. ID: {CategoryId}", id);
                TempData["Error"] = "An error occurred while deleting the category.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            try
            {
                var category = await _unitOfWork.MenuItemCategories.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Menu item category not found. ID: {CategoryId}", id);
                    return NotFound();
                }

                var viewModel = new MenuItemCategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    MenuItemCount = category.MenuItems?.Count ?? 0,
                    ImageUrl = category.ImageUrl,
                    MenuItems = category.MenuItems?.Select(m => new MenuItemSummaryViewModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        ImageUrl = m.ImageUrl,
                        Price = m.Price
                    }).ToList() ?? new List<MenuItemSummaryViewModel>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu item category details. ID: {CategoryId}", id);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
