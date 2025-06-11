using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface ICurrentUserService
    {
        string GetCurrentUsername();
        string GetCurrentUserId();
        string GetCurrentUserEmail();
        bool IsAuthenticated();

        bool IsAdmin();

        bool IsOwner();

        bool IsDriver();

        ApplicationUser GetCurrentUser();
    }
}
