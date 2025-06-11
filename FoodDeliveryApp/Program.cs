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
using FoodDeliveryApp.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using FoodDeliveryApp.Hubs;
using AutoMapper.Extensions;
using FoodDeliveryApp.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Db connection string.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(connectionString, 
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));

// Register HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure Email Settings and register as singleton
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<EmailSettings>>().Value);

// Register EmailSender service with EmailSettings injection
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Register Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Register repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IOrderTrackingRepository, OrderTrackingRepository>();
builder.Services.AddScoped<ISearchLogRepository, SearchLogRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register services
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IUserLocationService, UserLocationService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartCalculationService, CartCalculationService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IAddressService, AddressService>();

// Add services to the container
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Add MVC controllers with views
builder.Services.AddControllersWithViews();

// Add caching services
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

// Configure Identity options
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Error/403";
        options.ReturnUrlParameter = "ReturnUrl";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add SignalR
builder.Services.AddSignalR();

// Add HttpClient for Google Maps API
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/500");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

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

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == 403)
    {
        response.Clear();
        context.HttpContext.Request.Path = "/Error/403";
        await context.Next(context.HttpContext);
    }
    else if (response.StatusCode == 404)
    {
        response.Clear();
        context.HttpContext.Request.Path = "/Error/404";
        await context.Next(context.HttpContext);
    }
    else if (response.StatusCode == 503)
    {
        response.Clear();
        context.HttpContext.Request.Path = "/Error/503";
        await context.Next(context.HttpContext);
    }
    else if (response.StatusCode == 500)
    {
        response.Clear();
        context.HttpContext.Request.Path = "/Error/500";
        await context.Next(context.HttpContext);
    }
    else
    {
        await context.Next(context.HttpContext);
    }
});

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
        ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=" + durationInSeconds;
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
