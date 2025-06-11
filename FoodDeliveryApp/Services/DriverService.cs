using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.ViewModels.Driver;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Services
{
    public class DriverService : IDriverService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DriverService> _logger;

        public DriverService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<DriverService> logger
        )
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Driver> GetDriverByIdAsync(int driverId)
        {
            return await _context.Drivers
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == driverId);
        }

        public async Task<Driver> GetDriverByUserIdAsync(string userId)
        {
            return await _context.Drivers
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            return await _context.Drivers
                .Include(d => d.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetAvailableDriversAsync()
        {
            return await _context.Drivers
                .Include(d => d.User)
                .Where(d => d.IsAvailable && d.Status == DriverStatus.Available)
                .ToListAsync();
        }

        public async Task<Driver> CreateDriverAsync(DriverCreateViewModel model)
        {
            // Check if user with this email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            // Create the user account
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = UserType.Driver
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }

            // Create the driver profile
            var driver = new Driver
            {
                UserId = user.Id,
                LicenseNumber = model.LicenseNumber,
                VehicleInfo = model.VehicleInfo,
                Status = DriverStatus.Offline,
                IsAvailable = false,
                LastActive = DateTime.UtcNow
            };

            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            return driver;
        }

        public async Task<Driver> UpdateDriverAsync(int driverId, DriverUpdateViewModel model)
        {
            var driver = await _context.Drivers
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == driverId);

            if (driver == null)
                return null;

            // Update driver properties
            driver.LicenseNumber = model.LicenseNumber;
            driver.VehicleInfo = model.VehicleInfo;
            driver.Status = model.Status;
            driver.IsAvailable = model.Status == DriverStatus.Available;
            driver.LastActive = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return driver;
        }

        public async Task<bool> DeleteDriverAsync(int driverId)
        {
            var driver = await _context.Drivers
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == driverId);

            if (driver == null)
                return false;

            // Delete the driver profile
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            // Delete the associated user account
            var user = await _userManager.FindByIdAsync(driver.UserId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return true;
        }

        public async Task<bool> VerifyDriverAsync(string driverId, string documentUrl)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
                return false;

            // Add verification logic here
            // For now, we'll just return true
            return true;
        }

        public async Task<Driver> UpdateDriverStatusAsync(int driverId, DriverStatus status)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
                return null;

            driver.Status = status;
            driver.IsAvailable = status == DriverStatus.Available;
            driver.LastActive = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return driver;
        }

        public async Task<bool> UpdateDriverAvailabilityAsync(string driverId, bool isAvailable)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
                return false;

            driver.IsAvailable = isAvailable;
            driver.LastActive = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDriverWorkingHoursAsync(string driverId, TimeSpan start, TimeSpan end)
        {
            // Implement working hours update logic
            return true;
        }

        public async Task<Driver> UpdateDriverLocationAsync(int driverId, double latitude, double longitude)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
                return null;

            driver.UpdateLocation(latitude, longitude, null);
            await _context.SaveChangesAsync();
            return driver;
        }

        public async Task<IEnumerable<Order>> GetDriverOrdersAsync(int driverId)
        {
            return await _context.Orders
                .Include(o => o.DeliveryAddress)
                .Include(o => o.OrderItems)
                .Where(o => o.DriverId == driverId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetDriverActiveOrdersAsync(int driverId)
        {
            return await _context.Orders
                .Include(o => o.DeliveryAddress)
                .Include(o => o.OrderItems)
                .Where(o => o.DriverId == driverId && o.Status != OrderStatus.Delivered)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetDriverCompletedOrdersAsync(int driverId)
        {
            return await _context.Orders
                .Include(o => o.DeliveryAddress)
                .Include(o => o.OrderItems)
                .Where(o => o.DriverId == driverId && o.Status == OrderStatus.Delivered)
                .ToListAsync();
        }

        public async Task<Driver> AssignOrderAsync(int driverId, int orderId)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            var order = await _context.Orders.FindAsync(orderId);

            if (driver == null || order == null)
                return null;

            order.DriverId = driverId;
            await _context.SaveChangesAsync();

            return driver;
        }

        public async Task<Driver> CompleteOrderAsync(int driverId, int orderId)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            var order = await _context.Orders.FindAsync(orderId);

            if (driver == null || order == null)
                return null;

            order.Status = OrderStatus.Delivered;
            await _context.SaveChangesAsync();

            return driver;
        }

        public async Task<Driver> CancelOrderAsync(int driverId, int orderId)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            var order = await _context.Orders.FindAsync(orderId);

            if (driver == null || order == null)
                return null;

            order.Status = OrderStatus.Canceled;
            await _context.SaveChangesAsync();

            return driver;
        }

        public async Task<Driver> UpdateDriverRatingAsync(int driverId, double rating)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
                return null;

            // Implement rating update logic
            return driver;
        }

        public async Task<Driver> UpdateDriverEarningsAsync(int driverId, decimal amount)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
                return null;

            // Implement earnings update logic
            return driver;
        }

        public async Task<bool> DeleteDriverReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetDriverEarningsAsync(string driverId, DateTime start, DateTime end)
        {
            // Implement earnings calculation logic
            return 0;
        }

        public async Task<decimal> GetDriverRatingAsync(string driverId)
        {
            // Implement rating calculation logic
            return 0;
        }

        public async Task<bool> StartDriverBreakAsync(string driverId, string reason)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
                return false;

            // Implement break start logic
            return true;
        }

        public async Task<bool> EndDriverBreakAsync(string driverId)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
                return false;

            // Implement break end logic
            return true;
        }

        public async Task<DriverAnalytics> GetDriverAnalyticsAsync(string driverId, DateTime start, DateTime end)
        {
            // Implement analytics calculation logic
            return new DriverAnalytics();
        }

        public async Task<DriverPerformance> GetDriverPerformanceAsync(string driverId, DateTime start, DateTime end)
        {
            // Implement performance calculation logic
            return new DriverPerformance();
        }

        public async Task<IEnumerable<Driver>> GetDriversByStatusAsync(DriverStatus status)
        {
            return await _context.Drivers
                .Include(d => d.User)
                .Where(d => d.Status == status)
                .ToListAsync();
        }
    }

    public class DriverAnalytics
    {
        public int TotalOrders { get; set; }
        public decimal TotalEarnings { get; set; }
        public double AverageRating { get; set; }
        public TimeSpan TotalActiveTime { get; set; }
    }

    public class DriverPerformance
    {
        public int OnTimeDeliveries { get; set; }
        public int LateDeliveries { get; set; }
        public double CustomerSatisfaction { get; set; }
        public decimal EfficiencyScore { get; set; }
    }
}
