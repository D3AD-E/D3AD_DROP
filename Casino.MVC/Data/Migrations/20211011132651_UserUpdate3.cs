using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CasinoMVC.Migrations
{
    public partial class UserUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecentPlayerItems_AspNetUsers_UserId",
                table: "RecentPlayerItems");

            migrationBuilder.DropIndex(
                name: "IX_RecentPlayerItems_UserId",
                table: "RecentPlayerItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "RecentPlayerItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "RecentPlayerItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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
    }
}
