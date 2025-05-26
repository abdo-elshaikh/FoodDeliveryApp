using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.OrderViewModels;

namespace FoodDeliveryApp.ViewModels.Payment
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentMethodName { get; set; }
        public string? PaymentMethodType { get; set; }
        public string? LastFourDigits { get; set; }
        public string StatusBadgeClass => GetStatusBadgeClass(Status);
        public string FormattedAmount => $"${Amount:F2}";
        public string FormattedDate => PaymentDate.ToString("MM/dd/yyyy hh:mm tt");

        private static string GetStatusBadgeClass(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "badge bg-warning",
                PaymentStatus.Failed => "badge bg-danger",
                PaymentStatus.Refunded => "badge bg-info",
                _ => "badge bg-secondary"
            };
        }
    }

    public class PaymentListViewModel
    {
        public List<PaymentViewModel> Payments { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PaymentStatus? Status { get; set; }
    }

    public class PaymentDetailsViewModel
    {
        public PaymentViewModel Payment { get; set; } = new();
        public OrderViewModel Order { get; set; } = new();
        public PaymentMethodViewModel? PaymentMethod { get; set; }
        public List<PaymentTransactionViewModel> Transactions { get; set; } = new();
    }

    public class PaymentTransactionViewModel
    {
        public string TransactionId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string FormattedAmount => $"${Amount:F2}";
        public string FormattedDate => Date.ToString("MM/dd/yyyy hh:mm tt");
    }

    public class ProcessPaymentViewModel
    {
        [Required(ErrorMessage = "Payment method is required")]
        public int PaymentMethodId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
        public string Description { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }

    public class RefundPaymentViewModel
    {
        [Required(ErrorMessage = "Transaction ID is required")]
        public string TransactionId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
        public string Reason { get; set; } = string.Empty;
    }
} 