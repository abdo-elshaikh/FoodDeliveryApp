using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FoodDeliveryApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Table Names (Optional, but good practice for clarity)
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Address>().ToTable("Addresses");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetails");

            // Configure Primary Key Properties (Explicitly, though often inferred by convention)
            modelBuilder.Entity<Customer>().HasKey(c => c.CustomerId);
            modelBuilder.Entity<Address>().HasKey(a => a.AddressId);
            modelBuilder.Entity<Employee>().HasKey(e => e.EmployeeId);
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<Item>().HasKey(i => i.ItemId);
            modelBuilder.Entity<Order>().HasKey(o => o.OrdId);
            modelBuilder.Entity<OrderDetail>().HasKey(od => od.OrdDetId);

            // Configure Relationships

            //// Employee - User (One-to-One)
            //modelBuilder.Entity<Employee>()
            //    .HasOne(e => e.User)
            //    .WithOne() // User doesn't have a direct navigation property back to Employee
            //    .HasForeignKey<Employee>(e => e.UserId)
            //    .HasPrincipalKey<User>(u => u.Id) // Assuming Id is the primary key of User
            //    .OnDelete(DeleteBehavior.Cascade); // Consider if Cascade is appropriate here

            //// Customer - User (One-to-One)
            //modelBuilder.Entity<Customer>()
            //    .HasOne(c => c.User)
            //    .WithOne()
            //    .HasForeignKey<Customer>(c => c.UserId)
            //    .HasPrincipalKey<User>(u => u.Id)
            //    .OnDelete(DeleteBehavior.Cascade); // Consider if Cascade is appropriate here

            //// Employee - Order (One-to-Many)
            //modelBuilder.Entity<Employee>()
            //    .HasMany(e => e.Orders)
            //    .WithOne(o => o.Employee)
            //    .HasForeignKey(o => o.EmployeeId)
            //    .OnDelete(DeleteBehavior.SetNull);

            //// Customer - Order (One-to-Many)
            //modelBuilder.Entity<Customer>()
            //    .HasMany(c => c.Orders)
            //    .WithOne(o => o.Customer)
            //    .HasForeignKey(o => o.CustomerId)
            //    .OnDelete(DeleteBehavior.Cascade); // Consider if Cascade is appropriate here

            // Customer - Address (One-to-Many)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Category - Item (One-to-Many)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Category)
                .HasForeignKey(i => i.CateqId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order - OrderDetail (One-to-Many)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrdId)
                .OnDelete(DeleteBehavior.Cascade);

            // Item - OrderDetail (One-to-Many)
            modelBuilder.Entity<Item>()
                .HasMany(i => i.OrderDetails)
                .WithOne(od => od.Item)
                .HasForeignKey(od => od.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Sample Data
            // Seed Roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Customer", NormalizedName = "CUSTOMER" },
                new IdentityRole { Id = "2", Name = "Employee", NormalizedName = "EMPLOYEE" },
                new IdentityRole { Id = "3", Name = "Admin", NormalizedName = "ADMIN" }
            );

            // Seed Admin User
            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "1",
                    UserName = "admin@fooddelivery.com",
                    NormalizedUserName = "ADMIN@FOODDELIVERY.COM",
                    Email = "admin@fooddelivery.com",
                    NormalizedEmail = "ADMIN@FOODDELIVERY.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin123!"),
                    Role = UserRole.Admin,
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new User
                {
                    Id = "2",
                    Email = "employee@fooddelivery.com",
                    UserName = "employee",
                    NormalizedUserName = "EMPLOYEE",
                    NormalizedEmail = "EMPLOYEE@FOODDELIVERY.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Employee123!"),
                    AccessFailedCount = 0,
                    Role = UserRole.Employee,
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new User
                {
                    Id = "3",
                    Email = "customer@fooddelivery.com",
                    UserName = "customer",
                    NormalizedUserName = "CUSTOMER",
                    EmailConfirmed = true,
                    NormalizedEmail = "CUSTOMER@FOODDELIVERY.COM",
                    PasswordHash = hasher.HashPassword(null, "Customer123!"),
                    AccessFailedCount = 0,
                    Role = UserRole.Customer,
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            );

            // Seed Admin Role Assignment
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "1",
                    RoleId = "3" // Admin role
                },
                new IdentityUserRole<string>
                {
                    UserId = "2",
                    RoleId = "2" // Employee role
                },
                new IdentityUserRole<string>
                {
                    UserId = "3",
                    RoleId = "1" // Customer role
                }
            );

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Beverages", CategoryDescription = "Drinks and refreshments" },
                new Category { CategoryId = 2, CategoryName = "Snacks", CategoryDescription = "Light bites" },
                new Category { CategoryId = 3, CategoryName = "Desserts", CategoryDescription = "Sweet treats" }
            );

            // Seed Employee
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    UserId = "2",
                    EmpCategory = EmployeeCategory.Delivery,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );

            // Seed Customer
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    FirstName = "Jane",
                    LastName = "Smith",
                    PhoneNumber = "123-456-7890",
                    UserId = "3",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );

            // Seed Address
            modelBuilder.Entity<Address>().HasData(
                new Address { AddressId = 1, Street = "123 Main St", City = "Springfield", State = "IL", ZipCode = "62701", CustomerId = 1 },
                new Address { AddressId = 2, Street = "456 Elm St", City = "Springfield", State = "IL", ZipCode = "62702", CustomerId = 1 }
            );

            // Seed Items
            modelBuilder.Entity<Item>().HasData(
                new Item { ItemId = 1, ItemName = "Cola", ItemPrice = 2.50m, CateqId = 1 },
                new Item { ItemId = 2, ItemName = "Chips", ItemPrice = 1.75m, CateqId = 2 },
                new Item { ItemId = 3, ItemName = "Cake", ItemPrice = 3.00m, CateqId = 3 },
                new Item { ItemId = 4, ItemName = "Water", ItemPrice = 1.00m, CateqId = 1 },
                new Item { ItemId = 5, ItemName = "Cookies", ItemPrice = 2.00m, CateqId = 3 },
                new Item { ItemId = 6, ItemName = "Candy", ItemPrice = 0.50m, CateqId = 2 },
                new Item { ItemId = 7, ItemName = "Juice", ItemPrice = 2.00m, CateqId = 1 },
                new Item { ItemId = 8, ItemName = "Brownie", ItemPrice = 2.50m, CateqId = 3 },
                new Item { ItemId = 9, ItemName = "Soda", ItemPrice = 1.50m, CateqId = 1 },
                new Item { ItemId = 10, ItemName = "Granola Bar", ItemPrice = 1.25m, CateqId = 2 }
            );

            // Seed Order
            modelBuilder.Entity<Order>().HasData(
                new Order { OrdId = 1, CustomerId = 1, EmployeeId = 1, OrdDate = DateTime.Now, OrdSum = 6.25m, OrdStatus = OrderStatus.Pending }
            );

            // Seed OrderDetail
            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail { OrdDetId = 1, OrdId = 1, ItemId = 1, OrdQuantity = 1, OrdAmount = 2.50m },
                new OrderDetail { OrdDetId = 2, OrdId = 1, ItemId = 2, OrdQuantity = 2, OrdAmount = 1.75m }
            );
        }
    }
}