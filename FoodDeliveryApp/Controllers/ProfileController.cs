using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Account;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.ViewModels.Address;

namespace FoodDeliveryApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IFileService fileService,
            IUnitOfWork unitOfWork,
            ILogger<ProfileController> logger)
        {
            _userManager = userManager;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var addresses = await _unitOfWork.Addresses.GetUserAddressesAsync(user.Id);
            var orders = await _unitOfWork.Orders.GetUserOrders(user.Id, o => o.OrderItems, o => o.TrackingHistory).ToListAsync();
            var favorites = await _unitOfWork.Favorites.GetUserFavoritesAsync(user.Id);

            var viewModel = new ProfileViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePictureUrl = user.ProfilePictureUrl,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Addresses = addresses.Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    StreetAddress = a.StreetAddress,
                    City = a.City,
                    State = a.State,
                    PostalCode = a.PostalCode,
                    Country = a.Country,
                    IsDefault = a.IsDefault,
                    AddressType = a.AddressType
                }).ToList(),
                OrderHistory = orders.Select(o => new OrderHistoryViewModel
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.CreatedAt,
                    TotalAmount = o.Total,
                    Status = o.Status,
                    Items = o.OrderItems.Select(oi => new OrderItemViewModel
                    {
                        Id = oi.Id,
                        ItemName = oi.MenuItem?.Name,
                        Quantity = oi.Quantity,
                        Price = oi.Price,
                        SpecialInstructions = oi.SpecialInstructions
                    }).ToList()
                }).ToList(),
                Favorites = favorites.Select(f => new FavoriteViewModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Type = "MenuItem",
                    ImageUrl = f.ImageUrl,
                    Description = f.Description
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile profilePicture)
        {
            if (profilePicture == null || profilePicture.Length == 0)
            {
                TempData["Error"] = "Please select a valid image file.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                var (fileName, filePath) = await _fileService.SaveFileAsync(profilePicture, "profile-pictures");
                user.ProfilePictureUrl = $"/Uploads/profile-pictures/{fileName}";

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Profile picture updated successfully.";
                }
                else
                {
                    TempData["Error"] = "Failed to update profile picture.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile picture");
                TempData["Error"] = "An error occurred while updating profile picture.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var address = new Address
            {
                UserId = user.Id,
                StreetAddress = model.StreetAddress,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                Country = model.Country,
                IsDefault = model.IsDefault,
                AddressType = model.AddressType
            };

            await _unitOfWork.Addresses.AddAsync(address);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Address added successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var address = await _unitOfWork.Addresses.GetByIdAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (address.UserId != user.Id)
            {
                return Forbid();
            }

            await _unitOfWork.Addresses.DeleteAsync(address);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Address deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
} 