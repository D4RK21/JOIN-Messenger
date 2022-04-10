using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddRelationBetweeenRoomAndParticipantInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "ParticipantInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantInfos_RoomId",
                table: "ParticipantInfos",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantInfos_Rooms_RoomId",
                table: "ParticipantInfos",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantInfos_Rooms_RoomId",
                table: "ParticipantInfos");

            migrationBuilder.DropIndex(
                name: "IX_ParticipantInfos_RoomId",
                table: "ParticipantInfos");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "ParticipantInfos");
        }
    }
}
