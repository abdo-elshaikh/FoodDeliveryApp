using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories;
using Microsoft.EntityFrameworkCore;

// namespace FoodDeliveryApp.Controllers
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// MySQL connection string.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// register repositories.
builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IRepository<Item>, ItemRepository>();
builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
builder.Services.AddScoped<IRepository<OrderDetail>, OrderDetailRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IRepository<Employee>, EmployeeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
