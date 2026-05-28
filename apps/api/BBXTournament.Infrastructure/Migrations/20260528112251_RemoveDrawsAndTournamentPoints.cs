using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BBXTournament.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDrawsAndTournamentPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchDraws",
                table: "TournamentParticipants");

            migrationBuilder.DropColumn(
                name: "TournamentPoints",
                table: "TournamentParticipants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchDraws",
                table: "TournamentParticipants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TournamentPoints",
                table: "TournamentParticipants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
