using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.ViewModels.OrderViewModels
{
    public class OrderCreateViewModel
    {
        public string UserId { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
        public string DeliveryAddress { get; set; }
        public string SpecialInstructions { get; set; }
        public PaymentMethodType SelectedPaymentMethod { get; set; }
        public int SelectedPaymentMethodId { get; set; }
        public int SelectedAddressId { get; set; } = 0;
        public List<PaymentMethodViewModel> AvailablePaymentMethods { get; set; } = new List<PaymentMethodViewModel>();
        public List<AddressViewModel> Addresses { get; set; } = new List<AddressViewModel>();

    }

    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public int? DeliveryAddressId { get; set; }
        public string SpecialInstructions { get; set; }
        public PaymentMethodType PaymentMethodType { get; set; }
        public string PaymentDetails { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
        public AddressViewModel Address { get; set; } = new AddressViewModel();
    }

    public class OrderHistoryViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
        public string RestaurantName { get; set; }
    }

    public class OrderManagementViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
        public string CustomerName { get; set; }
        public string RestaurantName { get; set; }
        public int ItemsCount { get; set; }
        public int RestaurantCount { get; set; } = 1;
        public PaymentStatus PaymentStatus { get; set; }
    }

    public class OrderItemViewModel
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public List<OrderCustomizationViewModel> Customizations { get; set; } = new List<OrderCustomizationViewModel>();
    }

    public class OrderCustomizationViewModel
    {
        public int OptionId { get; set; }
        public int ChoiceId { get; set; }
        public decimal Price { get; set; }
    }

    public class PaymentMethodViewModel
    {
        public int Id { get; set; }
        public PaymentMethodType Type { get; set; }
        public string DisplayName { get; set; }
        public string LastFourDigits { get; set; }
    }
}