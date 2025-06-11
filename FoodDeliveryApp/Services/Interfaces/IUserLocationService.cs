using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface IUserLocationService
    {
        Task<string> GetUserLocationAsync(ClaimsPrincipal user);
        Task UpdateUserLocationAsync(ClaimsPrincipal user, string location);
    }
}
