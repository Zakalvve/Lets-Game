using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsGame.Data.Migrations
{
    public partial class AddUserFriendshipsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LetsGameFriends",
                columns: table => new
                {
                    RequesterID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddresseeID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetsGameFriends", x => new { x.RequesterID, x.AddresseeID });
                    table.ForeignKey(
                        name: "FK_LetsGameFriends_AspNetUsers_AddresseeID",
                        column: x => x.AddresseeID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LetsGameFriends_AspNetUsers_RequesterID",
                        column: x => x.RequesterID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LetsGameFriends_AddresseeID",
                table: "LetsGameFriends",
                column: "AddresseeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LetsGameFriends");
        }
    }
}
