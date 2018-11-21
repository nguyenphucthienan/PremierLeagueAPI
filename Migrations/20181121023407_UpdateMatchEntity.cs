using Microsoft.EntityFrameworkCore.Migrations;

namespace PremierLeagueAPI.Migrations
{
    public partial class UpdateMatchEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwayKitId",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeKitId",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayKitId",
                table: "Matches",
                column: "AwayKitId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeKitId",
                table: "Matches",
                column: "HomeKitId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Kits_AwayKitId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Kits_HomeKitId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_AwayKitId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_HomeKitId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "AwayKitId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HomeKitId",
                table: "Matches");
        }
    }
}
