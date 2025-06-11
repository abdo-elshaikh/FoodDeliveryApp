using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IFavoriteRepository : IRepository<Favorite>
    {
        Task<List<MenuItem>> GetUserFavoritesAsync(string userId);
        Task<bool> IsFavoriteAsync(string userId, int menuItemId);
        Task<int> GetFavoriteCountAsync(int menuItemId);
        Task<bool> ToggleFavoriteAsync(string userId, int menuItemId);
    }
}
