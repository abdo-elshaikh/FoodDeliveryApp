using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FoodDeliveryApp.Models
{
    public abstract class BaseEntity 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public bool IsDeleted { get; set; } = false;
        
        public DateTime? DeletedAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; } = "system";
        
        public DateTime? UpdatedAt { get; set; }
        
        [StringLength(100)]
        public string? UpdatedBy { get; set; }
    }
}
