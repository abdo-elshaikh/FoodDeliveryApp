using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;
using System.Collections.Generic;

namespace FoodDeliveryApp.ViewModels.OrderViewModels
{
    public class OrdersListViewModel
    {
        public List<OrderSummaryViewModel> Orders { get; set; } = new();
        public int TotalOrders { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string SortBy { get; set; } = "OrderDate";
        public string SortOrder { get; set; } = "desc";
        public string StatusFilter { get; set; } = "All";
    }
} 