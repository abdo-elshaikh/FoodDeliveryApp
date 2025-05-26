using FoodDeliveryApp.Controllers;
using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Implementations;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Db connection string.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the DbContext with MySQL.
// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Uncomment the following line to use SQL Server instead of MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Configure static files


// Register Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure Email Settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
// Register EmailSender service
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Configure AutoMapper
builder.Services.AddHttpContextAccessor();

// Register Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

// Add Cart service
builder.Services.AddScoped<ICartService, CartService>();

// Add Braintree service
builder.Services.AddTransient<IBraintreeService, BraintreeService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();

// Add File service
builder.Services.AddScoped<IFileService, FileService>();

// register generic repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ILogger<CartController>, Logger<CartController>>();
// Configure Identity options
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/AccessDenied";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// handle errors globally.
app.Use(async (context, next) =>
{
    try
    {
        await next();
        if (context.Response.StatusCode == 404)
        {
            context.Request.Path = "/NotFound";
            await next();
        }
    }
    catch (Exception ex)
    {
        // Log the exception
        // You can use a logging framework like Serilog, NLog, etc.
        Console.WriteLine($"An error occurred: {ex.Message}");
        context.Response.Redirect("/Error");
    }
});

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log the error
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/NotFound");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseSession();

app.UseHttpsRedirection();



// Configure static files with proper MIME types
var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
provider.Mappings[".webmanifest"] = "application/manifest+json";
provider.Mappings[".json"] = "application/json";
provider.Mappings[".js"] = "application/javascript";

// First, configure static files for the wwwroot directory
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider,
    OnPrepareResponse = ctx =>
    {
        // Cache static files for 1 day
        const int durationInSeconds = 86400;
        ctx.Context.Response.Headers["Cache-Control"] = 
            "public,max-age=" + durationInSeconds;
    }
});

// Then, configure static files for the Uploads directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "Uploads")),
    RequestPath = "/Uploads",
    OnPrepareResponse = ctx =>
    {
        // Cache uploaded files for 1 hour
        const int durationInSeconds = 3600;
        ctx.Context.Response.Headers["Cache-Control"] = 
            "public,max-age=" + durationInSeconds;
    }
});

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
