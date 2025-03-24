// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            // seed data for the database

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Orders)
                .WithOne(o => o.Employee)
                .HasForeignKey(o => o.EmpId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Employee)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.EmpId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustId);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CateqId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Item)
                .WithMany(i => i.OrderDetails)
                .HasForeignKey(od => od.ItemId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrdId);
        }

    }
}