using FoodDeliveryApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IAnalyticsRepository
    {
        Task<Dictionary<string, decimal>> GetSalesByCategoryAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<int, int>> GetPopularMenuItemsAsync(int count);
        Task<Dictionary<TimeSpan, int>> GetOrderDistributionByTimeAsync();
        Task<Dictionary<string, int>> GetCustomerOrderFrequencyAsync();
    }
}