using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services.Interfaces;
using System.Linq;

namespace FoodDeliveryApp.Services
{
    public class CartCalculationService : ICartCalculationService
    {
        public decimal CalculateDeliveryFee(Cart cart)
        {
            if (cart == null || cart.Items == null)
                return 0m;
            var uniqueRestaurants = cart.Items.Select(item => item.RestaurantId).Distinct().ToList();
            decimal deliveryFee = 0m;
            foreach (var restaurant in uniqueRestaurants)
            {
                deliveryFee += cart.Items.Where(item => item.RestaurantId == restaurant).Sum(item => item.Restaurant.DeliveryFee);
            }
            return deliveryFee;
        }

        public decimal CalculateTaxFee(Cart cart)
        {
            if (cart == null || cart.Items == null)
                return 0m;
            decimal taxFee = 0m;
            foreach (var item in cart.Items)
            {
                taxFee += item.MenuItem.Price * item.MenuItem.Restaurant.TaxRate;
            }
            return taxFee;
        }

        public decimal CalculateTotal(Cart cart, decimal deliveryFee, decimal tax)
        {
            if (cart == null || cart.Items == null)
                return 0m;
            decimal itemsTotal = cart.Items.Sum(item => item.MenuItem.Price * item.Quantity);
            return itemsTotal + deliveryFee + tax;
        }
    }
}
