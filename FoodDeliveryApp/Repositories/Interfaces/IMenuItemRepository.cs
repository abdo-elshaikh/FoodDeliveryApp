﻿using FoodDeliveryApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> GetByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<MenuItem>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<MenuItem>> GetPopularItemsAsync(int count = 8);
        Task<IEnumerable<MenuItemCategory>> GetPopularCategoriesAsync(int count = 6);
        Task<MenuItem> GetByIdWithDetailsAsync(int id);
        Task<MenuItem> GetByIdWithRestaurantAsync(int id);
        Task<bool> UpdateAvailabilityAsync(int id, bool isAvailable);
        Task<IEnumerable<MenuItem>> SearchMenuItemsAsync(string searchTerm);

        Task<IEnumerable<MenuItem>> GetByIdsAsync(IEnumerable<int> ids);
    }

}
