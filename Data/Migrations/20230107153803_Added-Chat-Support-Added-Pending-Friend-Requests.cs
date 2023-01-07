using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsGame.Data.Migrations
{
    public partial class AddedChatSupportAddedPendingFriendRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPendingAccept",
                table: "LetsGameFriends",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "LetsGame_Chats",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetsGame_Chats", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LetsGame_ChatMessages",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatID = table.Column<long>(type: "bigint", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetsGame_ChatMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LetsGame_ChatMessages_LetsGame_Chats_ChatID",
                        column: x => x.ChatID,
                        principalTable: "LetsGame_Chats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LetsGame_UserChats",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChatID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetsGame_UserChats", x => new { x.UserID, x.ChatID });
                    table.ForeignKey(
                        name: "FK_LetsGame_UserChats_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LetsGame_UserChats_LetsGame_Chats_ChatID",
                        column: x => x.ChatID,
                        principalTable: "LetsGame_Chats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LetsGame_ChatMessages_ChatID",
                table: "LetsGame_ChatMessages",
                column: "ChatID");

            migrationBuilder.CreateIndex(
                name: "IX_LetsGame_UserChats_ChatID",
                table: "LetsGame_UserChats",
                column: "ChatID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LetsGame_ChatMessages");

            migrationBuilder.DropTable(
                name: "LetsGame_UserChats");

            migrationBuilder.DropTable(
                name: "LetsGame_Chats");

            migrationBuilder.DropColumn(
                name: "IsPendingAccept",
                table: "LetsGameFriends");
        }
    }
}
