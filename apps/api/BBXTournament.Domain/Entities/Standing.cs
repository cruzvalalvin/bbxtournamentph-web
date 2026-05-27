namespace BBXTournament.Domain.Entities;

public class Standing
{
    public Guid Id { get; private set; }
    public Guid TournamentStageId { get; private set; }
    public Guid ParticipantId { get; private set; }
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Draws { get; private set; }
    public int MatchPoints { get; private set; }
    public int PointDifference { get; private set; }
    public decimal Buchholz { get; private set; }
    public decimal MedianBuchholz { get; private set; }
    public int HeadToHeadScore { get; private set; }
    public int Rank { get; private set; }

    // Navigation properties
    public TournamentStage TournamentStage { get; private set; } = null!;
    public TournamentParticipant Participant { get; private set; } = null!;

    private Standing()
    {
    }

    public Standing(
        Guid tournamentStageId,
        Guid participantId)
    {
        Id = Guid.NewGuid();
        TournamentStageId = tournamentStageId;
        ParticipantId = participantId;
        Wins = 0;
        Losses = 0;
        Draws = 0;
        MatchPoints = 0;
        PointDifference = 0;
        Buchholz = 0;
        MedianBuchholz = 0;
        HeadToHeadScore = 0;
        Rank = 0;
    }

    public void RecordWin(int pointsScored, int pointsAgainst)
    {
        Wins++;
        MatchPoints += 3; // Standard 3 points for a win
        PointDifference += (pointsScored - pointsAgainst);
    }

    public void RecordLoss(int pointsScored, int pointsAgainst)
    {
        Losses++;
        PointDifference += (pointsScored - pointsAgainst);
    }

    public void RecordDraw(int pointsScored, int pointsAgainst)
    {
        Draws++;
        MatchPoints += 1; // Standard 1 point for a draw
        PointDifference += (pointsScored - pointsAgainst);
    }

    public void UpdateTiebreakers(
        decimal buchholz,
        decimal medianBuchholz,
        int headToHeadScore)
    {
        Buchholz = buchholz;
        MedianBuchholz = medianBuchholz;
        HeadToHeadScore = headToHeadScore;
    }

    public void UpdateRank(int rank)
    {
        Rank = rank;
    }

    public void Reset()
    {
        Wins = 0;
        Losses = 0;
        Draws = 0;
        MatchPoints = 0;
        PointDifference = 0;
        Buchholz = 0;
        MedianBuchholz = 0;
        HeadToHeadScore = 0;
        Rank = 0;
    }
}

// Made with Bob