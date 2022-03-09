using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramBot.Application.Migrations
{
    public partial class AddGroupForUsersNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Users",
                newName: "GroupsId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                newName: "IX_Users_GroupsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupsId",
                table: "Users",
                column: "GroupsId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupsId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "GroupsId",
                table: "Users",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GroupsId",
                table: "Users",
                newName: "IX_Users_GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
