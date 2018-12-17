using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PremierLeagueAPI.Migrations
{
    public partial class AddCreatedAtToSquadPlayerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SquadPlayer_Players_PlayerId",
                table: "SquadPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_SquadPlayer_Squad_SquadId",
                table: "SquadPlayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SquadPlayer",
                table: "SquadPlayer");

            migrationBuilder.RenameTable(
                name: "SquadPlayer",
                newName: "SquadPlayers");

            migrationBuilder.RenameIndex(
                name: "IX_SquadPlayer_PlayerId",
                table: "SquadPlayers",
                newName: "IX_SquadPlayers_PlayerId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SquadPlayers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SquadPlayers",
                table: "SquadPlayers",
                columns: new[] { "SquadId", "PlayerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SquadPlayers_Players_PlayerId",
                table: "SquadPlayers",
                column: "PlayerId",
                principalTable: "Players",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SquadPlayers_Players_PlayerId",
                table: "SquadPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_SquadPlayers_Squad_SquadId",
                table: "SquadPlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SquadPlayers",
                table: "SquadPlayers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SquadPlayers");

            migrationBuilder.RenameTable(
                name: "SquadPlayers",
                newName: "SquadPlayer");

            migrationBuilder.RenameIndex(
                name: "IX_SquadPlayers_PlayerId",
                table: "SquadPlayer",
                newName: "IX_SquadPlayer_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SquadPlayer",
                table: "SquadPlayer",
                columns: new[] { "SquadId", "PlayerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SquadPlayer_Players_PlayerId",
                table: "SquadPlayer",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SquadPlayer_Squad_SquadId",
                table: "SquadPlayer",
                column: "SquadId",
                principalTable: "Squad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
