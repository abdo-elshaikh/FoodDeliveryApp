using System;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
    public class SearchLog : BaseEntity
    {
        [Required]
        [StringLength(500)]
        public string Query { get; set; } = string.Empty;

        [Required]
        [StringLength(450)]
        public string UserId { get; set; } = string.Empty;

        public int ResultCount { get; set; }

        [StringLength(255)]
        public string Location { get; set; } = string.Empty;
    }
}
