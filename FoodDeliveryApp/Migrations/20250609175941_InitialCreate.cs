using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Query = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ResultCount = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchLogs", x => x.Id);
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
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StreetAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    AddressType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
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
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PromotionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPromotionApplied = table.Column<bool>(type: "bit", nullable: false),
                    PromotionCodeExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromotionDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalWithDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VehicleInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LastActive = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentLatitude = table.Column<double>(type: "float", nullable: true),
                    CurrentLongitude = table.Column<double>(type: "float", nullable: true),
                    CurrentAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastLocationUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_AspNetUsers_UserId",
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
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveryTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    LocationUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    OpeningTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ClosingTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurants_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
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
                name: "MenuItemCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItemCategories_Restaurants_RestaurantId",
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
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    DeliveryAddressId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrackingUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDelayed = table.Column<bool>(type: "bit", nullable: false),
                    DelayReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RequiresSignature = table.Column<bool>(type: "bit", nullable: false),
                    SignatureImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DeliveryPhotos = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RestaurantId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                        name: "FK_Orders_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UsageCount = table.Column<int>(type: "int", nullable: false),
                    MaxUsageLimit = table.Column<int>(type: "int", nullable: true),
                    RestaurantId = table.Column<int>(type: "int", nullable: true),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsageLimit = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    PreparationTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Calories = table.Column<int>(type: "int", nullable: false),
                    SpiceLevel = table.Column<int>(type: "int", nullable: false),
                    IsVegetarian = table.Column<bool>(type: "bit", nullable: false),
                    IsVegan = table.Column<bool>(type: "bit", nullable: false),
                    RestaurantCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_MenuItemCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "MenuItemCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "OrderTracking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    DeliveryAddressId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    EstimatedArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelayed = table.Column<bool>(type: "bit", nullable: false),
                    DelayReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RequiresSignature = table.Column<bool>(type: "bit", nullable: false),
                    SignatureImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrackingUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTracking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTracking_Addresses_DeliveryAddressId",
                        column: x => x.DeliveryAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderTracking_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderTracking_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                    table.ForeignKey(
                        name: "FK_CartItems_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_MenuItems_MenuItemId",
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
                    SpecialInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Modifiers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrepared = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PreparedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: true),
                    MenuItemId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Admin", "ADMIN" },
                    { "2", null, "Owner", "OWNER" },
                    { "3", null, "Driver", "DRIVER" },
                    { "4", null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "ImageUrl", "IsActive", "LastLogin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureUrl", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "4d6ca63e-e331-4553-a2ba-d5ff0e401d5a", 0, "04f5d4a0-15d9-4c92-bf1c-0a76fe24d9c0", new DateTime(2025, 6, 9, 17, 59, 37, 899, DateTimeKind.Utc).AddTicks(6860), null, "owner@fooddeliveryapp", true, "Jane", null, true, null, "Doe", false, null, "owner@fooddeliveryapp", "owner@fooddeliveryapp", null, "AQAAAAIAAYagAAAAEG5l3FMUZd8FK4vWNsW+uWD4MUikQurw1Z68AEu1h3ie5RtFpJDgw6bIzDGbTY8mVg==", "1234567890", false, "", 3, "a2e06bd6-506b-4902-9719-9f394539fa81", false, "owner@fooddeliveryapp" },
                    { "9c81cb6a-9f7e-46b8-b2fe-cd86d8ccb095", 0, "da9de4bf-2a29-4533-8774-1ced0807b444", new DateTime(2025, 6, 9, 17, 59, 37, 744, DateTimeKind.Utc).AddTicks(393), null, "admin@fooddeliveryapp", true, "John", null, true, null, "Doe", false, null, "admin@fooddeliveryapp", "admin@fooddeliveryapp", null, "AQAAAAIAAYagAAAAEGFb41o1Ua5Dvk+JihgQxvjFuuaH73Qz190krpPdjWndTYZ+yPpjzo3gjdGBibYnoA==", "1234567890", false, "", 2, "21533671-8646-43e5-8ba1-6bc3553fe6cd", false, "admin@fooddeliveryapp" },
                    { "b6db1b82-b9df-480d-87bd-868408cc08c8", 0, "c61545f3-5c6a-45c6-88e0-d38933a5ed34", new DateTime(2025, 6, 9, 17, 59, 38, 72, DateTimeKind.Utc).AddTicks(1435), null, "user@fooddeliveryapp", true, "John", null, true, null, "Doe", false, null, "user@fooddeliveryapp", "user@fooddeliveryapp", null, "AQAAAAIAAYagAAAAED8tD1kPnmSgX9+lZFWjFRQ32ECst7FMt7lIkH3f/fjmcWhFUYAc/ylcyt/jfDAJog==", "1234567890", false, "", 0, "ccad4298-14b0-4703-8141-a7c637bb80c6", false, "user@fooddeliveryapp" },
                    { "cc235c9c-cbf4-42d3-8d35-342b3edb7fd7", 0, "589b3995-7c0d-4b80-9c0e-e6ff3881440d", new DateTime(2025, 6, 9, 17, 59, 38, 228, DateTimeKind.Utc).AddTicks(9070), null, "driver@fooddeliveryapp", true, "John", null, true, null, "Doe", false, null, "driver@fooddeliveryapp", "driver@fooddeliveryapp", null, "AQAAAAIAAYagAAAAEA7Ej6zpEONm7TNqNFoOTio4MT/t/aoBBuGKogeiIUHgK9PZvcpB76ww+VoVF4BuPw==", "1234567890", false, "", 1, "f42e862e-8406-43e9-bf79-c2c10a8e410d", false, "driver@fooddeliveryapp" }
                });

            migrationBuilder.InsertData(
                table: "MenuItemCategories",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "ImageUrl", "IsDeleted", "Name", "RestaurantId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(295), "system", null, "Add toppings to your pizza", "https://images.unsplash.com/photo-1528137871618-79d2761e3fd5?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", false, "Pizza Toppings", null, null, null },
                    { 2, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(300), "system", null, "Add toppings to your burger", "https://images.unsplash.com/photo-1550317138-10000687a72b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1520&q=80", false, "Burger Toppings", null, null, null },
                    { 3, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(302), "system", null, "Add toppings to your french fries", "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80", false, "French Fries Toppings", null, null, null },
                    { 4, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(304), "system", null, "Add toppings to your salad", "https://images.unsplash.com/photo-1540420773420-3366772f4999?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1384&q=80", false, "Salad Toppings", null, null, null },
                    { 5, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(305), "system", null, "Add toppings to your sushi", "https://images.unsplash.com/photo-1617196034183-421b4917c92d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", false, "Sushi Toppings", null, null, null },
                    { 6, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(307), "system", null, "Start your meal with delicious appetizers", "https://images.unsplash.com/photo-1626645738196-c2a7c87a8f58?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", false, "Appetizers", null, null, null },
                    { 7, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(309), "system", null, "Hearty and satisfying main dishes", "https://images.unsplash.com/photo-1544025162-d76694265947?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1469&q=80", false, "Main Course", null, null, null },
                    { 8, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(311), "system", null, "Sweet treats to finish your meal", "https://images.unsplash.com/photo-1563805042-7684c019e1cb?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1527&q=80", false, "Desserts", null, null, null },
                    { 9, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(313), "system", null, "Refreshing drinks to complement your food", "https://images.unsplash.com/photo-1544145945-f90425340c7e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80", false, "Beverages", null, null, null },
                    { 10, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(314), "system", null, "Perfect accompaniments to your main dish", "https://images.unsplash.com/photo-1604908176997-125f25cc6f3d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1013&q=80", false, "Sides", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "DiscountAmount", "DiscountType", "DiscountValue", "EndDate", "ImageUrl", "IsActive", "IsDeleted", "IsPercentage", "MaxUsageLimit", "MinimumOrderAmount", "RestaurantId", "StartDate", "Title", "UpdatedAt", "UpdatedBy", "UsageCount", "UsageLimit", "ValidUntil" },
                values: new object[,]
                {
                    { 5, "SUMMER2022", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(833), "system", null, "15% off summer deals", 15m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1541518763669-27fef04b14ea?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1472&q=80", false, false, true, null, 50m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "15% Off Summer", null, null, 0, null, new DateTime(2025, 9, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(831) },
                    { 6, "SUMMER2022", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(837), "system", null, "15% off summer deals", 15m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1534349762230-e0cadf78f5da?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, true, null, 50m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "15% Off Summer", null, null, 0, null, new DateTime(2025, 9, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(836) },
                    { 7, "WELCOME15", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(842), "system", null, "15% off your first order", 15m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, true, null, 20m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Welcome Discount", null, null, 0, null, new DateTime(2025, 12, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(841) },
                    { 12, "FREESHIPPING", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1019), "system", null, "Free delivery on all orders", 0m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1558&q=80", true, false, false, null, 25m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Free Delivery", null, null, 0, null, new DateTime(2025, 6, 23, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1017) }
                });

            migrationBuilder.InsertData(
                table: "RestaurantCategories",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "ImageUrl", "IsDeleted", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(38), "system", null, "Delicious pizza with a twist", "https://images.unsplash.com/photo-1513104890138-7c749659a591?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", false, "Pizza", null, null },
                    { 2, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(44), "system", null, "Juicy and crispy burgers", "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1299&q=80", false, "Burger", null, null },
                    { 3, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(46), "system", null, "Crunchy and tangy fries", "https://images.unsplash.com/photo-1630384060421-cb20d0e0649d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1025&q=80", false, "French Fries", null, null },
                    { 4, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(48), "system", null, "Fresh and healthy salads", "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", false, "Salad", null, null },
                    { 5, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(50), "system", null, "Fresh and juicy sushi", "https://images.unsplash.com/photo-1579871494447-9811cf80d66c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", false, "Sushi", null, null },
                    { 6, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(52), "system", null, "Authentic Italian cuisine", "https://images.unsplash.com/photo-1498579150354-977475b7ea0b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", false, "Italian", null, null },
                    { 7, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(54), "system", null, "Spicy and flavorful Mexican food", "https://images.unsplash.com/photo-1565299585323-38d6b0865b47?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1080&q=80", false, "Mexican", null, null },
                    { 8, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(55), "system", null, "Traditional and modern Chinese dishes", "https://images.unsplash.com/photo-1563245372-f21724e3856d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80", false, "Chinese", null, null },
                    { 9, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(57), "system", null, "Aromatic and spicy Indian cuisine", "https://images.unsplash.com/photo-1585937421612-70a008356c36?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1036&q=80", false, "Indian", null, null },
                    { 10, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(206), "system", null, "Sweet treats and desserts", "https://images.unsplash.com/photo-1551024601-bec78aea704b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1364&q=80", false, "Dessert", null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "2", "4d6ca63e-e331-4553-a2ba-d5ff0e401d5a" },
                    { "1", "9c81cb6a-9f7e-46b8-b2fe-cd86d8ccb095" },
                    { "4", "b6db1b82-b9df-480d-87bd-868408cc08c8" },
                    { "3", "cc235c9c-cbf4-42d3-8d35-342b3edb7fd7" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Address", "ApplicationUserId", "CategoryId", "City", "ClosingTime", "CreatedAt", "CreatedBy", "DeletedAt", "DeliveryFee", "DeliveryTime", "Description", "Email", "ImageUrl", "IsActive", "IsDeleted", "Latitude", "LocationUrl", "Longitude", "MinimumOrderAmount", "Name", "OpeningTime", "OwnerId", "PhoneNumber", "PostalCode", "Rating", "State", "TaxRate", "UpdatedAt", "UpdatedBy", "WebsiteUrl" },
                values: new object[,]
                {
                    { 1, "123 Main St", null, 1, "New York", new TimeSpan(0, 22, 0, 0, 0), new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(422), "system", null, 5.99m, "30-45 minutes", "Our famous pizza place", "pizza@fooddeliveryapp", "https://images.unsplash.com/photo-1513104890138-7c749659a591?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, null, "https://maps.google.com/maps?q=New+York+City", null, 20.99m, "Pizza Hut", new TimeSpan(0, 10, 0, 0, 0), "4d6ca63e-e331-4553-a2ba-d5ff0e401d5a", "123-456-7890", "10001", 4.5m, "NY", 8.5m, null, null, "" },
                    { 2, "456 Main St", null, 2, "New York", new TimeSpan(0, 22, 0, 0, 0), new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(433), "system", null, 5.99m, "30-45 minutes", "Fast and fresh burgers", "burgerking@fooddeliveryapp", "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1299&q=80", true, false, null, "https://maps.google.com/maps?q=New+York+City", null, 20.99m, "Burger King", new TimeSpan(0, 10, 0, 0, 0), "4d6ca63e-e331-4553-a2ba-d5ff0e401d5a", "123-456-7890", "10001", 4.0m, "NY", 8.5m, null, null, "" },
                    { 3, "456 Main St", null, 2, "New York", new TimeSpan(0, 22, 0, 0, 0), new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(447), "system", null, 5.99m, "30-45 minutes", "Fast and fresh burgers", "mcdonalds@fooddeliveryapp", "https://images.unsplash.com/photo-1619881585376-15ad0d9b8b43?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80", true, false, null, "https://maps.google.com/maps?q=New+York+City", null, 20.99m, "McDonald's", new TimeSpan(0, 10, 0, 0, 0), "4d6ca63e-e331-4553-a2ba-d5ff0e401d5a", "123-456-7890", "10001", 4.0m, "NY", 8.5m, null, null, "https://www.mcdonalds.com/" },
                    { 4, "789 Main St", null, 2, "New York", new TimeSpan(0, 22, 0, 0, 0), new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(455), "system", null, 5.99m, "30-45 minutes", "Fresh and healthy sandwiches", "subway@fooddeliveryapp", "https://images.unsplash.com/photo-1509722747041-616f39b57569?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, null, "https://maps.google.com/maps?q=New+York+City", null, 20.99m, "Subway", new TimeSpan(0, 10, 0, 0, 0), "4d6ca63e-e331-4553-a2ba-d5ff0e401d5a", "123-456-7890", "10001", 4.5m, "NY", 8.5m, null, null, "https://www.subway.com/" },
                    { 5, "101 Broadway", null, 6, "New York", new TimeSpan(0, 23, 0, 0, 0), new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(464), "system", null, 6.99m, "35-50 minutes", "Authentic Italian cuisine", "olivegarden@fooddeliveryapp", "https://images.unsplash.com/photo-1498579150354-977475b7ea0b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, null, "https://maps.google.com/maps?q=New+York+City", null, 25.99m, "Olive Garden", new TimeSpan(0, 11, 0, 0, 0), "4d6ca63e-e331-4553-a2ba-d5ff0e401d5a", "123-456-7891", "10002", 4.7m, "NY", 8.5m, null, null, "https://www.olivegarden.com/" },
                    { 6, "222 5th Avenue", null, 7, "New York", new TimeSpan(0, 23, 0, 0, 0), new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(477), "system", null, 4.99m, "25-40 minutes", "Delicious Mexican-inspired fast food", "tacobell@fooddeliveryapp", "https://images.unsplash.com/photo-1565299585323-38d6b0865b47?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1080&q=80", true, false, null, "https://maps.google.com/maps?q=New+York+City", null, 20.99m, "Taco Bell", new TimeSpan(0, 10, 0, 0, 0), "4d6ca63e-e331-4553-a2ba-d5ff0e401d5a", "123-456-7892", "10003", 4.2m, "NY", 8.5m, null, null, "https://www.tacobell.com/" },
                    { 7, "333 7th Avenue", null, 8, "New York", new TimeSpan(0, 22, 30, 0, 0), new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(484), "system", null, 5.49m, "30-45 minutes", "Fast and fresh Chinese cuisine", "pandaexpress@fooddeliveryapp", "https://images.unsplash.com/photo-1563245372-f21724e3856d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80", true, false, null, "https://maps.google.com/maps?q=New+York+City", null, 20.99m, "Panda Express", new TimeSpan(0, 11, 0, 0, 0), "4d6ca63e-e331-4553-a2ba-d5ff0e401d5a", "123-456-7893", "10004", 4.3m, "NY", 8.5m, null, null, "https://www.pandaexpress.com/" },
                    { 8, "444 Lexington Ave", null, 9, "New York", new TimeSpan(0, 23, 0, 0, 0), new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(501), "system", null, 6.49m, "40-55 minutes", "Authentic Indian cuisine with rich flavors", "tajmahal@fooddeliveryapp", "https://images.unsplash.com/photo-1585937421612-70a008356c36?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1036&q=80", true, false, null, "https://maps.google.com/maps?q=New+York+City", null, 25.99m, "Taj Mahal", new TimeSpan(0, 12, 0, 0, 0), "4d6ca63e-e331-4553-a2ba-d5ff0e401d5a", "123-456-7894", "10005", 4.8m, "NY", 8.5m, null, null, "https://www.tajmahal.com/" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Calories", "CategoryId", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "ImageUrl", "IsAvailable", "IsDeleted", "IsVegan", "IsVegetarian", "Name", "PreparationTime", "Price", "Rating", "RestaurantCategoryId", "RestaurantId", "SpiceLevel", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 500, 1, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(591), "system", null, "Classic pizza with tomatoes, mozzarella, and basil", "https://images.unsplash.com/photo-1604382354936-07c5d9983bd3?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, false, false, "Margherita Pizza", new TimeSpan(0, 0, 15, 0, 0), 12.99m, 4.5, null, 1, 3, null, null },
                    { 2, 550, 1, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(599), "system", null, "Pepperoni pizza with tomatoes, mozzarella, and basil", "https://images.unsplash.com/photo-1628840042765-356cda07504e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1480&q=80", true, false, false, false, "Pepperoni Pizza", new TimeSpan(0, 0, 20, 0, 0), 13.99m, 4.7000000000000002, null, 1, 4, null, null },
                    { 3, 250, 2, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(604), "system", null, "Tasty cheeseburger with lettuce, tomatoes, and onions", "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1299&q=80", true, false, false, false, "Cheeseburger", new TimeSpan(0, 0, 10, 0, 0), 6.99m, 4.2999999999999998, null, 2, 2, null, null },
                    { 4, 300, 2, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(646), "system", null, "Delicious chicken sandwich with lettuce, tomatoes, and mayo", "https://images.unsplash.com/photo-1606755962773-d324e0a13086?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80", true, false, false, false, "Chicken Sandwich", new TimeSpan(0, 0, 15, 0, 0), 8.99m, 4.5999999999999996, null, 2, 3, null, null },
                    { 5, 450, 1, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(651), "system", null, "Fresh vegetable pizza with bell peppers, onions, and mushrooms", "https://images.unsplash.com/photo-1593560708920-61dd98c46a4e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, false, true, "Vegetable Pizza", new TimeSpan(0, 0, 18, 0, 0), 11.99m, 4.4000000000000004, null, 1, 2, null, null },
                    { 6, 580, 1, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(657), "system", null, "Delicious BBQ chicken pizza with red onions and cilantro", "https://images.unsplash.com/photo-1594007654729-407eedc4fe24?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1428&q=80", true, false, false, false, "BBQ Chicken Pizza", new TimeSpan(0, 0, 20, 0, 0), 14.99m, 4.7999999999999998, null, 1, 3, null, null },
                    { 7, 680, 2, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(663), "system", null, "Juicy burger with bacon, cheese, lettuce, and special sauce", "https://images.unsplash.com/photo-1553979459-d2229ba7433b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1368&q=80", true, false, false, false, "Bacon Cheeseburger", new TimeSpan(0, 0, 12, 0, 0), 8.99m, 4.7000000000000002, null, 2, 2, null, null },
                    { 8, 420, 2, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(668), "system", null, "Plant-based burger with all the fixings", "https://images.unsplash.com/photo-1520072959219-c595dc870360?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1490&q=80", true, false, true, true, "Veggie Burger", new TimeSpan(0, 0, 10, 0, 0), 7.99m, 4.5, null, 2, 1, null, null },
                    { 9, 650, 6, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(673), "system", null, "Creamy fettuccine pasta with parmesan cheese sauce", "https://images.unsplash.com/photo-1645112411341-6c4fd023882a?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, false, true, "Fettuccine Alfredo", new TimeSpan(0, 0, 15, 0, 0), 13.99m, 4.5999999999999996, null, 5, 1, null, null },
                    { 10, 720, 7, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(678), "system", null, "Breaded chicken topped with marinara sauce and melted cheese", "https://images.unsplash.com/photo-1632778149955-e80f8ceca2e8?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, false, false, "Chicken Parmesan", new TimeSpan(0, 0, 25, 0, 0), 15.99m, 4.7999999999999998, null, 5, 2, null, null },
                    { 11, 170, 7, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(683), "system", null, "Classic crunchy taco with seasoned beef, lettuce, and cheese", "https://images.unsplash.com/photo-1551504734-5ee1c4a1479b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, false, false, "Crunchy Taco", new TimeSpan(0, 0, 5, 0, 0), 2.99m, 4.2999999999999998, null, 6, 3, null, null },
                    { 12, 650, 7, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(688), "system", null, "Large burrito filled with beef, beans, rice, and all the toppings", "https://images.unsplash.com/photo-1626700051175-6818013e1d4f?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1374&q=80", true, false, false, false, "Burrito Supreme", new TimeSpan(0, 0, 8, 0, 0), 7.99m, 4.5, null, 6, 4, null, null },
                    { 13, 490, 7, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(693), "system", null, "Crispy chicken pieces tossed in a sweet and tangy orange sauce", "https://images.unsplash.com/photo-1525755662778-989d0524087e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, false, false, "Orange Chicken", new TimeSpan(0, 0, 15, 0, 0), 9.99m, 4.7000000000000002, null, 7, 2, null, null },
                    { 14, 380, 7, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(704), "system", null, "Stir-fried rice with vegetables, eggs, and your choice of protein", "https://images.unsplash.com/photo-1603133872878-684f208fb84b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1625&q=80", true, false, false, false, "Fried Rice", new TimeSpan(0, 0, 12, 0, 0), 8.99m, 4.4000000000000004, null, 7, 2, null, null },
                    { 15, 550, 7, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(709), "system", null, "Tender chicken in a rich and creamy tomato-based sauce", "https://images.unsplash.com/photo-1603894584373-5ac82b2ae398?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, false, false, "Butter Chicken", new TimeSpan(0, 0, 20, 0, 0), 14.99m, 4.9000000000000004, null, 8, 3, null, null },
                    { 16, 210, 10, new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(714), "system", null, "Soft flatbread with garlic and butter", "https://images.unsplash.com/photo-1596428043595-8eee307e6979?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, false, true, "Garlic Naan", new TimeSpan(0, 0, 8, 0, 0), 3.99m, 4.7000000000000002, null, 8, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "DeletedAt", "Description", "DiscountAmount", "DiscountType", "DiscountValue", "EndDate", "ImageUrl", "IsActive", "IsDeleted", "IsPercentage", "MaxUsageLimit", "MinimumOrderAmount", "RestaurantId", "StartDate", "Title", "UpdatedAt", "UpdatedBy", "UsageCount", "UsageLimit", "ValidUntil" },
                values: new object[,]
                {
                    { 1, "10OFF", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(806), "system", null, "10% off your weekly orders", 10m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1607083206968-13611e3d76db?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1515&q=80", true, false, true, null, 50m, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10% Off", null, null, 0, null, new DateTime(2025, 7, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(795) },
                    { 2, "SAVE25", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(813), "system", null, "25% off your weekly orders", 25m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1626684496076-07e23c6361ff?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, true, null, 50m, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "25% Off", null, null, 0, null, new DateTime(2025, 7, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(810) },
                    { 3, "NEWYEAR2022", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(823), "system", null, "Buy 2 get 1 free on new year's eve", 0m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1577563908411-5077b6dc7624?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, false, null, 50m, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Buy 2 Get 1 Free", null, null, 0, null, new DateTime(2025, 6, 10, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(817) },
                    { 4, "SUMMER2022", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(828), "system", null, "15% off summer deals", 15m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1473186578172-c141e6798cf4?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1473&q=80", true, false, true, null, 50m, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "15% Off Summer", null, null, 0, null, new DateTime(2025, 9, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(826) },
                    { 8, "PASTA20", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(847), "system", null, "20% off all pasta dishes", 20m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1551183053-bf91a1d81141?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1632&q=80", true, false, true, null, 15m, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pasta Special", null, null, 0, null, new DateTime(2025, 8, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(845) },
                    { 9, "TACOTUESDAY", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(852), "system", null, "Buy one taco, get one free every Tuesday", 0m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1504544750208-dc0358e63f7f?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, false, null, 5m, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taco Tuesday", null, null, 0, null, new DateTime(2026, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(850) },
                    { 10, "FAMILYMEAL", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(857), "system", null, "Save $10 on family meal combos", 10m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1493770348161-369560ae357d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1470&q=80", true, false, false, null, 40m, 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family Meal Deal", null, null, 0, null, new DateTime(2025, 9, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(855) },
                    { 11, "SPICY25", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1013), "system", null, "25% off all spicy dishes", 25m, 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://images.unsplash.com/photo-1505253758473-96b7015fcd40?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1400&q=80", true, false, true, null, 30m, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spicy Special", null, null, 0, null, new DateTime(2025, 8, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1010) }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Content", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "MenuItemId", "Rating", "RestaurantId", "Title", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 9, "Pizza Hut never disappoints. Fast delivery and delicious food!", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1139), "system", null, false, null, 4.5m, 1, "Great restaurant", null, null, "b6db1b82-b9df-480d-87bd-868408cc08c8" },
                    { 10, "The staff at Olive Garden was very friendly and attentive.", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1142), "system", null, false, null, 4.8m, 5, "Excellent service", null, null, "b6db1b82-b9df-480d-87bd-868408cc08c8" },
                    { 1, "The Margherita Pizza was absolutely delicious. Fresh ingredients and perfect crust!", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1115), "system", null, false, 1, 5.0m, 1, "Amazing Pizza!", null, null, "b6db1b82-b9df-480d-87bd-868408cc08c8" },
                    { 2, "The pepperoni pizza was good, but I wish it had more toppings.", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1118), "system", null, false, 2, 3.5m, 1, "Good but could be better", null, null, "b6db1b82-b9df-480d-87bd-868408cc08c8" },
                    { 3, "This cheeseburger is amazing. Juicy, flavorful, and perfectly cooked.", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1121), "system", null, false, 3, 4.8m, 2, "Best burger in town!", null, null, "b6db1b82-b9df-480d-87bd-868408cc08c8" },
                    { 4, "The chicken sandwich was okay, but nothing special.", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1124), "system", null, false, 4, 3.0m, 2, "Decent sandwich", null, null, "b6db1b82-b9df-480d-87bd-868408cc08c8" },
                    { 5, "The pasta was cooked to perfection and the sauce was delicious!", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1127), "system", null, false, 9, 4.7m, 5, "Authentic Italian", null, null, "b6db1b82-b9df-480d-87bd-868408cc08c8" },
                    { 6, "The tacos had the perfect amount of spice and the ingredients were fresh.", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1130), "system", null, false, 11, 4.5m, 6, "Spicy and flavorful", null, null, "b6db1b82-b9df-480d-87bd-868408cc08c8" },
                    { 7, "The orange chicken was crispy and the sauce was perfect - not too sweet.", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1133), "system", null, false, 13, 4.6m, 7, "Delicious Chinese food", null, null, "b6db1b82-b9df-480d-87bd-868408cc08c8" },
                    { 8, "The butter chicken was rich, creamy, and full of flavor. Highly recommend!", new DateTime(2025, 6, 9, 17, 59, 38, 229, DateTimeKind.Utc).AddTicks(1137), "system", null, false, 15, 5.0m, 8, "Best Indian food I've had", null, null, "b6db1b82-b9df-480d-87bd-868408cc08c8" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

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
                name: "IX_CartItems_RestaurantId",
                table: "CartItems",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_UserId",
                table: "Drivers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_MenuItemId",
                table: "Favorites",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemCategories_RestaurantId",
                table: "MenuItemCategories",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryId",
                table: "MenuItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_RestaurantCategoryId",
                table: "MenuItems",
                column: "RestaurantCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_RestaurantId",
                table: "MenuItems",
                column: "RestaurantId");

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
                name: "IX_Orders_DeliveryAddressId",
                table: "Orders",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DriverId",
                table: "Orders",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RestaurantId",
                table: "Orders",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTracking_DeliveryAddressId",
                table: "OrderTracking",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTracking_DriverId",
                table: "OrderTracking",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTracking_OrderId",
                table: "OrderTracking",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_RestaurantId",
                table: "Promotions",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_ApplicationUserId",
                table: "Restaurants",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_CategoryId",
                table: "Restaurants",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MenuItemId",
                table: "Reviews",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RestaurantId",
                table: "Reviews",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
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
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "OrderTracking");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "SearchLogs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "MenuItemCategories");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RestaurantCategories");
        }
    }
}
