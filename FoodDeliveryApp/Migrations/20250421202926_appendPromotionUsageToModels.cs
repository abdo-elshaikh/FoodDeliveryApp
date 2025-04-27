using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class appendPromotionUsageToModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromotionUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PromotionId = table.Column<int>(type: "int", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionUsages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionUsages_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionUsages_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2025, 4, 11, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2025, 4, 16, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 24, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 20, 29, 23, 924, DateTimeKind.Utc).AddTicks(9384));

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

            migrationBuilder.CreateIndex(
                name: "IX_PromotionUsages_OrderId",
                table: "PromotionUsages",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionUsages_PromotionId",
                table: "PromotionUsages",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionUsages_UserId",
                table: "PromotionUsages",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionUsages");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 21, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 30, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

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
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 20, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), new DateTime(2025, 4, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786) });

            migrationBuilder.UpdateData(
                table: "EmployeeProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HireDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), new DateTime(2024, 10, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786), new DateTime(2025, 4, 19, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786) });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 21, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 21, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 26, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 26, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 31, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 31, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2025, 4, 9, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2025, 4, 14, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 22, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 5, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 9, 15, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 14, 15, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

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
                column: "CreatedAt",
                value: new DateTime(2025, 3, 20, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 25, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 30, 14, 11, 57, 422, DateTimeKind.Utc).AddTicks(3786));

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
        }
    }
}
