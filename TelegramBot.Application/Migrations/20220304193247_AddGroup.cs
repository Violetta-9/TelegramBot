using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TelegramBot.Application.Migrations
{
    public partial class AddGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Week",
                table: "TimeTables",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdChat = table.Column<int>(type: "integer", nullable: false),
                    NameGroup = table.Column<string>(type: "text", nullable: true),
                    TimeTableId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_TimeTables_TimeTableId",
                        column: x => x.TimeTableId,
                        principalTable: "TimeTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TimeTableId",
                table: "Groups",
                column: "TimeTableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.AlterColumn<byte>(
                name: "Week",
                table: "TimeTables",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
