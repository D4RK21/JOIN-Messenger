using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddRelationBetweenPersonalChatAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersPersonalChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    PersonalChatId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersPersonalChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersPersonalChats_PersonalChats_PersonalChatId",
                        column: x => x.PersonalChatId,
                        principalTable: "PersonalChats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersPersonalChats_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersPersonalChats_PersonalChatId",
                table: "UsersPersonalChats",
                column: "PersonalChatId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersPersonalChats_UserId",
                table: "UsersPersonalChats",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersPersonalChats");
        }
    }
}
