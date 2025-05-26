using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class SearchHistoryRepository : ISearchHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public SearchHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SearchHistory>> GetUserSearchesAsync(string userId)
        {
            return await _context.SearchHistory
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SearchDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SearchHistory>> GetPopularSearchesAsync()
        {
            return await _context.SearchHistory
                .GroupBy(s => s.Query)
                .Select(g => new SearchHistory { Query = g.Key, Id = g.Count() })
                .OrderByDescending(s => s.Id)
                .ToListAsync();
        }

        public async Task AddSearchAsync(string userId, string query)
        {
            await _context.SearchHistory.AddAsync(new SearchHistory
            {
                UserId = userId,
                Query = query,
                SearchDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
    }
} 