using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class updataSeedsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_RestaurantCategories_CategoryId",
                table: "Restaurants");

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "City", "Country", "CreatedAt", "CustomerProfileId", "IsDefault", "PostalCode", "State", "Street", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "New York", "USA", new DateTime(2025, 3, 21, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), 1, true, "10001", "NY", "123 Main St", "Home", null },
                    { 2, "New York", "USA", new DateTime(2025, 3, 30, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), 1, false, "10002", "NY", "456 Work Ave", "Home", null }
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "81173f50-e3e5-43b8-8412-daffa3d0d2b9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "334d89b3-a5fe-4328-b3b5-9970e93cc734");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "875c8938-184d-454c-ad18-062794d5030e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "b260a7b2-7212-4401-9ee5-99e9d2ebd8ce");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bbdcd5cc-2d03-40ca-b9b4-4bf0ff623496", new DateTime(2025, 4, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "AQAAAAIAAYagAAAAEFnn7/X6EaJhJjJdI99zOljVgcrR57WbfetSsJ4SCEdKYMEVp0zDVtwpK2PzWMFGLw==", "4c1ab6d0-73ef-4d5c-a661-dde3173c2f49" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4018351a-71c7-4d79-bda0-fe72dc458a9c", new DateTime(2025, 4, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "AQAAAAIAAYagAAAAEBfkXXCvgLUk4bKYdFc/vqRPSYnjdNkvzglFivdB/QADSc3KiNrmauyYCCXV0ICL2Q==", "a998d207-74e8-4ee6-a30b-3760ede73442" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e6da6176-dfa7-408a-aaf8-4ac5709a6b51", new DateTime(2025, 4, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "AQAAAAIAAYagAAAAEDFiwsXc1PVOfAlz6w3s7U2ubsIVdXqCGVpUt2FepG8M7zsCjLISMmx3+ikq47/fMA==", "aef850c6-ce50-4858-85c3-df36c0fef89d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "af23c196-1bb5-4493-9768-523fc7dbbfad", new DateTime(2025, 4, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "AQAAAAIAAYagAAAAEM5roDKDGUYmlintN8kGn348E6Jyck1TuBqObz354qEPUK65jxW5MJ+66/SUmfDfQg==", "a736dc58-4f3a-4f1f-9c95-25d65fb2c50b" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ProfilePictureUrl", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 20, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "/images/users/user2.jpg", new DateTime(2025, 4, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786) });

            migrationBuilder.InsertData(
                table: "EmployeeProfiles",
                columns: new[] { "Id", "CreatedAt", "FirstName", "HireDate", "IsActive", "LastName", "PhoneNumber", "Position", "ProfilePictureUrl", "TerminationDate", "UpdatedAt", "UserId" },
                values: new object[] { 1, new DateTime(2024, 10, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "Jane", new DateTime(2024, 10, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), true, "Smith", "555-222-2222", 1, null, null, new DateTime(2025, 4, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "3" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 21, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "/images/menu/carbonara.jpg" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 21, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "/images/menu/pizza.jpg" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 26, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "/images/menu/quesadilla.jpg" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 26, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "/images/menu/burrito.jpg" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 31, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "/images/menu/general-tsos.jpg" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 31, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "/images/menu/lo-mein.jpg" });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "AccountNumberMasked", "CreatedAt", "CustomerProfileId", "IsDefault", "Provider", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "************1234", new DateTime(2025, 3, 22, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), 1, true, "Visa", 0, null },
                    { 2, "********@paypal.com", new DateTime(2025, 4, 5, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), 1, false, "PayPal", 2, null }
                });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), new DateTime(2025, 4, 9, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 4, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), new DateTime(2025, 4, 14, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImageUrl", "OwnerId" },
                values: new object[] { new DateTime(2025, 3, 20, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "/images/restaurants/italian.jpg", "4" });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImageUrl", "OwnerId" },
                values: new object[] { new DateTime(2025, 3, 25, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "/images/restaurants/mexican.jpg", "4" });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImageUrl", "OwnerId" },
                values: new object[] { new DateTime(2025, 3, 30, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), "/images/restaurants/asian.jpg", "4" });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 10, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 15, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerProfileId", "DeliveryDate", "DeliveryEmployeeId", "DeliveryFee", "DeliveryInstructions", "OrderDate", "RestaurantId", "Status", "Subtotal", "Tax", "TotalAmount" },
                values: new object[,]
                {
                    { 1, 1, null, 1, 4.99m, "Ring the bell twice", new DateTime(2025, 4, 9, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), 1, 5, 0m, 2.50m, 22.48m },
                    { 2, 1, null, 1, 3.99m, null, new DateTime(2025, 4, 14, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), 2, 5, 0m, 1.80m, 17.78m }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "Discount", "MenuItemId", "OrderId", "Quantity", "SpecialInstructions", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 0m, 1, 1, 1, "Extra cheese please", 14.99m },
                    { 2, 0m, 3, 2, 2, null, 9.99m }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "OrderId", "PaymentDate", "PaymentMethodId", "Status", "TransactionId" },
                values: new object[,]
                {
                    { 1, 22.48m, 1, new DateTime(2025, 4, 9, 15, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), 1, 1, "PAY-123456789" },
                    { 2, 17.78m, 2, new DateTime(2025, 4, 14, 15, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), 1, 1, "PAY-987654321" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_RestaurantCategories_CategoryId",
                table: "Restaurants",
                column: "CategoryId",
                principalTable: "RestaurantCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_RestaurantCategories_CategoryId",
                table: "Restaurants");

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EmployeeProfiles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "075b6f22-6080-49f8-ac10-43d9912831bf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "f26aa7c1-9745-42d1-8fa1-5736fb64c0f8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "9db92942-6022-463e-9f95-d72f376c14ce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "d0f12222-00ef-4568-ac88-3e0ce590aead");

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "Admin", "true", "1" },
                    { 2, "Customer", "true", "2" },
                    { 3, "Employee", "true", "3" },
                    { 4, "Owner", "true", "4" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b19a41fe-74fe-467f-813b-bdd6c50e951e", new DateTime(2025, 4, 18, 22, 48, 3, 890, DateTimeKind.Utc).AddTicks(9502), "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", "72d9a81a-7e0d-42c1-a7d8-2291c5454046" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9e26e461-8b01-4d2e-85e7-ec9705dbaef2", new DateTime(2025, 4, 18, 22, 48, 3, 890, DateTimeKind.Utc).AddTicks(9581), "mOxlSo3yj48PjwIiBIPUaRa4UBfeC3TY7HVcKMuFOag=", "a84b552b-24ae-4025-82d9-cc811a08d97b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e131ada3-1f5d-424b-b2fe-2ef87022140a", new DateTime(2025, 4, 18, 22, 48, 3, 890, DateTimeKind.Utc).AddTicks(9722), "tL0pSAqxlvqnguDU7NEML0ISgUEFIn5feZL1v0shKmQ=", "4ab0f37a-69b8-4874-a270-c93fa0e22685" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d8963e07-2ccc-46a1-a240-280486d3fd08", new DateTime(2025, 4, 18, 22, 48, 3, 890, DateTimeKind.Utc).AddTicks(9776), "VT/qz4macpFrK1DkyV9YegbW0bi05rQzvrX+oxXQrLA=", "0fa765e2-88c4-439a-92ca-a35fceb8b520" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ProfilePictureUrl", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 19, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(276), "", new DateTime(2025, 4, 18, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(283) });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 20, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1363), "" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 20, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1374), "" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 25, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1378), "" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 25, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1383), "" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 30, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1387), "" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 30, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1395), "" });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 18, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1482), new DateTime(2025, 4, 8, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1481) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 3, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1496), new DateTime(2025, 4, 13, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1493) });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Code", "Description", "DiscountValue", "EndDate", "IsActive", "IsPercentage", "MinimumOrderAmount", "RestaurantId", "StartDate", "UsageLimit" },
                values: new object[] { 3, "FREEDELIVERY", "Free delivery on orders over $25", 5.99m, new DateTime(2025, 4, 25, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1504), true, false, 25, null, new DateTime(2025, 4, 15, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1503), null });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImageUrl", "OwnerId" },
                values: new object[] { new DateTime(2025, 3, 19, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1194), "", "1" });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImageUrl", "OwnerId" },
                values: new object[] { new DateTime(2025, 3, 24, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1211), "", "1" });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImageUrl", "OwnerId" },
                values: new object[] { new DateTime(2025, 3, 29, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1220), "", "1" });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1578));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1583));

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "CustomerProfileId", "Rating", "RestaurantId", "UpdatedAt" },
                values: new object[] { 3, "Average Chinese food, nothing special.", new DateTime(2025, 4, 16, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1586), 1, 3, 3, null });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_RestaurantCategories_CategoryId",
                table: "Restaurants",
                column: "CategoryId",
                principalTable: "RestaurantCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
