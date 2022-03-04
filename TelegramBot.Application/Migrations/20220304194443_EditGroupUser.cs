using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramBot.Application.Migrations
{
    public partial class EditGroupUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_TimeTables_TimeTableId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TimeTableId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TimeTableId",
                table: "Groups");

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "TimeTables",
                type: "text",
                nullable: true);

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
                name: "FK_TimeTables_Groups_UserId",
                table: "TimeTables",
                column: "UserId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeTables_Groups_UserId",
                table: "TimeTables");

            migrationBuilder.DropIndex(
                name: "IX_TimeTables_UserId",
                table: "TimeTables");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "TimeTables");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TimeTables");

            migrationBuilder.AddColumn<int>(
                name: "TimeTableId",
                table: "Groups",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TimeTableId",
                table: "Groups",
                column: "TimeTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_TimeTables_TimeTableId",
                table: "Groups",
                column: "TimeTableId",
                principalTable: "TimeTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
