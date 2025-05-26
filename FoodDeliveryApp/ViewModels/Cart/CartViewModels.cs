using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.ViewModels.Cart
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal Subtotal { get; set; } = 0.0m;
        public decimal Tax { get; set; } = 0.0m;
        public decimal DeliveryFee { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; } = 0.0m;
        public int ItemCount => Items.Sum(item => item.Quantity);
        public bool IsEmpty => !Items.Any();
        public Dictionary<string, decimal> RestaurantTotals => 
            Items.GroupBy(i => i.RestaurantName)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Total));
    }

    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? SpecialInstructions { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public decimal TaxRate { get; set; }
        public decimal DeliveryFee { get; set; }
        public List<CartItemCustomizationViewModel> Customizations { get; set; } = new();
        public decimal Subtotal => Price * Quantity;
        public decimal Total => Subtotal + Customizations.Sum(c => c.Price * Quantity);
    }

    public class CartItemCustomizationViewModel
    {
        public int Id { get; set; }
        public int OptionId { get; set; }
        public int ChoiceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Choice { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class AddToCartViewModel
    {
        [Required(ErrorMessage = "Menu item is required")]
        public int MenuItemId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int RestaurantId { get; set; }
        public List<CartItemCustomizationViewModel> Customizations { get; set; } = new();
        public string? SpecialInstructions { get; set; }
    }

    public class UpdateCartItemViewModel
    {
        [Required(ErrorMessage = "Cart item ID is required")]
        public int CartItemId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }

        public string? SpecialInstructions { get; set; }
        public List<CartItemCustomizationViewModel>? Customizations { get; set; }
    }

    public class CartSummaryViewModel
    {
        public int ItemCount { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Discount { get; set; }
        public decimal Total => Subtotal + Tax + DeliveryFee - Discount;
        public bool IsEmpty => ItemCount == 0;
    }
} 