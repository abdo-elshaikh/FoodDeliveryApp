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
        public DbSet<CustomerProfile> CustomerProfiles { get; set; } // remove
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; } // remove
        public DbSet<Address> Addresses { get; set; }
        public DbSet<RestaurantCategory> RestaurantCategories { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItemCategory> MenuItemCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<CustomizationOption> CustomizationOptions { get; set; } // remove
        public DbSet<CustomizationChoice> CustomizationChoices { get; set; } // remove
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderCustomization> OrderCustomizations { get; set; } // remove
        public DbSet<PaymentMethod> PaymentMethods { get; set; } // remove
        public DbSet<Payment> Payments { get; set; } // remove
        public DbSet<Promotion> Promotions { get; set; } // remove
        public DbSet<PromotionUsage> PromotionUsages { get; set; } // remove
        public DbSet<Review> Reviews { get; set; } // to restaurant
        public DbSet<Cart> Carts { get; set; } 
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Customization> Customizations { get; set; } // remove
        public DbSet<SearchHistory> SearchHistory { get; set; } // remove

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships and constraints
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasOne(u => u.CustomerProfile)
                    .WithOne(c => c.User)
                    .HasForeignKey<CustomerProfile>(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(u => u.EmployeeProfile)
                    .WithOne(e => e.User)
                    .HasForeignKey<EmployeeProfile>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Restaurants)
                    .WithOne(r => r.Owner)
                    .HasForeignKey(r => r.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.SearchHistories)
                    .WithOne(sh => sh.User)
                    .HasForeignKey(sh => sh.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<CustomerProfile>(entity =>
            {
                entity.Property(c => c.UserId).HasMaxLength(450).IsRequired();
                entity.HasMany(c => c.Addresses)
                    .WithOne(a => a.CustomerProfile)
                    .HasForeignKey(a => a.CustomerProfileId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.Reviews)
                    .WithOne(r => r.CustomerProfile)
                    .HasForeignKey(r => r.CustomerProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<EmployeeProfile>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(450).IsRequired();
            });

            builder.Entity<Address>(entity =>
            {
                entity.Property(a => a.CustomerProfileId).IsRequired();
                entity.HasIndex(a => a.PostalCode);
            });

            builder.Entity<RestaurantCategory>(entity =>
            {
                entity.HasMany(c => c.Restaurants)
                    .WithOne(r => r.Category)
                    .HasForeignKey(r => r.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<MenuItemCategory>(entity =>
            {
                entity.HasMany(c => c.MenuItems)
                    .WithOne(m => m.Category)
                    .HasForeignKey(m => m.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasIndex(c => c.Name).IsUnique();
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Description).HasMaxLength(500);
            });

            builder.Entity<Restaurant>(entity =>
            {
                entity.Property(r => r.OwnerId).HasMaxLength(450).IsRequired();
                entity.HasMany(r => r.MenuItems)
                    .WithOne(m => m.Restaurant)
                    .HasForeignKey(m => m.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(r => r.Orders)
                    .WithOne(o => o.Restaurant)
                    .HasForeignKey(o => o.RestaurantId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(r => r.Reviews)
                    .WithOne(rev => rev.Restaurant)
                    .HasForeignKey(rev => rev.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(r => r.Promotions)
                    .WithOne(p => p.Restaurant)
                    .HasForeignKey(p => p.RestaurantId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(r => r.DeliveryFee).HasColumnType("decimal(18,2)");
                entity.Property(r => r.Rating).HasColumnType("decimal(3,2)");
                entity.Property(r => r.TaxRate).HasColumnType("decimal(5,4)");

                entity.HasIndex(r => r.Name);
                entity.HasIndex(r => r.City);
                entity.HasIndex(r => r.PostalCode);
            });


            builder.Entity<MenuItem>(entity =>
            {
                entity.Property(m => m.Price).HasColumnType("decimal(18,2)");
                entity.HasOne(m => m.Restaurant)
                    .WithMany(r => r.MenuItems)
                    .HasForeignKey(m => m.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Category)
                    .WithMany(c => c.MenuItems)
                    .HasForeignKey(m => m.CategoryId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(m => m.OrderItems)
                    .WithOne(oi => oi.MenuItem)
                    .HasForeignKey(oi => oi.MenuItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(m => m.Reviews)
                    .WithOne(r => r.MenuItem)
                    .HasForeignKey(r => r.MenuItemId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(m => m.CustomizationOptions)
                    .WithOne(co => co.MenuItem)
                    .HasForeignKey(co => co.MenuItemId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(m => m.Name);
                entity.HasIndex(m => m.Price);
                entity.HasIndex(m => m.IsAvailable);
            });

            builder.Entity<CustomizationOption>(entity =>
            {
                entity.HasMany(co => co.Choices)
                    .WithOne(cc => cc.CustomizationOption)
                    .HasForeignKey(cc => cc.CustomizationOptionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<CustomizationChoice>(entity =>
            {
                entity.Property(cc => cc.Price).HasColumnType("decimal(18,2)");
            });

            builder.Entity<Order>(entity =>
            {
                entity.Property(o => o.UserId).HasMaxLength(450).IsRequired();
                entity.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
                entity.Property(o => o.DeliveryFee).HasColumnType("decimal(18,2)");
                entity.Property(o => o.Tax).HasColumnType("decimal(18,2)");
                entity.Property(o => o.Total).HasColumnType("decimal(18,2)");

                entity.HasOne(o => o.User)
                    .WithMany()
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(o => o.Restaurant)
                    .WithMany(r => r.Orders)
                    .HasForeignKey(o => o.RestaurantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Address)
                    .WithMany()
                    .HasForeignKey(o => o.DeliveryAddressId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Payment)
                    .WithOne(p => p.Order)
                    .HasForeignKey<Payment>(p => p.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.Reviews)
                    .WithOne(r => r.Order)
                    .HasForeignKey(r => r.OrderId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(o => o.OrderDate);
                entity.HasIndex(o => o.Status);
                entity.HasIndex(o => o.UserId);
                entity.HasIndex(o => o.RestaurantId);
            });

            builder.Entity<OrderItem>(entity =>
            {
                entity.Property(oi => oi.Price).HasColumnType("decimal(18,2)");
                entity.HasMany(oi => oi.Customizations)
                    .WithOne(oc => oc.OrderItem)
                    .HasForeignKey(oc => oc.OrderItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<OrderCustomization>(entity =>
            {
                entity.HasOne(oc => oc.CustomizationOption)
                    .WithMany()
                    .HasForeignKey(oc => oc.OptionId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(oc => oc.CustomizationChoice)
                    .WithMany()
                    .HasForeignKey(oc => oc.ChoiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(pm => pm.UserId).HasMaxLength(450).IsRequired();
                entity.HasOne(pm => pm.User)
                    .WithMany()
                    .HasForeignKey(pm => pm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Payment>(entity =>
            {
                entity.Property(p => p.Amount).HasColumnType("decimal(18,2)");
                entity.HasOne(p => p.PaymentMethod)
                    .WithMany(pm => pm.Payments)
                    .HasForeignKey(p => p.PaymentMethodId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);

                
                entity.HasOne(p => p.User)
                    .WithMany()
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(p => p.Order)
                    .WithOne(o => o.Payment)
                    .HasForeignKey<Payment>(p => p.OrderId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            builder.Entity<Promotion>(entity =>
            {
                entity.Property(p => p.DiscountValue).HasColumnType("decimal(18,2)");
                entity.HasMany(p => p.PromotionUsages)
                    .WithOne(u => u.Promotion)
                    .HasForeignKey(u => u.PromotionId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(p => p.Code).IsUnique();
            });

            builder.Entity<PromotionUsage>(entity =>
            {
                entity.Property(pu => pu.UserId).HasMaxLength(450).IsRequired();
                entity.HasOne(pu => pu.User)
                    .WithMany()
                    .HasForeignKey(pu => pu.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(pu => pu.Order)
                    .WithMany()
                    .HasForeignKey(pu => pu.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Review>(entity =>
            {
                entity.Property(r => r.Rating).HasColumnType("decimal(2,1)");

                entity.HasOne(r => r.MenuItem)
                    .WithMany(m => m.Reviews)
                    .HasForeignKey(r => r.MenuItemId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(r => r.Order)
                    .WithMany(o => o.Reviews)
                    .HasForeignKey(r => r.OrderId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(r => r.Restaurant)
                    .WithMany(rev => rev.Reviews)
                    .HasForeignKey(r => r.RestaurantId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(r => r.CustomerProfile)
                    .WithMany(c => c.Reviews)
                    .HasForeignKey(r => r.CustomerProfileId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(r => r.RestaurantId);
                entity.HasIndex(r => r.CustomerProfileId);
                entity.HasIndex(r => r.MenuItemId);
                entity.HasIndex(r => r.OrderId);
            });

            builder.Entity<Cart>(entity =>
            {
                entity.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.Items)
                    .WithOne(ci => ci.Cart)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<CartItem>(entity =>
            {
                entity.HasOne(ci => ci.MenuItem)
                    .WithMany()
                    .HasForeignKey(ci => ci.MenuItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(ci => ci.Customizations)
                    .WithOne(c => c.CartItem)
                    .HasForeignKey(c => c.CartItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Customization>(entity => {
                entity.Property(c => c.Price).HasColumnType("decimal(18,2)");
                entity.HasOne(c => c.Option)
                    .WithMany()
                    .HasForeignKey(c => c.OptionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Choice)
                    .WithMany()
                    .HasForeignKey(c => c.ChoiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            builder.Entity<SearchHistory>(entity =>
            {
                entity.Property(sh => sh.UserId).HasMaxLength(450).IsRequired();
                entity.HasIndex(sh => sh.Query);
                entity.HasIndex(sh => sh.SearchDate);
            });

            // Configure owned types for value objects
            builder.Entity<ApplicationUser>().OwnsOne(u => u.NotificationPreferences, np =>
                {
                    np.Property(n => n.OrderUpdates).HasDefaultValue(true);
                    np.Property(n => n.Promotions).HasDefaultValue(true);
                    np.Property(n => n.Newsletter).HasDefaultValue(false);
                    np.Property(n => n.PushNotifications).HasDefaultValue(true);
                    np.Property(n => n.EmailNotifications).HasDefaultValue(true);
                    np.Property(n => n.SMSNotifications).HasDefaultValue(true);
                }
            );
            builder.Entity<ApplicationUser>().OwnsOne(u => u.PrivacySettings, ps =>
                {
                    ps.Property(p => p.ShowProfilePicture).HasDefaultValue(true);
                    ps.Property(p => p.ShowFullName).HasDefaultValue(true);
                    ps.Property(p => p.ShowLocation).HasDefaultValue(true);
                    ps.Property(p => p.ShowOrderHistory).HasDefaultValue(false);
                    ps.Property(p => p.ShareDataWithPartners).HasDefaultValue(false);
                }
            );

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

            // Seed Users (without navigation properties)
            var users = new List<ApplicationUser>
    {
        new ApplicationUser
        {
            Id = "1",
            Email = "admin@foodfast.com",
            UserName = "admin@foodfast.com",
            PasswordHash = hasher.HashPassword(null, "Admin@123"),
            Role = UserType.Admin,
            NormalizedUserName = "ADMIN@FOODFAST.COM",
            NormalizedEmail = "ADMIN@FOODFAST.COM",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = now,
            SecurityStamp = Guid.NewGuid().ToString(),
            PhoneNumber = "555-000-0000",
            PhoneNumberConfirmed = true,
            FirstName = "Admin",
            LastName = "User",
            ProfilePictureUrl = "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?ixlib=rb-4.0.3&auto=format&fit=crop&w=200&q=80"
        },
        new ApplicationUser
        {
            Id = "2",
            Email = "customer@foodfast.com",
            UserName = "customer@foodfast.com",
            PasswordHash = hasher.HashPassword(null, "Customer@123"),
            Role = UserType.Customer,
            NormalizedEmail = "CUSTOMER@FOODFAST.COM",
            NormalizedUserName = "CUSTOMER@FOODFAST.COM",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = now,
            SecurityStamp = Guid.NewGuid().ToString(),
            PhoneNumber = "555-111-1111",
            PhoneNumberConfirmed = true,
            FirstName = "John",
            LastName = "Doe",
            ProfilePictureUrl = "https://images.unsplash.com/photo-1535713875002-d1d0cf3356de?ixlib=rb-4.0.3&auto=format&fit=crop&w=200&q=80"
        },
        new ApplicationUser
        {
            Id = "3",
            Email = "employee@foodfast.com",
            UserName = "employee@foodfast.com",
            PasswordHash = hasher.HashPassword(null, "Employee@123"),
            Role = UserType.Employee,
            NormalizedEmail = "EMPLOYEE@FOODFAST.COM",
            NormalizedUserName = "EMPLOYEE@FOODFAST.COM",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = now,
            SecurityStamp = Guid.NewGuid().ToString(),
            PhoneNumber = "555-222-2222",
            PhoneNumberConfirmed = true,
            FirstName = "Jane",
            LastName = "Smith",
            ProfilePictureUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?ixlib=rb-4.0.3&auto=format&fit=crop&w=200&q=80"
        },
        new ApplicationUser
        {
            Id = "4",
            Email = "owner@foodfast.com",
            UserName = "owner@foodfast.com",
            PasswordHash = hasher.HashPassword(null, "Owner@123"),
            Role = UserType.Owner,
            NormalizedEmail = "OWNER@FOODFAST.COM",
            NormalizedUserName = "OWNER@FOODFAST.COM",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = now,
            SecurityStamp = Guid.NewGuid().ToString(),
            PhoneNumber = "555-333-3333",
            PhoneNumberConfirmed = true,
            FirstName = "Robert",
            LastName = "Johnson",
            ProfilePictureUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?ixlib=rb-4.0.3&auto=format&fit=crop&w=200&q=80"
        }
    };
            builder.Entity<ApplicationUser>().HasData(users);

            // Seed NotificationPreferences
            builder.Entity<ApplicationUser>().OwnsOne(u => u.NotificationPreferences).HasData(
                new
                {
                    ApplicationUserId = "1",
                    OrderUpdates = true,
                    Promotions = true,
                    Newsletter = false,
                    PushNotifications = true,
                    EmailNotifications = true,
                    SMSNotifications = true
                },
                new
                {
                    ApplicationUserId = "2",
                    OrderUpdates = true,
                    Promotions = true,
                    Newsletter = true,
                    PushNotifications = true,
                    EmailNotifications = true,
                    SMSNotifications = true
                },
                new
                {
                    ApplicationUserId = "3",
                    OrderUpdates = true,
                    Promotions = false,
                    Newsletter = false,
                    PushNotifications = true,
                    EmailNotifications = true,
                    SMSNotifications = false
                },
                new
                {
                    ApplicationUserId = "4",
                    OrderUpdates = true,
                    Promotions = true,
                    Newsletter = true,
                    PushNotifications = true,
                    EmailNotifications = true,
                    SMSNotifications = true
                }
            );

            // Seed PrivacySettings
            builder.Entity<ApplicationUser>().OwnsOne(u => u.PrivacySettings).HasData(
                new
                {
                    ApplicationUserId = "1",
                    ShowProfilePicture = false,
                    ShowFullName = true,
                    ShowLocation = true,
                    ShowOrderHistory = false,
                    ShareDataWithPartners = false
                },
                new
                {
                    ApplicationUserId = "2",
                    ShowProfilePicture = true,
                    ShowFullName = true,
                    ShowLocation = true,
                    ShowOrderHistory = true,
                    ShareDataWithPartners = false
                },
                new
                {
                    ApplicationUserId = "3",
                    ShowProfilePicture = true,
                    ShowFullName = true,
                    ShowLocation = false,
                    ShowOrderHistory = false,
                    ShareDataWithPartners = false
                },
                new
                {
                    ApplicationUserId = "4",
                    ShowProfilePicture = true,
                    ShowFullName = true,
                    ShowLocation = true,
                    ShowOrderHistory = true,
                    ShareDataWithPartners = true
                }
            );


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
                ProfilePictureUrl = "https://images.unsplash.com/photo-1535713875002-d1d0cf3356de?ixlib=rb-4.0.3&auto=format&fit=crop&w=200&q=80",
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
        new Address
        {
            Id = 1,
            CustomerProfileId = 1,
            Street = "123 Main St",
            City = "New York",
            State = "NY",
            PostalCode = "10001",
            IsDefault = true,
            CreatedAt = now.AddDays(-29)
        },
        new Address
        {
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
        new PaymentMethod
        {
            Id = 1,
            UserId = "2",
            Type = PaymentMethodType.CreditCard,
            AccountNumberMasked = "************1234",
            Provider = "Visa",
            IsDefault = true,
            CreatedAt = now.AddDays(-28)
        },
        new PaymentMethod
        {
            Id = 2,
            UserId = "2",
            Type = PaymentMethodType.PayPal,
            AccountNumberMasked = "********@paypal.com",
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
        new RestaurantCategory { Id = 3, Name = "Asian", Description = "Various Asian cuisines" },
        new RestaurantCategory { Id = 4, Name = "American", Description = "Classic American dishes" },
        new RestaurantCategory { Id = 5, Name = "Vegetarian", Description = "Plant-based meals" }
    };
            builder.Entity<RestaurantCategory>().HasData(categories);

            // Seed MenuItem Categories
            var menuItemCategories = new List<MenuItemCategory>
    {
        new MenuItemCategory { Id = 1, Name = "Pasta", Description = "Delicious pasta dishes" },
        new MenuItemCategory { Id = 2, Name = "Pizza", Description = "Classic and gourmet pizzas" },
        new MenuItemCategory { Id = 3, Name = "Tacos", Description = "Authentic Mexican tacos" },
        new MenuItemCategory { Id = 4, Name = "Burritos", Description = "Hearty burritos with various fillings" },
        new MenuItemCategory { Id = 5, Name = "Noodles", Description = "Asian-style noodles and stir-fries" },
        new MenuItemCategory { Id = 6, Name = "Salads", Description = "Fresh and healthy salads" },
        new MenuItemCategory { Id = 7, Name = "Burgers", Description = "Juicy burgers with various toppings" },
        new MenuItemCategory { Id = 8, Name = "Desserts", Description = "Sweet treats and desserts" },
        new MenuItemCategory { Id = 9, Name = "Beverages", Description = "Drinks and beverages" }
    };
            builder.Entity<MenuItemCategory>().HasData(menuItemCategories);

            // Seed Restaurants
            var restaurants = new List<Restaurant>
    {
        new Restaurant
        {
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
            ImageUrl = "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
            OwnerId = "4",
            CreatedAt = now.AddDays(-30)
        },
        new Restaurant
        {
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
            ImageUrl = "https://images.unsplash.com/photo-1551504734-5ee1c4a1479b?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
            OwnerId = "4",
            CreatedAt = now.AddDays(-25)
        },
        new Restaurant
        {
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
            ImageUrl = "https://images.unsplash.com/photo-1552566626-52f8b828add9?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
            OwnerId = "4",
            CreatedAt = now.AddDays(-20)
        },
        new Restaurant
        {
            Id = 4,
            Name = "Burger Haven",
            Description = "Best burgers in the city",
            PhoneNumber = "555-456-7890",
            CategoryId = 4,
            Address = "321 Burger Blvd",
            City = "Houston",
            State = "TX",
            PostalCode = "77001",
            OpeningTime = new TimeSpan(11, 0, 0),
            ClosingTime = new TimeSpan(23, 0, 0),
            IsActive = true,
            ImageUrl = "https://images.unsplash.com/photo-1551782450-a2132b4a6d74?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
            OwnerId = "4",
            CreatedAt = now.AddDays(-15)
        },
        new Restaurant
        {
            Id = 5,
            Name = "Green Plate",
            Description = "Delicious vegetarian and vegan options",
            PhoneNumber = "555-567-8901",
            CategoryId = 5,
            Address = "654 Veggie Lane",
            City = "San Francisco",
            State = "CA",
            PostalCode = "94101",
            OpeningTime = new TimeSpan(10, 0, 0),
            ClosingTime = new TimeSpan(22, 0, 0),
            IsActive = true,
            ImageUrl = "https://images.unsplash.com/photo-1540420773420-3366772f4999?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
            OwnerId = "4",
            CreatedAt = now.AddDays(-10)
        }
    };
            builder.Entity<Restaurant>().HasData(restaurants);

            // Seed Menu Items
            var menuItems = new List<MenuItem>
    {
        new MenuItem
        {
            Id = 1,
            Name = "Spaghetti Carbonara",
            Description = "Classic pasta with eggs, cheese, pancetta, and pepper",
            Price = 14.99m,
            ImageUrl = "https://images.unsplash.com/photo-1608897013039-887f21d8c804?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
            IsAvailable = true,
            RestaurantId = 1,
            CreatedAt = now.AddDays(-29),
            CategoryId = 1
        },
        new MenuItem
        {
            Id = 2,
            Name = "Margherita Pizza",
            Description = "Traditional pizza with tomato sauce, mozzarella, and basil",
            Price = 12.99m,
            ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cd1935f4db7?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
            IsAvailable = true,
            RestaurantId = 1,
            CreatedAt = now.AddDays(-29),
            CategoryId = 2
        },
        new MenuItem
        {
            Id = 3,
            Name = "Chicken Quesadilla",
            Description = "Grilled tortilla filled with cheese and chicken",
            Price = 9.99m,
            ImageUrl = "https://images.unsplash.com/photo-1595877171414-6d34e1b0a5b1?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
            IsAvailable = true,
            RestaurantId = 2,
            CreatedAt = now.AddDays(-24),
            CategoryId = 3
        },
        new MenuItem
        {
            Id = 4,
            Name = "Beef Burrito",
            Description = "Large flour tortilla with beef, rice, and beans",
            Price = 11.99m,
            ImageUrl = "https://images.unsplash.com/photo-1626700051175-6818013e1d4f?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
            IsAvailable = true,
            RestaurantId = 2,
            CreatedAt = now.AddDays(-24),
            CategoryId = 4
        },
        new MenuItem
        {
            Id = 5,
            Name = "General Tso's Chicken",
            Description = "Crispy chicken in a sweet and spicy sauce",
            Price = 13.99m,
            ImageUrl = "https://images.unsplash.com/photo-1600891964092-4316d7c6a64e?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
            IsAvailable = true,
            RestaurantId = 3,
            CreatedAt = now.AddDays(-19),
            CategoryId = 5
        },
        new MenuItem
        {
            Id = 6,
            Name = "Vegetable Lo Mein",
            Description = "Stir-fried noodles with mixed vegetables",
            Price = 10.99m,
            ImageUrl = "https://images.unsplash.com/photo-1585032226651-618b368f4053?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
            IsAvailable = true,
            RestaurantId = 3,
            CreatedAt = now.AddDays(-19),
            CategoryId = 5
        },
        new MenuItem
        {
            Id = 7,
            Name = "Cheeseburger",
            Description = "Juicy beef burger with cheese, lettuce, and tomato",
            Price = 9.99m,
            ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
            IsAvailable = true,
            RestaurantId = 4,
            CreatedAt = now.AddDays(-14),
            CategoryId = 7
        },
        new MenuItem
        {
            Id = 8,
            Name = "Caesar Salad",
            Description = "Crisp romaine lettuce with Caesar dressing and croutons",
            Price = 8.99m,
            ImageUrl = "https://images.unsplash.com/photo-1550304943-4f24f54ddde9?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
            IsAvailable = true,
            RestaurantId = 4,
            CreatedAt = now.AddDays(-14),
            CategoryId = 6
        },
        new MenuItem
        {
            Id = 9,
            Name = "Vegan Burger",
            Description = "Plant-based burger with lettuce, tomato, and vegan mayo",
            Price = 10.99m,
            ImageUrl = "https://images.unsplash.com/photo-1550547660-d7ef7d7d5e2b?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
            IsAvailable = true,
            RestaurantId = 5,
            CreatedAt = now.AddDays(-9),
            CategoryId = 7
        },
        new MenuItem
        {
            Id = 10,
            Name = "Quinoa Salad",
            Description = "Healthy salad with quinoa, mixed greens, and vinaigrette",
            Price = 9.99m,
            ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
            IsAvailable = true,
            RestaurantId = 5,
            CreatedAt = now.AddDays(-9),
            CategoryId = 6
        }
    };
            builder.Entity<MenuItem>().HasData(menuItems);

            // Seed Orders
            var orders = new List<Order>
    {
        new Order
        {
            Id = 1,
            UserId = "2",
            RestaurantId = 1,
            OrderDate = now.AddDays(-10),
            Status = OrderStatus.Delivered,
            Subtotal = 14.99m,
            DeliveryFee = 3.99m,
            Tax = 1.50m,
            Total = 20.48m,
            DeliveryAddressId = 1,
            SpecialInstructions = "Please knock loudly"
        },
        new Order
        {
            Id = 2,
            UserId = "2",
            RestaurantId = 2,
            OrderDate = now.AddDays(-5),
            Status = OrderStatus.Delivered,
            Subtotal = 19.98m,
            DeliveryFee = 2.99m,
            Tax = 1.15m,
            Total = 24.12m,
            DeliveryAddressId = 1,
            SpecialInstructions = "Extra napkins please"
        }
    };
            builder.Entity<Order>().HasData(orders);

            // Seed Order Items
            var orderItems = new List<OrderItem>
    {
        new OrderItem
        {
            Id = 1,
            OrderId = 1,
            MenuItemId = 1,
            Quantity = 1,
            Price = 14.99m,
            RestaurantId = 1,
            SpecialInstructions = "No cheese"
        },
        new OrderItem
        {
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
    new Payment
    {
        Id = 1,
        OrderId = 1,
        PaymentMethodId = 1,
        Amount = 20.48m,
        PaymentDate = now.AddDays(-10).AddHours(1),
        Status = PaymentStatus.Paid,
        TransactionId = "PAY-123456789",
        UserId = "2" // Set to the customer user ID
    },
    new Payment
    {
        Id = 2,
        OrderId = 2,
        PaymentMethodId = 1,
        Amount = 24.12m,
        PaymentDate = now.AddDays(-5).AddHours(1),
        Status = PaymentStatus.Paid,
        TransactionId = "PAY-987654321",
        UserId = "2" // Set to the customer user ID
    }
};
            builder.Entity<Payment>().HasData(payments);

            // Seed Promotions
            var promotions = new List<Promotion>
    {
        new Promotion
        {
            Id = 1,
            Code = "WELCOME20",
            Description = "20% off your first order",
            DiscountValue = 20,
            IsPercentage = true,
            StartDate = now.AddDays(-10),
            EndDate = now.AddDays(30),
            UsageLimit = 1000,
            MinimumOrderAmount = 15,
            IsActive = true
        },
        new Promotion
        {
            Id = 2,
            Code = "ITALIAN10",
            Description = "10% off all Italian restaurants",
            DiscountValue = 10,
            IsPercentage = true,
            StartDate = now.AddDays(-5),
            EndDate = now.AddDays(15),
            RestaurantId = 1,
            IsActive = true
        }
    };
            builder.Entity<Promotion>().HasData(promotions);

            // Seed Reviews
            var reviews = new List<Review>
    {
        new Review
        {
            Id = 1,
            RestaurantId = 1,
            CustomerProfileId = 1,
            Rating = 5.0m,
            Comment = "Best Italian food I've ever had!",
            CreatedAt = now.AddDays(-9)
        },
        new Review
        {
            Id = 2,
            RestaurantId = 2,
            CustomerProfileId = 1,
            Rating = 4.5m,
            Comment = "Great tacos, but a bit spicy for my taste.",
            CreatedAt = now.AddDays(-4)
        },
        new Review
        {
            Id = 3,
            RestaurantId = 3,
            CustomerProfileId = 1,
            Rating = 4.0m,
            Comment = "Good Chinese food, but the service was slow.",
            CreatedAt = now.AddDays(-2)
        },
        new Review
        {
            Id = 4,
            RestaurantId = 1,
            CustomerProfileId = 1,
            Rating = 3.5m,
            Comment = "Decent food, but not as good as I expected.",
            CreatedAt = now.AddDays(-1)
        },
        new Review
        {
            Id = 5,
            RestaurantId = 2,
            CustomerProfileId = 1,
            Rating = 4.0m,
            Comment = "Loved the burrito, will order again!",
            CreatedAt = now.AddDays(-3)
        },
        new Review
        {
            Id = 6,
            RestaurantId = 3,
            CustomerProfileId = 1,
            Rating = 5.0m,
            Comment = "The best General Tso's chicken in town!",
            CreatedAt = now.AddDays(-2)
        },
        new Review
        {
            Id = 7,
            RestaurantId = 1,
            CustomerProfileId = 1,
            Rating = 4.5m,
            Comment = "Great pizza, but a bit overpriced.",
            CreatedAt = now.AddDays(-1)
        },
        new Review
        {
            Id = 8,
            RestaurantId = 2,
            CustomerProfileId = 1,
            Rating = 4.0m,
            Comment = "Good food, but the delivery was late.",
            CreatedAt = now.AddDays(-3)
        }
    };
            builder.Entity<Review>().HasData(reviews);
        }


    }
}
