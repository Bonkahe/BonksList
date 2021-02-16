using Microsoft.EntityFrameworkCore.Migrations;

namespace BonksList.Data.Migrations
{
    public partial class UpdateListings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "accountId",
                table: "Listing",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accountId",
                table: "Listing");
        }
    }
}
