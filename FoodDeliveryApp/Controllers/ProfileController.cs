using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.ViewModels.Profile;
using FoodDeliveryApp.ViewModels.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodDeliveryApp.ViewModels.ProfileViewModels;

namespace FoodDeliveryApp.Controllers
{
    //[Authorize]
    [Route("[controller]/[action]")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment,
            ILogger<ProfileController> logger)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        #region Customer Profile Actions

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CompleteCustomerProfile(string email)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }
                
                var existingProfile = await _unitOfWork.Customers.GetByUserIdAsync(userId);

                if (existingProfile != null)
                {
                    return RedirectToAction(nameof(CustomerProfile));
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var viewModel = new CustomerProfileViewModel
                {
                    Email = email ?? user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    DateOfBirth = DateTime.Today.AddYears(-18),
                    FirstName = string.Empty,
                    LastName = string.Empty
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading complete customer profile page for user {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "An error occurred while loading the profile page.";
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteCustomerProfile(CustomerProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }
                
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                string? profilePicturePath = null;
                if (model.ProfilePicture != null)
                {
                    profilePicturePath = await SaveProfilePictureAsync(model.ProfilePicture);
                }

                var customer = new CustomerProfile
                {
                    UserId = userId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    DateOfBirth = model.DateOfBirth,
                    ProfilePictureUrl = profilePicturePath,
                    ReceivePromotions = model.ReceivePromotions,
                    LoyaltyPoints = 0,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await _unitOfWork.Customers.AddAsync(customer);
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Customer profile created for user {UserId}", userId);
                TempData["SuccessMessage"] = "Profile completed successfully! Please add a delivery address.";
                return RedirectToAction(nameof(AddAddress));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing customer profile for user {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "An error occurred while saving your profile.";
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }
                
                var customer = await _unitOfWork.Customers.GetByUserIdAsync(userId);

                if (customer == null)
                {
                    return RedirectToAction(nameof(CompleteCustomerProfile));
                }

                var user = await _userManager.FindByIdAsync(userId);
                
                var viewModel = new CustomerProfileViewModel
                {
                    CustomerId = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = user?.Email ?? string.Empty,
                    PhoneNumber = customer.PhoneNumber,
                    DateOfBirth = customer.DateOfBirth ?? DateTime.Today.AddYears(-18),
                    ProfilePicturePath = customer.ProfilePictureUrl,
                    ReceivePromotions = customer.ReceivePromotions,
                    LoyaltyPoints = (int)customer.LoyaltyPoints
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customer profile for user {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "An error occurred while loading your profile.";
                return StatusCode(500);
            }
        }

        #region Address Management

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddAddress()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }
                
                var customer = await _unitOfWork.Customers.GetByUserIdAsync(userId);
                if (customer == null)
                {
                    return NotFound("Customer profile not found.");
                }



                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading add address page for user {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "An error occurred while loading the add address page.";
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(ViewModels.Address.AddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }
                
                var customer = await _unitOfWork.Customers.GetByUserIdAsync(userId);
                if (customer == null)
                {
                    return NotFound("Customer profile not found.");
                }

                var address = new Address
                {
                    Title = model.AddressLine,
                    Street = model.AddressLine,
                    City = model.City,
                    State = model.State,
                    PostalCode = model.PostalCode,
                    IsDefault = model.IsDefault,
                    Id = customer.Id,
                    CreatedAt = DateTime.UtcNow
                };

                if (model.IsDefault)
                {
                    var existingDefaults = await _unitOfWork.Addresses.FindAsync(a => a.Id == customer.Id && a.IsDefault);
                    foreach (var existing in existingDefaults)
                    {
                        existing.IsDefault = false;
                        await _unitOfWork.Addresses.UpdateAsync(existing);
                    }
                }

                await _unitOfWork.Addresses.AddAsync(address);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Address added for customer {CustomerId}", customer.Id);
                TempData["SuccessMessage"] = "Address added successfully! You're ready to start ordering.";
                return RedirectToAction(nameof(CustomerProfile));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding address for user {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "An error occurred while adding the address.";
                return View(model);
            }
        }

        #endregion

        #endregion

        #region Employee Profile Actions

        [HttpGet]
        [Authorize(Roles = "Employee,Admin,Owner")]
        public async Task<IActionResult> CompleteEmployeeProfile(string email)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }
                
                var existingProfile = await _unitOfWork.Employees.GetByUserIdAsync(userId);
                if (existingProfile != null)
                {
                    return RedirectToAction(nameof(EmployeeProfile));
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var viewModel = new EmployeeProfileViewModel
                {
                    Email = email ?? user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    HireDate = DateTime.Today,
                    FirstName = string.Empty,
                    LastName = string.Empty
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading complete employee profile page for user {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "An error occurred while loading the profile page.";
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Employee,Admin,Owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteEmployeeProfile(EmployeeProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }
                
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                string? profilePicturePath = null;
                if (model.ProfilePicture != null)
                {
                    profilePicturePath = await SaveProfilePictureAsync(model.ProfilePicture);
                }

                var employee = new EmployeeProfile
                {
                    UserId = userId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Position = model.Position,
                    HireDate = model.HireDate,
                    TerminationDate = model.TerminationDate,
                    ProfilePictureUrl = profilePicturePath,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await _unitOfWork.Employees.AddAsync(employee);
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Employee profile created for user {UserId}", userId);
                TempData["SuccessMessage"] = "Profile completed successfully!";
                return RedirectToAction(nameof(EmployeeProfile));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing employee profile for user {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "An error occurred while saving your profile.";
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Employee,Admin,Owner")]
        public async Task<IActionResult> EmployeeProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }
                
                var employee = await _unitOfWork.Employees.GetByUserIdAsync(userId);
                if (employee == null)
                {
                    return RedirectToAction(nameof(CompleteEmployeeProfile));
                }

                var user = await _userManager.FindByIdAsync(userId);
                
                var viewModel = new EmployeeProfileViewModel
                {
                    EmployeeId = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = user?.Email ?? string.Empty,
                    PhoneNumber = employee.PhoneNumber,
                    Position = employee.Position,
                    HireDate = employee.HireDate,
                    TerminationDate = employee.TerminationDate,
                    ProfilePicturePath = employee.ProfilePictureUrl
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading employee profile for user {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "An error occurred while loading your profile.";
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Employee,Admin,Owner")]
        public async Task<IActionResult> ViewEmployeeTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var employeeProfile = await _unitOfWork.Employees.GetByUserIdAsync(userId);
            if (employeeProfile == null)
            {
                TempData["ErrorMessage"] = "Employee profile not found. Please complete your profile.";
                return RedirectToAction(nameof(CompleteEmployeeProfile));
            }

            // TODO: Implement GetOrdersAssignedToEmployeeAsync(employeeProfile.Id) in IOrderRepository and OrderRepository
            // For now, returning an empty list of tasks to allow build to pass.
            // var assignedOrders = await _unitOfWork.Orders.GetOrdersAssignedToEmployeeAsync(employeeProfile.Id);
            var assignedOrders = new List<Order>(); // Placeholder

            var tasksViewModel = new EmployeeTasksViewModel
            {
                EmployeeId = employeeProfile.Id,
                EmployeeName = $"{employeeProfile.FirstName} {employeeProfile.LastName}",
                Tasks = assignedOrders.Select(order => new EmployeeTaskViewModel
                {
                    OrderId = order.Id,
                    OrderNumber = order.Id.ToString(), // Or a dedicated OrderNumber field
                    CustomerName = order.User?.CustomerProfile != null ? $"{order.User.CustomerProfile.FirstName} {order.User.CustomerProfile.LastName}" : order.User?.UserName ?? "N/A",
                    Status = order.Status,
                    AssignedDate = order.OrderDate, // Assuming UpdatedAt might be when it was assigned or last status change
                    DeliveryAddress = order.Address != null ? $"{order.Address.Street}, {order.Address.City}" : "N/A",
                    EstimatedDeliveryTime = order.EstimatedDeliveryTime
                }).ToList()
            };

            return View(tasksViewModel);
        }

        #endregion

        private async Task<string?> SaveProfilePictureAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");
            
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
                
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/profiles/{fileName}";
        }
    }
}