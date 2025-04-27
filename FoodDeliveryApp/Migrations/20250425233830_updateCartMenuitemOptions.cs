using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class updateCartMenuitemOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomizationOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowMultiple = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomizationChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CustomizationOptionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizationChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomizationChoices_CustomizationOptions_CustomizationOptio~",
                        column: x => x.CustomizationOptionId,
                        principalTable: "CustomizationOptions",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_CustomizationChoices_CustomizationOptionId",
                table: "CustomizationChoices",
                column: "CustomizationOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizationOptions_MenuItemId",
                table: "CustomizationOptions",
                column: "MenuItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomizationChoices");

            migrationBuilder.DropTable(
                name: "CustomizationOptions");

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
                column: "CreatedAt",
                value: new DateTime(2025, 3, 25, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 30, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 4, 13, 35, 56, 618, DateTimeKind.Utc).AddTicks(8997));

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
    }
}
