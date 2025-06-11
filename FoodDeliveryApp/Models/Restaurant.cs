﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Restaurant : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ImageUrl { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; } = string.Empty;

        [StringLength(100)]
        public string? City { get; set; } = string.Empty;

        [StringLength(50)]
        public string? State { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PostalCode { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PhoneNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Email { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryFee { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxRate { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumOrderAmount { get; set; }

        [StringLength(50)]
        public string? DeliveryTime { get; set; } = "30-45 min";

        [Column(TypeName = "decimal(18,2)")]
        public decimal Rating { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        [Required]
        public int CategoryId { get; set; }

        [StringLength(255)]
        public string? LocationUrl { get; set; } = string.Empty;

        [StringLength(255)]
        public string? WebsiteUrl { get; set; } = string.Empty;

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        // Opening and closing time
        public TimeSpan? OpeningTime { get; set; } = TimeSpan.FromHours(10);
        public TimeSpan? ClosingTime { get; set; } = TimeSpan.FromHours(22);

        [Required]
        [StringLength(450)]
        [ForeignKey(nameof(Owner))]
        public string OwnerId { get; set; } = string.Empty;

        // Navigation properties
        public virtual RestaurantCategory Category { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<MenuItemCategory> Categories { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }

        // help methods
        public bool IsOpen()
        {
            return DateTime.Now.TimeOfDay >= OpeningTime && DateTime.Now.TimeOfDay <= ClosingTime;
        }

        // is restaurant owned by the current user
        public bool IsOwner(string userId)
        {
            return OwnerId == userId;
        }
    }
}
