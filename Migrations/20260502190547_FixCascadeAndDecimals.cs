using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteTour.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeAndDecimals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourDestinations_Destinations_DestinationId",
                table: "TourDestinations");

            migrationBuilder.AddForeignKey(
                name: "FK_TourDestinations_Destinations_DestinationId",
                table: "TourDestinations",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourDestinations_Destinations_DestinationId",
                table: "TourDestinations");

            migrationBuilder.AddForeignKey(
                name: "FK_TourDestinations_Destinations_DestinationId",
                table: "TourDestinations",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
