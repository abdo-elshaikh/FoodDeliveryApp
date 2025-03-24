using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "User ID is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "User ID must be between 3 and 100 characters.")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Email must be between 3 and 100 characters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 100 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "User Category is required.")]

        public UserCategory UserCategory { get; set; }
    }
}
