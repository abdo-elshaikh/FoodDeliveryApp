using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class AppendLocationUrlAndTaxFeeToRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFee",
                table: "Restaurants",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "LocationUrl",
                table: "Restaurants",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxRate",
                table: "Restaurants",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "Restaurants",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 26, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 4, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "efc85486-71ec-4c81-b187-a0ad65b1a374");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "907e628f-80c8-4136-967f-dcf1625c8328");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "9dc7c331-9d18-40c6-9385-6a9c563fd05c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "a3eda877-a892-4e31-99f8-330dfa52fb2e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b8ceca9f-730d-4688-b704-c274c9838ca7", new DateTime(2025, 4, 24, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), "AQAAAAIAAYagAAAAEMARcompvsv4fe7V+HrJg+BBrgBUFhO2CBlt038G9w7EkFbsw+QYdV4jMAg2cb30Wg==", "28ecbf36-d267-48aa-86d4-b92266fea45a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5ab6f01c-d897-4d3a-b4c6-4ca508050234", new DateTime(2025, 4, 24, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), "AQAAAAIAAYagAAAAEBSMWQzRI+fFjWHdInYhyj8Ga77xYhG9ZqNlix4jgF7P22j2H0wjwrmAVbX5kUFwWw==", "14ce2baf-47a2-4da5-a6a9-c5599eca4b06" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9c9f3c2a-1f64-432a-8942-6489546a7743", new DateTime(2025, 4, 24, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), "AQAAAAIAAYagAAAAEOX4yMiSzjSZiJ/7Sn4hMdO9gl/DG9kP8yzKCrurg7lBVQbKDkOHgO5yeAo7R5sA/g==", "a824b2dc-d287-4135-8b0a-b833cde2df5a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c854eaf3-ad1c-4762-bd46-bd20c89217a6", new DateTime(2025, 4, 24, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), "AQAAAAIAAYagAAAAEENtLHVYz++RG4iSy24hd1ohW6clgZyBeQCaf8kLtg/uDOE0g0JYNyZn0iIH4zCHDQ==", "5afd58e2-8947-491e-a0d7-03d1aa205959" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 25, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), new DateTime(2025, 4, 24, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997) });

            migrationBuilder.UpdateData(
                table: "EmployeeProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HireDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 24, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), new DateTime(2024, 10, 24, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), new DateTime(2025, 4, 24, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997) });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 26, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 26, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 31, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 31, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 5, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 5, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 27, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 10, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 14, 14, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 19, 14, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 24, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), new DateTime(2025, 4, 14, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 9, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), new DateTime(2025, 4, 19, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DeliveryFee", "LocationUrl", "TaxRate", "WebsiteUrl" },
                values: new object[] { new DateTime(2025, 3, 25, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), 0m, null, 0m, null });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DeliveryFee", "LocationUrl", "TaxRate", "WebsiteUrl" },
                values: new object[] { new DateTime(2025, 3, 30, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), 0m, null, 0m, null });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DeliveryFee", "LocationUrl", "TaxRate", "WebsiteUrl" },
                values: new object[] { new DateTime(2025, 4, 4, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997), 0m, null, 0m, null });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 15, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryFee",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "LocationUrl",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "Restaurants");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 24, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 2, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "03ad1dd9-65f7-433b-9d8d-bf49a8d8644c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "05df9e5e-19d8-40ac-a2f0-1a3b25d0b54d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "97abf9cc-a218-4666-8d13-4c8515184262");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "01c4c771-9f0d-4d5d-8b6b-6570d8dfe219");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "75bed3a0-54b6-4f6f-b608-a26f5cc711bd", new DateTime(2025, 4, 22, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848), "AQAAAAIAAYagAAAAEEufEAKe87bapu+THOuwD7Kxt+VexLE4v4OlYYZUCaQoXZWPR7COBMc/b50OFYgnpA==", "cb5c8c43-6b2e-4550-a1fd-e492864d64c3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d861c865-68f1-4f28-b684-60195e740429", new DateTime(2025, 4, 22, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848), "AQAAAAIAAYagAAAAEKNuMEVoZc6M7m2lZuPlUN4QGnf93Lg43/m3HqBCvju7SkgZWk5Z0r2b50+jhrUDbQ==", "aebde59a-5305-42be-bd32-67552db6ec6d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9b3c3fbe-75b9-43a5-9c9c-339e8ca69a88", new DateTime(2025, 4, 22, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848), "AQAAAAIAAYagAAAAEFcM/SSXBN3ftrNHyFlIfbcmsco5nNx6vXaO0rTGDzrNOlGpkJQJbaHp6909MQ3OhA==", "df402ee5-10e6-4b40-9caf-8ccc5ab119f8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5f239426-559b-4181-911d-5c26cac444cd", new DateTime(2025, 4, 22, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848), "AQAAAAIAAYagAAAAEBNCwNFV8/yYFy3oKm8q7VHNCkaW7BUFWF6Qk8wILkwVgXq6Sddbt33w8qVmb4ieaQ==", "7f42504d-6088-4566-b6c1-cfb0e6c87356" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 23, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848), new DateTime(2025, 4, 22, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848) });

            migrationBuilder.UpdateData(
                table: "EmployeeProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HireDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 22, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848), new DateTime(2024, 10, 22, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848), new DateTime(2025, 4, 22, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848) });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 24, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 24, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 25, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 12, 21, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 17, 21, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 22, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848), new DateTime(2025, 4, 12, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 7, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848), new DateTime(2025, 4, 17, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 23, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 28, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 2, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 18, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848));
        }
    }
}
