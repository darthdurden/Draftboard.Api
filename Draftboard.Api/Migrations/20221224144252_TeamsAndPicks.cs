using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Draftboard.Api.Migrations
{
    public partial class TeamsAndPicks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FantasyTeams",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FantasyTeams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FantasyTeamPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    FantasyTeamId = table.Column<string>(type: "varchar(255)", nullable: true),
                    Contract = table.Column<string>(type: "longtext", nullable: true),
                    Salary = table.Column<float>(type: "float", nullable: false),
                    PickNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FantasyTeamPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FantasyTeamPlayers_FantasyTeams_FantasyTeamId",
                        column: x => x.FantasyTeamId,
                        principalTable: "FantasyTeams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FantasyTeamPlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FantasyTeamPlayers_FantasyTeamId",
                table: "FantasyTeamPlayers",
                column: "FantasyTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_FantasyTeamPlayers_PlayerId",
                table: "FantasyTeamPlayers",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FantasyTeamPlayers");

            migrationBuilder.DropTable(
                name: "FantasyTeams");
        }
    }
}
