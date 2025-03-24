using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IRepository<User> _userRepository;

        public UsersController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View(_userRepository.GetAll());
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            user.UserId = Guid.NewGuid().ToString(); // generate a new user id
            if (ModelState.IsValid)
            {
                _userRepository.Add(user);
                _userRepository.SaveChanges();
                TempData["Success"] = "User created successfully!";
                return Ok();
            }
            return BadRequest();
        }

        public IActionResult Edit(string id)
        {
            var user = _userRepository.GetById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, User user)
        {
            var existingUser = _userRepository.GetById(id);
            if (existingUser == null) return NotFound();

            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.UserCategory = user.UserCategory;

            _userRepository.SaveChanges();
            TempData["Success"] = "User updated successfully!";
            return Ok();
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var user = _userRepository.GetById(id);
            if (user == null) return NotFound();
            _userRepository.Delete(user);
            _userRepository.SaveChanges();
            TempData["Success"] = "User Deleted Successfuly";
            return RedirectToAction(nameof(Index));
        }
    }
}
