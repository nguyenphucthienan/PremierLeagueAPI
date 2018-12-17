using Microsoft.EntityFrameworkCore.Migrations;

namespace PremierLeagueAPI.Migrations
{
    public partial class UpdateHomeClubKitAndAwayClubKit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Kits_AwayKitId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Kits_HomeKitId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "HomeKitId",
                table: "Matches",
                newName: "HomeClubKitId");

            migrationBuilder.RenameColumn(
                name: "AwayKitId",
                table: "Matches",
                newName: "AwayClubKitId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_HomeKitId",
                table: "Matches",
                newName: "IX_Matches_HomeClubKitId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_AwayKitId",
                table: "Matches",
                newName: "IX_Matches_AwayClubKitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Kits_AwayClubKitId",
                table: "Matches",
                column: "AwayClubKitId",
                principalTable: "Kits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Kits_HomeClubKitId",
                table: "Matches",
                column: "HomeClubKitId",
                principalTable: "Kits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Kits_AwayClubKitId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Kits_HomeClubKitId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "HomeClubKitId",
                table: "Matches",
                newName: "HomeKitId");

            migrationBuilder.RenameColumn(
                name: "AwayClubKitId",
                table: "Matches",
                newName: "AwayKitId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_HomeClubKitId",
                table: "Matches",
                newName: "IX_Matches_HomeKitId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_AwayClubKitId",
                table: "Matches",
                newName: "IX_Matches_AwayKitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Kits_AwayKitId",
                table: "Matches",
                column: "AwayKitId",
                principalTable: "Kits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Kits_HomeKitId",
                table: "Matches",
                column: "HomeKitId",
                principalTable: "Kits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
