using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddRelationsBetweenRoomAndRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseRoleId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BaseRoleId",
                table: "Rooms",
                column: "BaseRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoomId",
                table: "Roles",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Rooms_RoomId",
                table: "Roles",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Roles_BaseRoleId",
                table: "Rooms",
                column: "BaseRoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Rooms_RoomId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Roles_BaseRoleId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_BaseRoleId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RoomId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "BaseRoleId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Roles");
        }
    }
}
