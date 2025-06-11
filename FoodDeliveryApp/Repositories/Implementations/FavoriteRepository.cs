using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
    {
        protected new readonly ApplicationDbContext _context;
        protected new readonly ILogger<Repository<Favorite>> _logger;

        public FavoriteRepository(
            ApplicationDbContext context,
            ILogger<FavoriteRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<MenuItem>> GetUserFavoritesAsync(string userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.MenuItem)
                .ToListAsync();
        }

        public async Task<bool> IsFavoriteAsync(string userId, int menuItemId)
        {
            return await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.MenuItemId == menuItemId);
        }

        public async Task<int> GetFavoriteCountAsync(int menuItemId)
        {
            return await _context.Favorites
                .CountAsync(f => f.MenuItemId == menuItemId);
        }

        public async Task<bool> ToggleFavoriteAsync(string userId, int menuItemId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MenuItemId == menuItemId);

            if (favorite == null)
            {
                favorite = new Favorite
                {
                    UserId = userId,
                    MenuItemId = menuItemId
                };
                _context.Favorites.Add(favorite);
            }
            else
            {
                _context.Favorites.Remove(favorite);
            }

            await _context.SaveChangesAsync();
            return favorite != null;
        }
    }
}
