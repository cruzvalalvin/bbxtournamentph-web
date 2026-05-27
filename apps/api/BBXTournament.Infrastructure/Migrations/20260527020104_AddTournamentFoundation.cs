using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BBXTournament.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTournamentFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CommunityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MaxParticipants = table.Column<int>(type: "INTEGER", nullable: false),
                    Region = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Province = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tournaments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TournamentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    TeamName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    IsManualEntry = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    Seed = table.Column<int>(type: "INTEGER", nullable: true),
                    CheckedIn = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentParticipants_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TournamentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    StageOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    FormatType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NumberOfRounds = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupCount = table.Column<int>(type: "INTEGER", nullable: true),
                    AdvanceCount = table.Column<int>(type: "INTEGER", nullable: true),
                    HasThirdPlaceMatch = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasGrandFinalReset = table.Column<bool>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentStages_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TournamentStageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoundNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Player1Id = table.Column<Guid>(type: "TEXT", nullable: true),
                    Player2Id = table.Column<Guid>(type: "TEXT", nullable: true),
                    WinnerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LoserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Score1 = table.Column<int>(type: "INTEGER", nullable: true),
                    Score2 = table.Column<int>(type: "INTEGER", nullable: true),
                    IsBye = table.Column<bool>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_TournamentParticipants_LoserId",
                        column: x => x.LoserId,
                        principalTable: "TournamentParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_TournamentParticipants_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "TournamentParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_TournamentParticipants_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "TournamentParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_TournamentParticipants_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "TournamentParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_TournamentStages_TournamentStageId",
                        column: x => x.TournamentStageId,
                        principalTable: "TournamentStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Standings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TournamentStageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Wins = table.Column<int>(type: "INTEGER", nullable: false),
                    Losses = table.Column<int>(type: "INTEGER", nullable: false),
                    Draws = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    PointDifference = table.Column<int>(type: "INTEGER", nullable: false),
                    Buchholz = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    MedianBuchholz = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    HeadToHeadScore = table.Column<int>(type: "INTEGER", nullable: false),
                    Rank = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Standings_TournamentParticipants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "TournamentParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Standings_TournamentStages_TournamentStageId",
                        column: x => x.TournamentStageId,
                        principalTable: "TournamentStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_LoserId",
                table: "Matches",
                column: "LoserId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player1Id",
                table: "Matches",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player2Id",
                table: "Matches",
                column: "Player2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentStageId",
                table: "Matches",
                column: "TournamentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_WinnerId",
                table: "Matches",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Standings_ParticipantId",
                table: "Standings",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Standings_TournamentStageId_ParticipantId",
                table: "Standings",
                columns: new[] { "TournamentStageId", "ParticipantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipants_TournamentId",
                table: "TournamentParticipants",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipants_UserId",
                table: "TournamentParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_CommunityId",
                table: "Tournaments",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_CreatedById",
                table: "Tournaments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentStages_TournamentId_StageOrder",
                table: "TournamentStages",
                columns: new[] { "TournamentId", "StageOrder" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Standings");

            migrationBuilder.DropTable(
                name: "TournamentParticipants");

            migrationBuilder.DropTable(
                name: "TournamentStages");

            migrationBuilder.DropTable(
                name: "Tournaments");
        }
    }
}
