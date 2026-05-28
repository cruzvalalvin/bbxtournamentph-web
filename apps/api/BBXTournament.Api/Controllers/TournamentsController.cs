using System.Security.Claims;
using BBXTournament.Application.Contracts.Tournaments;
using BBXTournament.Application.Interfaces.Tournaments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBXTournament.Api.Controllers;

[ApiController]
[Route("tournaments")]
public class TournamentsController : ControllerBase
{
    private readonly ITournamentService _tournamentService;

    public TournamentsController(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TournamentResponse>> CreateTournament(
        [FromBody] CreateTournamentRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var userId = GetUserId();
            var response = await _tournamentService.CreateTournamentAsync(request, userId, cancellationToken);
            return CreatedAtAction(nameof(GetTournamentById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<TournamentResponse>>> GetAllTournaments(
        CancellationToken cancellationToken)
    {
        var tournaments = await _tournamentService.GetAllTournamentsAsync(cancellationToken);
        return Ok(tournaments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentResponse>> GetTournamentById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var tournament = await _tournamentService.GetTournamentByIdAsync(id, cancellationToken);
        
        if (tournament is null)
        {
            return NotFound(new { error = "Tournament not found." });
        }

        return Ok(tournament);
    }

    [HttpPatch("{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelTournament(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _tournamentService.CancelTournamentAsync(
                id,
                GetUserId(),
                GetUserRole(),
                cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteTournament(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _tournamentService.DeleteTournamentAsync(
                id,
                GetUserId(),
                GetUserRole(),
                cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
    }

    [HttpPost("{id}/participants")]
    [Authorize]
    public async Task<ActionResult<ParticipantResponse>> AddParticipant(
        Guid id,
        [FromBody] AddParticipantRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var response = await _tournamentService.AddParticipantAsync(id, request, cancellationToken);
            return CreatedAtAction(nameof(GetParticipants), new { id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}/participants")]
    public async Task<ActionResult<List<ParticipantResponse>>> GetParticipants(
        Guid id,
        CancellationToken cancellationToken)
    {
        var participants = await _tournamentService.GetParticipantsAsync(id, cancellationToken);
        return Ok(participants);
    }

    [HttpPatch("participants/{participantId}/checkin")]
    [Authorize]
    public async Task<IActionResult> CheckInParticipant(
        Guid participantId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _tournamentService.CheckInParticipantAsync(
                participantId,
                GetUserId(),
                GetUserRole(),
                cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPatch("participants/{participantId}/checkout")]
    [Authorize]
    public async Task<IActionResult> CheckOutParticipant(
        Guid participantId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _tournamentService.CheckOutParticipantAsync(
                participantId,
                GetUserId(),
                GetUserRole(),
                cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPatch("participants/{participantId}/mark-paid")]
    [Authorize]
    public async Task<IActionResult> MarkParticipantAsPaid(
        Guid participantId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _tournamentService.MarkParticipantAsPaidAsync(
                participantId,
                GetUserId(),
                GetUserRole(),
                cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPatch("participants/{participantId}/mark-unpaid")]
    [Authorize]
    public async Task<IActionResult> MarkParticipantAsUnpaid(
        Guid participantId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _tournamentService.MarkParticipantAsUnpaidAsync(
                participantId,
                GetUserId(),
                GetUserRole(),
                cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost("{id}/stages")]
    [Authorize]
    public async Task<ActionResult<StageResponse>> CreateStage(
        Guid id,
        [FromBody] CreateStageRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var response = await _tournamentService.CreateStageAsync(id, request, cancellationToken);
            return CreatedAtAction(nameof(GetStages), new { id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}/stages")]
    public async Task<ActionResult<List<StageResponse>>> GetStages(
        Guid id,
        CancellationToken cancellationToken)
    {
        var stages = await _tournamentService.GetStagesAsync(id, cancellationToken);
        return Ok(stages);
    }

    [HttpPost("stages/{stageId}/rounds/generate")]
    [Authorize]
    public async Task<ActionResult<RoundResponse>> GenerateRound(
        Guid stageId,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _tournamentService.GenerateRoundAsync(stageId, cancellationToken);
            return CreatedAtAction(nameof(GetRoundsByStage), new { stageId }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("stages/{stageId}/rounds")]
    public async Task<ActionResult<List<RoundResponse>>> GetRoundsByStage(
        Guid stageId,
        CancellationToken cancellationToken)
    {
        var rounds = await _tournamentService.GetRoundsByStageAsync(stageId, cancellationToken);
        return Ok(rounds);
    }

    [HttpGet("rounds/{roundId}/matches")]
    public async Task<ActionResult<List<MatchResponse>>> GetMatchesByRound(
        Guid roundId,
        CancellationToken cancellationToken)
    {
        var matches = await _tournamentService.GetMatchesByRoundAsync(roundId, cancellationToken);
        return Ok(matches);
    }

    [HttpPatch("matches/{matchId}/result")]
    [Authorize]
    public async Task<ActionResult<MatchResponse>> SubmitMatchResult(
        Guid matchId,
        [FromBody] SubmitMatchResultRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var userId = GetUserId();
            var response = await _tournamentService.SubmitMatchResultAsync(matchId, request, userId, cancellationToken);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}/standings")]
    public async Task<ActionResult<List<StandingResponse>>> GetTournamentStandings(
        Guid id,
        CancellationToken cancellationToken)
    {
        var standings = await _tournamentService.GetTournamentStandingsAsync(id, cancellationToken);
        return Ok(standings);
    }

    [HttpPost("rounds/{roundId}/shuffle")]
    [Authorize]
    public async Task<ActionResult<RoundResponse>> ShuffleRound(
        Guid roundId,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _tournamentService.ShuffleRoundAsync(roundId, cancellationToken);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token.");
        }
        return userId;
    }

    private string GetUserRole()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrWhiteSpace(roleClaim))
        {
            throw new UnauthorizedAccessException("User role not found in token.");
        }

        return roleClaim;
    }
}

// Made with Bob