
namespace FoodDeliveryApp.ViewModels.Cart
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; } = 0;
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }

    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public decimal TaxRate { get; set; } = 0.0m;
        public decimal DeliveryFee { get; set; } = 0;
        public decimal Subtotal { get; set; }
        public List<CustomizationViewModel> Customizations { get; set; } = new List<CustomizationViewModel>();
    }

    public class AddToCartViewModel
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; } = null;
        public int RestaurantId { get; set; }
        public List<CustomizationViewModel> Customizations { get; set; } = new List<CustomizationViewModel>();
    }

    public class CustomizationViewModel
    {
        public int OptionId { get; set; }
        public int ChoiceId { get; set; }
        public decimal Price { get; set; }
    }
}