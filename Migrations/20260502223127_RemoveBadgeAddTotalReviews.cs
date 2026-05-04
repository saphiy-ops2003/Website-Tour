using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteTour.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBadgeAddTotalReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Badge",
                table: "Tours");

            migrationBuilder.AddColumn<int>(
                name: "TotalReviews",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalReviews",
                table: "Tours");

            migrationBuilder.AddColumn<string>(
                name: "Badge",
                table: "Tours",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
