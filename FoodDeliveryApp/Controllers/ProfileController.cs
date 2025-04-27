using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Address = FoodDeliveryApp.Models.Address;

namespace FoodDeliveryApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            IRepository<Address> addressRepository,
            IWebHostEnvironment webHostEnvironment,
            ILogger<ProfileController> logger)
        {
            _userManager = userManager;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _addressRepository = addressRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        #region Customer Profile Actions

        [HttpGet("complete-customer-profile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CompleteCustomerProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var existingProfile = await _customerRepository.GetByUserIdAsync(userId);

                if (existingProfile != null)
                {
                    return RedirectToAction(nameof(CustomerProfile));
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                var viewModel = new CustomerProfileViewModel
                {
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = DateTime.Today.AddYears(-18) // Default to 18 years ago
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading complete customer profile page");
                return StatusCode(500);
            }
        }

        [HttpPost("complete-customer-profile")]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteCustomerProfile(CustomerProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return NotFound();

                var profilePicturePath = model.ProfilePicture != null
                    ? await SaveProfilePictureAsync(model.ProfilePicture)
                    : null;

                var customer = new CustomerProfile
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    DateOfBirth = model.DateOfBirth,
                    ProfilePictureUrl = profilePicturePath,
                    ReceivePromotions = model.ReceivePromotions,
                    LoyaltyPoints = 0, // Default loyalty points
                    CreatedAt = DateTime.UtcNow,
                    UserId = userId,
                    IsActive = true,
                };

                await _customerRepository.AddAsync(customer);

                TempData["SuccessMessage"] = "Profile completed successfully!";
                return RedirectToAction(nameof(CustomerProfile));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing customer profile");
                ModelState.AddModelError("", "An error occurred while saving your profile.");
                return View(model);
            }
        }

        [HttpGet("customer-profile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = await _customerRepository.GetByUserIdAsync(userId);

                if (customer == null)
                {
                    return RedirectToAction(nameof(CompleteCustomerProfile));
                }

                var viewModel = new CustomerProfileViewModel
                {
                    CustomerId = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PhoneNumber = customer.PhoneNumber,
                    DateOfBirth = customer.DateOfBirth ?? DateTime.Today.AddYears(-18),
                    ProfilePicturePath = customer.ProfilePictureUrl,
                    ReceivePromotions = customer.ReceivePromotions,
                    LoyaltyPoints = customer.LoyaltyPoints
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customer profile");
                return StatusCode(500);
            }
        }

        [HttpGet("edit-customer-profile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> EditCustomerProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = await _customerRepository.GetByUserIdAsync(userId);

                if (customer == null)
                {
                    return RedirectToAction(nameof(CompleteCustomerProfile));
                }

                var viewModel = new CustomerProfileViewModel
                {
                    CustomerId = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PhoneNumber = customer.PhoneNumber,
                    DateOfBirth = customer.DateOfBirth ?? DateTime.Today.AddYears(-18),
                    ProfilePicturePath = customer.ProfilePictureUrl,
                    ReceivePromotions = customer.ReceivePromotions
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit customer profile page");
                return StatusCode(500);
            }
        }

        [HttpPost("edit-customer-profile")]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomerProfile(CustomerProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = await _customerRepository.GetByUserIdAsync(userId);

                if (customer == null)
                {
                    return RedirectToAction(nameof(CompleteCustomerProfile));
                }

                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.PhoneNumber = model.PhoneNumber;
                customer.DateOfBirth = model.DateOfBirth;
                customer.ReceivePromotions = model.ReceivePromotions;

                if (model.ProfilePicture != null)
                {
                    customer.ProfilePictureUrl = await SaveProfilePictureAsync(model.ProfilePicture);
                }

                await _customerRepository.UpdateAsync(customer);

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction(nameof(CustomerProfile));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer profile");
                ModelState.AddModelError("", "An error occurred while updating your profile.");
                return View(model);
            }
        }

        #endregion

        #region Employee Profile Actions

        [HttpGet("complete-employee-profile")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CompleteEmployeeProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var existingProfile = await _employeeRepository.GetByUserIdAsync(userId);

                if (existingProfile != null)
                {
                    return RedirectToAction(nameof(EmployeeProfile));
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                var viewModel = new EmployeeProfileViewModel
                {
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    HireDate = DateTime.Today
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading complete employee profile page");
                return StatusCode(500);
            }
        }

        [HttpPost("complete-employee-profile")]
        [Authorize(Roles = "Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteEmployeeProfile(EmployeeProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return NotFound();

                var profilePicturePath = model.ProfilePicture != null
                    ? await SaveProfilePictureAsync(model.ProfilePicture)
                    : null;

                var employee = new EmployeeProfile
                {
                    UserId = userId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    HireDate = model.HireDate,
                    Position = model.Position,
                    ProfilePictureUrl = profilePicturePath,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _employeeRepository.AddAsync(employee);

                TempData["SuccessMessage"] = "Profile completed successfully!";
                return RedirectToAction(nameof(EmployeeProfile));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing employee profile");
                ModelState.AddModelError("", "An error occurred while saving your profile.");
                return View(model);
            }
        }

        [HttpGet("employee-profile")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> EmployeeProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var employee = await _employeeRepository.GetByUserIdAsync(userId);

                if (employee == null)
                {
                    return RedirectToAction(nameof(CompleteEmployeeProfile));
                }

                var viewModel = new EmployeeProfileViewModel
                {
                    EmployeeId = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    PhoneNumber = employee.PhoneNumber,
                    HireDate = employee.HireDate,
                    Position = employee.Position,
                    ProfilePicturePath = employee.ProfilePictureUrl
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading employee profile");
                return StatusCode(500);
            }
        }

        [HttpGet("edit-employee-profile")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> EditEmployeeProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var employee = await _employeeRepository.GetByUserIdAsync(userId);

                if (employee == null)
                {
                    return RedirectToAction(nameof(CompleteEmployeeProfile));
                }

                var viewModel = new EmployeeProfileViewModel
                {
                    EmployeeId = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    PhoneNumber = employee.PhoneNumber,
                    HireDate = employee.HireDate,
                    Position = employee.Position,
                    ProfilePicturePath = employee.ProfilePictureUrl
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit employee profile page");
                return StatusCode(500);
            }
        }

        [HttpPost("edit-employee-profile")]
        [Authorize(Roles = "Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployeeProfile(EmployeeProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var employee = await _employeeRepository.GetByUserIdAsync(userId);

                if (employee == null)
                {
                    return RedirectToAction(nameof(CompleteEmployeeProfile));
                }

                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.PhoneNumber = model.PhoneNumber;
                employee.Position = model.Position;

                if (model.ProfilePicture != null)
                {
                    employee.ProfilePictureUrl = await SaveProfilePictureAsync(model.ProfilePicture);
                }

                await _employeeRepository.UpdateAsync(employee);

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction(nameof(EmployeeProfile));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee profile");
                ModelState.AddModelError("", "An error occurred while updating your profile.");
                return View(model);
            }
        }

        #endregion

        #region Address Management

        [HttpGet("addresses")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ViewAddresses()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = await _customerRepository.GetByUserIdAsync(userId);
                if (customer == null) return NotFound();

                var addresses = await _addressRepository.FindAsync(a => a.Id == customer.Id);

                var viewModel = new AddressListViewModel
                {
                    CustomerId = customer.Id,
                    Addresses = addresses.Select(a => new AddressViewModel
                    {
                        AddressId = a.Id,
                        AddressName = a.Title,
                        Street = a.Street,
                        City = a.City,
                        State = a.State,
                        ZipCode = a.PostalCode,
                        IsDefault = a.IsDefault
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading addresses");
                return StatusCode(500);
            }
        }

        [HttpGet("add-address")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddAddress()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = await _customerRepository.GetByUserIdAsync(userId);
                if (customer == null) return NotFound();

                return View(new AddressViewModel { CustomerId = customer.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading add address page");
                return StatusCode(500);
            }
        }

        [HttpPost("add-address")]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var address = new Address
                {
                    Title = model.AddressName,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    PostalCode = model.ZipCode,
                    IsDefault = model.IsDefault,
                    Id = model.CustomerId,
                    CreatedAt = DateTime.UtcNow
                };

                // If setting as default, unset any existing default address
                if (model.IsDefault)
                {
                    var existingDefaults = await _addressRepository.FindAsync(a =>
                        a.Id == model.CustomerId && a.IsDefault);

                    foreach (var existing in existingDefaults)
                    {
                        existing.IsDefault = false;
                        await _addressRepository.UpdateAsync(existing);
                    }
                }

                await _addressRepository.AddAsync(address);

                TempData["SuccessMessage"] = "Address added successfully!";
                return RedirectToAction(nameof(ViewAddresses));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding address");
                ModelState.AddModelError("", "An error occurred while adding the address.");
                return View(model);
            }
        }

        [HttpGet("edit-address/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> EditAddress(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = await _customerRepository.GetByUserIdAsync(userId);
                if (customer == null) return NotFound();

                var address = await _addressRepository.GetByIdAsync(id);
                if (address == null || address.Id != customer.Id)
                {
                    return NotFound();
                }

                var viewModel = new AddressViewModel
                {
                    AddressId = address.Id,
                    AddressName = address.Title,
                    Street = address.Street,
                    City = address.City,
                    State = address.State,
                    ZipCode = address.PostalCode,
                    IsDefault = address.IsDefault,
                    CustomerId = customer.Id
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit address page");
                return StatusCode(500);
            }
        }

        [HttpPost("edit-address/{id}")]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(int id, AddressViewModel model)
        {
            if (id != model.AddressId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = await _customerRepository.GetByUserIdAsync(userId);
                if (customer == null) return NotFound();

                var address = await _addressRepository.GetByIdAsync(id);
                if (address == null || address.Id != customer.Id)
                {
                    return NotFound();
                }

                // If setting as default, unset any existing default address
                if (model.IsDefault && !address.IsDefault)
                {
                    var existingDefaults = await _addressRepository.FindAsync(a =>
                        a.Id == model.CustomerId && a.IsDefault);

                    foreach (var existing in existingDefaults)
                    {
                        existing.IsDefault = false;
                        await _addressRepository.UpdateAsync(existing);
                    }
                }

                address.Title = model.AddressName;
                address.Street = model.Street;
                address.City = model.City;
                address.State = model.State;
                address.PostalCode = model.ZipCode;
                address.IsDefault = model.IsDefault;
                address.UpdatedAt = DateTime.UtcNow;

                await _addressRepository.UpdateAsync(address);

                TempData["SuccessMessage"] = "Address updated successfully!";
                return RedirectToAction(nameof(ViewAddresses));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating address");
                ModelState.AddModelError("", "An error occurred while updating the address.");
                return View(model);
            }
        }

        [HttpPost("delete-address/{id}")]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = await _customerRepository.GetByUserIdAsync(userId);
                if (customer == null) return NotFound();

                var address = await _addressRepository.GetByIdAsync(id);
                if (address == null || address.Id != customer.Id)
                {
                    return NotFound();
                }

                await _addressRepository.RemoveAsync(address);

                TempData["SuccessMessage"] = "Address deleted successfully!";
                return RedirectToAction(nameof(ViewAddresses));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting address");
                TempData["ErrorMessage"] = "An error occurred while deleting the address.";
                return RedirectToAction(nameof(ViewAddresses));
            }
        }

        #endregion

        #region Private Helpers

        private async Task<string> SaveProfilePictureAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // Validate file type and size
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Invalid file type. Only image files are allowed.");
            }

            if (file.Length > 5 * 1024 * 1024) // 5MB limit
            {
                throw new InvalidOperationException("File size exceeds the maximum limit of 5MB.");
            }

            // Create unique filename
            var fileName = $"{Guid.NewGuid()}{extension}";
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profile-pictures");

            // Ensure directory exists
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/profile-pictures/{fileName}";
        }

        #endregion
    }
}