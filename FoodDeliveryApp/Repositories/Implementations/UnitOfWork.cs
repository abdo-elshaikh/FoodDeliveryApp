using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICustomerRepository Customers { get; private set; }
        public IRestaurantRepository Restaurants { get; private set; }
        public IMenuItemRepository MenuItems { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public IAddressRepository Addresses { get; private set; }
        public IPromotionRepository Promotions { get; private set; }
        public IPaymentRepository Payments { get; private set; }
        public IReviewRepository Reviews { get; private set; }
        public ISearchHistoryRepository SearchHistory { get; private set; }
        public IRepository<RestaurantCategory> RestaurantCategories { get; private set; }
        public IRepository<PaymentMethod> PaymentMethods { get; private set; }
        public IRepository<OrderItem> OrderItems { get; private set; }
        public IAnalyticsRepository Analytics { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Customers = new CustomerRepository(_context);
            Restaurants = new RestaurantRepository(_context);
            MenuItems = new MenuItemRepository(_context);
            Orders = new OrderRepository(_context);
            Employees = new EmployeeRepository(_context);
            Addresses = new AddressRepository(_context);
            Promotions = new PromotionRepository(_context);
            Payments = new PaymentRepository(_context);
            Reviews = new ReviewRepository(_context);
            SearchHistory = new SearchHistoryRepository(_context);
            RestaurantCategories = new Repository<RestaurantCategory>(_context);
            PaymentMethods = new Repository<PaymentMethod>(_context);
            OrderItems = new Repository<OrderItem>(_context);
            Analytics = new AnalyticsRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
