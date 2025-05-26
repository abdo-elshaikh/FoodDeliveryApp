using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [PersonalData]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [PersonalData]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(255)]
        public string ProfilePictureUrl { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(50)]
        public string State { get; set; } = string.Empty;

        [MaxLength(20)]
        public string PostalCode { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        public bool ReceivePromotions { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        public string FullName => $"{FirstName} {LastName}".Trim();

        public int LoyaltyPoints { get; set; }

        public string PreferredLanguage { get; set; } = "en";

        public string TimeZone { get; set; } = "UTC";

        [MaxLength(500)]
        public string Bio { get; set; } = string.Empty;

        public override bool TwoFactorEnabled { get; set; } = false;

        public override bool EmailConfirmed { get; set; } = false;

        public override bool PhoneNumberConfirmed { get; set; } = false;

        public UserType Role { get; set; } = UserType.Customer;
        public virtual CustomerProfile? CustomerProfile { get; set; }
        public virtual EmployeeProfile? EmployeeProfile { get; set; }
        public virtual ICollection<Restaurant> Restaurants { get; set; } = new HashSet<Restaurant>();
        public virtual ICollection<SearchHistory> SearchHistories { get; set; } = new HashSet<SearchHistory>();

        //public virtual ICollection<string> FavoriteRestaurants { get; set; } = new List<string>();

        //public virtual IList<string> DietaryPreferences { get; set; } = new List<string>();

        //public virtual IList<string> Allergies { get; set; } = new List<string>();

        //public virtual IList<string> PaymentMethods { get; set; } = new List<string>();


        public string DefaultPaymentMethod { get; set; } = string.Empty;

        public string DefaultDeliveryAddress { get; set; } = string.Empty;

        public NotificationPreferences NotificationPreferences { get; set; }

        public PrivacySettings PrivacySettings { get; set; }
    }


    [Owned]
    public class NotificationPreferences
    {
        public bool? OrderUpdates { get; set; } = true;
        public bool? Promotions { get; set; } = true;
        public bool? Newsletter { get; set; } = true;
        public bool? PushNotifications { get; set; } = true;
        public bool? EmailNotifications { get; set; } = true;
        public bool? SMSNotifications { get; set; } = true;
    }

    [Owned]
    public class PrivacySettings
    {
        // No ApplicationUserId or navigation/foreign key
        public bool? ShowProfilePicture { get; set; } = true;
        public bool? ShowFullName { get; set; } = true;
        public bool? ShowLocation { get; set; } = true;
        public bool? ShowOrderHistory { get; set; } = false;
        public bool? ShareDataWithPartners { get; set; } = false;
    }
}
