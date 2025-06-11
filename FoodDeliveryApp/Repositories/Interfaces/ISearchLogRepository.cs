using FoodDeliveryApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface ISearchLogRepository : IRepository<SearchLog>
    {
        Task<List<string>> GetPopularSearchesAsync(int count);
        Task LogSearchAsync(string query, string userId, int resultCount, string location);
    }
} 