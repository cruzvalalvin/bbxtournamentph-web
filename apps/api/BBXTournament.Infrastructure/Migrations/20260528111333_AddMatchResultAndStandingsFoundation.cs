using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BBXTournament.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchResultAndStandingsFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BuchholzScore",
                table: "TournamentParticipants",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MatchDraws",
                table: "TournamentParticipants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchLosses",
                table: "TournamentParticipants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchWins",
                table: "TournamentParticipants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PointsAgainst",
                table: "TournamentParticipants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PointsDifference",
                table: "TournamentParticipants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PointsScored",
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

            migrationBuilder.AddColumn<string>(
                name: "JudgeNotes",
                table: "Matches",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuchholzScore",
                table: "TournamentParticipants");

            migrationBuilder.DropColumn(
                name: "MatchDraws",
                table: "TournamentParticipants");

            migrationBuilder.DropColumn(
                name: "MatchLosses",
                table: "TournamentParticipants");

            migrationBuilder.DropColumn(
                name: "MatchWins",
                table: "TournamentParticipants");

            migrationBuilder.DropColumn(
                name: "PointsAgainst",
                table: "TournamentParticipants");

            migrationBuilder.DropColumn(
                name: "PointsDifference",
                table: "TournamentParticipants");

            migrationBuilder.DropColumn(
                name: "PointsScored",
                table: "TournamentParticipants");

            migrationBuilder.DropColumn(
                name: "TournamentPoints",
                table: "TournamentParticipants");

            migrationBuilder.DropColumn(
                name: "JudgeNotes",
                table: "Matches");
        }
    }
}
