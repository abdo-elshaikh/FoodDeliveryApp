using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodDeliveryApp.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public ApplicationUser GetCurrentUser()
        {
            return _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result ?? null;
        }

        public string GetCurrentUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public string GetCurrentUsername()
        {
            var username = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            return string.IsNullOrEmpty(username) ? "system" : username;
        }

        public bool IsAdmin()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role) == "Admin";
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }

        public bool IsDriver()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role) == "Driver";
        }

        public bool IsOwner()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role) == "Owner";
        }
    }
}
