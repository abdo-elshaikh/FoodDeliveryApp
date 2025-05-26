using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class SearchHistory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Query { get; set; }
        
        public DateTime SearchDate { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ApplicationUser User { get; set; }
    }
} 