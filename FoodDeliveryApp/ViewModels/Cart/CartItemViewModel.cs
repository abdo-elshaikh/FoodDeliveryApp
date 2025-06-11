using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.ViewModels.Cart
{
    public class CartItemViewModel
    {
        public int Id { get; }

        public int MenuItemId { get; }

        [Required]
        public string Name { get; }

        public string Description { get; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; }

        public int RestaurantId { get; }

        [Required]
        public string RestaurantName { get; }

        public string? SpecialInstructions { get; }

        [DataType(DataType.Currency)]
        public decimal Price { get; }

        [DataType(DataType.Currency)]
        public decimal DeliveryFee { get; }

        public decimal TaxRate { get; }

        [Required]
        public string ImageUrl { get; }

        [DataType(DataType.Currency)]
        public decimal LineTotal => Price * Quantity;

        public CartItemViewModel(CartItem cartItem)
        {
            Id = cartItem.Id;
            MenuItemId = cartItem.MenuItemId;
            Name = cartItem.MenuItem.Name;
            Description = cartItem.MenuItem.Description ?? "No description provided.";
            Quantity = cartItem.Quantity;
            RestaurantId = cartItem.RestaurantId;
            RestaurantName = cartItem.Restaurant.Name;
            SpecialInstructions = cartItem.SpecialInstructions;
            Price = cartItem.MenuItem.Price;
            DeliveryFee = cartItem.Restaurant.DeliveryFee;
            TaxRate = cartItem.MenuItem.Restaurant?.TaxRate?? 0;
            ImageUrl = cartItem.MenuItem.ImageUrl ?? "";
        }
    }
}
