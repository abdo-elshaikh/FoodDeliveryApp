using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class SearchLogRepository : Repository<SearchLog>, ISearchLogRepository
    {
        protected new readonly ApplicationDbContext _context;
        protected new readonly ILogger<Repository<SearchLog>> _logger;

        public SearchLogRepository(
            ApplicationDbContext context,
            ILogger<SearchLogRepository> logger) : base(context, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<string>> GetPopularSearchesAsync(int count)
        {
            try
            {
                return await _context.SearchLogs
                    .GroupBy(s => s.Query)
                    .OrderByDescending(g => g.Count())
                    .Take(count)
                    .Select(g => g.Key)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving popular searches");
                throw;
            }
        }

        public async Task LogSearchAsync(string query, string userId, int resultCount, string location)
        {
            try
            {
                var searchLog = new SearchLog
                {
                    Query = query,
                    UserId = userId,
                    ResultCount = resultCount,
                    Location = location,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.SearchLogs.AddAsync(searchLog);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging search query: {Query}", query);
                throw;
            }
        }
    }
} 