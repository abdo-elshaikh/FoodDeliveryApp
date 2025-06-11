using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Driver;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface IDriverService
    {
        // Driver Management
        Task<Driver> GetDriverByIdAsync(int driverId);
        Task<Driver> GetDriverByUserIdAsync(string userId);
        Task<IEnumerable<Driver>> GetAllDriversAsync();
        Task<IEnumerable<Driver>> GetAvailableDriversAsync();
        Task<Driver> CreateDriverAsync(DriverCreateViewModel model);
        Task<Driver> UpdateDriverAsync(int driverId, DriverUpdateViewModel model);
        Task<bool> DeleteDriverAsync(int driverId);
        Task<bool> VerifyDriverAsync(string driverId, string documentUrl);

        // Driver Status
        Task<Driver> UpdateDriverStatusAsync(int driverId, DriverStatus status);
        Task<bool> UpdateDriverAvailabilityAsync(string driverId, bool isAvailable);
        Task<bool> UpdateDriverWorkingHoursAsync(string driverId, TimeSpan start, TimeSpan end);

        // Location Management
        Task<Driver> UpdateDriverLocationAsync(int driverId, double latitude, double longitude);

        // Order Management
        Task<IEnumerable<Order>> GetDriverOrdersAsync(int driverId);
        Task<IEnumerable<Order>> GetDriverActiveOrdersAsync(int driverId);
        Task<IEnumerable<Order>> GetDriverCompletedOrdersAsync(int driverId);
        Task<Driver> AssignOrderAsync(int driverId, int orderId);
        Task<Driver> CompleteOrderAsync(int driverId, int orderId);
        Task<Driver> CancelOrderAsync(int driverId, int orderId);
        Task<Driver> UpdateDriverRatingAsync(int driverId, double rating);
        Task<Driver> UpdateDriverEarningsAsync(int driverId, decimal amount);
        Task<bool> DeleteDriverReviewAsync(int reviewId);

        // Earnings
        Task<decimal> GetDriverEarningsAsync(string driverId, DateTime start, DateTime end);

        // Reviews
        Task<decimal> GetDriverRatingAsync(string driverId);

        // Break Management
        Task<bool> StartDriverBreakAsync(string driverId, string reason);
        Task<bool> EndDriverBreakAsync(string driverId);

        // Analytics
        Task<DriverAnalytics> GetDriverAnalyticsAsync(string driverId, DateTime start, DateTime end);
        Task<DriverPerformance> GetDriverPerformanceAsync(string driverId, DateTime start, DateTime end);

        // Additional methods
        Task<IEnumerable<Driver>> GetDriversByStatusAsync(DriverStatus status);
    }
}
