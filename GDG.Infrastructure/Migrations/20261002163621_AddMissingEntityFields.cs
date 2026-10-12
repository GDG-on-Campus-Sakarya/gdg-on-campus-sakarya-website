using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GDG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingEntityFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorHex",
                table: "Technologies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconClass",
                table: "Technologies",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GitHubUrl",
                table: "Projects",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Projects",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Projects",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LiveDemoUrl",
                table: "Projects",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubUrl",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationUrl",
                table: "Events",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorHex",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "IconClass",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LiveDemoUrl",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "GitHubUrl",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RegistrationUrl",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "GitHubUrl",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
