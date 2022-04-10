using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddRelationBetweenInviteLinkAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InviteLinksUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    InviteLinkId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InviteLinksUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InviteLinksUsers_InviteLinks_InviteLinkId",
                        column: x => x.InviteLinkId,
                        principalTable: "InviteLinks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InviteLinksUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InviteLinksUsers_InviteLinkId",
                table: "InviteLinksUsers",
                column: "InviteLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_InviteLinksUsers_UserId",
                table: "InviteLinksUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InviteLinksUsers");
        }
    }
}
