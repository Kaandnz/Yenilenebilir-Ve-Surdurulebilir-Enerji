using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Web.Migrations
{
    /// <inheritdoc />
    public partial class admintopicdetails1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TopicDetails",
                table: "Topics",
                newName: "TopicDetailsTr");

            migrationBuilder.AddColumn<string>(
                name: "TopicDetailsEn",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TopicDetailsEn",
                table: "Topics");

            migrationBuilder.RenameColumn(
                name: "TopicDetailsTr",
                table: "Topics",
                newName: "TopicDetails");
        }
    }
}
