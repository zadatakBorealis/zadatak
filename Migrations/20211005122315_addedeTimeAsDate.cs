using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Borealis.Migrations
{
    public partial class addedeTimeAsDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeAsDate",
                table: "TopTimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeAsDate",
                table: "TopTimes");
        }
    }
}
