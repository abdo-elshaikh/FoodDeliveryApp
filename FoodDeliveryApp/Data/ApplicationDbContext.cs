using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Models;
using Microsoft.AspNetCore.Identity;


namespace FoodDeliveryApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for each model
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantCategory> RestaurantCategories { get; set; }
        public DbSet<MenuItemCategory> MenuItemCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<OrderTracking> OrderTracking { get; set; }
        public DbSet<SearchLog> SearchLogs { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        // Drivers 
        public DbSet<Driver> Drivers { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add indexes for frequently queried fields
            modelBuilder.Entity<Restaurant>()
                .HasIndex(r => r.CategoryId);

            modelBuilder.Entity<MenuItem>()
                .HasIndex(mi => mi.RestaurantId);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.UserId);

            modelBuilder.Entity<Review>()
                .HasIndex(r => r.RestaurantId);

            // Restaurant - RestaurantCategory (Category)
            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.Category)
                .WithMany(c => c.Restaurants)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Restaurant - Owner (ApplicationUser)
            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.Owner)
                .WithMany()
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Restaurant - Reviews
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Reviews)
                .WithOne(rv => rv.Restaurant)
                .HasForeignKey(rv => rv.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Restaurant - MenuItemCategories (Categories)
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Categories)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Restaurant - MenuItems
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.MenuItems)
                .WithOne(mi => mi.Restaurant)
                .HasForeignKey(mi => mi.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Restaurant - Promotions
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Promotions)
                .WithOne(p => p.Restaurant)
                .HasForeignKey(p => p.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            // MenuItemCategory - MenuItems
            modelBuilder.Entity<MenuItemCategory>()
                .HasMany(mic => mic.MenuItems)
                .WithOne(mi => mi.Category)
                .HasForeignKey(mi => mi.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order - User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Order - DeliveryAddress
            modelBuilder.Entity<Order>()
                .HasOne(o => o.DeliveryAddress)
                .WithMany()
                .HasForeignKey(o => o.DeliveryAddressId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Order - OrderItems
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order - OrderTracking
            modelBuilder.Entity<Order>()
                .HasMany(o => o.TrackingHistory)
                .WithOne(ot => ot.Order)
                .HasForeignKey(ot => ot.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem - MenuItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany(mi => mi.OrderItems)
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // OrderItem - Restaurant
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Restaurant)
                .WithMany()
                .HasForeignKey(oi => oi.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Cart - User
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Cart - CartItems
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem - MenuItem
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.MenuItem)
                .WithMany()
                .HasForeignKey(ci => ci.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Review - User
            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(rv => rv.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Review - Restaurant
            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.Restaurant)
                .WithMany(r => r.Reviews)
                .HasForeignKey(rv => rv.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review - MenuItem
            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.MenuItem)
                .WithMany(mi => mi.Reviews)
                .HasForeignKey(rv => rv.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal precision for Review.Rating
            modelBuilder.Entity<Review>()
                .Property(rv => rv.Rating)
                .HasColumnType("decimal(18,2)");

            // Configure decimal precision for Cart.DeliveryFee and Cart.TaxRate
            modelBuilder.Entity<Cart>()
                .Property(c => c.DeliveryFee)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Cart>()
                .Property(c => c.TaxRate)
                .HasColumnType("decimal(18,2)");

            // OrderTracking - Order
            modelBuilder.Entity<OrderTracking>()
                .HasOne(ot => ot.Order)
                .WithMany(o => o.TrackingHistory)
                .HasForeignKey(ot => ot.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // OrderTracking - Driver
            modelBuilder.Entity<OrderTracking>()
                .HasOne(ot => ot.Driver)
                .WithMany()
                .HasForeignKey(ot => ot.DriverId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Favorite - User
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Favorite - MenuItem 
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.MenuItem)
                .WithMany()
                .HasForeignKey(f => f.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            //add roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Owner",
                    NormalizedName = "OWNER"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "Driver",
                    NormalizedName = "DRIVER"
                },
                new IdentityRole
                {
                    Id = "4",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                }
            );

            var hasher = new PasswordHasher<ApplicationUser>();

            // Use fixed GUIDs for seeded users
            var adminId = "9c81cb6a-9f7e-46b8-b2fe-cd86d8ccb095";
            var ownerId = "4d6ca63e-e331-4553-a2ba-d5ff0e401d5a";
            var userId = "b6db1b82-b9df-480d-87bd-868408cc08c8";
            var driverId = "cc235c9c-cbf4-42d3-8d35-342b3edb7fd7";

            // Admin
            var admin = new ApplicationUser
            {
                Id = adminId,
                UserName = "admin@fooddeliveryapp",
                Email = "admin@fooddeliveryapp",
                NormalizedEmail = "admin@fooddeliveryapp",
                EmailConfirmed = true,
                NormalizedUserName = "admin@fooddeliveryapp",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "Admin123!"),
                LockoutEnabled = false,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Role = UserType.Admin,
            };

            // Owner
            var owner = new ApplicationUser
            {
                Id = ownerId,
                UserName = "owner@fooddeliveryapp",
                Email = "owner@fooddeliveryapp",
                NormalizedEmail = "owner@fooddeliveryapp",
                EmailConfirmed = true,
                NormalizedUserName = "owner@fooddeliveryapp",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "Owner123!"),
                LockoutEnabled = false,
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Role = UserType.Owner,
            };

            // User
            var user = new ApplicationUser
            {
                Id = userId,
                UserName = "user@fooddeliveryapp",
                Email = "user@fooddeliveryapp",
                NormalizedEmail = "user@fooddeliveryapp",
                EmailConfirmed = true,
                NormalizedUserName = "user@fooddeliveryapp",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "User123!"),
                LockoutEnabled = false,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Role = UserType.Customer,
            };

            // Driver
            var driver = new ApplicationUser
            {
                Id = driverId,
                UserName = "driver@fooddeliveryapp",
                Email = "driver@fooddeliveryapp",
                NormalizedEmail = "driver@fooddeliveryapp",
                EmailConfirmed = true,
                NormalizedUserName = "driver@fooddeliveryapp",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "Driver123!"),
                LockoutEnabled = false,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Role = UserType.Driver,
            };

            modelBuilder.Entity<ApplicationUser>().HasData(admin, owner, user, driver);

            // add user to role
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "1", UserId = adminId },
                new IdentityUserRole<string> { RoleId = "2", UserId = ownerId },
                new IdentityUserRole<string> { RoleId = "4", UserId = userId },
                new IdentityUserRole<string> { RoleId = "3", UserId = driverId }
            );

            // Seed restaurant categories
            modelBuilder.Entity<RestaurantCategory>().HasData(
                new RestaurantCategory { Id = 1, Name = "Pizza", Description = "Delicious pizza with a twist", ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80" },
                new RestaurantCategory { Id = 2, Name = "Burger", Description = "Juicy and crispy burgers", ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1299&q=80" },
                new RestaurantCategory { Id = 3, Name = "French Fries", Description = "Crunchy and tangy fries", ImageUrl = "https://images.unsplash.com/photo-1630384060421-cb20d0e0649d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1025&q=80" },
                new RestaurantCategory { Id = 4, Name = "Salad", Description = "Fresh and healthy salads", ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80" },
                new RestaurantCategory { Id = 5, Name = "Sushi", Description = "Fresh and juicy sushi", ImageUrl = "https://images.unsplash.com/photo-1579871494447-9811cf80d66c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80" },
                new RestaurantCategory { Id = 6, Name = "Italian", Description = "Authentic Italian cuisine", ImageUrl = "https://images.unsplash.com/photo-1498579150354-977475b7ea0b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80" },
                new RestaurantCategory { Id = 7, Name = "Mexican", Description = "Spicy and flavorful Mexican food", ImageUrl = "https://images.unsplash.com/photo-1565299585323-38d6b0865b47?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1080&q=80" },
                new RestaurantCategory { Id = 8, Name = "Chinese", Description = "Traditional and modern Chinese dishes", ImageUrl = "https://images.unsplash.com/photo-1563245372-f21724e3856d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80" },
                new RestaurantCategory { Id = 9, Name = "Indian", Description = "Aromatic and spicy Indian cuisine", ImageUrl = "https://images.unsplash.com/photo-1585937421612-70a008356c36?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1036&q=80" },
                new RestaurantCategory { Id = 10, Name = "Dessert", Description = "Sweet treats and desserts", ImageUrl = "https://images.unsplash.com/photo-1551024601-bec78aea704b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1364&q=80" }
            );

            // Seed menu items categories
            modelBuilder.Entity<MenuItemCategory>().HasData(
                new MenuItemCategory { Id = 1, Name = "Pizza Toppings", Description = "Add toppings to your pizza", ImageUrl = "https://images.unsplash.com/photo-1528137871618-79d2761e3fd5?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80" },
                new MenuItemCategory { Id = 2, Name = "Burger Toppings", Description = "Add toppings to your burger", ImageUrl = "https://images.unsplash.com/photo-1550317138-10000687a72b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1520&q=80" },
                new MenuItemCategory { Id = 3, Name = "French Fries Toppings", Description = "Add toppings to your french fries", ImageUrl = "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80" },
                new MenuItemCategory { Id = 4, Name = "Salad Toppings", Description = "Add toppings to your salad", ImageUrl = "https://images.unsplash.com/photo-1540420773420-3366772f4999?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1384&q=80" },
                new MenuItemCategory { Id = 5, Name = "Sushi Toppings", Description = "Add toppings to your sushi", ImageUrl = "https://images.unsplash.com/photo-1617196034183-421b4917c92d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80" },
                new MenuItemCategory { Id = 6, Name = "Appetizers", Description = "Start your meal with delicious appetizers", ImageUrl = "https://images.unsplash.com/photo-1626645738196-c2a7c87a8f58?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80" },
                new MenuItemCategory { Id = 7, Name = "Main Course", Description = "Hearty and satisfying main dishes", ImageUrl = "https://images.unsplash.com/photo-1544025162-d76694265947?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1469&q=80" },
                new MenuItemCategory { Id = 8, Name = "Desserts", Description = "Sweet treats to finish your meal", ImageUrl = "https://images.unsplash.com/photo-1563805042-7684c019e1cb?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1527&q=80" },
                new MenuItemCategory { Id = 9, Name = "Beverages", Description = "Refreshing drinks to complement your food", ImageUrl = "https://images.unsplash.com/photo-1544145945-f90425340c7e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80" },
                new MenuItemCategory { Id = 10, Name = "Sides", Description = "Perfect accompaniments to your main dish", ImageUrl = "https://images.unsplash.com/photo-1604908176997-125f25cc6f3d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1013&q=80" }
            );

            // Seed restaurants
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant
                {
                    Id = 1,
                    Name = "Pizza Hut",
                    Description = "Our famous pizza place",
                    Address = "123 Main St",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    IsActive = true,
                    CategoryId = 1,
                    Rating = 4.5m,
                    PhoneNumber = "123-456-7890",
                    OpeningTime = new TimeSpan(10, 0, 0),
                    ClosingTime = new TimeSpan(22, 0, 0),
                    DeliveryFee = 5.99m,
                    DeliveryTime = "30-45 minutes",
                    OwnerId = ownerId,
                    Email = "pizza@fooddeliveryapp",
                    LocationUrl = "https://maps.google.com/maps?q=New+York+City",
                    TaxRate = 8.5m,
                    MinimumOrderAmount = 20.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Restaurant
                {
                    Id = 2,
                    Name = "Burger King",
                    Description = "Fast and fresh burgers",
                    Address = "456 Main St",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1299&q=80",
                    IsActive = true,
                    CategoryId = 2,
                    Rating = 4.0m,
                    PhoneNumber = "123-456-7890",
                    OpeningTime = new TimeSpan(10, 0, 0),
                    ClosingTime = new TimeSpan(22, 0, 0),
                    DeliveryFee = 5.99m,
                    DeliveryTime = "30-45 minutes",
                    OwnerId = ownerId,
                    Email = "burgerking@fooddeliveryapp",
                    LocationUrl = "https://maps.google.com/maps?q=New+York+City",
                    TaxRate = 8.5m,
                    MinimumOrderAmount = 20.99m,
                    CreatedAt = DateTime.UtcNow,
                },
                new Restaurant
                {
                    Id = 3,
                    Name = "McDonald's",
                    Description = "Fast and fresh burgers",
                    Address = "456 Main St",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    ImageUrl = "https://images.unsplash.com/photo-1619881585376-15ad0d9b8b43?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80",
                    IsActive = true,
                    CategoryId = 2,
                    Rating = 4.0m,
                    PhoneNumber = "123-456-7890",
                    OpeningTime = new TimeSpan(10, 0, 0),
                    ClosingTime = new TimeSpan(22, 0, 0),
                    DeliveryFee = 5.99m,
                    DeliveryTime = "30-45 minutes",
                    OwnerId = ownerId,
                    Email = "mcdonalds@fooddeliveryapp",
                    LocationUrl = "https://maps.google.com/maps?q=New+York+City",
                    WebsiteUrl = "https://www.mcdonalds.com/",
                    TaxRate = 8.5m,
                    MinimumOrderAmount = 20.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Restaurant
                {
                    Id = 4,
                    Name = "Subway",
                    Description = "Fresh and healthy sandwiches",
                    Address = "789 Main St",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    ImageUrl = "https://images.unsplash.com/photo-1509722747041-616f39b57569?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    IsActive = true,
                    CategoryId = 2,
                    Rating = 4.5m,
                    PhoneNumber = "123-456-7890",
                    OpeningTime = new TimeSpan(10, 0, 0),
                    ClosingTime = new TimeSpan(22, 0, 0),
                    DeliveryFee = 5.99m,
                    DeliveryTime = "30-45 minutes",
                    OwnerId = ownerId,
                    Email = "subway@fooddeliveryapp",
                    LocationUrl = "https://maps.google.com/maps?q=New+York+City",
                    WebsiteUrl = "https://www.subway.com/",
                    TaxRate = 8.5m,
                    MinimumOrderAmount = 20.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Restaurant
                {
                    Id = 5,
                    Name = "Olive Garden",
                    Description = "Authentic Italian cuisine",
                    Address = "101 Broadway",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10002",
                    ImageUrl = "https://images.unsplash.com/photo-1498579150354-977475b7ea0b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    IsActive = true,
                    CategoryId = 6,
                    Rating = 4.7m,
                    PhoneNumber = "123-456-7891",
                    OpeningTime = new TimeSpan(11, 0, 0),
                    ClosingTime = new TimeSpan(23, 0, 0),
                    DeliveryFee = 6.99m,
                    DeliveryTime = "35-50 minutes",
                    OwnerId = ownerId,
                    Email = "olivegarden@fooddeliveryapp",
                    LocationUrl = "https://maps.google.com/maps?q=New+York+City",
                    WebsiteUrl = "https://www.olivegarden.com/",
                    TaxRate = 8.5m,
                    MinimumOrderAmount = 25.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Restaurant
                {
                    Id = 6,
                    Name = "Taco Bell",
                    Description = "Delicious Mexican-inspired fast food",
                    Address = "222 5th Avenue",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10003",
                    ImageUrl = "https://images.unsplash.com/photo-1565299585323-38d6b0865b47?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1080&q=80",
                    IsActive = true,
                    CategoryId = 7,
                    Rating = 4.2m,
                    PhoneNumber = "123-456-7892",
                    OpeningTime = new TimeSpan(10, 0, 0),
                    ClosingTime = new TimeSpan(23, 0, 0),
                    DeliveryFee = 4.99m,
                    DeliveryTime = "25-40 minutes",
                    OwnerId = ownerId,
                    Email = "tacobell@fooddeliveryapp",
                    LocationUrl = "https://maps.google.com/maps?q=New+York+City",
                    WebsiteUrl = "https://www.tacobell.com/",
                    TaxRate = 8.5m,
                    MinimumOrderAmount = 20.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Restaurant
                {
                    Id = 7,
                    Name = "Panda Express",
                    Description = "Fast and fresh Chinese cuisine",
                    Address = "333 7th Avenue",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10004",
                    ImageUrl = "https://images.unsplash.com/photo-1563245372-f21724e3856d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80",
                    IsActive = true,
                    CategoryId = 8,
                    Rating = 4.3m,
                    PhoneNumber = "123-456-7893",
                    OpeningTime = new TimeSpan(11, 0, 0),
                    ClosingTime = new TimeSpan(22, 30, 0),
                    DeliveryFee = 5.49m,
                    DeliveryTime = "30-45 minutes",
                    OwnerId = ownerId,
                    Email = "pandaexpress@fooddeliveryapp",
                    LocationUrl = "https://maps.google.com/maps?q=New+York+City",
                    WebsiteUrl = "https://www.pandaexpress.com/",
                    TaxRate = 8.5m,
                    MinimumOrderAmount = 20.99m,
                    CreatedAt = DateTime.UtcNow
                },
                new Restaurant
                {
                    Id = 8,
                    Name = "Taj Mahal",
                    Description = "Authentic Indian cuisine with rich flavors",
                    Address = "444 Lexington Ave",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10005",
                    ImageUrl = "https://images.unsplash.com/photo-1585937421612-70a008356c36?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1036&q=80",
                    IsActive = true,
                    CategoryId = 9,
                    Rating = 4.8m,
                    PhoneNumber = "123-456-7894",
                    OpeningTime = new TimeSpan(12, 0, 0),
                    ClosingTime = new TimeSpan(23, 0, 0),
                    DeliveryFee = 6.49m,
                    DeliveryTime = "40-55 minutes",
                    OwnerId = ownerId,
                    Email = "tajmahal@fooddeliveryapp",
                    LocationUrl = "https://maps.google.com/maps?q=New+York+City",
                    WebsiteUrl = "https://www.tajmahal.com/",
                    TaxRate = 8.5m,
                    MinimumOrderAmount = 25.99m,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed menu items
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    Id = 1,
                    Name = "Margherita Pizza",
                    Description = "Classic pizza with tomatoes, mozzarella, and basil",
                    Price = 12.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1604382354936-07c5d9983bd3?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    IsAvailable = true,
                    RestaurantId = 1,
                    CategoryId = 1,
                    Calories = 500,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.5,
                    SpiceLevel = 3,
                    PreparationTime = new TimeSpan(0, 15, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 2,
                    Name = "Pepperoni Pizza",
                    Description = "Pepperoni pizza with tomatoes, mozzarella, and basil",
                    Price = 13.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1628840042765-356cda07504e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1480&q=80",
                    IsAvailable = true,
                    RestaurantId = 1,
                    CategoryId = 1,
                    Calories = 550,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.7,
                    SpiceLevel = 4,
                    PreparationTime = new TimeSpan(0, 20, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 3,
                    Name = "Cheeseburger",
                    Description = "Tasty cheeseburger with lettuce, tomatoes, and onions",
                    Price = 6.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1299&q=80",
                    IsAvailable = true,
                    RestaurantId = 2,
                    CategoryId = 2,
                    Calories = 250,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.3,
                    SpiceLevel = 2,
                    PreparationTime = new TimeSpan(0, 10, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 4,
                    Name = "Chicken Sandwich",
                    Description = "Delicious chicken sandwich with lettuce, tomatoes, and mayo",
                    Price = 8.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1606755962773-d324e0a13086?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80",
                    IsAvailable = true,
                    RestaurantId = 2,
                    CategoryId = 2,
                    Calories = 300,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.6,
                    SpiceLevel = 3,
                    PreparationTime = new TimeSpan(0, 15, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 5,
                    Name = "Vegetable Pizza",
                    Description = "Fresh vegetable pizza with bell peppers, onions, and mushrooms",
                    Price = 11.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1593560708920-61dd98c46a4e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    IsAvailable = true,
                    RestaurantId = 1,
                    CategoryId = 1,
                    Calories = 450,
                    IsVegan = false,
                    IsVegetarian = true,
                    Rating = 4.4,
                    SpiceLevel = 2,
                    PreparationTime = new TimeSpan(0, 18, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 6,
                    Name = "BBQ Chicken Pizza",
                    Description = "Delicious BBQ chicken pizza with red onions and cilantro",
                    Price = 14.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1594007654729-407eedc4fe24?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1428&q=80",
                    IsAvailable = true,
                    RestaurantId = 1,
                    CategoryId = 1,
                    Calories = 580,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.8,
                    SpiceLevel = 3,
                    PreparationTime = new TimeSpan(0, 20, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 7,
                    Name = "Bacon Cheeseburger",
                    Description = "Juicy burger with bacon, cheese, lettuce, and special sauce",
                    Price = 8.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1553979459-d2229ba7433b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1368&q=80",
                    IsAvailable = true,
                    RestaurantId = 2,
                    CategoryId = 2,
                    Calories = 680,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.7,
                    SpiceLevel = 2,
                    PreparationTime = new TimeSpan(0, 12, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 8,
                    Name = "Veggie Burger",
                    Description = "Plant-based burger with all the fixings",
                    Price = 7.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1520072959219-c595dc870360?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1490&q=80",
                    IsAvailable = true,
                    RestaurantId = 2,
                    CategoryId = 2,
                    Calories = 420,
                    IsVegan = true,
                    IsVegetarian = true,
                    Rating = 4.5,
                    SpiceLevel = 1,
                    PreparationTime = new TimeSpan(0, 10, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 9,
                    Name = "Fettuccine Alfredo",
                    Description = "Creamy fettuccine pasta with parmesan cheese sauce",
                    Price = 13.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1645112411341-6c4fd023882a?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    IsAvailable = true,
                    RestaurantId = 5,
                    CategoryId = 6,
                    Calories = 650,
                    IsVegan = false,
                    IsVegetarian = true,
                    Rating = 4.6,
                    SpiceLevel = 1,
                    PreparationTime = new TimeSpan(0, 15, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 10,
                    Name = "Chicken Parmesan",
                    Description = "Breaded chicken topped with marinara sauce and melted cheese",
                    Price = 15.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1632778149955-e80f8ceca2e8?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    IsAvailable = true,
                    RestaurantId = 5,
                    CategoryId = 7,
                    Calories = 720,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.8,
                    SpiceLevel = 2,
                    PreparationTime = new TimeSpan(0, 25, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 11,
                    Name = "Crunchy Taco",
                    Description = "Classic crunchy taco with seasoned beef, lettuce, and cheese",
                    Price = 2.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1551504734-5ee1c4a1479b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    IsAvailable = true,
                    RestaurantId = 6,
                    CategoryId = 7,
                    Calories = 170,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.3,
                    SpiceLevel = 3,
                    PreparationTime = new TimeSpan(0, 5, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 12,
                    Name = "Burrito Supreme",
                    Description = "Large burrito filled with beef, beans, rice, and all the toppings",
                    Price = 7.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1626700051175-6818013e1d4f?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80",
                    IsAvailable = true,
                    RestaurantId = 6,
                    CategoryId = 7,
                    Calories = 650,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.5,
                    SpiceLevel = 4,
                    PreparationTime = new TimeSpan(0, 8, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 13,
                    Name = "Orange Chicken",
                    Description = "Crispy chicken pieces tossed in a sweet and tangy orange sauce",
                    Price = 9.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1525755662778-989d0524087e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    IsAvailable = true,
                    RestaurantId = 7,
                    CategoryId = 7,
                    Calories = 490,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.7,
                    SpiceLevel = 2,
                    PreparationTime = new TimeSpan(0, 15, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 14,
                    Name = "Fried Rice",
                    Description = "Stir-fried rice with vegetables, eggs, and your choice of protein",
                    Price = 8.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1603133872878-684f208fb84b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1625&q=80",
                    IsAvailable = true,
                    RestaurantId = 7,
                    CategoryId = 7,
                    Calories = 380,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.4,
                    SpiceLevel = 2,
                    PreparationTime = new TimeSpan(0, 12, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 15,
                    Name = "Butter Chicken",
                    Description = "Tender chicken in a rich and creamy tomato-based sauce",
                    Price = 14.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1603894584373-5ac82b2ae398?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    IsAvailable = true,
                    RestaurantId = 8,
                    CategoryId = 7,
                    Calories = 550,
                    IsVegan = false,
                    IsVegetarian = false,
                    Rating = 4.9,
                    SpiceLevel = 3,
                    PreparationTime = new TimeSpan(0, 20, 0),
                    CreatedAt = DateTime.UtcNow
                },
                new MenuItem
                {
                    Id = 16,
                    Name = "Garlic Naan",
                    Description = "Soft flatbread with garlic and butter",
                    Price = 3.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1596428043595-8eee307e6979?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    IsAvailable = true,
                    RestaurantId = 8,
                    CategoryId = 10,
                    Calories = 210,
                    IsVegan = false,
                    IsVegetarian = true,
                    Rating = 4.7,
                    SpiceLevel = 1,
                    PreparationTime = new TimeSpan(0, 8, 0),
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed promotions
            modelBuilder.Entity<Promotion>().HasData(
                new Promotion
                {
                    Id = 1,
                    Code = "10OFF",
                    Description = "10% off your weekly orders",
                    IsActive = true,
                    DiscountAmount = 10,
                    MinimumOrderAmount = 50,
                    ValidUntil = DateTime.UtcNow.AddMonths(1),
                    RestaurantId = 1,
                    IsPercentage = true,
                    ImageUrl = "https://images.unsplash.com/photo-1607083206968-13611e3d76db?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1515&q=80",
                    Title = "10% Off",
                    CreatedAt = DateTime.UtcNow
                },
                new Promotion
                {
                    Id = 2,
                    Code = "SAVE25",
                    Description = "25% off your weekly orders",
                    IsActive = true,
                    DiscountAmount = 25,
                    MinimumOrderAmount = 50,
                    ValidUntil = DateTime.UtcNow.AddMonths(1),
                    RestaurantId = 2,
                    IsPercentage = true,
                    ImageUrl = "https://images.unsplash.com/photo-1626684496076-07e23c6361ff?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    Title = "25% Off",
                    CreatedAt = DateTime.UtcNow
                },
                new Promotion
                {
                    Id = 3,
                    Code = "NEWYEAR2022",
                    Description = "Buy 2 get 1 free on new year's eve",
                    IsActive = true,
                    DiscountAmount = 0,
                    MinimumOrderAmount = 50,
                    ValidUntil = DateTime.UtcNow.AddDays(1),
                    RestaurantId = 3,
                    IsPercentage = false,
                    ImageUrl = "https://images.unsplash.com/photo-1577563908411-5077b6dc7624?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    Title = "Buy 2 Get 1 Free",
                    CreatedAt = DateTime.UtcNow
                },
                new Promotion
                {
                    Id = 4,
                    Code = "SUMMER2022",
                    Description = "15% off summer deals",
                    IsActive = true,
                    DiscountAmount = 15,
                    MinimumOrderAmount = 50,
                    ValidUntil = DateTime.UtcNow.AddMonths(3),
                    RestaurantId = 4,
                    IsPercentage = true,
                    ImageUrl = "https://images.unsplash.com/photo-1473186578172-c141e6798cf4?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1473&q=80",
                    Title = "15% Off Summer",
                    CreatedAt = DateTime.UtcNow
                },
                new Promotion
                {
                    Id = 5,
                    Code = "SUMMER2022",
                    Description = "15% off summer deals",
                    IsActive = false,
                    DiscountAmount = 15,
                    MinimumOrderAmount = 50,
                    ValidUntil = DateTime.UtcNow.AddMonths(3),
                    RestaurantId = null,
                    IsPercentage = true,
                    ImageUrl = "https://images.unsplash.com/photo-1541518763669-27fef04b14ea?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1472&q=80",
                    Title = "15% Off Summer",
                    CreatedAt = DateTime.UtcNow
                },
                new Promotion
                {
                    Id = 6,
                    Code = "SUMMER2022",
                    Description = "15% off summer deals",
                    IsActive = true,
                    DiscountAmount = 15,
                    MinimumOrderAmount = 50,
                    ValidUntil = DateTime.UtcNow.AddMonths(3),
                    RestaurantId = null,
                    IsPercentage = true,
                    ImageUrl = "https://images.unsplash.com/photo-1534349762230-e0cadf78f5da?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    Title = "15% Off Summer",
                    CreatedAt = DateTime.UtcNow
                },
                new Promotion
                {
                    Id = 7,
                    Code = "WELCOME15",
                    Description = "15% off your first order",
                    IsActive = true,
                    DiscountAmount = 15,
                    MinimumOrderAmount = 20,
                    ValidUntil = DateTime.UtcNow.AddMonths(6),
                    RestaurantId = null, // Global promotion
                    IsPercentage = true,
                    ImageUrl = "https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    Title = "Welcome Discount",
                    CreatedAt = DateTime.UtcNow
                },
                new Promotion
                {
                    Id = 8,
                    Code = "PASTA20",
                    Description = "20% off all pasta dishes",
                    IsActive = true,
                    DiscountAmount = 20,
                    MinimumOrderAmount = 15,
                    ValidUntil = DateTime.UtcNow.AddMonths(2),
                    RestaurantId = 5, // Olive Garden
                    IsPercentage = true,
                    ImageUrl = "https://images.unsplash.com/photo-1551183053-bf91a1d81141?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1632&q=80",
                    Title = "Pasta Special",
                    CreatedAt = DateTime.UtcNow
                },
                new Promotion
                {
                    Id = 9,
                    Code = "TACOTUESDAY",
                    Description = "Buy one taco, get one free every Tuesday",
                    IsActive = true,
                    DiscountAmount = 0, // Special promotion
                    MinimumOrderAmount = 5,
                    ValidUntil = DateTime.UtcNow.AddMonths(12),
                    RestaurantId = 6, // Taco Bell
                    IsPercentage = false,
                    ImageUrl = "https://images.unsplash.com/photo-1504544750208-dc0358e63f7f?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    Title = "Taco Tuesday",
                    CreatedAt = DateTime.UtcNow
                },
                new Promotion
                {
                    Id = 10,
                    Code = "FAMILYMEAL",
                    Description = "Save $10 on family meal combos",
                    IsActive = true,
                    DiscountAmount = 10,
                    MinimumOrderAmount = 40,
                    ValidUntil = DateTime.UtcNow.AddMonths(3),
                    RestaurantId = 7, // Panda Express
                    IsPercentage = false,
                    ImageUrl = "https://images.unsplash.com/photo-1493770348161-369560ae357d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80",
                    Title = "Family Meal Deal",
                    CreatedAt = DateTime.UtcNow
                },
                new Promotion
                {
                    Id = 11,
                    Code = "SPICY25",
                    Description = "25% off all spicy dishes",
                    IsActive = true,
                    DiscountAmount = 25,
                    MinimumOrderAmount = 30,
                    ValidUntil = DateTime.UtcNow.AddMonths(2),
                    RestaurantId = 8, // Taj Mahal
                    IsPercentage = true,
                    ImageUrl = "https://images.unsplash.com/photo-1505253758473-96b7015fcd40?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1400&q=80",
                    Title = "Spicy Special",
                    CreatedAt = DateTime.UtcNow
                },
                new Promotion
                {
                    Id = 12,
                    Code = "FREESHIPPING",
                    Description = "Free delivery on all orders",
                    IsActive = true,
                    DiscountAmount = 0, // Special promotion
                    MinimumOrderAmount = 25,
                    ValidUntil = DateTime.UtcNow.AddDays(14),
                    RestaurantId = null, // Global promotion
                    IsPercentage = false,
                    ImageUrl = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1558&q=80",
                    Title = "Free Delivery",
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed reviews
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    Title = "Amazing Pizza!",
                    Content = "The Margherita Pizza was absolutely delicious. Fresh ingredients and perfect crust!",
                    Rating = 5.0m,
                    UserId = userId, // Use the userId variable from the context
                    RestaurantId = 1,
                    MenuItemId = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Review
                {
                    Id = 2,
                    Title = "Good but could be better",
                    Content = "The pepperoni pizza was good, but I wish it had more toppings.",
                    Rating = 3.5m,
                    UserId = userId,
                    RestaurantId = 1,
                    MenuItemId = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Review
                {
                    Id = 3,
                    Title = "Best burger in town!",
                    Content = "This cheeseburger is amazing. Juicy, flavorful, and perfectly cooked.",
                    Rating = 4.8m,
                    UserId = userId,
                    RestaurantId = 2,
                    MenuItemId = 3,
                    CreatedAt = DateTime.UtcNow
                },
                new Review
                {
                    Id = 4,
                    Title = "Decent sandwich",
                    Content = "The chicken sandwich was okay, but nothing special.",
                    Rating = 3.0m,
                    UserId = userId,
                    RestaurantId = 2,
                    MenuItemId = 4,
                    CreatedAt = DateTime.UtcNow
                },
                new Review
                {
                    Id = 5,
                    Title = "Authentic Italian",
                    Content = "The pasta was cooked to perfection and the sauce was delicious!",
                    Rating = 4.7m,
                    UserId = userId,
                    RestaurantId = 5,
                    MenuItemId = 9,
                    CreatedAt = DateTime.UtcNow
                },
                new Review
                {
                    Id = 6,
                    Title = "Spicy and flavorful",
                    Content = "The tacos had the perfect amount of spice and the ingredients were fresh.",
                    Rating = 4.5m,
                    UserId = userId,
                    RestaurantId = 6,
                    MenuItemId = 11,
                    CreatedAt = DateTime.UtcNow
                },
                new Review
                {
                    Id = 7,
                    Title = "Delicious Chinese food",
                    Content = "The orange chicken was crispy and the sauce was perfect - not too sweet.",
                    Rating = 4.6m,
                    UserId = userId,
                    RestaurantId = 7,
                    MenuItemId = 13,
                    CreatedAt = DateTime.UtcNow
                },
                new Review
                {
                    Id = 8,
                    Title = "Best Indian food I've had",
                    Content = "The butter chicken was rich, creamy, and full of flavor. Highly recommend!",
                    Rating = 5.0m,
                    UserId = userId,
                    RestaurantId = 8,
                    MenuItemId = 15,
                    CreatedAt = DateTime.UtcNow
                },
                new Review
                {
                    Id = 9,
                    Title = "Great restaurant",
                    Content = "Pizza Hut never disappoints. Fast delivery and delicious food!",
                    Rating = 4.5m,
                    UserId = userId,
                    RestaurantId = 1,
                    MenuItemId = null,
                    CreatedAt = DateTime.UtcNow
                },
                new Review
                {
                    Id = 10,
                    Title = "Excellent service",
                    Content = "The staff at Olive Garden was very friendly and attentive.",
                    Rating = 4.8m,
                    UserId = userId,
                    RestaurantId = 5,
                    MenuItemId = null,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
