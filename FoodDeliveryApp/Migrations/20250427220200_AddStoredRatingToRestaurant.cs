using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredRatingToRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Restaurants",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "2ad256b5-1982-44c8-8eab-5b77e3e3c845");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "9c1e53b5-f042-4fdf-a087-6fed2e8514b4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "0ff02069-02f6-4507-ade2-4f6cbad8a9df");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "419cb23b-a550-442b-bd4e-04b2f9ea6e90");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3a50bcb5-cfda-42d4-a25f-805f8a583bb4", new DateTime(2025, 4, 27, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), "AQAAAAIAAYagAAAAEE1Sny3g3Ji2kx2PBn6MJptfdG4+V0gX7SOvyl5uxTz4uJ3cLeyOhF9LWhAGQ3iThQ==", "dc4286f1-6a3d-4500-8ab7-bf4134aafcd3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b8c50b8a-e1ef-42b6-9aa7-81cd6d5d18af", new DateTime(2025, 4, 27, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), "AQAAAAIAAYagAAAAEJC76ekXQMgU68nyOyyMquLBA19M2be9pNwfzO1pGMyFvuXSPxDviRY29EPUVhz2sw==", "9b443810-f729-4c53-b002-97a1bcb1f14d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c63cd1e2-c373-4050-bd20-5130a59409ea", new DateTime(2025, 4, 27, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), "AQAAAAIAAYagAAAAEDFEE4gunP/1A+NRx4wQxJeuriTdpKmv57qNcCG2aJpwD48+JIRgHiy4hcyUJGkWWA==", "5f1fad9e-c480-4701-a14f-eb8fd84075cd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cda6d085-4dfa-428b-a03b-7d5ae1e575f1", new DateTime(2025, 4, 27, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), "AQAAAAIAAYagAAAAEEvszjMJyyxWgtxEtaikM/nO5npZeYXgE4i2PDjk3eM2gt5+xyhnb8/sLeFslTGvKA==", "0efe198a-67f5-4afa-91ac-dc0e4f26875a" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 28, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), new DateTime(2025, 4, 27, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326) });

            migrationBuilder.UpdateData(
                table: "EmployeeProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HireDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 27, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), new DateTime(2024, 10, 27, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), new DateTime(2025, 4, 27, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326) });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 29, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 3, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 30, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 17, 23, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                column: "PaymentDate",
                value: new DateTime(2025, 4, 22, 23, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 27, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), new DateTime(2025, 4, 17, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 12, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), new DateTime(2025, 4, 22, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326) });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Rating" },
                values: new object[] { new DateTime(2025, 3, 28, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), 0m });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Rating" },
                values: new object[] { new DateTime(2025, 4, 2, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), 0m });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Rating" },
                values: new object[] { new DateTime(2025, 4, 7, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326), 0m });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 18, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 23, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 25, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 26, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 24, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 25, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 26, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 24, 22, 1, 54, 990, DateTimeKind.Utc).AddTicks(1326));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Restaurants");

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
                column: "CreatedAt",
                value: new DateTime(2025, 4, 18, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 23, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 25, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 26, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 24, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 25, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 26, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 24, 21, 50, 6, 943, DateTimeKind.Utc).AddTicks(3369));
        }
    }
}
