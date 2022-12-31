using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsGame.Data.Migrations
{
    public partial class AddedPollVotingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Votes",
                table: "LetsGamePollOptions");

            migrationBuilder.CreateTable(
                name: "LetsGamePollVotes",
                columns: table => new
                {
                    VoterID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PollID = table.Column<long>(type: "bigint", nullable: false),
                    PollOptionID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetsGamePollVotes", x => new { x.VoterID, x.PollID });
                    table.ForeignKey(
                        name: "FK_LetsGamePollVotes_AspNetUsers_VoterID",
                        column: x => x.VoterID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LetsGamePollVotes_LetsGamePollOptions_PollOptionID",
                        column: x => x.PollOptionID,
                        principalTable: "LetsGamePollOptions",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LetsGamePollVotes_LetsGamePolls_PollID",
                        column: x => x.PollID,
                        principalTable: "LetsGamePolls",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LetsGamePollVotes_PollID",
                table: "LetsGamePollVotes",
                column: "PollID");

            migrationBuilder.CreateIndex(
                name: "IX_LetsGamePollVotes_PollOptionID",
                table: "LetsGamePollVotes",
                column: "PollOptionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LetsGamePollVotes");

            migrationBuilder.AddColumn<int>(
                name: "Votes",
                table: "LetsGamePollOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
