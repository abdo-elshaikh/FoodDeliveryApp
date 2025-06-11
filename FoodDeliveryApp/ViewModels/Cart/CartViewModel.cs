using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Cart
{
    public class CartViewModel
    {
        public IReadOnlyList<CartItemViewModel> Items { get; }

        [DataType(DataType.Currency)]
        public decimal Subtotal { get; }

        [DataType(DataType.Currency)]
        public decimal DeliveryFee { get; }

        [DataType(DataType.Currency)]
        public decimal Tax { get; }

        [DataType(DataType.Currency)]
        public decimal TotalWithDiscount { get; }

        //PromotionApplied
        public bool IsPromotionApplied { get; set; } = false;
        [Display(Name = "Promotion Code")]
        public string? PromotionCode { get; set; } = string.Empty;
        public DateTime? PromotionCodeExpiration { get; set; } = null;

        [DataType(DataType.Currency)]
        public decimal Total => Subtotal + DeliveryFee + Tax;

        
        public CartViewModel(IReadOnlyList<CartItemViewModel> items, decimal subtotal, decimal deliveryFee, decimal tax)
        {
            Items = items ?? new List<CartItemViewModel>();
            Subtotal = subtotal;
            DeliveryFee = deliveryFee;
            Tax = tax;
        }
    }
}
