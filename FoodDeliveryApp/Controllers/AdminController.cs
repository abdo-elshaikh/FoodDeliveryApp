using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IAnalyticsRepository _analyticsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            IOrderRepository orderRepository,
            IRestaurantRepository restaurantRepository,
            UserManager<ApplicationUser> userManager,
            IAnalyticsRepository analyticsRepository)
        {
            _orderRepository = orderRepository;
            _restaurantRepository = restaurantRepository;
            _analyticsRepository = analyticsRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var recentOrders = await _orderRepository.GetRecentOrdersAsync(10);
            var salesByCategory = await _analyticsRepository.GetSalesByCategoryAsync(
                DateTime.UtcNow.AddDays(-30), DateTime.UtcNow);
            var popularItems = await _analyticsRepository.GetPopularMenuItemsAsync(5);

            var viewModel = new ViewModels.DashboardViewModels.AdminDashboardViewModel
            {
                RecentOrders = recentOrders.Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    //CustomerName = $"{o.CustomerProfile.FirstName} {o.CustomerProfile.LastName}",
                    //RestaurantName = o.Restaurant.Name,
                    //OrderDate = o.OrderDate,
                    //TotalAmount = o.TotalAmount,
                    Status = o.Status
                }).ToList(),
                SalesByCategory = salesByCategory,
                PopularItems = popularItems
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ManageRestaurants()
        {
            var restaurants = await _restaurantRepository.GetAllAsync();
            return View(restaurants);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users
                .Include(u => u.CustomerProfile)
                .Include(u => u.Restaurants)
                .ToListAsync();

            // Assuming you have a UserViewModel to represent user data
            var viewModel = users.Select(u => new ViewModels.DashboardViewModels.UserViewModel
            {
                Id = u.Id,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive,
                EmailConfirmed = u.EmailConfirmed,
            }).ToList();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> OrderReports()
        {
            var orders = await _orderRepository.GetAllAsync();
            var viewModel = new ViewModels.DashboardViewModels.OrderReportViewModel
            {
                TotalOrders = orders.Count(),
                TotalRevenue = orders.Sum(o => o.Total),
                AverageOrderValue = orders.Average(o => o.Total)
            };

            return View(viewModel);
        }
    }
}
