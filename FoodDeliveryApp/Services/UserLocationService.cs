using System.Security.Claims;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FoodDeliveryApp.Services
{
    public class UserLocationService : IUserLocationService
    {
        private readonly IMemoryCache _cache;

        public UserLocationService(IMemoryCache cache)
        {
            _cache = cache;
        }

        
        public Task<string> GetUserLocationAsync(ClaimsPrincipal user)
        {
            return Task.FromResult("Unknown, Unknown");
        }

        public Task UpdateUserLocationAsync(ClaimsPrincipal user, string location)
        {
            return Task.CompletedTask;
        }
        
    }
}
