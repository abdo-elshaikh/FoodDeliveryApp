using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface ICartCalculationService
    {
        decimal CalculateDeliveryFee(Cart cart);

        decimal CalculateTaxFee(Cart cart);

        decimal CalculateTotal(Cart cart, decimal deliveryFee, decimal tax);
    }
}
