using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsGame.Data.Migrations
{
    public partial class ReworkEventAndPollModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameSubmissionsDeadline",
                table: "LetsGameEvents");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LetsGamePolls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PollStart",
                table: "LetsGamePolls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "LetsGamePolls");

            migrationBuilder.DropColumn(
                name: "PollStart",
                table: "LetsGamePolls");

            migrationBuilder.AddColumn<DateTime>(
                name: "GameSubmissionsDeadline",
                table: "LetsGameEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
