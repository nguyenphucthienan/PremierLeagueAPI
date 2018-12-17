using Microsoft.EntityFrameworkCore.Migrations;

namespace PremierLeagueAPI.Migrations
{
    public partial class UpdateEntityNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kits_Squad_SquadId",
                table: "Kits");

            migrationBuilder.DropForeignKey(
                name: "FK_SeasonClub_Clubs_ClubId",
                table: "SeasonClub");

            migrationBuilder.DropForeignKey(
                name: "FK_SeasonClub_Seasons_SeasonId",
                table: "SeasonClub");

            migrationBuilder.DropForeignKey(
                name: "FK_Squad_Clubs_ClubId",
                table: "Squad");

            migrationBuilder.DropForeignKey(
                name: "FK_Squad_Seasons_SeasonId",
                table: "Squad");

            migrationBuilder.DropForeignKey(
                name: "FK_SquadPlayers_Squad_SquadId",
                table: "SquadPlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Squad",
                table: "Squad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeasonClub",
                table: "SeasonClub");

            migrationBuilder.RenameTable(
                name: "Squad",
                newName: "Squads");

            migrationBuilder.RenameTable(
                name: "SeasonClub",
                newName: "SeasonClubs");

            migrationBuilder.RenameIndex(
                name: "IX_Squad_SeasonId",
                table: "Squads",
                newName: "IX_Squads_SeasonId");

            migrationBuilder.RenameIndex(
                name: "IX_Squad_ClubId",
                table: "Squads",
                newName: "IX_Squads_ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_SeasonClub_ClubId",
                table: "SeasonClubs",
                newName: "IX_SeasonClubs_ClubId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Squads",
                table: "Squads",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeasonClubs",
                table: "SeasonClubs",
                columns: new[] { "SeasonId", "ClubId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Kits_Squads_SquadId",
                table: "Kits",
                column: "SquadId",
                principalTable: "Squads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonClubs_Clubs_ClubId",
                table: "SeasonClubs",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonClubs_Seasons_SeasonId",
                table: "SeasonClubs",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SquadPlayers_Squads_SquadId",
                table: "SquadPlayers",
                column: "SquadId",
                principalTable: "Squads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Squads_Clubs_ClubId",
                table: "Squads",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Squads_Seasons_SeasonId",
                table: "Squads",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kits_Squads_SquadId",
                table: "Kits");

            migrationBuilder.DropForeignKey(
                name: "FK_SeasonClubs_Clubs_ClubId",
                table: "SeasonClubs");

            migrationBuilder.DropForeignKey(
                name: "FK_SeasonClubs_Seasons_SeasonId",
                table: "SeasonClubs");

            migrationBuilder.DropForeignKey(
                name: "FK_SquadPlayers_Squads_SquadId",
                table: "SquadPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_Squads_Clubs_ClubId",
                table: "Squads");

            migrationBuilder.DropForeignKey(
                name: "FK_Squads_Seasons_SeasonId",
                table: "Squads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Squads",
                table: "Squads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeasonClubs",
                table: "SeasonClubs");

            migrationBuilder.RenameTable(
                name: "Squads",
                newName: "Squad");

            migrationBuilder.RenameTable(
                name: "SeasonClubs",
                newName: "SeasonClub");

            migrationBuilder.RenameIndex(
                name: "IX_Squads_SeasonId",
                table: "Squad",
                newName: "IX_Squad_SeasonId");

            migrationBuilder.RenameIndex(
                name: "IX_Squads_ClubId",
                table: "Squad",
                newName: "IX_Squad_ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_SeasonClubs_ClubId",
                table: "SeasonClub",
                newName: "IX_SeasonClub_ClubId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Squad",
                table: "Squad",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeasonClub",
                table: "SeasonClub",
                columns: new[] { "SeasonId", "ClubId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Kits_Squad_SquadId",
                table: "Kits",
                column: "SquadId",
                principalTable: "Squad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonClub_Clubs_ClubId",
                table: "SeasonClub",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonClub_Seasons_SeasonId",
                table: "SeasonClub",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Squad_Clubs_ClubId",
                table: "Squad",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Squad_Seasons_SeasonId",
                table: "Squad",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SquadPlayers_Squad_SquadId",
                table: "SquadPlayers",
                column: "SquadId",
                principalTable: "Squad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
