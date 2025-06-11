using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.ViewModels.Account;
using FoodDeliveryApp.ViewModels.Address;
using FoodDeliveryApp.ViewModels.Order;
using FoodDeliveryApp.ViewModels.Restaurant;
using FoodDeliveryApp.ViewModels.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Encodings.Web;

namespace FoodDeliveryApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            IFileService fileService,
            ILogger<AccountController> logger
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileService = fileService?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null, CancellationToken cancellationToken = default)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors in the form.");
                return View(model);
            }

            try
            {
                // Check for existing user
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email is already registered.");
                    return View(model);
                }
                
                // Create user
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Role = model.Role,
                    IsActive = model.Role == UserType.Customer? true : false,
                    CreatedAt = DateTime.UtcNow,
                    Notes = $"Created by {model.FirstName} {model.LastName} on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}.",
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    ModelState.AddModelError(string.Empty, "Registration failed. Please check the errors below.");
                    return View(model);
                }

                // Create and assign role
                var roleName = model.Role.ToString();
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        _logger.LogError("Failed to create role {RoleName}", roleName);
                        ModelState.AddModelError(string.Empty, "Failed to create user role.");
                        return View(model);
                    }
                }

                await _userManager.AddToRoleAsync(user, roleName);

                // Send confirmation email
                try
                {
                    await SendEmailConfirmationAsync(user);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send confirmation email to user {Email}. Deleting user.", model.Email);
                    // Delete the user if email sending fails
                    var deleteResult = await _userManager.DeleteAsync(user);
                    if (!deleteResult.Succeeded)
                    {
                        _logger.LogError("Failed to delete user {Email} after email sending failure.", model.Email);
                    }
                    ModelState.AddModelError(string.Empty, "Registration failed: unable to send confirmation email. Please try again later.");
                    return View(model);
                }

                // Save all changes
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("User {Email} registered successfully with role {Role}", model.Email, model.Role);
                TempData["Success"] = "Registration successful! Please check your email to confirm your account.";
                return RedirectToAction(nameof(RegisterConfirmation), new { email = model.Email });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for user {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred during registration. Please try again later.");
                return View(model);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirmation(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Email is required.";
                return BadRequest();
            }

            ViewData["Title"] = "Registration Complete";
            ViewData["SubTitle"] = "Please check your email to confirm your account.";
            return View(new { Email = email });
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                TempData["Error"] = "Invalid email confirmation link.";
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Unable to find user account.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ConfirmEmailAsync(user, decodedCode);
                if (result.Succeeded)
                {
                    await _unitOfWork.Carts.CreateUserCartAsync(user.Id.ToString());

                    _logger.LogInformation("Email confirmed for user {Email}", user.Email);
                    TempData["Success"] = "Email confirmed successfully!";
                    return RedirectToAction("Index", "Home");
                }

                TempData["Error"] = "Error confirming email. The link may be invalid or expired.";
                return View("Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming email for user {UserId}", userId);
                TempData["Error"] = "An error occurred during email confirmation.";
                return View("Error");
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null, CancellationToken cancellationToken = default)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors in the form.");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Account does not exist.");
                return View(model);
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Please confirm your email before logging in.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                if (!user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "Your account is not active.");
                    return View(model);
                }

                _logger.LogInformation("User {Email} logged in successfully.", model.Email);
                TempData["Success"] = "Login successful!";
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User {Email} account locked out.", model.Email);
                ModelState.AddModelError(string.Empty, "Your account is locked out. Please try again later.");
                return RedirectToAction(nameof(Lockout));
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email or password.");
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken = default)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            TempData["Success"] = "Logout successful!";
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            ViewData["Title"] = "Account Locked";
            ViewData["SubTitle"] = "Your account has been locked due to multiple failed login attempts.";
            return View();
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors in the form.");
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                _logger.LogInformation("User {Email} changed password successfully.", user.Email);
                TempData["Success"] = "Password changed successfully!";
                return RedirectToAction(nameof(Index), "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            ModelState.AddModelError(string.Empty, "Failed to change password. Please check the errors.");
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action(
                nameof(ResetPassword),
                "Account",
                new { userId = user.Id, code = encodedCode },
                protocol: Request.Scheme);

            if (string.IsNullOrEmpty(callbackUrl))
            {
                _logger.LogError("Could not generate password reset callback URL for user {UserId}.", user.Id);
                return View("Error");
            }

            await _emailSender.SendEmailAsync(
                model.Email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await _userManager.ResetPasswordAsync(user, decodedCode, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #region Helpers

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task SendEmailConfirmationAsync(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action(
                nameof(ConfirmEmail),
                "Account",
                new { userId = user.Id, code = encodedCode },
                protocol: Request.Scheme);

            if (string.IsNullOrEmpty(callbackUrl))
            {
                _logger.LogError("Could not generate email confirmation callback URL for user {UserId}.", user.Id);
                return;
            }

            await _emailSender.SendEmailAsync(
                user?.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }

        #endregion


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return View(user);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(ApplicationUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Email = model.Email;
            user.UserName = model.Email;
            user.Role = model.Role;
            user.IsActive = model.IsActive;

            var emailToken = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
            var emailChangeResult = await _userManager.ChangeEmailAsync(user, model.Email, emailToken);
            if (!emailChangeResult.Succeeded)
            {
                foreach (var error in emailChangeResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            _logger.LogInformation("User {Email} updated successfully.", model.Email);
            TempData["Success"] = "User updated successfully.";
            return RedirectToAction(nameof(ManageUsers));
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserProfile(string activeTab = "profile-info")
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogWarning("User not found when accessing UserProfile");
                    return NotFound("User not found.");
                }

                // Load user addresses
                var userAddresses = await _unitOfWork.Addresses.GetUserAddressesAsync(user.Id);
                var selectedAddress = userAddresses.FirstOrDefault(a => a.IsDefault);

                // Load user reviews
                var userReviews = await _unitOfWork.Reviews.GetByUserIdAsync(user.Id);

                // Load user orders with related data
                var userOrders = _unitOfWork.Orders.GetUserOrders(user.Id, o => o.OrderItems, o => o.TrackingHistory)
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();

                // Load restaurants owned by the user
                var restaurants = (await _unitOfWork.Restaurants.GetAllAsync())
                    .Where(r => r.IsOwner(user.Id))
                    .ToList();

                // Create and populate the view model
                var model = new UserProfileViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    ExistingProfilePicturePath = user.ProfilePictureUrl,
                    AccountCreatedDate = user.CreatedAt,
                    LastLoginDate = user.DateOfBirth ?? DateTime.Now ,
                    ActiveTab = activeTab,
                    Addresses = userAddresses.Select(a => new AddressViewModel
                    {
                        Id = a.Id,
                        StreetAddress = a.StreetAddress,
                        City = a.City,
                        State = a.State,
                        Country = a.Country,
                        PostalCode = a.PostalCode,
                        IsDefault = a.IsDefault
                    }).ToList(),
                    SelectedAddressId = selectedAddress?.Id,
                    Reviews = userReviews.Select(r => new RestaurantReviewViewModel
                    {
                        Id = r.Id,
                        RestaurantName = r.Restaurant.Name,
                        Rating = (double)r.Rating,
                        Content = r.Content,
                        UserName = r.User.UserName,
                        CreatedAt = r.CreatedAt,
                    }).ToList(),
                    Orders = userOrders.Select(o => new OrderSummaryViewModel
                    {
                        OrderId = o.Id,
                        OrderNumber = o.OrderNumber,
                        CreatedAt = o.OrderDate,
                        Status = o.Status,
                        TotalAmount = o.Total,
                        PaymentMethod = o.PaymentMethod,
                    }).ToList(),
                    Restaurants = restaurants.Select(r => new RestaurantViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        PhoneNumber = r.PhoneNumber,
                        ImageUrl = r.ImageUrl,
                    }).ToList(),
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user profile");
                TempData["Error"] = "An error occurred while loading your profile. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload related data to properly display the form again
                await ReloadUserProfileRelatedData(model);
                return View("UserProfile", model);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogWarning("User not found when updating profile");
                    return Challenge();
                }

                // Handle email change if needed
                if (user.Email != model.Email)
                {
                    var emailToken = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                    var emailChangeResult = await _userManager.ChangeEmailAsync(user, model.Email, emailToken);
                    if (!emailChangeResult.Succeeded)
                    {
                        foreach (var error in emailChangeResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        await ReloadUserProfileRelatedData(model);
                        return View("UserProfile", model);
                    }
                }

                // Update user properties
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;

                // Handle profile picture upload
                if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
                {
                    try
                    {
                        var (fileName, filePath) = await _fileService.SaveFileAsync(model.ProfilePicture, "ProfilePictures");
                        user.ProfilePictureUrl = filePath;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error uploading profile picture for user {UserId}", user.Id);
                        ModelState.AddModelError("ProfilePicture", "Failed to upload profile picture. Please try again.");
                        await ReloadUserProfileRelatedData(model);
                        return View("UserProfile", model);
                    }
                }

                // Save user changes
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    await ReloadUserProfileRelatedData(model);
                    return View("UserProfile", model);
                }

                // Update default address if selected
                if (model.SelectedAddressId.HasValue)
                {
                    await _unitOfWork.Addresses.SetDefaultAddressAsync( model.SelectedAddressId.Value, user.Id);
                    await _unitOfWork.SaveChangesAsync();
                }

                _logger.LogInformation("User {Email} updated their profile successfully", model.Email);
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction(nameof(UserProfile), new { activeTab = model.ActiveTab });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile");
                ModelState.AddModelError(string.Empty, "An error occurred while updating your profile. Please try again.");
                await ReloadUserProfileRelatedData(model);
                return View("UserProfile", model);
            }
        }

        // Helper method to reload related data for the user profile
        private async Task ReloadUserProfileRelatedData(UserProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return;

            var userAddresses = await _unitOfWork.Addresses.GetUserAddressesAsync(user.Id);
            var userReviews = await _unitOfWork.Reviews.GetByUserIdAsync(user.Id);
            var userOrders = _unitOfWork.Orders.GetUserOrders(user.Id, o => o.OrderItems, o => o.TrackingHistory);
            var restaurants = (await _unitOfWork.Restaurants.GetAllAsync())
                .Where(r => r.IsOwner(user.Id))
                .ToList();

            model.Addresses = userAddresses.Select(a => new AddressViewModel
            {
                Id = a.Id,
                StreetAddress = a.StreetAddress,
                City = a.City,
                State = a.State,
                Country = a.Country,
                PostalCode = a.PostalCode,
                IsDefault = a.IsDefault
            }).ToList();

            model.Reviews = userReviews.Select(r => new RestaurantReviewViewModel
            {
                Id = r.Id,
                RestaurantName = r.Restaurant.Name,
                Rating = (double)r.Rating,
                Content = r.Content,
                UserName = r.User.UserName,
                CreatedAt = r.CreatedAt,
            }).ToList();

            model.Orders = userOrders.Select(o => new OrderSummaryViewModel
            {
                OrderId = o.Id,
                OrderNumber = o.OrderNumber,
                CreatedAt = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.Total,
                PaymentMethod = o.PaymentMethod,
                
            }).ToList();

            model.Restaurants = restaurants.Select(r => new RestaurantViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                PhoneNumber = r.PhoneNumber,
                ImageUrl = r.ImageUrl,
            }).ToList();

            model.ExistingProfilePicturePath = user.ProfilePictureUrl;
            model.AccountCreatedDate = user.CreatedAt;
            model.LastLoginDate = user.DateOfBirth ?? user.CreatedAt;
        }
    }
}
