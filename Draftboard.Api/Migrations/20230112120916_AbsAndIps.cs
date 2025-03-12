using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Draftboard.Api.Migrations
{
    public partial class AbsAndIps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AtBats",
                table: "PlayerStats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "InningsPitched",
                table: "PlayerStats",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AtBats",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "InningsPitched",
                table: "PlayerStats");
        }
    }
}
