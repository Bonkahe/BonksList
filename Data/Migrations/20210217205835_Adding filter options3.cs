using Microsoft.EntityFrameworkCore.Migrations;

namespace BonksList.Data.Migrations
{
    public partial class Addingfilteroptions3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "listingType",
                table: "Listing",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(24)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "listingType",
                table: "Listing",
                type: "nvarchar(24)",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
