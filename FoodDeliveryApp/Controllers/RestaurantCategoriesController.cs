using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace FoodDeliveryApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Restaurant/Categories")]
    public class RestaurantCategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RestaurantCategoriesController> _logger;
        private readonly IFileService _fileService;

        public RestaurantCategoriesController(
            IUnitOfWork unitOfWork,
            ILogger<RestaurantCategoriesController> logger,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _unitOfWork.RestaurantCategories.GetAllAsync();
                var viewModel = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    RestaurantCount = c.Restaurants?.Count ?? 0
                }).ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving restaurant categories");
                TempData["Error"] = "An error occurred while retrieving categories.";
                return View(new List<CategoryViewModel>());
            }
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View(new CategoryCreateViewModel());
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel viewModel)
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

                var category = new RestaurantCategory
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    ImageUrl = imageUrl
                };

                await _unitOfWork.RestaurantCategories.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Category created successfully. ID: {CategoryId}", category.Id);
                TempData["Success"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the category.");
                return View(viewModel);
            }
        }

        [HttpGet]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _unitOfWork.RestaurantCategories.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category not found. ID: {CategoryId}", id);
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
                _logger.LogError(ex, "Error retrieving category for deletion. ID: {CategoryId}", id);
                TempData["Error"] = "An error occurred while retrieving the category.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var category = await _unitOfWork.RestaurantCategories.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category not found. ID: {CategoryId}", id);
                    return NotFound();
                }

                var viewModel = new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    RestaurantCount = category.Restaurants?.Count ?? 0,
                    Restaurants = category.Restaurants?.Select(r => new RestaurantSummaryViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ImageUrl = r.ImageUrl,
                        Description = r.Description
                    }).ToList() ?? new List<RestaurantSummaryViewModel>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category details. ID: {CategoryId}", id);
                TempData["Error"] = "An error occurred while retrieving the category details.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var category = await _unitOfWork.RestaurantCategories.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category not found. ID: {CategoryId}", id);
                    return NotFound();
                }

                if (category.Restaurants?.Any() == true)
                {
                    _logger.LogWarning("Cannot delete category with associated restaurants. ID: {CategoryId}", id);
                    TempData["Error"] = "Cannot delete category that has associated restaurants.";
                    return RedirectToAction(nameof(Index));
                }

                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    var fileNameToDelete = category.ImageUrl.Split('/').Last();
                    await _fileService.DeleteFileAsync(fileNameToDelete);
                }

                await _unitOfWork.RestaurantCategories.DeleteAsync(category);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Category deleted successfully. ID: {CategoryId}", id);
                TempData["Success"] = "Category deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category. ID: {CategoryId}", id);
                TempData["Error"] = "An error occurred while deleting the category.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool CategoryExists(int id)
        {
            return _unitOfWork.RestaurantCategories.GetAllAsync().Result.Any(c => c.Id == id);
        }
    }
}
