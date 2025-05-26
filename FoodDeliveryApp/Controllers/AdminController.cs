using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            try
            {
                var customers = await _unitOfWork.Customers.GetAllAsync();
                var employees = await _unitOfWork.Employees.GetAllAsync();

                var userViewModels = customers.Select(c => new UserViewModel
                {
                    UserId = c.UserId,
                    Email = _userManager.FindByIdAsync(c.UserId).Result?.Email,
                    Role = UserType.Customer,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    IsActive = _userManager.FindByIdAsync(c.UserId).Result?.IsActive ?? false
                }).Concat(employees.Select(e => new UserViewModel
                {
                    UserId = e.UserId,
                    Email = _userManager.FindByIdAsync(e.UserId).Result?.Email,
                    Role = UserType.Employee,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    PhoneNumber = e.PhoneNumber,
                    IsActive = _userManager.FindByIdAsync(e.UserId).Result?.IsActive ?? false
                })).ToList();

                return View(userViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user list");
                TempData["ErrorMessage"] = "An error occurred while loading the user list.";
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var customer = await _unitOfWork.Customers.GetByUserIdAsync(userId);
                var employee = await _unitOfWork.Employees.GetByUserIdAsync(userId);

                var viewModel = new UserViewModel
                {
                    UserId = userId,
                    Email = user.Email,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    FirstName = customer?.FirstName ?? employee?.FirstName,
                    LastName = customer?.LastName ?? employee?.LastName,
                    PhoneNumber = customer?.PhoneNumber ?? employee?.PhoneNumber
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user {UserId} for editing", userId);
                TempData["ErrorMessage"] = "An error occurred while loading the user.";
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(model);
            }

            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.IsActive = model.IsActive;
                await _userManager.UpdateAsync(user);

                if (model.Role == UserType.Customer)
                {
                    var customer = await _unitOfWork.Customers.GetByUserIdAsync(model.UserId);
                    if (customer != null)
                    {
                        customer.FirstName = model.FirstName;
                        customer.LastName = model.LastName;
                        customer.PhoneNumber = model.PhoneNumber;
                        await _unitOfWork.Customers.UpdateAsync(customer);
                    }
                }
                else
                {
                    var employee = await _unitOfWork.Employees.GetByUserIdAsync(model.UserId);
                    if (employee != null)
                    {
                        employee.FirstName = model.FirstName;
                        employee.LastName = model.LastName;
                        employee.PhoneNumber = model.PhoneNumber;
                        await _unitOfWork.Employees.UpdateAsync(employee);
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("User {UserId} updated successfully.", model.UserId);
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction(nameof(UserList));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", model.UserId);
                TempData["ErrorMessage"] = "An error occurred while updating the user.";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var customer = await _unitOfWork.Customers.GetByUserIdAsync(userId);
                var employee = await _unitOfWork.Employees.GetByUserIdAsync(userId);

                if (customer != null)
                {
                    await _unitOfWork.Customers.RemoveAsync(customer);
                }
                else if (employee != null)
                {
                    await _unitOfWork.Employees.RemoveAsync(employee);
                }

                await _userManager.DeleteAsync(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("User {UserId} deleted successfully.", userId);
                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction(nameof(UserList));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", userId);
                TempData["ErrorMessage"] = "An error occurred while deleting the user.";
                return RedirectToAction(nameof(UserList));
            }
        }
    }
}