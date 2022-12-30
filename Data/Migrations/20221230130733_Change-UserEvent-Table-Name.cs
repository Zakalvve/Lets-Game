using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsGame.Data.Migrations
{
    public partial class ChangeUserEventTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEvents_AspNetUsers_UserID",
                table: "UserEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvents_LetsGameEvents_EventID",
                table: "UserEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEvents",
                table: "UserEvents");

            migrationBuilder.RenameTable(
                name: "UserEvents",
                newName: "LetsGameUserEvents");

            migrationBuilder.RenameIndex(
                name: "IX_UserEvents_EventID",
                table: "LetsGameUserEvents",
                newName: "IX_LetsGameUserEvents_EventID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LetsGameUserEvents",
                table: "LetsGameUserEvents",
                columns: new[] { "UserID", "EventID" });

            migrationBuilder.AddForeignKey(
                name: "FK_LetsGameUserEvents_AspNetUsers_UserID",
                table: "LetsGameUserEvents",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LetsGameUserEvents_LetsGameEvents_EventID",
                table: "LetsGameUserEvents",
                column: "EventID",
                principalTable: "LetsGameEvents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LetsGameUserEvents_AspNetUsers_UserID",
                table: "LetsGameUserEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LetsGameUserEvents_LetsGameEvents_EventID",
                table: "LetsGameUserEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LetsGameUserEvents",
                table: "LetsGameUserEvents");

            migrationBuilder.RenameTable(
                name: "LetsGameUserEvents",
                newName: "UserEvents");

            migrationBuilder.RenameIndex(
                name: "IX_LetsGameUserEvents_EventID",
                table: "UserEvents",
                newName: "IX_UserEvents_EventID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEvents",
                table: "UserEvents",
                columns: new[] { "UserID", "EventID" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_AspNetUsers_UserID",
                table: "UserEvents",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_LetsGameEvents_EventID",
                table: "UserEvents",
                column: "EventID",
                principalTable: "LetsGameEvents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
