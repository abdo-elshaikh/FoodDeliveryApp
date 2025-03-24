// Controllers/OrdersController.cs
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories;

namespace FoodDeliveryApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Customer> _customerRepository;

        public OrdersController(IRepository<Order> orderRepository, IRepository<Employee> employeeRepository, IRepository<Customer> customerRepository)
        {
            _orderRepository = orderRepository;
            _employeeRepository = employeeRepository;
            _customerRepository = customerRepository;
        }

        public IActionResult Index()
        {
            return View(_orderRepository.GetAll());
        }

        public IActionResult Create()
        {
            ViewBag.Employees = _employeeRepository.GetAll();
            ViewBag.Customers = _customerRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.Add(order);
                _orderRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Employees = _employeeRepository.GetAll();
            ViewBag.Customers = _customerRepository.GetAll();
            return View(order);
        }

        public IActionResult Edit(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null) return NotFound();
            ViewBag.Employees = _employeeRepository.GetAll();
            ViewBag.Customers = _customerRepository.GetAll();
            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(int id, Order order)
        {
            if (id != order.OrdId) return NotFound();

            if (ModelState.IsValid)
            {
                _orderRepository.Update(order);
                _orderRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Employees = _employeeRepository.GetAll();
            ViewBag.Customers = _customerRepository.GetAll();
            return View(order);
        }

        public IActionResult Delete(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _orderRepository.GetById(id);
            _orderRepository.Delete(order);
            _orderRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}