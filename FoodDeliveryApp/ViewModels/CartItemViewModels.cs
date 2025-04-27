namespace FoodDeliveryApp.ViewModels.CartItemViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }

    public class CartItemViewModel
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; } = null;
        public decimal Price { get; set; }
        public string? SpecialInstructions { get; set; } = string.Empty;
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public List<CustomizationViewModel> Customizations { get; set; } = new List<CustomizationViewModel>();
    }

    public class AddToCartViewModel
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<CustomizationViewModel> Customizations { get; set; }
    }

    public class CustomizationViewModel
    {
        public int OptionId { get; set; }
        public int ChoiceId { get; set; }
        public decimal Price { get; set; }
    }
}