using BBXTournament.Domain.Enums;

namespace BBXTournament.Domain.Entities;

public class TournamentStage
{
    public Guid Id { get; private set; }
    public Guid TournamentId { get; private set; }
    public string Name { get; private set; }
    public int StageOrder { get; private set; }
    public StageFormatType FormatType { get; private set; }
    public int NumberOfRounds { get; private set; }
    public int? GroupCount { get; private set; }
    public int? AdvanceCount { get; private set; }
    public bool HasThirdPlaceMatch { get; private set; }
    public bool HasGrandFinalReset { get; private set; }
    public TournamentStatus Status { get; private set; }

    // Navigation properties
    public Tournament Tournament { get; private set; } = null!;
    public ICollection<Match> Matches { get; private set; } = new List<Match>();
    public ICollection<Standing> Standings { get; private set; } = new List<Standing>();

    private TournamentStage()
    {
        Name = string.Empty;
    }

    public TournamentStage(
        Guid tournamentId,
        string name,
        int stageOrder,
        StageFormatType formatType,
        int numberOfRounds,
        int? groupCount = null,
        int? advanceCount = null,
        bool hasThirdPlaceMatch = false,
        bool hasGrandFinalReset = false)
    {
        Id = Guid.NewGuid();
        TournamentId = tournamentId;
        Name = name.Trim();
        StageOrder = stageOrder;
        FormatType = formatType;
        NumberOfRounds = numberOfRounds;
        GroupCount = groupCount;
        AdvanceCount = advanceCount;
        HasThirdPlaceMatch = hasThirdPlaceMatch;
        HasGrandFinalReset = hasGrandFinalReset;
        Status = TournamentStatus.Draft;
    }

    public void Update(
        string name,
        int numberOfRounds,
        int? groupCount,
        int? advanceCount,
        bool hasThirdPlaceMatch,
        bool hasGrandFinalReset)
    {
        Name = name.Trim();
        NumberOfRounds = numberOfRounds;
        GroupCount = groupCount;
        AdvanceCount = advanceCount;
        HasThirdPlaceMatch = hasThirdPlaceMatch;
        HasGrandFinalReset = hasGrandFinalReset;
    }

    public void UpdateStatus(TournamentStatus status)
    {
        Status = status;
    }

    public void Start()
    {
        if (Status != TournamentStatus.Draft)
        {
            throw new InvalidOperationException("Can only start stage from Draft status.");
        }
        Status = TournamentStatus.Ongoing;
    }

    public void Complete()
    {
        if (Status != TournamentStatus.Ongoing)
        {
            throw new InvalidOperationException("Can only complete stage from Ongoing status.");
        }
        Status = TournamentStatus.Finished;
    }
}

// Made with Bob