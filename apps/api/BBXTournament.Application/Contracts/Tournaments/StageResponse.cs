using BBXTournament.Domain.Enums;

namespace BBXTournament.Application.Contracts.Tournaments;

public class StageResponse
{
    public Guid Id { get; set; }
    public Guid TournamentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StageOrder { get; set; }
    public StageFormatType FormatType { get; set; }
    public int NumberOfRounds { get; set; }
    public int? GroupCount { get; set; }
    public int? AdvanceCount { get; set; }
    public bool HasThirdPlaceMatch { get; set; }
    public bool HasGrandFinalReset { get; set; }
    public TournamentStatus Status { get; set; }
}

// Made with Bob