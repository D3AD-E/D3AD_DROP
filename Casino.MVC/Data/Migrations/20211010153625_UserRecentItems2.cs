using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CasinoMVC.Migrations
{
    public partial class UserRecentItems2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "RecentPlayerItems");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "RecentPlayerItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecentPlayerItems_UserId",
                table: "RecentPlayerItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecentPlayerItems_AspNetUsers_UserId",
                table: "RecentPlayerItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecentPlayerItems_AspNetUsers_UserId",
                table: "RecentPlayerItems");

            migrationBuilder.DropIndex(
                name: "IX_RecentPlayerItems_UserId",
                table: "RecentPlayerItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RecentPlayerItems");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "RecentPlayerItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
