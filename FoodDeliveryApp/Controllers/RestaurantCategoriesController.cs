using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RestaurantCategoriesController : Controller
    {
        private readonly IRepository<RestaurantCategory> _categoryRepo;

        public RestaurantCategoriesController(IRepository<RestaurantCategory> categoryRepo)
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
        public async Task<IActionResult> Create(RestaurantCategory category)
        {
            if (!ModelState.IsValid)
                return View(category);

            await _categoryRepo.AddAsync(category);
            return RedirectToAction(nameof(Index));
        }
    }
}
