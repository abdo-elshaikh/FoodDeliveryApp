using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryApp.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class MenuItemCategoriesController : Controller
    {
        private readonly IMenuItemCategoryRepository _categoryRepo;

        public MenuItemCategoriesController(IMenuItemCategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItemCategory category)
        {
            if (!ModelState.IsValid)
                return View(category);

            await _categoryRepo.AddAsync(category);
            return RedirectToAction(nameof(Index));
        }
    }
}
