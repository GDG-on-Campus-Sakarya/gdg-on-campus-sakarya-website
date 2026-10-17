using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GDG.Infrastructure.Migrations
{
    public partial class AddTeamLeaderToTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeaderMemberId",
                table: "Teams",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeaderMemberId",
                table: "Teams",
                column: "LeaderMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Members_LeaderMemberId",
                table: "Teams",
                column: "LeaderMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Members_LeaderMemberId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_LeaderMemberId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "LeaderMemberId",
                table: "Teams");
        }
    }
}
