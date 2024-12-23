using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Web.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class AdminChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37cc67e1-41ca-461c-bf34-2b5e62dbae32",
                column: "NormalizedName",
                value: "ADMIN");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3cfd9eee-08cb-4da3-9e6f-c3166b50d3b0",
                column: "NormalizedName",
                value: "SUPERADMIN");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0cab2c3-6558-4a1c-be81-dfb39180da3d",
                column: "NormalizedName",
                value: "USER");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "472ba632-6133-44a1-b158-6c10bd7d850d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15fc714d-da77-450e-b5a2-538161125cc9", "AQAAAAIAAYagAAAAEEO8ngU1ISbR5Az6WWloV39o66RCXotGBUdsfO8T50dDMFfynv0KZ0VLLbZ0a79z+g==", "7eac558f-9dd6-4bae-9768-46694afdfd2e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37cc67e1-41ca-461c-bf34-2b5e62dbae32",
                column: "NormalizedName",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3cfd9eee-08cb-4da3-9e6f-c3166b50d3b0",
                column: "NormalizedName",
                value: "SuperAdmin");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0cab2c3-6558-4a1c-be81-dfb39180da3d",
                column: "NormalizedName",
                value: "User");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "472ba632-6133-44a1-b158-6c10bd7d850d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "461a836e-f7d5-40c2-8089-7c6ec83b35f3", "AQAAAAIAAYagAAAAEBHwxy5fV+KdCF7bOHjp8IJi7FpM1I37Ye1xfwozyRoShdxIHxTEwMTmexzXN211Lw==", "193f84e7-6df5-4a6f-abd5-c212ac8a7faa" });
        }
    }
}
