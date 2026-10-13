using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GDG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateMemberAdditionalTeamsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Teams_TeamId",
                table: "Members");

            migrationBuilder.CreateTable(
                name: "MemberAdditionalTeams",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "integer", nullable: false),
                    TeamId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberAdditionalTeams", x => new { x.MemberId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_MemberAdditionalTeams_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberAdditionalTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberAdditionalTeams_TeamId",
                table: "MemberAdditionalTeams",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Teams_TeamId",
                table: "Members",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Teams_TeamId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "MemberAdditionalTeams");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Teams_TeamId",
                table: "Members",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
