using Microsoft.EntityFrameworkCore.Migrations;

namespace Borealis.Migrations
{
    public partial class addedIsReadyToDeleteToTopTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReadyToDelete",
                table: "TopTimes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReadyToDelete",
                table: "TopTimes");
        }
    }
}
