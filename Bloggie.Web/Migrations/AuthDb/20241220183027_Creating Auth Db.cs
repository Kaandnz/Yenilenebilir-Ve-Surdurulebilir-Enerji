using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Web.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class CreatingAuthDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "472ba632-6133-44a1-b158-6c10bd7d850d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "461a836e-f7d5-40c2-8089-7c6ec83b35f3", "AQAAAAIAAYagAAAAEBHwxy5fV+KdCF7bOHjp8IJi7FpM1I37Ye1xfwozyRoShdxIHxTEwMTmexzXN211Lw==", "193f84e7-6df5-4a6f-abd5-c212ac8a7faa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "472ba632-6133-44a1-b158-6c10bd7d850d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac4ad0a2-7450-43bb-9347-a176d54e679b", "AQAAAAIAAYagAAAAEGw75VKyDa921EMYCF6oA294QICheVNpsQ6M0HH7ubernMKdm9YWlHCRLbrhlG4pTQ==", "1b6dd7b4-7511-4909-8dc5-556f6ee40dfa" });
        }
    }
}
