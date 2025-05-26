﻿﻿﻿﻿﻿using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Controllers
{
    /// <summary>
    /// Controller responsible for user account management including registration, login, logout, email confirmation, and password management.
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
    [AutoValidateAntiforgeryToken]
    // [RequireHttps]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            ILogger<AccountController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Displays the registration form.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new RegisterViewModel());
        }

        /// <summary>
        /// Handles user registration.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Role = model.UserType,
                IsActive = false,
                CreatedAt = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData["ErrorMessage"] = "Registration failed. Please check the errors below.";
                return View(model);
            }

            var roleName = model.UserType.ToString();
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
            await _unitOfWork.SaveChangesAsync();

            await SendEmailConfirmationAsync(user);

            _logger.LogInformation("User {Email} registered successfully.", model.Email);
            TempData["SuccessMessage"] = "Registration successful! Please check your email to confirm your account.";
            return RedirectToAction(nameof(RegisterConfirmation), new { email = model.Email });
        }

        /// <summary>
        /// Displays registration confirmation page.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterConfirmation(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Email is required.";
                return BadRequest();
            }
            ViewData["Title"] = "Registration Complete";
            ViewData["SubTitle"] = "Please check your email to confirm your account.";
            return View(new { Email = email });
        }

        /// <summary>
        /// Confirms user email.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                TempData["ErrorMessage"] = "Invalid email confirmation link.";
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Unable to find user account.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ConfirmEmailAsync(user, decodedCode);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Email confirmed for user {Email}", user.Email);
                    TempData["SuccessMessage"] = "Email confirmed successfully! Please complete your profile.";
                    return RedirectToProfileCompletion(user.Role, user.Email);
                }

                TempData["ErrorMessage"] = "Error confirming email. The link may be invalid or expired.";
                return View("Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming email for user {UserId}", userId);
                TempData["ErrorMessage"] = "An error occurred during email confirmation.";
                return View("Error");
            }
        }

        /// <summary>
        /// Displays login form.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        /// <summary>
        /// Handles user login.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid login attempt. Account does not exist.";
                return View(model);
            }

            if (!user.EmailConfirmed)
            {
                TempData["ErrorMessage"] = "Please confirm your email before logging in.";
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
            bool hasProfile = user.Role == UserType.Customer
                ? await _unitOfWork.Customers.GetByUserIdAsync(user.Id) != null
                : await _unitOfWork.Employees.GetByUserIdAsync(user.Id) != null;

            if (!hasProfile || !user.IsActive)
            {
                TempData["ErrorMessage"] = "Please complete your profile before proceeding.";
                    // log the user out
                    //await _signInManager.SignOutAsync();
                return RedirectToProfileCompletion(user.Role, user.Email ?? string.Empty);
            }


                _logger.LogInformation("User {Email} logged in successfully.", model.Email);
                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User {Email} account locked out.", model.Email);
                TempData["ErrorMessage"] = "Your account is locked out. Please try again later.";
                return RedirectToAction(nameof(Lockout));
            }

            TempData["ErrorMessage"] = "Invalid login attempt. Please check your email or password.";
            return View(model);
        }

        /// <summary>
        /// Handles user logout.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            TempData["SuccessMessage"] = "Logout successful!";
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Displays account lockout page.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            ViewData["Title"] = "Account Locked";
            ViewData["SubTitle"] = "Your account has been locked due to multiple failed login attempts.";
            return View();
        }

        /// <summary>
        /// Displays change password form.
        /// </summary>
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        /// <summary>
        /// Handles password change.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                _logger.LogInformation("User {Email} changed password successfully.", user.Email);
                TempData["SuccessMessage"] = "Password changed successfully!";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            TempData["ErrorMessage"] = "Failed to change password. Please check the errors.";
            return View(model);
        }

        /// <summary>
        /// Displays forgot password form.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Handles forgot password request.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
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
                "ResetPassword",
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

        /// <summary>
        /// Displays forgot password confirmation page.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Displays reset password form.
        /// </summary>
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

        /// <summary>
        /// Handles reset password request.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
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

        /// <summary>
        /// Displays reset password confirmation page.
        /// </summary>
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

        private IActionResult RedirectToProfileCompletion(UserType role, string email)
        {
            var routeValues = new { email = email };
            return role switch
            {
                UserType.Customer => RedirectToAction("CompleteCustomerProfile", "Profile", routeValues),
                UserType.Employee or UserType.Admin or UserType.Owner => RedirectToAction("CompleteEmployeeProfile", "Profile", routeValues),
                _ => RedirectToAction("Index", "Home")
            };
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
    }
}
