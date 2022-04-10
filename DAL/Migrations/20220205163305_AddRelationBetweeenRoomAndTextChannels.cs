using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddRelationBetweeenRoomAndTextChannels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "TextChannels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TextChannels_RoomId",
                table: "TextChannels",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextChannels_Rooms_RoomId",
                table: "TextChannels",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextChannels_Rooms_RoomId",
                table: "TextChannels");

            migrationBuilder.DropIndex(
                name: "IX_TextChannels_RoomId",
                table: "TextChannels");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "TextChannels");
        }
    }
}
