using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CasinoMVC.Migrations
{
    public partial class UserUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DotaItems_AspNetUsers_ApplicationUserId",
                table: "DotaItems");

            migrationBuilder.DropIndex(
                name: "IX_DotaItems_ApplicationUserId",
                table: "DotaItems");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "DotaItems");

            migrationBuilder.AddColumn<string>(
                name: "OwnedItemIds",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnedItemIds",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "DotaItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DotaItems_ApplicationUserId",
                table: "DotaItems",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DotaItems_AspNetUsers_ApplicationUserId",
                table: "DotaItems",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
