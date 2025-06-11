using System;
using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.ViewModels.Driver
{
    public class DriverCreateViewModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string LicenseNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string VehicleInfo { get; set; }
    }

    public class DriverUpdateViewModel
    {
        [Required]
        [StringLength(50)]
        public string LicenseNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string VehicleInfo { get; set; }

        public DriverStatus Status { get; set; }
    }

    public class DriverViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string VehicleInfo { get; set; }
        public DriverStatus Status { get; set; }
        public DateTime? LastActive { get; set; }
        public double? CurrentLatitude { get; set; }
        public double? CurrentLongitude { get; set; }
        public string CurrentAddress { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class DriverAnalytics
    {
        public int TotalDeliveries { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public TimeSpan AverageDeliveryTime { get; set; }
        public int OnTimeDeliveries { get; set; }
        public int LateDeliveries { get; set; }
        public decimal TotalDistance { get; set; }
        public decimal TotalTimeSpent { get; set; }
        public int TotalBreaks { get; set; }
        public TimeSpan TotalBreakTime { get; set; }
        public Dictionary<string, int> DeliveriesByDay { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, decimal> EarningsByDay { get; set; } = new Dictionary<string, decimal>();
    }

    public class DriverPerformance
    {
        public decimal OnTimeDeliveryRate { get; set; }
        public decimal CustomerSatisfactionRate { get; set; }
        public decimal AcceptanceRate { get; set; }
        public decimal CompletionRate { get; set; }
        public decimal AverageDeliveryTime { get; set; }
        public decimal AverageDistancePerDelivery { get; set; }
        public decimal AverageEarningsPerDelivery { get; set; }
        public decimal AverageEarningsPerHour { get; set; }
        public int PeakHoursDeliveries { get; set; }
        public int OffPeakHoursDeliveries { get; set; }
        public Dictionary<string, decimal> PerformanceByDay { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> PerformanceByHour { get; set; } = new Dictionary<string, decimal>();
    }
} 