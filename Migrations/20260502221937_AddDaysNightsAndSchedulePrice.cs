using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteTour.Migrations
{
    /// <inheritdoc />
    public partial class AddDaysNightsAndSchedulePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TourSchedules",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Nights",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "TourSchedules");

            migrationBuilder.DropColumn(
                name: "Days",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Nights",
                table: "Tours");
        }
    }
}
