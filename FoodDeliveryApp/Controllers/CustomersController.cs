using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories;

namespace FoodDeliveryApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<User> _userRepository;

        public CustomersController(IRepository<Customer> customerRepository, IRepository<User> userRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            var customers = _customerRepository.GetAll();
            return View(customers);
        }

        public IActionResult Create()
        {
            ViewBag.Users = _userRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _customerRepository.Add(customer);
                _customerRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public IActionResult Edit(int id)
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null) return NotFound();
            ViewBag.Users = _userRepository.GetAll();
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(int id, Customer customer)
        {
            if (id != customer.CustomerId) return NotFound();
            if (ModelState.IsValid)
            {
                _customerRepository.Update(customer);
                _customerRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = _userRepository.GetAll();
            return View(customer);
        }

        public IActionResult Delete(int id) {
            var customer = _customerRepository.GetById(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _customerRepository.GetById(id);
            _customerRepository.Delete(customer);
            _customerRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
