using BBXTournament.Application.Contracts.Auth;
using BBXTournament.Application.Contracts.Communities;
using BBXTournament.Application.Contracts.Tournaments;
using BBXTournament.Application.Interfaces.Tournaments;
using BBXTournament.Domain.Entities;
using BBXTournament.Domain.Enums;

namespace BBXTournament.Application.Services.Tournaments;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;

    public TournamentService(ITournamentRepository tournamentRepository)
    {
        _tournamentRepository = tournamentRepository;
    }

    public async Task<TournamentResponse> CreateTournamentAsync(
        CreateTournamentRequest request,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var tournament = new Tournament(
            request.CommunityId,
            request.Name,
            request.Description,
            request.MaxParticipants,
            userId,
            request.Region,
            request.Province,
            request.City);

        await _tournamentRepository.AddAsync(tournament, cancellationToken);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        // Generate PublicCode after save to ensure we have the ID
        var count = await _tournamentRepository.GetTournamentCountAsync(cancellationToken);
        var publicCode = $"TRN-{count:D4}";
        tournament.SetPublicCode(publicCode);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        var created = await _tournamentRepository.GetByIdAsync(tournament.Id, cancellationToken);
        return MapToTournamentResponse(created!);
    }

    public async Task<List<TournamentResponse>> GetAllTournamentsAsync(CancellationToken cancellationToken = default)
    {
        var tournaments = await _tournamentRepository.GetAllAsync(cancellationToken);
        return tournaments.Select(MapToTournamentResponse).ToList();
    }

    public async Task<TournamentResponse?> GetTournamentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(id, cancellationToken);
        return tournament is null ? null : MapToTournamentResponse(tournament);
    }

    public async Task CancelTournamentAsync(
        Guid id,
        Guid requesterId,
        string requesterRole,
        CancellationToken cancellationToken = default)
    {
        var tournament = await _tournamentRepository.GetForUpdateAsync(id, cancellationToken);
        if (tournament is null)
        {
            throw new InvalidOperationException("Tournament not found.");
        }

        if (!CanCancelTournament(tournament, requesterId, requesterRole))
        {
            throw new UnauthorizedAccessException("Only the community owner or super admin can cancel this tournament.");
        }

        tournament.Cancel();
        await _tournamentRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteTournamentAsync(
        Guid id,
        Guid requesterId,
        string requesterRole,
        CancellationToken cancellationToken = default)
    {
        var tournament = await _tournamentRepository.GetForUpdateAsync(id, cancellationToken);
        if (tournament is null)
        {
            throw new InvalidOperationException("Tournament not found.");
        }

        if (!string.Equals(requesterRole, UserRole.SuperAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Only super admin can delete tournaments.");
        }

        tournament.Delete();
        await _tournamentRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<ParticipantResponse> AddParticipantAsync(
        Guid tournamentId,
        AddParticipantRequest request,
        CancellationToken cancellationToken = default)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId, cancellationToken);
        if (tournament is null)
        {
            throw new InvalidOperationException("Tournament not found.");
        }

        var participants = await _tournamentRepository.GetParticipantsAsync(tournamentId, cancellationToken);
        if (participants.Count >= tournament.MaxParticipants)
        {
            throw new InvalidOperationException("Tournament is full.");
        }

        if (request.UserId.HasValue)
        {
            var existing = await _tournamentRepository.GetParticipantAsync(tournamentId, request.UserId.Value, cancellationToken);
            if (existing is not null)
            {
                throw new InvalidOperationException("User is already registered for this tournament.");
            }
        }

        // Check if Round 2 or later has been generated in any stage
        var stages = await _tournamentRepository.GetStagesAsync(tournamentId, cancellationToken);
        bool round2OrLaterExists = false;

        foreach (var stage in stages)
        {
            if (await _tournamentRepository.RoundExistsAsync(stage.Id, 2, cancellationToken))
            {
                round2OrLaterExists = true;
                break;
            }
        }

        // Validation: Cannot join if Round 2 or later has been generated
        if (round2OrLaterExists)
        {
            throw new InvalidOperationException("Cannot join tournament. Participant registration is locked after Round 2 has been generated.");
        }

        var participant = new TournamentParticipant(
            tournamentId,
            request.UserId,
            request.DisplayName,
            request.TeamName,
            request.IsManualEntry,
            request.Seed);

        // No auto-losses needed - participants can only join before Round 2 is generated
        // They will be included in Round 1 or can join after Round 1 completes but before Round 2 generates

        await _tournamentRepository.AddParticipantAsync(participant, cancellationToken);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        return MapToParticipantResponse(participant);
    }

    public async Task<List<ParticipantResponse>> GetParticipantsAsync(Guid tournamentId, CancellationToken cancellationToken = default)
    {
        var participants = await _tournamentRepository.GetParticipantsAsync(tournamentId, cancellationToken);
        return participants.Select(MapToParticipantResponse).ToList();
    }

    public async Task<StageResponse> CreateStageAsync(
        Guid tournamentId,
        CreateStageRequest request,
        CancellationToken cancellationToken = default)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId, cancellationToken);
        if (tournament is null)
        {
            throw new InvalidOperationException("Tournament not found.");
        }

        if (await _tournamentRepository.StageOrderExistsAsync(tournamentId, request.StageOrder, cancellationToken))
        {
            throw new InvalidOperationException("Stage order already exists for this tournament.");
        }

        var stage = new TournamentStage(
            tournamentId,
            request.Name,
            request.StageOrder,
            request.FormatType,
            request.NumberOfRounds,
            request.GroupCount,
            request.AdvanceCount,
            request.HasThirdPlaceMatch,
            request.HasGrandFinalReset);

        await _tournamentRepository.AddStageAsync(stage, cancellationToken);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        return MapToStageResponse(stage);
    }

    public async Task<List<StageResponse>> GetStagesAsync(Guid tournamentId, CancellationToken cancellationToken = default)
    {
        var stages = await _tournamentRepository.GetStagesAsync(tournamentId, cancellationToken);
        return stages.Select(MapToStageResponse).ToList();
    }

    private static bool CanCancelTournament(Tournament tournament, Guid requesterId, string requesterRole)
    {
        if (string.Equals(requesterRole, UserRole.SuperAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return tournament.Community.OwnerId == requesterId;
    }

    private static TournamentResponse MapToTournamentResponse(Tournament tournament)
    {
        return new TournamentResponse
        {
            Id = tournament.Id,
            PublicCode = tournament.PublicCode,
            Name = tournament.Name,
            Description = tournament.Description,
            Status = tournament.Status,
            MaxParticipants = tournament.MaxParticipants,
            CommunityId = tournament.CommunityId,
            CommunityName = tournament.Community?.Name ?? string.Empty,
            Region = tournament.Region,
            Province = tournament.Province,
            City = tournament.City,
            CreatedAt = tournament.CreatedAt
        };
    }

    private static ParticipantResponse MapToParticipantResponse(TournamentParticipant participant)
    {
        return new ParticipantResponse
        {
            Id = participant.Id,
            TournamentId = participant.TournamentId,
            UserId = participant.UserId,
            DisplayName = participant.DisplayName,
            TeamName = participant.TeamName,
            IsManualEntry = participant.IsManualEntry,
            IsPaid = participant.IsPaid,
            Seed = participant.Seed,
            CheckedIn = participant.CheckedIn,
            CreatedAt = participant.CreatedAt
        };
    }

    private static StageResponse MapToStageResponse(TournamentStage stage)
    {
        return new StageResponse
        {
            Id = stage.Id,
            TournamentId = stage.TournamentId,
            Name = stage.Name,
            StageOrder = stage.StageOrder,
            FormatType = stage.FormatType,
            NumberOfRounds = stage.NumberOfRounds,
            GroupCount = stage.GroupCount,
            AdvanceCount = stage.AdvanceCount,
            HasThirdPlaceMatch = stage.HasThirdPlaceMatch,
            HasGrandFinalReset = stage.HasGrandFinalReset,
            Status = stage.Status
        };
    }
    public async Task CheckInParticipantAsync(
        Guid participantId,
        Guid requesterId,
        string requesterRole,
        CancellationToken cancellationToken = default)
    {
        var participant = await _tournamentRepository.GetParticipantByIdAsync(participantId, cancellationToken);
        if (participant == null)
        {
            throw new InvalidOperationException("Participant not found.");
        }

        // Authorization: Only tournament owner, community admins, or platform admins can check in
        var tournament = participant.Tournament;
        bool isAuthorized = requesterRole == "Admin" ||
                           tournament.CreatedById == requesterId;

        if (!isAuthorized)
        {
            // Check if requester is a community admin
            var communityAdmins = await _tournamentRepository.GetByIdAsync(tournament.Id, cancellationToken);
            // For now, we'll allow if they're the tournament creator or platform admin
            // Community admin check would require ICommunityRepository
        }

        if (!isAuthorized && requesterRole != "Admin")
        {
            throw new UnauthorizedAccessException("Only tournament admins, community admins, or platform admins can check in participants.");
        }

        participant.CheckIn();
        await _tournamentRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task CheckOutParticipantAsync(
        Guid participantId,
        Guid requesterId,
        string requesterRole,
        CancellationToken cancellationToken = default)
    {
        var participant = await _tournamentRepository.GetParticipantByIdAsync(participantId, cancellationToken);
        if (participant == null)
        {
            throw new InvalidOperationException("Participant not found.");
        }

        // Authorization: Only tournament owner, community admins, or platform admins can check out
        var tournament = participant.Tournament;
        bool isAuthorized = requesterRole == "Admin" ||
                           tournament.CreatedById == requesterId;

        if (!isAuthorized && requesterRole != "Admin")
        {
            throw new UnauthorizedAccessException("Only tournament admins, community admins, or platform admins can check out participants.");
        }

        participant.UndoCheckIn();
        await _tournamentRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkParticipantAsPaidAsync(
        Guid participantId,
        Guid requesterId,
        string requesterRole,
        CancellationToken cancellationToken = default)
    {
        var participant = await _tournamentRepository.GetParticipantByIdAsync(participantId, cancellationToken);
        if (participant == null)
        {
            throw new InvalidOperationException("Participant not found.");
        }

        // Authorization: Only tournament owner, community admins, or platform admins can mark as paid
        var tournament = participant.Tournament;
        bool isAuthorized = requesterRole == "Admin" ||
                           tournament.CreatedById == requesterId;

        if (!isAuthorized && requesterRole != "Admin")
        {
            throw new UnauthorizedAccessException("Only tournament admins, community admins, or platform admins can mark participants as paid.");
        }

        participant.MarkAsPaid();
        await _tournamentRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkParticipantAsUnpaidAsync(
        Guid participantId,
        Guid requesterId,
        string requesterRole,
        CancellationToken cancellationToken = default)
    {
        var participant = await _tournamentRepository.GetParticipantByIdAsync(participantId, cancellationToken);
        if (participant == null)
        {
            throw new InvalidOperationException("Participant not found.");
        }

        // Authorization: Only tournament owner, community admins, or platform admins can mark as unpaid
        var tournament = participant.Tournament;
        bool isAuthorized = requesterRole == "Admin" ||
                           tournament.CreatedById == requesterId;

        if (!isAuthorized && requesterRole != "Admin")
        {
            throw new UnauthorizedAccessException("Only tournament admins, community admins, or platform admins can mark participants as unpaid.");
        }

        participant.UnmarkAsPaid();
        await _tournamentRepository.SaveChangesAsync(cancellationToken);
    }


    public async Task<RoundResponse> GenerateRoundAsync(Guid stageId, CancellationToken cancellationToken = default)
    {
        var stage = await _tournamentRepository.GetStageWithParticipantsAsync(stageId, cancellationToken);
        if (stage == null)
        {
            throw new InvalidOperationException("Stage not found.");
        }

        // Check if Round 1 already exists
        var existingRounds = await _tournamentRepository.GetRoundsByStageAsync(stageId, cancellationToken);
        if (existingRounds.Any())
        {
            throw new InvalidOperationException("Round 1 already exists for this stage.");
        }

        var participants = stage.Tournament.Participants.Where(p => p.CheckedIn).ToList();
        if (participants.Count < 2)
        {
            throw new InvalidOperationException("At least 2 checked-in participants are required to generate rounds.");
        }

        // Create Round 1
        var round = new TournamentRound(stageId, 1);
        await _tournamentRepository.AddRoundAsync(round, cancellationToken);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        // Generate matches based on format
        var matches = stage.FormatType switch
        {
            StageFormatType.SingleElimination => GenerateSingleEliminationMatches(round.Id, stage.Id, participants),
            StageFormatType.RoundRobin => GenerateRoundRobinMatches(round.Id, stage.Id, participants),
            StageFormatType.Swiss => GenerateSwissMatches(round.Id, stage.Id, participants),
            _ => throw new InvalidOperationException($"Format type {stage.FormatType} is not supported yet.")
        };

        await _tournamentRepository.AddMatchesAsync(matches, cancellationToken);
        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        // Fetch the round with matches
        var createdRound = await _tournamentRepository.GetRoundWithMatchesAsync(round.Id, cancellationToken);
        return MapToRoundResponse(createdRound!);
    }

    public async Task<List<RoundResponse>> GetRoundsByStageAsync(Guid stageId, CancellationToken cancellationToken = default)
    {
        var rounds = await _tournamentRepository.GetRoundsByStageAsync(stageId, cancellationToken);
        var responses = new List<RoundResponse>();

        foreach (var round in rounds)
        {
            var matches = await _tournamentRepository.GetMatchesByRoundAsync(round.Id, cancellationToken);
            var response = MapToRoundResponse(round);
            response.Matches = matches.Select(MapToMatchResponse).ToList();
            responses.Add(response);
        }

        return responses;
    }

    public async Task<List<MatchResponse>> GetMatchesByRoundAsync(Guid roundId, CancellationToken cancellationToken = default)
    {
        var matches = await _tournamentRepository.GetMatchesByRoundAsync(roundId, cancellationToken);
        return matches.Select(MapToMatchResponse).ToList();
    }

    public async Task<MatchResponse> SubmitMatchResultAsync(
        Guid matchId,
        SubmitMatchResultRequest request,
        Guid judgeUserId,
        CancellationToken cancellationToken = default)
    {
        // Get match with participants
        var match = await _tournamentRepository.GetMatchWithParticipantsAsync(matchId, cancellationToken);
        if (match == null)
        {
            throw new InvalidOperationException("Match not found.");
        }

        // Check if next round has been generated (round is locked)
        var nextRoundExists = await _tournamentRepository.RoundExistsAsync(
            match.TournamentStageId,
            match.RoundNumber + 1,
            cancellationToken);
        
        if (nextRoundExists)
        {
            throw new InvalidOperationException("Cannot edit match result. This round has been finalized by generating the next round.");
        }

        if (!match.Player1Id.HasValue || !match.Player2Id.HasValue)
        {
            throw new InvalidOperationException("Both players must be assigned before submitting result.");
        }

        // Validate scores: Only 0-0 is allowed for equal scores (double no-show)
        if (request.Player1Score == request.Player2Score && request.Player1Score != 0)
        {
            throw new InvalidOperationException("Scores cannot be equal except for 0-0 (double no-show). In Beyblade X, there must be a winner.");
        }

        // Report the score
        match.ReportScore(request.Player1Score, request.Player2Score, judgeUserId, request.JudgeNotes);

        // Update participant statistics
        var player1 = match.Player1!;
        var player2 = match.Player2!;

        // Special case: 0-0 (double no-show / double loss)
        if (request.Player1Score == 0 && request.Player2Score == 0)
        {
            // Both players get a loss, no points scored
            player1.RecordLoss(0, 0);
            player2.RecordLoss(0, 0);
        }
        else if (request.Player1Score > request.Player2Score)
        {
            // Player 1 wins
            player1.RecordWin(request.Player1Score, request.Player2Score);
            player2.RecordLoss(request.Player2Score, request.Player1Score);
        }
        else
        {
            // Player 2 wins
            player2.RecordWin(request.Player2Score, request.Player1Score);
            player1.RecordLoss(request.Player1Score, request.Player2Score);
        }

        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        // Fetch updated match
        var updatedMatch = await _tournamentRepository.GetMatchByIdAsync(matchId, cancellationToken);
        return MapToMatchResponse(updatedMatch!);
    }

    public async Task<List<StandingResponse>> GetTournamentStandingsAsync(
        Guid tournamentId,
        CancellationToken cancellationToken = default)
    {
        var participants = await _tournamentRepository.GetStandingsAsync(tournamentId, cancellationToken);
        
        var standings = new List<StandingResponse>();
        int currentRank = 1;
        int position = 0;

        TournamentParticipant? previousParticipant = null;

        foreach (var participant in participants)
        {
            position++;

            // Check if current participant has same stats as previous (tie condition)
            if (previousParticipant != null &&
                participant.MatchWins == previousParticipant.MatchWins &&
                participant.PointsDifference == previousParticipant.PointsDifference &&
                participant.PointsScored == previousParticipant.PointsScored)
            {
                // Same rank as previous (tied)
                // currentRank stays the same
            }
            else
            {
                // Different stats, assign new rank based on position
                currentRank = position;
            }

            standings.Add(new StandingResponse(
                participant.Id,
                participant.DisplayName,
                participant.TeamName,
                currentRank,
                participant.MatchWins,
                participant.MatchLosses,
                participant.PointsScored,
                participant.PointsAgainst,
                participant.PointsDifference,
                participant.BuchholzScore
            ));

            previousParticipant = participant;
        }

        return standings;
    }

    public async Task<RoundResponse> ShuffleRoundAsync(Guid roundId, CancellationToken cancellationToken = default)
    {
        // Get round with matches
        var round = await _tournamentRepository.GetRoundWithMatchesAsync(roundId, cancellationToken);
        if (round == null)
        {
            throw new InvalidOperationException("Round not found.");
        }

        // Validate: No match should be completed
        if (round.Matches.Any(m => m.Status == MatchStatus.Completed))
        {
            throw new InvalidOperationException("Cannot shuffle round. One or more matches have already been completed.");
        }

        // Get all participant IDs from current matches (excluding byes)
        var participantIds = new List<Guid>();
        Guid? byeParticipantId = null;

        foreach (var match in round.Matches)
        {
            if (match.IsBye)
            {
                // Store bye participant
                byeParticipantId = match.Player1Id;
            }
            else
            {
                if (match.Player1Id.HasValue)
                    participantIds.Add(match.Player1Id.Value);
                if (match.Player2Id.HasValue)
                    participantIds.Add(match.Player2Id.Value);
            }
        }

        // Shuffle participants randomly
        var shuffled = participantIds.OrderBy(_ => Guid.NewGuid()).ToList();

        // Reset all existing matches
        foreach (var match in round.Matches)
        {
            match.Reset();
        }

        // Reassign pairings
        int matchIndex = 0;
        
        // Handle bye if exists
        if (byeParticipantId.HasValue)
        {
            var byeMatch = round.Matches.FirstOrDefault(m => m.IsBye);
            if (byeMatch != null)
            {
                byeMatch.AssignPlayer1(byeParticipantId.Value);
                byeMatch.MarkAsBye(byeParticipantId.Value);
            }
        }

        // Pair remaining participants
        var regularMatches = round.Matches.Where(m => !m.IsBye).OrderBy(m => m.MatchNumber).ToList();
        for (int i = 0; i < shuffled.Count && matchIndex < regularMatches.Count; i += 2)
        {
            if (i + 1 < shuffled.Count)
            {
                regularMatches[matchIndex].AssignPlayer1(shuffled[i]);
                regularMatches[matchIndex].AssignPlayer2(shuffled[i + 1]);
                matchIndex++;
            }
        }

        await _tournamentRepository.SaveChangesAsync(cancellationToken);

        // Fetch updated round
        var updatedRound = await _tournamentRepository.GetRoundWithMatchesAsync(roundId, cancellationToken);
        return MapToRoundResponse(updatedRound!);
    }

    private List<Match> GenerateSingleEliminationMatches(Guid roundId, Guid stageId, List<TournamentParticipant> participants)
    {
        // Shuffle participants randomly
        var shuffled = participants.OrderBy(_ => Guid.NewGuid()).ToList();
        var matches = new List<Match>();
        var matchNumber = 1;

        // Handle odd number of participants with bye
        if (shuffled.Count % 2 != 0)
        {
            var byePlayer = shuffled.Last();
            shuffled.RemoveAt(shuffled.Count - 1);
            
            var byeMatch = new Match(stageId, roundId, 1, matchNumber++, byePlayer.Id, null, true);
            byeMatch.MarkAsBye(byePlayer.Id);
            matches.Add(byeMatch);
        }

        // Pair remaining participants
        for (int i = 0; i < shuffled.Count; i += 2)
        {
            var match = new Match(
                stageId,
                roundId,
                1,
                matchNumber++,
                shuffled[i].Id,
                shuffled[i + 1].Id);
            matches.Add(match);
        }

        return matches;
    }

    private List<Match> GenerateRoundRobinMatches(Guid roundId, Guid stageId, List<TournamentParticipant> participants)
    {
        // Shuffle participants randomly
        var shuffled = participants.OrderBy(_ => Guid.NewGuid()).ToList();
        var matches = new List<Match>();
        var matchNumber = 1;

        // Round 1: Generate all possible pairings (first round only for now)
        for (int i = 0; i < shuffled.Count; i++)
        {
            for (int j = i + 1; j < shuffled.Count; j++)
            {
                var match = new Match(
                    stageId,
                    roundId,
                    1,
                    matchNumber++,
                    shuffled[i].Id,
                    shuffled[j].Id);
                matches.Add(match);
            }
        }

        return matches;
    }

    private List<Match> GenerateSwissMatches(Guid roundId, Guid stageId, List<TournamentParticipant> participants)
    {
        // Shuffle participants randomly for Round 1
        var shuffled = participants.OrderBy(_ => Guid.NewGuid()).ToList();
        var matches = new List<Match>();
        var matchNumber = 1;

        // Handle odd number of participants with bye
        if (shuffled.Count % 2 != 0)
        {
            var byePlayer = shuffled.Last();
            shuffled.RemoveAt(shuffled.Count - 1);
            
            var byeMatch = new Match(stageId, roundId, 1, matchNumber++, byePlayer.Id, null, true);
            byeMatch.MarkAsBye(byePlayer.Id);
            matches.Add(byeMatch);
        }

        // Pair remaining participants
        for (int i = 0; i < shuffled.Count; i += 2)
        {
            var match = new Match(
                stageId,
                roundId,
                1,
                matchNumber++,
                shuffled[i].Id,
                shuffled[i + 1].Id);
            matches.Add(match);
        }

        return matches;
    }

    private static RoundResponse MapToRoundResponse(TournamentRound round)
    {
        return new RoundResponse
        {
            Id = round.Id,
            TournamentStageId = round.TournamentStageId,
            RoundNumber = round.RoundNumber,
            Status = round.Status,
            StartedAt = round.StartedAt,
            CompletedAt = round.CompletedAt,
            CreatedAt = round.CreatedAt,
            Matches = round.Matches?.Select(MapToMatchResponse).ToList() ?? new List<MatchResponse>()
        };
    }

    private static MatchResponse MapToMatchResponse(Match match)
    {
        return new MatchResponse
        {
            Id = match.Id,
            TournamentStageId = match.TournamentStageId,
            TournamentRoundId = match.TournamentRoundId,
            RoundNumber = match.RoundNumber,
            MatchNumber = match.MatchNumber,
            Player1Id = match.Player1Id,
            Player1DisplayName = match.Player1?.DisplayName,
            Player2Id = match.Player2Id,
            Player2DisplayName = match.Player2?.DisplayName,
            Player1Score = match.Player1Score,
            Player2Score = match.Player2Score,
            WinnerParticipantId = match.WinnerParticipantId,
            WinnerDisplayName = match.WinnerParticipant?.DisplayName,
            LoserParticipantId = match.LoserParticipantId,
            JudgeUserId = match.JudgeUserId,
            IsBye = match.IsBye,
            Status = match.Status,
            CompletedAt = match.CompletedAt,
            CreatedAt = match.CreatedAt
        };
    }
}

// Made with Bob