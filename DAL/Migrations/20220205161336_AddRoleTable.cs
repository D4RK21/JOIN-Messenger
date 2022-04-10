using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddRoleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanPin = table.Column<bool>(type: "bit", nullable: false),
                    CanInvite = table.Column<bool>(type: "bit", nullable: false),
                    CanDeleteOthersMessages = table.Column<bool>(type: "bit", nullable: false),
                    CanModerateParticipants = table.Column<bool>(type: "bit", nullable: false),
                    CanManageRoles = table.Column<bool>(type: "bit", nullable: false),
                    CanManageChannels = table.Column<bool>(type: "bit", nullable: false),
                    CanManageRoom = table.Column<bool>(type: "bit", nullable: false),
                    CanUseAdminChannels = table.Column<bool>(type: "bit", nullable: false),
                    CanViewAuditLog = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
