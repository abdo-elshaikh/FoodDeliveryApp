using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRestauranrModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Restaurants");

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
                column: "CreatedAt",
                value: new DateTime(2025, 4, 18, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 23, 21, 23, 8, 888, DateTimeKind.Utc).AddTicks(6631));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Restaurants",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "d86b329a-2ada-4224-b6ba-83a6b776b430");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "677e1870-511e-440a-87a8-f9e8acbe145c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "cd05b8b7-5033-4af7-a5c7-41bf71fa9b79");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "78773da7-c296-4db5-bcc7-8ef8bc758d5d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1b508c9d-77d3-4765-be28-948ce8d4cc20", new DateTime(2025, 4, 27, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), "AQAAAAIAAYagAAAAEIjR56rC27A81wVMqHyivi9BRGbJKFGxurSOUT8CeBBWQW2sAil+KRLXiCkfTTjp5w==", "1fd46b37-a3d0-49d6-b630-d84317251cfa" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f16a856a-f8b0-4c5d-9e69-4292d52a447a", new DateTime(2025, 4, 27, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), "AQAAAAIAAYagAAAAEOBzzDQcpwIboC5SnXZXf34odoT4uj+nr/Nt9Y9vmqDnRBHfIeey+Mk9ovgEde/Chw==", "ba62bd07-6122-4d52-9a81-82ad04501e31" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7c37acc2-1174-4eee-9019-0f1ce81c7081", new DateTime(2025, 4, 27, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), "AQAAAAIAAYagAAAAEI457SKIudrpuxVLWMtCG+yGHfGv6wm7LzDDIR5PAW/vzKGGZ/h49mTzeXH61e8hBw==", "8b09e99a-d881-4b61-a2c3-ae093976ea7a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2bbce06e-eb9e-4f79-a74b-371c0b724c7b", new DateTime(2025, 4, 27, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), "AQAAAAIAAYagAAAAEE6DvJ6wf2VHIBjbUAOE3NvPzniouer/uugqHsQU5spRgqDXnQ44KytSgM2+kcp2xg==", "15aa65cd-1b24-4beb-94f7-c1764c1e2f83" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 28, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), new DateTime(2025, 4, 27, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774) });

            migrationBuilder.UpdateData(
                table: "EmployeeProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HireDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 27, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), new DateTime(2024, 10, 27, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), new DateTime(2025, 4, 27, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774) });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 30, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 17, 21, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 22, 21, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 27, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), new DateTime(2025, 4, 17, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 12, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), new DateTime(2025, 4, 22, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Rating" },
                values: new object[] { new DateTime(2025, 3, 28, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), 4.5m });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Rating" },
                values: new object[] { new DateTime(2025, 4, 2, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), 4.2m });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Rating" },
                values: new object[] { new DateTime(2025, 4, 7, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774), 4.7m });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 18, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 23, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));
        }
    }
}
