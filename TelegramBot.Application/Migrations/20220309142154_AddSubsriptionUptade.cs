using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramBot.Application.Migrations
{
    public partial class AddSubsriptionUptade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeTables_Users_UserId",
                table: "TimeTables");

            migrationBuilder.DropIndex(
                name: "IX_TimeTables_UserId",
                table: "TimeTables");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TimeTables");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TimeTables",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_UserId",
                table: "TimeTables",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeTables_Users_UserId",
                table: "TimeTables",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
