using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRatingRivewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Reviews",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "087eba77-3446-45fb-84bb-7ebba9e0cf3c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "a891a656-c6b0-4433-be5d-14d231072617");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "97d8966a-0a71-4338-be76-352735ad157f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "7948374f-16e2-4e1a-9508-a3643363cd27");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e8c2ac45-aa9a-4771-adc1-86654f646910", new DateTime(2025, 4, 27, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), "AQAAAAIAAYagAAAAEHvxy2IBbM/Ijk6dVHqgfWj6qUc08Y/3cIsPzy7Iz/PK+hgZLIDqJMmNw8NePwIfhg==", "d4d4404e-e19d-4177-bada-a517b05d9b78" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "19e1c6d0-9cb6-4648-af94-b58828565a71", new DateTime(2025, 4, 27, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), "AQAAAAIAAYagAAAAEG9d/BGYoHVMwDHBmy0lO1O766K3wRFKGY3m0YZXBL3MmOeG93BxoTDkG4mNcd+9cQ==", "dd5e690f-1faf-4168-92d2-f7708e654146" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "32e2d901-0ac7-43ba-ba73-56f581bdbfaa", new DateTime(2025, 4, 27, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), "AQAAAAIAAYagAAAAEKkLDBtniA3F/9qNsaNehuv3x96YcRLN5N+2kkP9qU5URemlEnI0hgx/QaBpRMpjLw==", "4fc4bafd-e9f8-4f69-a802-25b26a207f1e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54fb4137-2018-48ce-8a24-a59efc6be0e6", new DateTime(2025, 4, 27, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), "AQAAAAIAAYagAAAAEEwao01jHY2C02ZRHPvprFEqxzYhZzl6+xX/ezbm+G9vUxfl9Kp063OGFedNO/iifg==", "90659c8c-d306-4211-81f5-fd81056ec4e9" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 28, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), new DateTime(2025, 4, 27, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369) });

            migrationBuilder.UpdateData(
                table: "EmployeeProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HireDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 27, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), new DateTime(2024, 10, 27, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), new DateTime(2025, 4, 27, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369) });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 30, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 17, 22, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 22, 22, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 27, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), new DateTime(2025, 4, 17, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 12, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), new DateTime(2025, 4, 22, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 28, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 2, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Rating" },
                values: new object[] { new DateTime(2025, 4, 18, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), 5.0m });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Rating" },
                values: new object[] { new DateTime(2025, 4, 23, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), 4.5m });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "CustomerProfileId", "Rating", "RestaurantId", "UpdatedAt" },
                values: new object[,]
                {
                    { 3, "Good Chinese food, but the service was slow.", new DateTime(2025, 4, 25, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), 1, 4.0m, 3, null },
                    { 4, "Decent food, but not as good as I expected.", new DateTime(2025, 4, 26, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), 1, 3.5m, 1, null },
                    { 5, "Loved the burrito, will order again!", new DateTime(2025, 4, 24, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), 1, 4.0m, 2, null },
                    { 6, "The best General Tso's chicken in town!", new DateTime(2025, 4, 25, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), 1, 5.0m, 3, null },
                    { 7, "Great pizza, but a bit overpriced.", new DateTime(2025, 4, 26, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), 1, 4.5m, 1, null },
                    { 8, "Good food, but the delivery was late.", new DateTime(2025, 4, 24, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369), 1, 4.0m, 2, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "7c00b9bd-5b49-4f09-9e4f-a9d17af0b4b1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "ac19a4f5-9c1f-49fe-8525-ea44c25e6427");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "93228729-f8bf-4463-947a-535126127fd6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "9067fc4b-c5e8-46f0-af09-1edcd83fe981");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2e3444ab-07e4-4e3e-81b4-104d65d347fe", new DateTime(2025, 4, 27, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631), "AQAAAAIAAYagAAAAEJnNfJBKXxgO3X6bf/nRsIaqYbA6HJcNU5D3Yhzt6fHmoAFlcQai8hLV0LENjZi1ng==", "cbcb2f4c-e54e-474e-879c-f65dda6b7fa4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd7d8a33-63b9-45e1-9af2-be3c7164db93", new DateTime(2025, 4, 27, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631), "AQAAAAIAAYagAAAAECTDfKDghWMmytnV6JzfKWi4QDE5hk69rFiweMvrzOyRLdtfysnsDA0fOP9lBv/2Jg==", "2647e7a2-39b2-4911-b266-5a133e558b06" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d523f696-8c49-46a8-81da-7febe0f4b1c9", new DateTime(2025, 4, 27, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631), "AQAAAAIAAYagAAAAEA0d4mJ5/2XbpnuK2LSNsEmNoRBIxVbLOJCyt/WrCOl+Qvbx7G684kzBOhfvk0agYg==", "b35cbfd1-72e8-4c6f-8f3c-e28a86520417" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ace6d318-15c4-4a95-94b3-5a7abc4ce314", new DateTime(2025, 4, 27, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631), "AQAAAAIAAYagAAAAEPFcb0kjNzDIMwVxipyWwg7uX7tUOi4QyRWWpX0cjpRqiv5JwJFdVkKtXpKtQGO3WA==", "dccd3c07-6fad-4b55-a406-7ba24a0e95e9" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 28, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631), new DateTime(2025, 4, 27, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631) });

            migrationBuilder.UpdateData(
                table: "EmployeeProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HireDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 27, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631), new DateTime(2024, 10, 27, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631), new DateTime(2025, 4, 27, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631) });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 30, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 17, 22, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 22, 22, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 27, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631), new DateTime(2025, 4, 17, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 12, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631), new DateTime(2025, 4, 22, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 28, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 2, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Rating" },
                values: new object[] { new DateTime(2025, 4, 18, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631), 5 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Rating" },
                values: new object[] { new DateTime(2025, 4, 23, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631), 4 });
        }
    }
}
