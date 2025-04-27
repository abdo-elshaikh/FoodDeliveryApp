using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class updateSeedsDat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "075b6f22-6080-49f8-ac10-43d9912831bf", "Admin", "ADMIN" },
                    { "2", "f26aa7c1-9745-42d1-8fa1-5736fb64c0f8", "Customer", "CUSTOMER" },
                    { "3", "9db92942-6022-463e-9f95-d72f376c14ce", "Employee", "EMPLOYEE" },
                    { "4", "d0f12222-00ef-4568-ac88-3e0ce590aead", "Owner", "OWNER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "Admin", "true", "1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "NormalizedEmail", "NormalizedUserName", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "UserName" },
                values: new object[] { "b19a41fe-74fe-467f-813b-bdd6c50e951e", new DateTime(2025, 4, 18, 22, 48, 3, 890, DateTimeKind.Utc).AddTicks(9502), "ADMIN@FOODFAST.COM", "ADMIN@FOODFAST.COM", "555-000-0000", true, "72d9a81a-7e0d-42c1-a7d8-2291c5454046", "admin@foodfast.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { "2", 0, "9e26e461-8b01-4d2e-85e7-ec9705dbaef2", new DateTime(2025, 4, 18, 22, 48, 3, 890, DateTimeKind.Utc).AddTicks(9581), "customer@foodfast.com", true, true, false, null, "CUSTOMER@FOODFAST.COM", "CUSTOMER@FOODFAST.COM", "mOxlSo3yj48PjwIiBIPUaRa4UBfeC3TY7HVcKMuFOag=", "555-111-1111", true, 0, "a84b552b-24ae-4025-82d9-cc811a08d97b", false, null, "customer@foodfast.com" },
                    { "3", 0, "e131ada3-1f5d-424b-b2fe-2ef87022140a", new DateTime(2025, 4, 18, 22, 48, 3, 890, DateTimeKind.Utc).AddTicks(9722), "employee@foodfast.com", true, true, false, null, "EMPLOYEE@FOODFAST.COM", "EMPLOYEE@FOODFAST.COM", "tL0pSAqxlvqnguDU7NEML0ISgUEFIn5feZL1v0shKmQ=", "555-222-2222", true, 1, "4ab0f37a-69b8-4874-a270-c93fa0e22685", false, null, "employee@foodfast.com" },
                    { "4", 0, "d8963e07-2ccc-46a1-a240-280486d3fd08", new DateTime(2025, 4, 18, 22, 48, 3, 890, DateTimeKind.Utc).AddTicks(9776), "owner@foodfast.com", true, true, false, null, "OWNER@FOODFAST.COM", "OWNER@FOODFAST.COM", "VT/qz4macpFrK1DkyV9YegbW0bi05rQzvrX+oxXQrLA=", "555-333-3333", true, 3, "0fa765e2-88c4-439a-92ca-a35fceb8b520", false, null, "owner@foodfast.com" }
                });

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

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 25, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1504), new DateTime(2025, 4, 15, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1503) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 19, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1194), "" });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 24, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1211), "" });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 29, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1220), "" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 2, "Customer", "true", "2" },
                    { 3, "Employee", "true", "3" },
                    { 4, "Owner", "true", "4" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1", "1" },
                    { "2", "2" },
                    { "3", "3" },
                    { "4", "4" }
                });

            migrationBuilder.InsertData(
                table: "CustomerProfiles",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FirstName", "IsActive", "LastName", "LoyaltyPoints", "PhoneNumber", "ProfilePictureUrl", "ReceivePromotions", "UpdatedAt", "UserId" },
                values: new object[] { 1, new DateTime(2025, 3, 19, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(276), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "John", true, "Doe", 0m, "555-111-1111", "", false, new DateTime(2025, 4, 18, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(283), "2" });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "CustomerProfileId", "Rating", "RestaurantId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Best Italian food I've ever had!", new DateTime(2025, 4, 8, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1578), 1, 5, 1, null },
                    { 2, "Great tacos, but a bit spicy for my taste.", new DateTime(2025, 4, 13, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1583), 1, 4, 2, null },
                    { 3, "Average Chinese food, nothing special.", new DateTime(2025, 4, 16, 22, 48, 3, 891, DateTimeKind.Utc).AddTicks(1586), 1, 3, 3, null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
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
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "2" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "3" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4", "4" });

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "NormalizedEmail", "NormalizedUserName", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "UserName" },
                values: new object[] { "dd06c829-cf75-46dc-bea4-91533da772b2", new DateTime(2025, 4, 17, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8461), null, null, null, false, "c9993002-628d-49c6-b2a0-d6835cddbd89", null });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 19, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8940), "/images/menu/carbonara.jpg" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 19, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8946), "/images/menu/margherita.jpg" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 24, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8948), "/images/menu/quesadilla.jpg" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 24, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8950), "/images/menu/burrito.jpg" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 29, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8953), "/images/menu/general-tsos.jpg" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 29, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8957), "/images/menu/lo-mein.jpg" });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 17, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8994), new DateTime(2025, 4, 7, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8993) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 2, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8999), new DateTime(2025, 4, 12, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8999) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 24, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(9002), new DateTime(2025, 4, 14, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(9001) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 18, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8790), "/images/restaurants/italian.jpg" });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 23, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8801), "/images/restaurants/mexican.jpg" });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 28, 21, 24, 13, 189, DateTimeKind.Utc).AddTicks(8895), "/images/restaurants/asian.jpg" });

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
