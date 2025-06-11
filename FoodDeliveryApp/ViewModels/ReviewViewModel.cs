using System;
using System.Collections.Generic;

namespace FoodDeliveryApp.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string RestaurantName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Images { get; set; }
        public int RestaurantId { get; set; }
        public string UserId { get; set; }
    }
}
