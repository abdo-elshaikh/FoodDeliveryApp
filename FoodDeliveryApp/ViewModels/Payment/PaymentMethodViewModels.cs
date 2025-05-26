using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.ViewModels.Address;

namespace FoodDeliveryApp.ViewModels.Payment
{
    public class PaymentMethodViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Payment type is required")]
        [Display(Name = "Payment Type")]
        public PaymentType PaymentType { get; set; }

        [Required(ErrorMessage = "Card number is required for credit/debit cards")]
        [CreditCard(ErrorMessage = "Invalid card number")]
        [Display(Name = "Card Number")]
        public string? CardNumber { get; set; }

        [Required(ErrorMessage = "Card holder name is required for credit/debit cards")]
        [StringLength(100, ErrorMessage = "Card holder name cannot exceed 100 characters")]
        [Display(Name = "Card Holder Name")]
        public string? CardHolderName { get; set; }

        [Required(ErrorMessage = "Expiration date is required for credit/debit cards")]
        [Display(Name = "Expiration Date")]
        public string? ExpirationDate { get; set; }

        [Required(ErrorMessage = "CVV is required for credit/debit cards")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV must be 3 or 4 digits")]
        [Display(Name = "CVV")]
        public string? CVV { get; set; }

        [Display(Name = "Is Default")]
        public bool IsDefault { get; set; }

        [Display(Name = "Billing Address")]
        public AddressViewModel? BillingAddress { get; set; }

        public string? LastFourDigits { get; set; }
        public string? CardType { get; set; }
    }

    public class CreatePaymentMethodViewModel
    {
        [Required(ErrorMessage = "Payment type is required")]
        [Display(Name = "Payment Type")]
        public PaymentType PaymentType { get; set; }

        [Required(ErrorMessage = "Card number is required for credit/debit cards")]
        [CreditCard(ErrorMessage = "Invalid card number")]
        [Display(Name = "Card Number")]
        public string? CardNumber { get; set; }

        [Required(ErrorMessage = "Card holder name is required for credit/debit cards")]
        [StringLength(100, ErrorMessage = "Card holder name cannot exceed 100 characters")]
        [Display(Name = "Card Holder Name")]
        public string? CardHolderName { get; set; }

        [Required(ErrorMessage = "Expiration date is required for credit/debit cards")]
        [Display(Name = "Expiration Date")]
        public string? ExpirationDate { get; set; }

        [Required(ErrorMessage = "CVV is required for credit/debit cards")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV must be 3 or 4 digits")]
        [Display(Name = "CVV")]
        public string? CVV { get; set; }

        [Display(Name = "Is Default")]
        public bool IsDefault { get; set; }

        [Display(Name = "Billing Address")]
        public CreateAddressViewModel? BillingAddress { get; set; }
    }

    public class EditPaymentMethodViewModel : CreatePaymentMethodViewModel
    {
        public int Id { get; set; }
        public string? LastFourDigits { get; set; }
        public string? CardType { get; set; }
    }

    public enum PaymentType
    {
        CreditCard,
        DebitCard,
        PayPal,
        CashOnDelivery
    }
} 