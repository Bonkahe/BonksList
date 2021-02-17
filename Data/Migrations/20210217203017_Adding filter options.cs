using Microsoft.EntityFrameworkCore.Migrations;

namespace BonksList.Data.Migrations
{
    public partial class Addingfilteroptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "listingType",
                table: "Listing",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "listingType",
                table: "Listing");

        }
    }
}
