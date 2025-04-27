using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartItemsModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Restaurants_RestaurantId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderRestaurants");

            migrationBuilder.DropIndex(
                name: "IX_Carts_RestaurantId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Carts");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryAddressId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CartItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Customizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customizations_CustomizationOptions_OptionId",
                        column: x => x.OptionId,
                        principalTable: "CustomizationOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderCustomizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    ChoiceId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCustomizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderCustomizations_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                column: "CreatedAt",
                value: new DateTime(2025, 3, 28, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 2, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 20, 34, 58, 954, DateTimeKind.Utc).AddTicks(7774));

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

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

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
                name: "IX_OrderCustomizations_OrderItemId",
                table: "OrderCustomizations",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Customizations");

            migrationBuilder.DropTable(
                name: "OrderCustomizations");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryAddressId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
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

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "CartItems",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

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
                value: new DateTime(2025, 3, 29, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "e8f2a470-82ba-4fa7-81df-1a0aec1de379");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "cbef4a98-c38b-4114-be21-fe4a0faafb13");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "0b64c29e-15a1-4e7a-bcc0-3234e3c59684");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "1ddf5171-fa74-4ef9-8569-dac9515bd2cc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "328488d3-e0ce-412d-870d-3dcac15c4e13", new DateTime(2025, 4, 27, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), "AQAAAAIAAYagAAAAECuy7xblQe+m0Zeu0By5oOTKYEo5KvqvR7DdNxnVJvtqmbjdK+I5rbhpDCP+xHewsg==", "b986ce72-6c6c-421e-ab8a-4a1a563f4d8b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b8cb313-d12d-41a2-8539-cfb665cc819a", new DateTime(2025, 4, 27, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), "AQAAAAIAAYagAAAAENlLULLh/2x7jxCQt5z2A0yZkPjF1BQl9NdEVscFNqJcoLIf717tL//5CAVumgT98w==", "b33eea08-cc16-48ad-a479-5edb08444385" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "64d39cab-94de-4afa-93ca-190ba7559706", new DateTime(2025, 4, 27, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), "AQAAAAIAAYagAAAAEOLtCLf9yV2bGgLlRmlVhIpnrPgOLCEhYdZGJnv9KZeDa1wp+gEGQ7jNkGvSy1c3wg==", "3f283c68-69a9-4020-8835-fd24fa4beafe" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "07ca70dd-5e6e-4343-abcc-d3fe1bcad5b7", new DateTime(2025, 4, 27, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), "AQAAAAIAAYagAAAAEEgpzZSHlkvt2Pi91DOLD8lA2q2sZJLTbP9fGGlYTwLemnTsDaTljgs4LKSVw7YxGQ==", "b271635e-c499-4f92-a898-91d8fb0843c8" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 28, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), new DateTime(2025, 4, 27, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696) });

            migrationBuilder.UpdateData(
                table: "EmployeeProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HireDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 27, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), new DateTime(2024, 10, 27, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), new DateTime(2025, 4, 27, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696) });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 30, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 17, 18, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 22, 18, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 27, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), new DateTime(2025, 4, 17, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 12, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), new DateTime(2025, 4, 22, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 28, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 2, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 18, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 23, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.CreateIndex(
                name: "IX_Carts_RestaurantId",
                table: "Carts",
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
                name: "FK_Carts_Restaurants_RestaurantId",
                table: "Carts",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
