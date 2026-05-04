using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteTour.Migrations
{
    /// <inheritdoc />
    public partial class IntroduceRegionsAndTourDestinations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Destinations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Destinations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.Sql(@"
                INSERT INTO Regions(Name, Slug)
                SELECT DISTINCT d.Region,
                    LOWER(REPLACE(REPLACE(REPLACE(d.Region, N' ', N'-'), N'Đ', N'D'), N'đ', N'd'))
                FROM Destinations d
                WHERE d.Region IS NOT NULL AND LTRIM(RTRIM(d.Region)) <> ''
            ");

            migrationBuilder.Sql(@"
                UPDATE d
                SET d.RegionId = r.Id
                FROM Destinations d
                JOIN Regions r ON r.Name = d.Region
            ");

            migrationBuilder.Sql(@"
                UPDATE d
                SET d.Slug = LOWER(REPLACE(REPLACE(REPLACE(d.Name, N' ', N'-'), N'Đ', N'D'), N'đ', N'd')) + N'-' + CAST(d.Id AS NVARCHAR(20))
                FROM Destinations d
            ");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Destinations");

            migrationBuilder.CreateTable(
                name: "TourDestinations",
                columns: table => new
                {
                    TourId = table.Column<int>(type: "int", nullable: false),
                    DestinationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourDestinations", x => new { x.TourId, x.DestinationId });
                    table.ForeignKey(
                        name: "FK_TourDestinations_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TourDestinations_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_RegionId",
                table: "Destinations",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_Slug",
                table: "Destinations",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Slug",
                table: "Regions",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourDestinations_DestinationId",
                table: "TourDestinations",
                column: "DestinationId");

            migrationBuilder.Sql(@"
                INSERT INTO TourDestinations(TourId, DestinationId)
                SELECT Id, DestinationId
                FROM Tours
                WHERE DestinationId IS NOT NULL
            ");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Regions_RegionId",
                table: "Destinations",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Regions_RegionId",
                table: "Destinations");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "TourDestinations");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_RegionId",
                table: "Destinations");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_Slug",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Destinations");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
