using Microsoft.EntityFrameworkCore.Migrations;

namespace CasinoMVC.Migrations
{
    public partial class UserChestAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OpenedChestAmount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenedChestAmount",
                table: "AspNetUsers");
        }
    }
}
