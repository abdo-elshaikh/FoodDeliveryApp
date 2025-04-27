using FoodDeliveryApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<CustomerProfile> CustomerProfiles { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<RestaurantCategory> RestaurantCategories { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<CustomizationOption> CustomizationOptions { get; set; }
        public DbSet<CustomizationChoice> CustomizationChoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderCustomization> OrderCustomizations { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionUsage> PromotionUsages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Customization> Customizations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships and constraints
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.CustomerProfile)
                .WithOne(c => c.User)
                .HasForeignKey<CustomerProfile>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.EmployeeProfile)
                .WithOne(e => e.User)
                .HasForeignKey<EmployeeProfile>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CustomerProfile>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.CustomerProfile)
                .HasForeignKey(a => a.CustomerProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Restaurant>()
                .HasMany(r => r.MenuItems)
                .WithOne(m => m.Restaurant)
                .HasForeignKey(m => m.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Restaurant>()
                .HasOne(r => r.Owner)
                .WithMany(u => u.Restaurants)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RestaurantCategory>()
                .HasMany(c => c.Restaurants)
                .WithOne(r => r.Category)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            // Configure relationships for Promotions and PromotionUsages
            builder.Entity<Promotion>()
                .HasMany(p => p.PromotionUsages)
                .WithOne(u => u.Promotion)
                .HasForeignKey(u => u.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cart configuration
            builder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem configuration
            builder.Entity<CartItem>()
                .HasOne(ci => ci.MenuItem)
                .WithMany()
                .HasForeignKey(ci => ci.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                .HasMany(ci => ci.Customizations)
                .WithOne(c => c.CartItem)
                .HasForeignKey(c => c.CartItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var now = DateTime.UtcNow;

            // Seed Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER", ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = "3", Name = "Employee", NormalizedName = "EMPLOYEE", ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = "4", Name = "Owner", NormalizedName = "OWNER", ConcurrencyStamp = Guid.NewGuid().ToString() }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            // Seed Users
            var users = new List<ApplicationUser>
            {
                new ApplicationUser {
                    Id = "1",
                    Email = "admin@foodfast.com",
                    UserName = "admin@foodfast.com",
                    PasswordHash = hasher.HashPassword(null, "Admin@123"),
                    Role = UserRole.Admin,
                    NormalizedUserName = "ADMIN@FOODFAST.COM",
                    NormalizedEmail = "ADMIN@FOODFAST.COM",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = now,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "555-000-0000",
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser {
                    Id = "2",
                    Email = "customer@foodfast.com",
                    UserName = "customer@foodfast.com",
                    PasswordHash = hasher.HashPassword(null, "Customer@123"),
                    Role = UserRole.Customer,
                    NormalizedEmail = "CUSTOMER@FOODFAST.COM",
                    NormalizedUserName = "CUSTOMER@FOODFAST.COM",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = now,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "555-111-1111",
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser {
                    Id = "3",
                    Email = "employee@foodfast.com",
                    UserName = "employee@foodfast.com",
                    PasswordHash = hasher.HashPassword(null, "Employee@123"),
                    Role = UserRole.Employee,
                    NormalizedEmail = "EMPLOYEE@FOODFAST.COM",
                    NormalizedUserName = "EMPLOYEE@FOODFAST.COM",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = now,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "555-222-2222",
                    PhoneNumberConfirmed = true
                },
                new ApplicationUser {
                    Id = "4",
                    Email = "owner@foodfast.com",
                    UserName = "owner@foodfast.com",
                    PasswordHash = hasher.HashPassword(null, "Owner@123"),
                    Role = UserRole.Owner,
                    NormalizedEmail = "OWNER@FOODFAST.COM",
                    NormalizedUserName = "OWNER@FOODFAST.COM",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = now,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "555-333-3333",
                    PhoneNumberConfirmed = true
                }
            };
            builder.Entity<ApplicationUser>().HasData(users);

            // Seed User Roles
            var userRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string> { UserId = "1", RoleId = "1" },
                new IdentityUserRole<string> { UserId = "2", RoleId = "2" },
                new IdentityUserRole<string> { UserId = "3", RoleId = "3" },
                new IdentityUserRole<string> { UserId = "4", RoleId = "4" }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);

            // Seed Customer Profile
            var customer = new CustomerProfile
            {
                Id = 1,
                UserId = "2",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "555-111-1111",
                DateOfBirth = new DateTime(1990, 1, 1),
                IsActive = true,
                ProfilePictureUrl = "/images/users/user2.jpg",
                CreatedAt = now.AddDays(-30),
                UpdatedAt = now
            };
            builder.Entity<CustomerProfile>().HasData(customer);

            // Seed Employee Profile
            var employee = new EmployeeProfile
            {
                Id = 1,
                UserId = "3",
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "555-222-2222",
                Position = EmployeePosition.DeliveryDriver,
                HireDate = now.AddMonths(-6),
                IsActive = true,
                CreatedAt = now.AddMonths(-6),
                UpdatedAt = now
            };
            builder.Entity<EmployeeProfile>().HasData(employee);

            // Seed Addresses
            var addresses = new List<Address>
            {
                new Address {
                    Id = 1,
                    CustomerProfileId = 1,
                    Street = "123 Main St",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    IsDefault = true,
                    CreatedAt = now.AddDays(-29)
                },
                new Address {
                    Id = 2,
                    CustomerProfileId = 1,
                    Street = "456 Work Ave",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10002",
                    IsDefault = false,
                    CreatedAt = now.AddDays(-20)
                }
            };
            builder.Entity<Address>().HasData(addresses);

            // Seed Payment Methods
            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod {
                    Id = 1,
                    UserId = "2",
                    Type = PaymentMethodType.CreditCard,
                    AccountNumberMasked = "************1234",
                    Payments = null,
                    Provider = "Visa",
                    IsDefault = true,
                    CreatedAt = now.AddDays(-28)
                },
                new PaymentMethod {
                    Id = 2,
                    UserId = "2",
                    Type = PaymentMethodType.PayPal,
                    AccountNumberMasked = "********@paypal.com",
                    Payments = null,
                    Provider = "PayPal",
                    IsDefault = false,
                    CreatedAt = now.AddDays(-14)
                }
            };
            builder.Entity<PaymentMethod>().HasData(paymentMethods);

            // Seed Restaurant Categories
            var categories = new List<RestaurantCategory>
            {
                new RestaurantCategory { Id = 1, Name = "Italian", Description = "Authentic Italian cuisine" },
                new RestaurantCategory { Id = 2, Name = "Mexican", Description = "Traditional Mexican food" },
                new RestaurantCategory { Id = 3, Name = "Asian", Description = "Various Asian cuisines"  },
                new RestaurantCategory { Id = 4, Name = "American", Description = "Classic American dishes" },
                new RestaurantCategory { Id = 5, Name = "Vegetarian", Description = "Plant-based meals" }
            };
            builder.Entity<RestaurantCategory>().HasData(categories);

            // Seed Restaurants
            var restaurants = new List<Restaurant>
            {
                new Restaurant {
                    Id = 1,
                    Name = "Mama Mia Italian",
                    Description = "Authentic Italian restaurant since 1985",
                    PhoneNumber = "555-123-4567",
                    CategoryId = 1,
                    Address = "123 Pasta Street",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    OpeningTime = new TimeSpan(11, 0, 0),
                    ClosingTime = new TimeSpan(22, 0, 0),
                    IsActive = true,
                    ImageUrl = "/images/restaurants/italian.jpg",
                    OwnerId = "4",
                    CreatedAt = now.AddDays(-30)
                },
                new Restaurant {
                    Id = 2,
                    Name = "Taco Fiesta",
                    Description = "The best Mexican food in town",
                    PhoneNumber = "555-234-5678",
                    CategoryId = 2,
                    Address = "456 Salsa Avenue",
                    City = "Los Angeles",
                    State = "CA",
                    PostalCode = "90001",
                    OpeningTime = new TimeSpan(10, 0, 0),
                    ClosingTime = new TimeSpan(23, 0, 0),
                    IsActive = true,
                    ImageUrl = "/images/restaurants/mexican.jpg",
                    OwnerId = "4",
                    CreatedAt = now.AddDays(-25)
                },
                new Restaurant {
                    Id = 3,
                    Name = "Golden Wok",
                    Description = "Authentic Chinese cuisine",
                    PhoneNumber = "555-345-6789",
                    CategoryId = 3,
                    Address = "789 Noodle Road",
                    City = "Chicago",
                    State = "IL",
                    PostalCode = "60601",
                    OpeningTime = new TimeSpan(10, 30, 0),
                    ClosingTime = new TimeSpan(21, 30, 0),
                    IsActive = true,
                    ImageUrl = "/images/restaurants/asian.jpg",
                    OwnerId = "4",
                    CreatedAt = now.AddDays(-20)
                }
            };
            builder.Entity<Restaurant>().HasData(restaurants);

            // Seed Menu Items
            var menuItems = new List<MenuItem>
            {
                // Italian restaurant items
                new MenuItem {
                    Id = 1,
                    Name = "Spaghetti Carbonara",
                    Description = "Classic pasta with eggs, cheese, pancetta, and pepper",
                    Price = 14.99m,
                    ImageUrl = "/images/menu/carbonara.jpg",
                    IsAvailable = true,
                    RestaurantId = 1,
                    CreatedAt = now.AddDays(-29)
                },
                new MenuItem {
                    Id = 2,
                    Name = "Margherita Pizza",
                    Description = "Traditional pizza with tomato sauce, mozzarella, and basil",
                    Price = 12.99m,
                    ImageUrl = "/images/menu/pizza.jpg",
                    IsAvailable = true,
                    RestaurantId = 1,
                    CreatedAt = now.AddDays(-29)
                },
                
                // Mexican restaurant items
                new MenuItem {
                    Id = 3,
                    Name = "Chicken Quesadilla",
                    Description = "Grilled tortilla filled with cheese and chicken",
                    Price = 9.99m,
                    ImageUrl = "/images/menu/quesadilla.jpg",
                    IsAvailable = true,
                    RestaurantId = 2,
                    CreatedAt = now.AddDays(-24)
                },
                new MenuItem {
                    Id = 4,
                    Name = "Beef Burrito",
                    Description = "Large flour tortilla with beef, rice, and beans",
                    Price = 11.99m,
                    ImageUrl = "/images/menu/burrito.jpg",
                    IsAvailable = true,
                    RestaurantId = 2,
                    CreatedAt = now.AddDays(-24)
                },
                
                // Asian restaurant items
                new MenuItem {
                    Id = 5,
                    Name = "General Tso's Chicken",
                    Description = "Crispy chicken in a sweet and spicy sauce",
                    Price = 13.99m,
                    ImageUrl = "/images/menu/general-tsos.jpg",
                    IsAvailable = true,
                    RestaurantId = 3,
                    CreatedAt = now.AddDays(-19)
                },
                new MenuItem {
                    Id = 6,
                    Name = "Vegetable Lo Mein",
                    Description = "Stir-fried noodles with mixed vegetables",
                    Price = 10.99m,
                    ImageUrl = "/images/menu/lo-mein.jpg",
                    IsAvailable = true,
                    RestaurantId = 3,
                    CreatedAt = now.AddDays(-19)
                }
            };
            builder.Entity<MenuItem>().HasData(menuItems);

            // Seed Order Items
            var orderItems = new List<OrderItem>
            {
                new OrderItem {
                    Id = 1,
                    OrderId = 1,
                    MenuItemId = 1,
                    Quantity = 1,
                    Price = 14.99m,
                    RestaurantId = 1,
                    SpecialInstructions = "No cheese"
                },
                new OrderItem {
                    Id = 2,
                    OrderId = 2,
                    MenuItemId = 3,
                    Quantity = 2,
                    Price = 9.99m,
                    RestaurantId = 2,
                    SpecialInstructions = "Extra cheese, no beans"
                }
            };
            builder.Entity<OrderItem>().HasData(orderItems);

            // Seed Payments
            var payments = new List<Payment>
            {
                new Payment {
                    Id = 1,
                    OrderId = 1,
                    PaymentMethodId = 1,
                    Amount = 22.48m,
                    PaymentDate = now.AddDays(-10).AddHours(1),
                    Status = PaymentStatus.Completed,
                    TransactionId = "PAY-123456789"
                },
                new Payment {
                    Id = 2,
                    OrderId = 2,
                    PaymentMethodId = 1,
                    Amount = 17.78m,
                    PaymentDate = now.AddDays(-5).AddHours(1),
                    Status = PaymentStatus.Completed,
                    TransactionId = "PAY-987654321"
                }
            };
            builder.Entity<Payment>().HasData(payments);

            // Seed Promotions
            var promotions = new List<Promotion>
            {
                new Promotion {
                    Id = 1,
                    Code = "WELCOME20",
                    Description = "20% off your first order",
                    DiscountValue = 20,
                    IsPercentage = true,
                    StartDate = now.AddDays(-10),
                    EndDate = now.AddDays(30),
                    UsageLimit = 1000,
                    MinimumOrderAmount = 15,
                    IsActive = true,
                },
                new Promotion {
                    Id = 2,
                    Code = "ITALIAN10",
                    Description = "10% off all Italian restaurants",
                    DiscountValue = 10,
                    IsPercentage = true,
                    StartDate = now.AddDays(-5),
                    EndDate = now.AddDays(15),
                    RestaurantId = 1,
                    IsActive = true,
                }
            };
            builder.Entity<Promotion>().HasData(promotions);

            // Seed Reviews
            var reviews = new List<Review>
            {
                new Review {
                    Id = 1,
                    RestaurantId = 1,
                    CustomerProfileId = 1,
                    Rating = 5.0M,
                    Comment = "Best Italian food I've ever had!",
                    CreatedAt = now.AddDays(-9)
                },
                new Review {
                    Id = 2,
                    RestaurantId = 2,
                    CustomerProfileId = 1,
                    Rating = 4.5M,
                    Comment = "Great tacos, but a bit spicy for my taste.",
                    CreatedAt = now.AddDays(-4)
                },
                new Review {
                    Id = 3,
                    RestaurantId = 3,
                    CustomerProfileId = 1,
                    Rating = 4.0M,
                    Comment = "Good Chinese food, but the service was slow.",
                    CreatedAt = now.AddDays(-2)
                },
                new Review {
                    Id = 4,
                    RestaurantId = 1,
                    CustomerProfileId = 1,
                    Rating = 3.5M,
                    Comment = "Decent food, but not as good as I expected.",
                    CreatedAt = now.AddDays(-1)
                },
                new Review {
                    Id = 5,
                    RestaurantId = 2,
                    CustomerProfileId = 1,
                    Rating = 4.0M,
                    Comment = "Loved the burrito, will order again!",
                    CreatedAt = now.AddDays(-3)
                },
                new Review {
                    Id = 6,
                    RestaurantId = 3,
                    CustomerProfileId = 1,
                    Rating = 5.0M,
                    Comment = "The best General Tso's chicken in town!",
                    CreatedAt = now.AddDays(-2)
                },
                new Review {
                    Id = 7,
                    RestaurantId = 1,
                    CustomerProfileId = 1,
                    Rating = 4.5M,
                    Comment = "Great pizza, but a bit overpriced.",
                    CreatedAt = now.AddDays(-1)
                },
                new Review {
                    Id = 8,
                    RestaurantId = 2,
                    CustomerProfileId = 1,
                    Rating = 4.0M,
                    Comment = "Good food, but the delivery was late.",
                    CreatedAt = now.AddDays(-3)
                },
            };
            builder.Entity<Review>().HasData(reviews);
        }
    }
}