using BBXTournament.Domain.Enums;

namespace BBXTournament.Application.Contracts.Tournaments;

public class CreateStageRequest
{
    public string Name { get; set; } = string.Empty;
    public int StageOrder { get; set; }
    public StageFormatType FormatType { get; set; }
    public int NumberOfRounds { get; set; }
    public int? GroupCount { get; set; }
    public int? AdvanceCount { get; set; }
    public bool HasThirdPlaceMatch { get; set; }
    public bool HasGrandFinalReset { get; set; }
}

// Made with Bob