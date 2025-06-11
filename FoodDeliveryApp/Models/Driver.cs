using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Driver 
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string VehicleInfo { get; set; } = string.Empty;

        [Required]
        public DriverStatus Status { get; set; } = DriverStatus.Offline;

        public DateTime? LastActive { get; set; }

        public double? CurrentLatitude { get; set; }

        public double? CurrentLongitude { get; set; }

        [StringLength(200)]
        public string? CurrentAddress { get; set; }

        public DateTime? LastLocationUpdate { get; set; }

        public bool IsAvailable { get; set; } = false;

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public void UpdateLocation(double latitude, double longitude, string address)
        {
            CurrentLatitude = latitude;
            CurrentLongitude = longitude;
            CurrentAddress = address;
            LastLocationUpdate = DateTime.UtcNow;
        }
    }

    public enum DriverStatus
    {
        Offline,
        Available,
        Busy
    }
} 