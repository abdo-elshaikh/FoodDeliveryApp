// Controllers/ItemsController.cs
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories;

namespace FoodDeliveryApp.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<Category> _categoryRepository;

        public ItemsController(IRepository<Item> itemRepository, IRepository<Category> categoryRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View(_itemRepository.GetAll());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _categoryRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                _itemRepository.Add(item);
                _itemRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _categoryRepository.GetAll();
            return View(item);
        }

        public IActionResult Edit(int id)
        {
            var item = _itemRepository.GetById(id);
            if (item == null) return NotFound();
            ViewBag.Categories = _categoryRepository.GetAll();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(int id, Item item)
        {
            if (id != item.ItemId) return NotFound();

            if (ModelState.IsValid)
            {
                _itemRepository.Update(item);
                _itemRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _categoryRepository.GetAll();
            return View(item);
        }

        public IActionResult Delete(int id)
        {
            var item = _itemRepository.GetById(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = _itemRepository.GetById(id);
            _itemRepository.Delete(item);
            _itemRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}