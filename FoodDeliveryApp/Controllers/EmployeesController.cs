// Controllers/EmployeesController.cs
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories;

namespace FoodDeliveryApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<User> _userRepository;

        public EmployeesController(IRepository<Employee> employeeRepository, IRepository<User> userRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View(_employeeRepository.GetAll());
        }

        public IActionResult Create()
        {
            ViewBag.Users = _userRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = _userRepository.GetAll();
            return View(employee);
        }

        public IActionResult Edit(string id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee == null) return NotFound();
            ViewBag.Users = _userRepository.GetAll();
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(string id, Employee employee)
        {
            if (id != employee.EmpId) return NotFound();

            if (ModelState.IsValid)
            {
                _employeeRepository.Update(employee);
                _employeeRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = _userRepository.GetAll();
            return View(employee);
        }

        public IActionResult Delete(string id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            var employee = _employeeRepository.GetById(id);
            _employeeRepository.Delete(employee);
            _employeeRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}