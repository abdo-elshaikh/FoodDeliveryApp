using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCartModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantCategoryId",
                table: "MenuItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                columns: new[] { "CreatedAt", "RestaurantCategoryId" },
                values: new object[] { new DateTime(2025, 3, 29, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), null });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "RestaurantCategoryId" },
                values: new object[] { new DateTime(2025, 3, 29, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), null });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "RestaurantCategoryId" },
                values: new object[] { new DateTime(2025, 4, 3, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), null });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "RestaurantCategoryId" },
                values: new object[] { new DateTime(2025, 4, 3, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), null });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "RestaurantCategoryId" },
                values: new object[] { new DateTime(2025, 4, 8, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), null });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "RestaurantCategoryId" },
                values: new object[] { new DateTime(2025, 4, 8, 17, 52, 10, 502, DateTimeKind.Utc).AddTicks(9696), null });

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
                name: "IX_MenuItems_RestaurantCategoryId",
                table: "MenuItems",
                column: "RestaurantCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_MenuItemId",
                table: "CartItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_RestaurantId",
                table: "Carts",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_RestaurantCategories_RestaurantCategoryId",
                table: "MenuItems",
                column: "RestaurantCategoryId",
                principalTable: "RestaurantCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_RestaurantCategories_RestaurantCategoryId",
                table: "MenuItems");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_RestaurantCategoryId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "RestaurantCategoryId",
                table: "MenuItems");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 27, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 5, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "cdf96290-9d4c-4ce0-b0d4-a29738ae3b2b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "c2a26c88-c1b5-4538-b191-5906d3a9be53");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "7a0082ec-7e85-4ef9-b6df-37290bd339eb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "694de1f0-1a36-4c5f-9a51-987d87bf6e16");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "962416c7-e58e-49ce-be48-741265a8782e", new DateTime(2025, 4, 25, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273), "AQAAAAIAAYagAAAAEIdbcpYUGmuVG34NFVtC167rK9cL3qhrCooE18Q/uEYi0FKnxCm+soHMHEJhqBrapQ==", "6d9eb882-84f8-4561-a2d6-155a26f5e363" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cdc9c9ac-43f4-4b72-868e-2b13da6a4769", new DateTime(2025, 4, 25, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273), "AQAAAAIAAYagAAAAEAkqjZkCiKn/0nMAQwCqjnfycq4PMv/t/njCDWTe3tCIwCLssWQ2Vcu5r+OoMfaAHA==", "8ecc309a-b054-4260-a5c1-69f976a3a097" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "87c25fc1-bf1e-4564-879f-e73e9325a604", new DateTime(2025, 4, 25, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273), "AQAAAAIAAYagAAAAED7Vot7V/Moh0+yEvXWu2iGUAUWihGTAOqI223jqfGuZ/T6o8tiLakQV8c6XDoVP7A==", "ecabbe9c-f266-4a1a-8607-8e2738cf1f8e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a03c5a2e-b94d-43f5-a30b-3a75c92dbccc", new DateTime(2025, 4, 25, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273), "AQAAAAIAAYagAAAAEDhrFqQ4kYJp5r2gSyfdbH5xE4PaNE4cxob90zcnDhFa95Hw2NYeSn1CsBqfkUEV2A==", "4b9a2d8d-0168-4a0a-894d-6b5691fa5e49" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 26, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273), new DateTime(2025, 4, 25, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273) });

            migrationBuilder.UpdateData(
                table: "EmployeeProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HireDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 25, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273), new DateTime(2024, 10, 25, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273), new DateTime(2025, 4, 25, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273) });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 27, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 27, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 1, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 1, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 6, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 6, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 28, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 11, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 16, 0, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 21, 0, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 25, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273), new DateTime(2025, 4, 15, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 10, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273), new DateTime(2025, 4, 20, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 26, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 31, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 5, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 16, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 21, 23, 38, 28, 13, DateTimeKind.Utc).AddTicks(3273));
        }
    }
}
