using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private bool _disposed;

        public IRestaurantRepository Restaurants { get; private set; }
        public IMenuItemRepository MenuItems { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IAddressRepository Addresses { get; private set; }
        public IPromotionRepository Promotions { get; private set; }
        public IReviewRepository Reviews { get; private set; }
        public IOrderTrackingRepository OrderTracking { get; private set; }
        private readonly ICartRepository _carts;
        private readonly ICartItemRepository _cartItems;

        public ICartRepository Carts => _carts;
        public ICartItemRepository CartItems => _cartItems;
        public IRepository<RestaurantCategory> RestaurantCategories { get; private set; }
        public IRepository<OrderItem> OrderItems { get; private set; }
        public IRepository<MenuItemCategory> MenuItemCategories { get; private set; }
        public ISearchLogRepository SearchLogs { get; private set; }
        public IFavoriteRepository Favorites { get; private set; }


        public UnitOfWork(
            ApplicationDbContext context,
            ILogger<UnitOfWork> logger,
            IRestaurantRepository restaurantRepository,
            IMenuItemRepository menuItemRepository,
            IOrderRepository orderRepository,
            IAddressRepository addressRepository,
            IPromotionRepository promotionRepository,
            IReviewRepository reviewRepository,
            IOrderTrackingRepository orderTrackingRepository,
            IRepository<RestaurantCategory> restaurantCategoryRepository,
            IRepository<MenuItemCategory> menuItemCategoryRepository,
            IRepository<OrderItem> orderItemRepository,
            ISearchLogRepository searchLogs,
            IFavoriteRepository favoriteRepository,
            ICartItemRepository cartItems,
            ICartRepository cartRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Restaurants = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
            MenuItems = menuItemRepository ?? throw new ArgumentNullException(nameof(menuItemRepository));
            Orders = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            Addresses = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
            Promotions = promotionRepository ?? throw new ArgumentNullException(nameof(promotionRepository));
            Reviews = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
            OrderTracking = orderTrackingRepository ?? throw new ArgumentNullException(nameof(orderTrackingRepository));
            RestaurantCategories = restaurantCategoryRepository ?? throw new ArgumentNullException(nameof(restaurantCategoryRepository));
            OrderItems = orderItemRepository ?? throw new ArgumentNullException(nameof(orderItemRepository));
            MenuItemCategories = menuItemCategoryRepository ?? throw new ArgumentNullException(nameof(menuItemCategoryRepository));
            _carts = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            SearchLogs = searchLogs ?? throw new ArgumentNullException(nameof(searchLogs));
            Favorites = favoriteRepository ?? throw new ArgumentNullException(nameof(favoriteRepository));
            _cartItems = cartItems ?? throw new ArgumentNullException(nameof(cartItems));
        }



        public async Task BeginTransactionAsync()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while beginning transaction");
                throw;
            }
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while committing transaction");
                throw;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _context.Database.RollbackTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while rolling back transaction");
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving changes to the database");
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
