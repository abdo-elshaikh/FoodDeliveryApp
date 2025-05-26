using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IRestaurantRepository Restaurants { get; }
        IMenuItemRepository MenuItems { get; }
        IOrderRepository Orders { get; }
        IEmployeeRepository Employees { get; }
        IAddressRepository Addresses { get; }
        IPromotionRepository Promotions { get; }
        IPaymentRepository Payments { get; }
        IReviewRepository Reviews { get; }
        ISearchHistoryRepository SearchHistory { get; }

        IRepository<RestaurantCategory> RestaurantCategories { get; }
        IRepository<PaymentMethod> PaymentMethods { get; }
        IRepository<OrderItem> OrderItems { get; }

        IAnalyticsRepository Analytics { get; }

        Task<int> SaveChangesAsync();
    }
}
