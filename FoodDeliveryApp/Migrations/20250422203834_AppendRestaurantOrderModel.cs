using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class AppendRestaurantOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomerProfiles_CustomerProfileId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_EmployeeProfiles_DeliveryEmployeeId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_CustomerProfiles_CustomerProfileId",
                table: "PaymentMethods");

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryInstructions",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Orders",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "DeliveryEmployeeId",
                table: "Orders",
                newName: "EmployeeProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryEmployeeId",
                table: "Orders",
                newName: "IX_Orders_EmployeeProfileId");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "OrderItems",
                newName: "Price");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerProfileId",
                table: "PaymentMethods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PaymentMethods",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerProfileId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PaymentDetails",
                table: "Orders",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodType",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SpecialInstructions",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "SpecialInstructions",
                keyValue: null,
                column: "SpecialInstructions",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialInstructions",
                table: "OrderItems",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderRestaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRestaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderRestaurants_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderRestaurants_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RestaurantId", "SpecialInstructions" },
                values: new object[] { 1, "No cheese" });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "RestaurantId", "SpecialInstructions" },
                values: new object[] { 2, "Extra cheese, no beans" });

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "CustomerProfileId", "UserId" },
                values: new object[] { new DateTime(2025, 3, 25, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848), null, "2" });

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "CustomerProfileId", "UserId" },
                values: new object[] { new DateTime(2025, 4, 8, 20, 38, 31, 713, DateTimeKind.Utc).AddTicks(9848), null, "2" });

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

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_UserId",
                table: "PaymentMethods",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_RestaurantId",
                table: "OrderItems",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRestaurants_OrderId",
                table: "OrderRestaurants",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRestaurants_RestaurantId",
                table: "OrderRestaurants",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Restaurants_RestaurantId",
                table: "OrderItems",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomerProfiles_CustomerProfileId",
                table: "Orders",
                column: "CustomerProfileId",
                principalTable: "CustomerProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_EmployeeProfiles_EmployeeProfileId",
                table: "Orders",
                column: "EmployeeProfileId",
                principalTable: "EmployeeProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_AspNetUsers_UserId",
                table: "PaymentMethods",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_CustomerProfiles_CustomerProfileId",
                table: "PaymentMethods",
                column: "CustomerProfileId",
                principalTable: "CustomerProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Restaurants_RestaurantId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomerProfiles_CustomerProfileId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_EmployeeProfiles_EmployeeProfileId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_AspNetUsers_UserId",
                table: "PaymentMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_CustomerProfiles_CustomerProfileId",
                table: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "OrderRestaurants");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_UserId",
                table: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_RestaurantId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentDetails",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethodType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SpecialInstructions",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Orders",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "EmployeeProfileId",
                table: "Orders",
                newName: "DeliveryEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_EmployeeProfileId",
                table: "Orders",
                newName: "IX_Orders_DeliveryEmployeeId");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderItems",
                newName: "UnitPrice");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerProfileId",
                table: "PaymentMethods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerProfileId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Orders",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryInstructions",
                table: "Orders",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialInstructions",
                table: "OrderItems",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 23, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 1, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "6571710b-6b63-4ce3-86a4-cb4c5d5e4a05");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "587a12a0-57d9-40c9-bda9-62e80103c0e1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "0aefb3fc-957e-4cda-ad80-b244742b02bd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "c68b0920-e2f2-43a6-884e-d109f05238a5");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "de60fa9a-b4a3-421d-a9d9-efa698d6a0e5", new DateTime(2025, 4, 21, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), "AQAAAAIAAYagAAAAEA+i49GP55vEPNms08P1sYyYpCuzyBp8zrXcS36gLZT3WlWnQT5y/2FTYYVLDWnOEQ==", "3aefcd10-98ab-4f47-baab-f8db58b5329c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7739709a-17b5-4d20-9f70-aaff5c4bf421", new DateTime(2025, 4, 21, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), "AQAAAAIAAYagAAAAEJN0ekxaXFQYBa1B89rHZz0ds+b/VV/HXx6wBRSXGtoD3J5jOxKXjAMbBx7t0vOQzw==", "41e0a66e-7284-491e-a7b0-6cee4678bc06" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "96b2d605-331f-4f56-88f4-80f9b25c0b9d", new DateTime(2025, 4, 21, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), "AQAAAAIAAYagAAAAEH8S3dkPpweJEy/3bdIZ3YxRjRyFlQZa0y20jBqUcl7c/8cnI6cHvun8bBegXsDhJw==", "fcc9f471-9a80-4ca2-baf9-34b0cbe5c24a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "916c6970-3780-44fc-94ac-b47ad02f6ee2", new DateTime(2025, 4, 21, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), "AQAAAAIAAYagAAAAEL6znXwKhXBkGXczkkX7gXIA2ZhBZflEC/4IqCyRU2t+wVSROALpcTmDFXYQxAAVUw==", "f71d1cb5-4163-48bb-a95c-50a3d2961101" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 22, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), new DateTime(2025, 4, 21, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384) });

            migrationBuilder.UpdateData(
                table: "EmployeeProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HireDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 21, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), new DateTime(2024, 10, 21, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), new DateTime(2025, 4, 21, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384) });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 23, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 23, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 28, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 28, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 2, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 2, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Discount", "SpecialInstructions" },
                values: new object[] { 0m, "Extra cheese please" });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Discount", "SpecialInstructions" },
                values: new object[] { 0m, null });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerProfileId", "DeliveryDate", "DeliveryEmployeeId", "DeliveryFee", "DeliveryInstructions", "OrderDate", "RestaurantId", "Status", "Subtotal", "Tax", "TotalAmount" },
                values: new object[,]
                {
                    { 1, 1, null, 1, 4.99m, "Ring the bell twice", new DateTime(2025, 4, 11, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), 1, 5, 0m, 2.50m, 22.48m },
                    { 2, 1, null, 1, 3.99m, null, new DateTime(2025, 4, 16, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), 2, 5, 0m, 1.80m, 17.78m }
                });

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "CustomerProfileId" },
                values: new object[] { new DateTime(2025, 3, 24, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), 1 });

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "CustomerProfileId" },
                values: new object[] { new DateTime(2025, 4, 7, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), 1 });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 11, 21, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 16, 21, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 21, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), new DateTime(2025, 4, 11, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 6, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384), new DateTime(2025, 4, 16, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 22, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 27, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 1, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 17, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomerProfiles_CustomerProfileId",
                table: "Orders",
                column: "CustomerProfileId",
                principalTable: "CustomerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_EmployeeProfiles_DeliveryEmployeeId",
                table: "Orders",
                column: "DeliveryEmployeeId",
                principalTable: "EmployeeProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_CustomerProfiles_CustomerProfileId",
                table: "PaymentMethods",
                column: "CustomerProfileId",
                principalTable: "CustomerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
