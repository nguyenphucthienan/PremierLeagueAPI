using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PremierLeagueAPI.Migrations
{
    public partial class AddManagerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Nationality = table.Column<string>(nullable: true),
                    Birthdate = table.Column<DateTime>(nullable: false),
                    PhotoUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SquadManagers",
                columns: table => new
                {
                    SquadId = table.Column<int>(nullable: false),
                    ManagerId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SquadManagers", x => new { x.SquadId, x.ManagerId });
                    table.ForeignKey(
                        name: "FK_SquadManagers_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SquadManagers_Squads_SquadId",
                        column: x => x.SquadId,
                        principalTable: "Squads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SquadManagers_ManagerId",
                table: "SquadManagers",
                column: "ManagerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SquadManagers");

            migrationBuilder.DropTable(
                name: "Managers");
        }
    }
}
