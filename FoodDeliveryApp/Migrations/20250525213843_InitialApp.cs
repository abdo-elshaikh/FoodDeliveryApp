using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceivePromotions = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoyaltyPoints = table.Column<int>(type: "int", nullable: false),
                    PreferredLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    FavoriteRestaurants = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DietaryPreferences = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Allergies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMethods = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultPaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultDeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationPreferences_OrderUpdates = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    NotificationPreferences_Promotions = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    NotificationPreferences_Newsletter = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    NotificationPreferences_PushNotifications = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    NotificationPreferences_EmailNotifications = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    NotificationPreferences_SMSNotifications = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    PrivacySettings_ShowProfilePicture = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    PrivacySettings_ShowFullName = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    PrivacySettings_ShowLocation = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    PrivacySettings_ShowOrderHistory = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    PrivacySettings_ShareDataWithPartners = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivePromotions = table.Column<bool>(type: "bit", nullable: false),
                    LoyaltyPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Query = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SearchDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OpeningTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClosingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurants_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Restaurants_RestaurantCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "RestaurantCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_CustomerProfiles_CustomerProfileId",
                        column: x => x.CustomerProfileId,
                        principalTable: "CustomerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccountNumberMasked = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerProfileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_CustomerProfiles_CustomerProfileId",
                        column: x => x.CustomerProfileId,
                        principalTable: "CustomerProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    RestaurantCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_MenuItemCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "MenuItemCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MenuItems_RestaurantCategories_RestaurantCategoryId",
                        column: x => x.RestaurantCategoryId,
                        principalTable: "RestaurantCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MenuItems_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: true),
                    UsageLimit = table.Column<int>(type: "int", nullable: true),
                    MinimumOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotions_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    DeliveryAddressId = table.Column<int>(type: "int", nullable: true),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentMethodType = table.Column<int>(type: "int", nullable: false),
                    PaymentDetails = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrackingUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerProfileId = table.Column<int>(type: "int", nullable: true),
                    EmployeeProfileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_DeliveryAddressId",
                        column: x => x.DeliveryAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_CustomerProfiles_CustomerProfileId",
                        column: x => x.CustomerProfileId,
                        principalTable: "CustomerProfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_EmployeeProfiles_EmployeeProfileId",
                        column: x => x.EmployeeProfileId,
                        principalTable: "EmployeeProfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomizationOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    AllowMultiple = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizationOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomizationOptions_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PromotionUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PromotionId = table.Column<int>(type: "int", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionUsages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionUsages_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionUsages_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    CustomerProfileId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_CustomerProfiles_CustomerProfileId",
                        column: x => x.CustomerProfileId,
                        principalTable: "CustomerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Reviews_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Reviews_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomizationChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomizationOptionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizationChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomizationChoices_CustomizationOptions_CustomizationOptionId",
                        column: x => x.CustomizationOptionId,
                        principalTable: "CustomizationOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartItemId = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    ChoiceId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customizations_CartItems_CartItemId",
                        column: x => x.CartItemId,
                        principalTable: "CartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customizations_CustomizationChoices_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "CustomizationChoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customizations_CustomizationOptions_OptionId",
                        column: x => x.OptionId,
                        principalTable: "CustomizationOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderCustomizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    ChoiceId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCustomizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderCustomizations_CustomizationChoices_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "CustomizationChoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderCustomizations_CustomizationOptions_OptionId",
                        column: x => x.OptionId,
                        principalTable: "CustomizationOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderCustomizations_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "9a4decae-6ce3-4179-ae4a-16539ba89423", "Admin", "ADMIN" },
                    { "2", "3e859bdd-e0ab-4c91-97fe-b2576cc4d7c0", "Customer", "CUSTOMER" },
                    { "3", "1fa42bea-db05-4012-a1e4-162a99533340", "Employee", "EMPLOYEE" },
                    { "4", "7feef34c-28bc-448b-9f2c-0a6c900aa432", "Owner", "OWNER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "Allergies", "Bio", "City", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DefaultDeliveryAddress", "DefaultPaymentMethod", "DietaryPreferences", "Email", "EmailConfirmed", "FavoriteRestaurants", "FirstName", "IsActive", "LastLoginAt", "LastName", "LockoutEnabled", "LockoutEnd", "LoyaltyPoints", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PaymentMethods", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "PreferredLanguage", "ProfilePictureUrl", "ReceivePromotions", "Role", "SecurityStamp", "State", "TimeZone", "TwoFactorEnabled", "UserName", "NotificationPreferences_EmailNotifications", "NotificationPreferences_Newsletter", "NotificationPreferences_OrderUpdates", "NotificationPreferences_Promotions", "NotificationPreferences_PushNotifications", "NotificationPreferences_SMSNotifications", "PrivacySettings_ShareDataWithPartners", "PrivacySettings_ShowFullName", "PrivacySettings_ShowLocation", "PrivacySettings_ShowOrderHistory", "PrivacySettings_ShowProfilePicture" },
                values: new object[,]
                {
                    { "1", 0, "", "[]", "", "", "2c091e02-b8bb-449e-9d8e-d36a480bc6a2", "", new DateTime(2025, 5, 25, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), null, "", "", "[]", "admin@foodfast.com", true, "[]", "Admin", true, null, "User", false, null, 0, "ADMIN@FOODFAST.COM", "ADMIN@FOODFAST.COM", "AQAAAAIAAYagAAAAEJEPBSn4hXT10ObtxdsvIMumozrnOT1Xt2SwW4ja4s/Tby6dDdaVS/0moK/1v5vAeQ==", "[]", "555-000-0000", true, "", "en", "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?ixlib=rb-4.0.3&auto=format&fit=crop&w=200&q=80", false, 2, "37e2d654-73a0-4fbd-85f6-d5ae39009e78", "", "UTC", false, "admin@foodfast.com", true, false, true, true, true, true, false, true, true, false, false },
                    { "2", 0, "", "[]", "", "", "686be7d6-0581-48f4-a2ec-14e73de2a42c", "", new DateTime(2025, 5, 25, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), null, "", "", "[]", "customer@foodfast.com", true, "[]", "John", true, null, "Doe", false, null, 0, "CUSTOMER@FOODFAST.COM", "CUSTOMER@FOODFAST.COM", "AQAAAAIAAYagAAAAEM8VeqIrHU9Oc5gtAZ1SsPb6xrm0wAtwDwcg7B1lT1/AgG+YTu0mneEasELs49mJUw==", "[]", "555-111-1111", true, "", "en", "https://images.unsplash.com/photo-1535713875002-d1d0cf3356de?ixlib=rb-4.0.3&auto=format&fit=crop&w=200&q=80", false, 0, "9f84cda9-2017-4415-b1b3-a2af7c50c666", "", "UTC", false, "customer@foodfast.com", true, true, true, true, true, true, false, true, true, true, true },
                    { "3", 0, "", "[]", "", "", "963e73bb-29ab-4ba5-aec5-dd6c27f8fd58", "", new DateTime(2025, 5, 25, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), null, "", "", "[]", "employee@foodfast.com", true, "[]", "Jane", true, null, "Smith", false, null, 0, "EMPLOYEE@FOODFAST.COM", "EMPLOYEE@FOODFAST.COM", "AQAAAAIAAYagAAAAEGxCz3NDRXCoobBt7vHrs6y8XnpRTQFP2/rqIP0+p5Xqd1jSrMpeTOH1yNTS9WQXtQ==", "[]", "555-222-2222", true, "", "en", "https://images.unsplash.com/photo-1494790108377-be9c29b29330?ixlib=rb-4.0.3&auto=format&fit=crop&w=200&q=80", false, 1, "958dc2c3-723d-45f8-986e-38ce96556e96", "", "UTC", false, "employee@foodfast.com", true, false, true, false, true, false, false, true, false, false, true },
                    { "4", 0, "", "[]", "", "", "e7f59f6a-a279-4715-9bac-16fb135dbd50", "", new DateTime(2025, 5, 25, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), null, "", "", "[]", "owner@foodfast.com", true, "[]", "Robert", true, null, "Johnson", false, null, 0, "OWNER@FOODFAST.COM", "OWNER@FOODFAST.COM", "AQAAAAIAAYagAAAAEDb75AoZJFlJ6BSs9FKr0qEL81XtE93h0P6X9Kts1WrxaWX6+K1Syeoi0tfBR6VsRg==", "[]", "555-333-3333", true, "", "en", "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?ixlib=rb-4.0.3&auto=format&fit=crop&w=200&q=80", false, 3, "7f47cc69-1b2b-47bb-a101-81daa3792e3c", "", "UTC", false, "owner@foodfast.com", true, true, true, true, true, true, true, true, true, true, true }
                });

            migrationBuilder.InsertData(
                table: "MenuItemCategories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Delicious pasta dishes", "Pasta" },
                    { 2, "Classic and gourmet pizzas", "Pizza" },
                    { 3, "Authentic Mexican tacos", "Tacos" },
                    { 4, "Hearty burritos with various fillings", "Burritos" },
                    { 5, "Asian-style noodles and stir-fries", "Noodles" },
                    { 6, "Fresh and healthy salads", "Salads" },
                    { 7, "Juicy burgers with various toppings", "Burgers" },
                    { 8, "Sweet treats and desserts", "Desserts" },
                    { 9, "Drinks and beverages", "Beverages" }
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Code", "Description", "DiscountValue", "EndDate", "IsActive", "IsPercentage", "MinimumOrderAmount", "RestaurantId", "StartDate", "UsageLimit" },
                values: new object[] { 1, "WELCOME20", "20% off your first order", 20m, new DateTime(2025, 6, 24, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), true, true, 15m, null, new DateTime(2025, 5, 15, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1000 });

            migrationBuilder.InsertData(
                table: "RestaurantCategories",
                columns: new[] { "Id", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Authentic Italian cuisine", null, "Italian" },
                    { 2, "Traditional Mexican food", null, "Mexican" },
                    { 3, "Various Asian cuisines", null, "Asian" },
                    { 4, "Classic American dishes", null, "American" },
                    { 5, "Plant-based meals", null, "Vegetarian" }
                });

            migrationBuilder.InsertData(
                table: "CustomerProfiles",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FirstName", "IsActive", "LastName", "LoyaltyPoints", "PhoneNumber", "ProfilePictureUrl", "ReceivePromotions", "UpdatedAt", "UserId" },
                values: new object[] { 1, new DateTime(2025, 4, 25, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "John", true, "Doe", 0m, "555-111-1111", "https://images.unsplash.com/photo-1535713875002-d1d0cf3356de?ixlib=rb-4.0.3&auto=format&fit=crop&w=200&q=80", false, new DateTime(2025, 5, 25, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "2" });

            migrationBuilder.InsertData(
                table: "EmployeeProfiles",
                columns: new[] { "Id", "CreatedAt", "FirstName", "HireDate", "IsActive", "LastName", "PhoneNumber", "Position", "ProfilePictureUrl", "TerminationDate", "UpdatedAt", "UserId" },
                values: new object[] { 1, new DateTime(2024, 11, 25, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "Jane", new DateTime(2024, 11, 25, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), true, "Smith", "555-222-2222", 1, null, null, new DateTime(2025, 5, 25, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "3" });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "AccountNumberMasked", "CreatedAt", "CustomerProfileId", "IsDefault", "Provider", "Type", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, "************1234", new DateTime(2025, 4, 27, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), null, true, "Visa", 0, null, "2" },
                    { 2, "********@paypal.com", new DateTime(2025, 5, 11, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), null, false, "PayPal", 2, null, "2" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Address", "CategoryId", "City", "ClosingTime", "CreatedAt", "DeliveryFee", "Description", "ImageUrl", "IsActive", "LocationUrl", "Name", "OpeningTime", "OwnerId", "PhoneNumber", "PostalCode", "Rating", "State", "TaxRate", "UpdatedAt", "WebsiteUrl" },
                values: new object[,]
                {
                    { 1, "123 Pasta Street", 1, "New York", new TimeSpan(0, 22, 0, 0, 0), new DateTime(2025, 4, 25, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 0m, "Authentic Italian restaurant since 1985", "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", true, null, "Mama Mia Italian", new TimeSpan(0, 11, 0, 0, 0), "4", "555-123-4567", "10001", 0m, "NY", 0m, null, null },
                    { 2, "456 Salsa Avenue", 2, "Los Angeles", new TimeSpan(0, 23, 0, 0, 0), new DateTime(2025, 4, 30, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 0m, "The best Mexican food in town", "https://images.unsplash.com/photo-1551504734-5ee1c4a1479b?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", true, null, "Taco Fiesta", new TimeSpan(0, 10, 0, 0, 0), "4", "555-234-5678", "90001", 0m, "CA", 0m, null, null },
                    { 3, "789 Noodle Road", 3, "Chicago", new TimeSpan(0, 21, 30, 0, 0), new DateTime(2025, 5, 5, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 0m, "Authentic Chinese cuisine", "https://images.unsplash.com/photo-1552566626-52f8b828add9?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", true, null, "Golden Wok", new TimeSpan(0, 10, 30, 0, 0), "4", "555-345-6789", "60601", 0m, "IL", 0m, null, null },
                    { 4, "321 Burger Blvd", 4, "Houston", new TimeSpan(0, 23, 0, 0, 0), new DateTime(2025, 5, 10, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 0m, "Best burgers in the city", "https://images.unsplash.com/photo-1551782450-a2132b4a6d74?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", true, null, "Burger Haven", new TimeSpan(0, 11, 0, 0, 0), "4", "555-456-7890", "77001", 0m, "TX", 0m, null, null },
                    { 5, "654 Veggie Lane", 5, "San Francisco", new TimeSpan(0, 22, 0, 0, 0), new DateTime(2025, 5, 15, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 0m, "Delicious vegetarian and vegan options", "https://images.unsplash.com/photo-1540420773420-3366772f4999?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", true, null, "Green Plate", new TimeSpan(0, 10, 0, 0, 0), "4", "555-567-8901", "94101", 0m, "CA", 0m, null, null }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "City", "Country", "CreatedAt", "CustomerProfileId", "IsDefault", "PostalCode", "State", "Street", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "New York", "USA", new DateTime(2025, 4, 26, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, true, "10001", "NY", "123 Main St", "Home", null },
                    { 2, "New York", "USA", new DateTime(2025, 5, 5, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, false, "10002", "NY", "456 Work Ave", "Home", null }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "Name", "Price", "RestaurantCategoryId", "RestaurantId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 4, 26, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "Classic pasta with eggs, cheese, pancetta, and pepper", "https://images.unsplash.com/photo-1608897013039-887f21d8c804?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", true, "Spaghetti Carbonara", 14.99m, null, 1, null },
                    { 2, 2, new DateTime(2025, 4, 26, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "Traditional pizza with tomato sauce, mozzarella, and basil", "https://images.unsplash.com/photo-1574071318508-1cd1935f4db7?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", true, "Margherita Pizza", 12.99m, null, 1, null },
                    { 3, 3, new DateTime(2025, 5, 1, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "Grilled tortilla filled with cheese and chicken", "https://images.unsplash.com/photo-1595877171414-6d34e1b0a5b1?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", true, "Chicken Quesadilla", 9.99m, null, 2, null },
                    { 4, 4, new DateTime(2025, 5, 1, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "Large flour tortilla with beef, rice, and beans", "https://images.unsplash.com/photo-1626700051175-6818013e1d4f?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", true, "Beef Burrito", 11.99m, null, 2, null },
                    { 5, 5, new DateTime(2025, 5, 6, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "Crispy chicken in a sweet and spicy sauce", "https://images.unsplash.com/photo-1600891964092-4316d7c6a64e?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", true, "General Tso's Chicken", 13.99m, null, 3, null },
                    { 6, 5, new DateTime(2025, 5, 6, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "Stir-fried noodles with mixed vegetables", "https://images.unsplash.com/photo-1585032226651-618b368f4053?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", true, "Vegetable Lo Mein", 10.99m, null, 3, null },
                    { 7, 7, new DateTime(2025, 5, 11, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "Juicy beef burger with cheese, lettuce, and tomato", "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", true, "Cheeseburger", 9.99m, null, 4, null },
                    { 8, 6, new DateTime(2025, 5, 11, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "Crisp romaine lettuce with Caesar dressing and croutons", "https://images.unsplash.com/photo-1550304943-4f24f54ddde9?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", true, "Caesar Salad", 8.99m, null, 4, null },
                    { 9, 7, new DateTime(2025, 5, 16, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "Plant-based burger with lettuce, tomato, and vegan mayo", "https://images.unsplash.com/photo-1550547660-d7ef7d7d5e2b?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", true, "Vegan Burger", 10.99m, null, 5, null },
                    { 10, 6, new DateTime(2025, 5, 16, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), "Healthy salad with quinoa, mixed greens, and vinaigrette", "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80", true, "Quinoa Salad", 9.99m, null, 5, null }
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Code", "Description", "DiscountValue", "EndDate", "IsActive", "IsPercentage", "MinimumOrderAmount", "RestaurantId", "StartDate", "UsageLimit" },
                values: new object[] { 2, "ITALIAN10", "10% off all Italian restaurants", 10m, new DateTime(2025, 6, 9, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), true, true, null, 1, new DateTime(2025, 5, 20, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), null });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "CustomerProfileId", "MenuItemId", "OrderId", "Rating", "RestaurantId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Best Italian food I've ever had!", new DateTime(2025, 5, 16, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, null, null, 5.0m, 1, null },
                    { 2, "Great tacos, but a bit spicy for my taste.", new DateTime(2025, 5, 21, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, null, null, 4.5m, 2, null },
                    { 3, "Good Chinese food, but the service was slow.", new DateTime(2025, 5, 23, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, null, null, 4.0m, 3, null },
                    { 4, "Decent food, but not as good as I expected.", new DateTime(2025, 5, 24, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, null, null, 3.5m, 1, null },
                    { 5, "Loved the burrito, will order again!", new DateTime(2025, 5, 22, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, null, null, 4.0m, 2, null },
                    { 6, "The best General Tso's chicken in town!", new DateTime(2025, 5, 23, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, null, null, 5.0m, 3, null },
                    { 7, "Great pizza, but a bit overpriced.", new DateTime(2025, 5, 24, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, null, null, 4.5m, 1, null },
                    { 8, "Good food, but the delivery was late.", new DateTime(2025, 5, 22, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, null, null, 4.0m, 2, null }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerProfileId", "DeliveryAddressId", "DeliveryDate", "DeliveryFee", "DeliveryInstructions", "Discount", "EmployeeProfileId", "EstimatedDeliveryTime", "OrderDate", "PaymentDetails", "PaymentMethodType", "RestaurantId", "SpecialInstructions", "Status", "Subtotal", "Tax", "Total", "TrackingUrl", "UserId" },
                values: new object[,]
                {
                    { 1, null, 1, null, 3.99m, null, 0m, null, null, new DateTime(2025, 5, 15, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), null, 0, 1, "Please knock loudly", 5, 14.99m, 1.50m, 20.48m, null, "2" },
                    { 2, null, 1, null, 2.99m, null, 0m, null, null, new DateTime(2025, 5, 20, 21, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), null, 0, 2, "Extra napkins please", 5, 19.98m, 1.15m, 24.12m, null, "2" }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "MenuItemId", "OrderId", "Price", "Quantity", "RestaurantId", "SpecialInstructions" },
                values: new object[,]
                {
                    { 1, 1, 1, 14.99m, 1, 1, "No cheese" },
                    { 2, 3, 2, 9.99m, 2, 2, "Extra cheese, no beans" }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "OrderId", "PaymentDate", "PaymentMethodId", "Status", "TransactionId", "UserId" },
                values: new object[,]
                {
                    { 1, 20.48m, 1, new DateTime(2025, 5, 15, 22, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, 1, "PAY-123456789", "2" },
                    { 2, 24.12m, 2, new DateTime(2025, 5, 20, 22, 38, 42, 130, DateTimeKind.Utc).AddTicks(8343), 1, 1, "PAY-987654321", "2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerProfileId",
                table: "Addresses",
                column: "CustomerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PostalCode",
                table: "Addresses",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_MenuItemId",
                table: "CartItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomizationChoices_CustomizationOptionId",
                table: "CustomizationChoices",
                column: "CustomizationOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizationOptions_MenuItemId",
                table: "CustomizationOptions",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_CartItemId",
                table: "Customizations",
                column: "CartItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_ChoiceId",
                table: "Customizations",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_OptionId",
                table: "Customizations",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfiles_UserId",
                table: "EmployeeProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemCategories_Name",
                table: "MenuItemCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryId",
                table: "MenuItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_IsAvailable",
                table: "MenuItems",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_Name",
                table: "MenuItems",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_Price",
                table: "MenuItems",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_RestaurantCategoryId",
                table: "MenuItems",
                column: "RestaurantCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_RestaurantId",
                table: "MenuItems",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCustomizations_ChoiceId",
                table: "OrderCustomizations",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCustomizations_OptionId",
                table: "OrderCustomizations",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCustomizations_OrderItemId",
                table: "OrderCustomizations",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MenuItemId",
                table: "OrderItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_RestaurantId",
                table: "OrderItems",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerProfileId",
                table: "Orders",
                column: "CustomerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryAddressId",
                table: "Orders",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeProfileId",
                table: "Orders",
                column: "EmployeeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDate",
                table: "Orders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RestaurantId",
                table: "Orders",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_CustomerProfileId",
                table: "PaymentMethods",
                column: "CustomerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_UserId",
                table: "PaymentMethods",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodId",
                table: "Payments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Code",
                table: "Promotions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_RestaurantId",
                table: "Promotions",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionUsages_OrderId",
                table: "PromotionUsages",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionUsages_PromotionId",
                table: "PromotionUsages",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionUsages_UserId",
                table: "PromotionUsages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_CategoryId",
                table: "Restaurants",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_City",
                table: "Restaurants",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_Name",
                table: "Restaurants",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_PostalCode",
                table: "Restaurants",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerProfileId",
                table: "Reviews",
                column: "CustomerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MenuItemId",
                table: "Reviews",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderId",
                table: "Reviews",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RestaurantId",
                table: "Reviews",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistory_Query",
                table: "SearchHistory",
                column: "Query");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistory_SearchDate",
                table: "SearchHistory",
                column: "SearchDate");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistory_UserId",
                table: "SearchHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Customizations");

            migrationBuilder.DropTable(
                name: "OrderCustomizations");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PromotionUsages");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "SearchHistory");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "CustomizationChoices");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "CustomizationOptions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "EmployeeProfiles");

            migrationBuilder.DropTable(
                name: "MenuItemCategories");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "CustomerProfiles");

            migrationBuilder.DropTable(
                name: "RestaurantCategories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
