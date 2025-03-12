using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Draftboard.Api.Migrations
{
    public partial class ExpandedTeamInfo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "FantasyTeams",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "FantasyTeams");
        }
    }
}
