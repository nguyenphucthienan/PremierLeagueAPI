using Microsoft.EntityFrameworkCore.Migrations;

namespace PremierLeagueAPI.Migrations
{
    public partial class UpdateDeleteBehaviorGoalCardEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Matches_MatchId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Matches_MatchId",
                table: "Goals");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Players",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Players_Name",
                table: "Players",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Matches_MatchId",
                table: "Cards",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Matches_MatchId",
                table: "Goals",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Matches_MatchId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Matches_MatchId",
                table: "Goals");

            migrationBuilder.DropIndex(
                name: "IX_Players_Name",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Players",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Matches_MatchId",
                table: "Cards",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Matches_MatchId",
                table: "Goals",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
