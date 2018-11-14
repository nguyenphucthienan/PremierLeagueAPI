﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace PremierLeagueAPI.Migrations
{
    public partial class AddStadiumToMatchEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StadiumId",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_StadiumId",
                table: "Matches",
                column: "StadiumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Stadiums_StadiumId",
                table: "Matches",
                column: "StadiumId",
                principalTable: "Stadiums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Stadiums_StadiumId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_StadiumId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "StadiumId",
                table: "Matches");
        }
    }
}
