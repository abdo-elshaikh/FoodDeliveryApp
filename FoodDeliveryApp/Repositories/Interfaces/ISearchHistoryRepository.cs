using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface ISearchHistoryRepository
    {
        Task<IEnumerable<SearchHistory>> GetUserSearchesAsync(string userId);
        Task<IEnumerable<SearchHistory>> GetPopularSearchesAsync();
        Task AddSearchAsync(string userId, string query);
    }
} 