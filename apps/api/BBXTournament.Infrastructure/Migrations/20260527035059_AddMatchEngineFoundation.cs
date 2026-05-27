using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BBXTournament.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchEngineFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_TournamentParticipants_LoserId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_TournamentParticipants_WinnerId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "WinnerId",
                table: "Matches",
                newName: "WinnerParticipantId");

            migrationBuilder.RenameColumn(
                name: "Score2",
                table: "Matches",
                newName: "Player2Score");

            migrationBuilder.RenameColumn(
                name: "Score1",
                table: "Matches",
                newName: "Player1Score");

            migrationBuilder.RenameColumn(
                name: "LoserId",
                table: "Matches",
                newName: "LoserParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_WinnerId",
                table: "Matches",
                newName: "IX_Matches_WinnerParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_LoserId",
                table: "Matches",
                newName: "IX_Matches_LoserParticipantId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Matches",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Matches",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "JudgeUserId",
                table: "Matches",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TournamentRoundId",
                table: "Matches",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TournamentRounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TournamentStageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoundNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentRounds_TournamentStages_TournamentStageId",
                        column: x => x.TournamentStageId,
                        principalTable: "TournamentStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_JudgeUserId",
                table: "Matches",
                column: "JudgeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentRoundId",
                table: "Matches",
                column: "TournamentRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRounds_TournamentStageId",
                table: "TournamentRounds",
                column: "TournamentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRounds_TournamentStageId_RoundNumber",
                table: "TournamentRounds",
                columns: new[] { "TournamentStageId", "RoundNumber" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_TournamentParticipants_LoserParticipantId",
                table: "Matches",
                column: "LoserParticipantId",
                principalTable: "TournamentParticipants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_TournamentParticipants_WinnerParticipantId",
                table: "Matches",
                column: "WinnerParticipantId",
                principalTable: "TournamentParticipants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_TournamentRounds_TournamentRoundId",
                table: "Matches",
                column: "TournamentRoundId",
                principalTable: "TournamentRounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_JudgeUserId",
                table: "Matches",
                column: "JudgeUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_TournamentParticipants_LoserParticipantId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_TournamentParticipants_WinnerParticipantId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_TournamentRounds_TournamentRoundId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_JudgeUserId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "TournamentRounds");

            migrationBuilder.DropIndex(
                name: "IX_Matches_JudgeUserId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_TournamentRoundId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "JudgeUserId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TournamentRoundId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "WinnerParticipantId",
                table: "Matches",
                newName: "WinnerId");

            migrationBuilder.RenameColumn(
                name: "Player2Score",
                table: "Matches",
                newName: "Score2");

            migrationBuilder.RenameColumn(
                name: "Player1Score",
                table: "Matches",
                newName: "Score1");

            migrationBuilder.RenameColumn(
                name: "LoserParticipantId",
                table: "Matches",
                newName: "LoserId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_WinnerParticipantId",
                table: "Matches",
                newName: "IX_Matches_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_LoserParticipantId",
                table: "Matches",
                newName: "IX_Matches_LoserId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Matches",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_TournamentParticipants_LoserId",
                table: "Matches",
                column: "LoserId",
                principalTable: "TournamentParticipants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_TournamentParticipants_WinnerId",
                table: "Matches",
                column: "WinnerId",
                principalTable: "TournamentParticipants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
