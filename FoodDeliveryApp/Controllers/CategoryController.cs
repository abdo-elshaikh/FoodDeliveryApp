using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace FoodDeliveryApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(
            IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment,
            ILogger<CategoryController> logger)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: Category
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            try
            {
                var categories = await _unitOfWork.RestaurantCategories.GetAllAsync();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    categories = categories.Where(c => 
                        c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        (c.Description != null && c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                var viewModel = new CategoryListViewModel
                {
                    Categories = categories.Select(c => new CategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        ImageUrl = c.ImageUrl,
                        RestaurantCount = c.Restaurants?.Count ?? 0
                    }).ToList(),
                    SearchTerm = searchTerm,
                    TotalCount = categories.Count()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving restaurant categories");
                TempData["ErrorMessage"] = "An error occurred while retrieving categories.";
                return View(new CategoryListViewModel());
            }
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var category = await _unitOfWork.RestaurantCategories.GetByIdAsync(id);

                if (category == null)
                {
                    return NotFound();
                }

                var viewModel = new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    RestaurantCount = category.Restaurants?.Count ?? 0
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category details for ID: {CategoryId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving category details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Category/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new CategoryCreateViewModel());
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CategoryCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                string? imageUrl = null;
                if (viewModel.ImageFile != null)
                {
                    imageUrl = await SaveImageAsync(viewModel.ImageFile);
                }

                var category = new RestaurantCategory
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    ImageUrl = imageUrl
                };

                await _unitOfWork.RestaurantCategories.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("Category created successfully. ID: {CategoryId}", category.Id);
                TempData["SuccessMessage"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the category.");
                return View(viewModel);
            }
        }

        // GET: Category/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _unitOfWork.RestaurantCategories.GetByIdAsync(id);

                if (category == null)
                {
                    return NotFound();
                }

                var viewModel = new CategoryEditViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    CurrentImageUrl = category.ImageUrl,
                    RestaurantCount = category.Restaurants?.Count ?? 0
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category for edit, ID: {CategoryId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the category.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, CategoryEditViewModel viewModel)
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
                var category = await _unitOfWork.RestaurantCategories.GetByIdAsync(id);

                if (category == null)
                {
                    return NotFound();
                }

                // Update the image if a new one was uploaded
                if (viewModel.NewImageFile != null)
                {
                    var imageUrl = await SaveImageAsync(viewModel.NewImageFile);
                    category.ImageUrl = imageUrl;
                }

                // Update only the properties that are allowed to be changed
                category.Name = viewModel.Name;
                category.Description = viewModel.Description;

                await _unitOfWork.RestaurantCategories.UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("Category updated successfully. ID: {CategoryId}", id);
                TempData["SuccessMessage"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category, ID: {CategoryId}", id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the category.");
                return View(viewModel);
            }
        }

        // GET: Category/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _unitOfWork.RestaurantCategories.GetByIdAsync(id);

                if (category == null)
                {
                    return NotFound();
                }

                var viewModel = new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    RestaurantCount = category.Restaurants?.Count ?? 0
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category for delete, ID: {CategoryId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the category.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var category = await _unitOfWork.RestaurantCategories.GetByIdAsync(id);

                if (category == null)
                {
                    return NotFound();
                }

                // Check if there are any restaurants associated with this category
                if (category.Restaurants != null && category.Restaurants.Any())
                {
                    TempData["ErrorMessage"] = "This category cannot be deleted because it has associated restaurants. Please reassign or delete them first.";
                    return RedirectToAction(nameof(Index));
                }

                await _unitOfWork.RestaurantCategories.RemoveAsync(category);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("Category deleted successfully. ID: {CategoryId}", id);
                TempData["SuccessMessage"] = "Category deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category, ID: {CategoryId}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the category.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper method for saving images
        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "categories");

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique filename
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the relative path for storing in the database
            return $"/images/categories/{uniqueFileName}";
        }
    }
}
