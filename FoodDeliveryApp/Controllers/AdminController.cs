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
    [Route("Dashboard")]
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

        [HttpGet("")]
        public async Task<IActionResult> Dashboard()
        {
            var viewModel = new AdminDashboardViewModel();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync("Customer");
                var userViewModels = users.Select(user => new UserViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Role = UserType.Customer,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive
                }).ToList();


                return View(userViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user list");
                TempData["Error"] = "An error occurred while loading the user list.";
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

                var viewModel = new UserViewModel
                {
                    UserId = userId,
                    Email = user.Email,
                    Role = user.Role,
                    IsActive = user.IsActive,
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user {UserId} for editing", userId);
                TempData["Error"] = "An error occurred while loading the user.";
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors in the form.";
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
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("User {UserId} updated successfully.", model.UserId);
                TempData["Success"] = "User updated successfully!";
                return RedirectToAction(nameof(UserList));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", model.UserId);
                TempData["Error"] = "An error occurred while updating the user.";
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

                await _userManager.DeleteAsync(user);

                _logger.LogInformation("User {UserId} deleted successfully.", userId);
                TempData["Success"] = "User deleted successfully!";
                return RedirectToAction(nameof(UserList));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", userId);
                TempData["Error"] = "An error occurred while deleting the user.";
                return RedirectToAction(nameof(UserList));
            }
        }
    }
}