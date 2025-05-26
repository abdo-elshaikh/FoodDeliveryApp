namespace FoodDeliveryApp.Services
{
    public interface IRegistrationService
    {
        Task<bool> IsRegistrationAllowedAsync(string email, string ipAddress, string captchaToken);
        Task<string> GenerateRegistrationTokenAsync(string userId);
        Task<bool> ValidateRegistrationTokenAsync(string userId, string token);
        Task RemoveRegistrationTokenAsync(string userId);
    }
}
