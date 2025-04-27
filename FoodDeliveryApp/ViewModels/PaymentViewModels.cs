using FoodDeliveryApp.Models;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels
{
    public class PaymentViewModels
    {
        public class PaymentViewModel
        {
            public int Id { get; set; }
            public int OrderId { get; set; }

            [Display(Name = "Payment Method")]
            public string PaymentMethod { get; set; }

            [Display(Name = "Amount")]
            [DataType(DataType.Currency)]
            public decimal Amount { get; set; }

            [Display(Name = "Status")]
            public PaymentStatus Status { get; set; }

            [Display(Name = "Date")]
            public DateTime PaymentDate { get; set; }

            [Display(Name = "Transaction ID")]
            public string TransactionId { get; set; }
        }

        public class PaymentMethodViewModel
        {
            public int Id { get; set; }

            [Display(Name = "Type")]
            public PaymentMethodType Type { get; set; }

            [Display(Name = "Provider")]
            public string Provider { get; set; }

            [Display(Name = "Account Number")]
            public string AccountNumberMasked { get; set; }

            [Display(Name = "Default")]
            public bool IsDefault { get; set; }
        }

        public class AddPaymentMethodViewModel
        {
            [Required]
            [Display(Name = "Payment Type")]
            public PaymentMethodType Type { get; set; }

            [Required]
            [Display(Name = "Card Number")]
            [CreditCard]
            public string CardNumber { get; set; }

            [Required]
            [Display(Name = "Cardholder Name")]
            public string CardholderName { get; set; }

            [Required]
            [Display(Name = "Expiration Date")]
            [DataType(DataType.Date)]
            public DateTime ExpiryDate { get; set; }

            [Required]
            [Display(Name = "CVV")]
            [StringLength(4, MinimumLength = 3)]
            public string CVV { get; set; }

            [Display(Name = "Set as default")]
            public bool IsDefault { get; set; } = false;
        }

        public class PaymentResultViewModel
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public string TransactionId { get; set; }
            public string ReceiptUrl { get; set; }
            public int OrderId { get; set; }
        }
    }
}
