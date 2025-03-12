using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Draftboard.Api.Migrations
{
    public partial class FantasyPitchingStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ERA",
                table: "PlayerStats",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "RPC",
                table: "PlayerStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SPC",
                table: "PlayerStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Strikeouts",
                table: "PlayerStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "WHIP",
                table: "PlayerStats",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ERA",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "RPC",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "SPC",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "Strikeouts",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "WHIP",
                table: "PlayerStats");
        }
    }
}
