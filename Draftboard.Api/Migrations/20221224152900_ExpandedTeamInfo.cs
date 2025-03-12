using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Draftboard.Api.Migrations
{
    public partial class ExpandedTeamInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DraftCashAdjustment",
                table: "FantasyTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FantasyTeams",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DraftCashAdjustment",
                table: "FantasyTeams");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FantasyTeams");
        }
    }
}
