using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.Data;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.ViewModels.Restaurant;

namespace FoodDeliveryApp.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ApplicationDbContext _context;

        public RestaurantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RestaurantViewModel>> GetFeaturedRestaurantsAsync()
        {
            return await _context.Restaurants
                //.Where(r => r.IsFeatured) // Commented out, property does not exist
                .OrderByDescending(r => r.Rating)
                .Take(6)
                .Select(r => new FoodDeliveryApp.ViewModels.Restaurant.RestaurantViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    ImageUrl = r.ImageUrl,
                    CuisineType = "", // Property not in entity, set to empty
                    Rating = (double)r.Rating,
                    DeliveryTime = r.DeliveryTime ?? "",
                    AverageDeliveryTime = "", // Property not in entity, set to empty
                    DeliveryFee = r.DeliveryFee,
                    Description = r.Description,
                    PhoneNumber = r.PhoneNumber,
                    ReviewCount = r.Reviews != null ? r.Reviews.Count : 0,
                    Categories = r.Categories != null ? r.Categories.Select(c => c.Name).ToArray() : new string[0],
                    Website = r.WebsiteUrl ?? "",
                    LocationUrl = r.LocationUrl ?? "",
                    IsOpen = true, // Set default or implement logic
                    IsActive = r.IsActive,
                    OpeningTime = r.OpeningTime ?? TimeSpan.FromHours(10),
                    ClosingTime = r.ClosingTime ?? TimeSpan.FromHours(22),
                    Address = new RestaurantAddressViewModel(), // Set as needed
                    CategoryName = r.Categories != null && r.Categories.Any() ? r.Categories.First().Name : string.Empty,
                    IsAdminOrOwner = false, // Set as needed
                    TaxRate = r.TaxRate
                })
                .ToListAsync();
        }

        public async Task<RestaurantViewModel> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == id);

            if (restaurant == null)
                return null;

            return new FoodDeliveryApp.ViewModels.Restaurant.RestaurantViewModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                ImageUrl = restaurant.ImageUrl,
                CuisineType = "", // Property not in entity, set to empty
                Rating = (double)restaurant.Rating,
                DeliveryTime = restaurant.DeliveryTime ?? "",
                AverageDeliveryTime = "", // Property not in entity, set to empty
                DeliveryFee = restaurant.DeliveryFee,
                Description = restaurant.Description,
                PhoneNumber = restaurant.PhoneNumber,
                ReviewCount = restaurant.Reviews != null ? restaurant.Reviews.Count : 0,
                Categories = restaurant.Categories != null ? restaurant.Categories.Select(c => c.Name).ToArray() : new string[0],
                Website = restaurant.WebsiteUrl ?? "",
                LocationUrl = restaurant.LocationUrl ?? "",
                IsOpen = true, // Set default or implement logic
                IsActive = restaurant.IsActive,
                OpeningTime = restaurant.OpeningTime ?? TimeSpan.FromHours(10),
                ClosingTime = restaurant.ClosingTime ?? TimeSpan.FromHours(22),
                Address = new RestaurantAddressViewModel(), // Set as needed
                CategoryName = restaurant.Categories != null && restaurant.Categories.Any() ? restaurant.Categories.First().Name : string.Empty,
                IsAdminOrOwner = false, // Set as needed
                TaxRate = restaurant.TaxRate
            };
        }

        public async Task<List<RestaurantViewModel>> SearchRestaurantsAsync(string query, string location)
        {
            var restaurants = _context.Restaurants.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                restaurants = restaurants.Where(r =>
                    r.Name.Contains(query) ||
                    r.Category.Name.Contains(query) ||
                    r.Description.Contains(query));
            }

            if (!string.IsNullOrEmpty(location))
            {
                // Add location-based filtering logic here
                // This could involve geocoding and distance calculations
            }

            return await restaurants
                .OrderByDescending(r => r.Rating)
                .Select(r => new RestaurantViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    ImageUrl = r.ImageUrl ?? "",
                    CuisineType = r.Category.Name,
                    Rating = (double)r.Rating,
                    DeliveryTime = r.DeliveryTime ?? "",
                    DeliveryFee = r.DeliveryFee
                })
                .ToListAsync();
        }
    }
} 