using System;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRestaurantRepository Restaurants { get; }
        IPromotionRepository Promotions { get; }
        IMenuItemRepository MenuItems { get; }
        IOrderRepository Orders { get; }
        IAddressRepository Addresses { get; }
        IReviewRepository Reviews { get; }
        IOrderTrackingRepository OrderTracking { get; }
        IRepository<RestaurantCategory> RestaurantCategories { get; }
        IRepository<MenuItemCategory> MenuItemCategories { get; }
        IRepository<OrderItem> OrderItems { get; }
        IFavoriteRepository Favorites { get; }
        ISearchLogRepository SearchLogs { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
