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

        var participant = new TournamentParticipant(
            tournamentId,
            request.UserId,
            request.DisplayName,
            request.TeamName,
            request.IsManualEntry,
            request.Seed);

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