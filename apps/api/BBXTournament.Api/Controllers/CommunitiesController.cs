using System.Security.Claims;
using BBXTournament.Application.Contracts.Communities;
using BBXTournament.Application.Interfaces.Communities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBXTournament.Api.Controllers;

[ApiController]
[Route("communities")]
public class CommunitiesController : ControllerBase
{
    private readonly ICommunityService _communityService;

    public CommunitiesController(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CommunityResponse>> CreateCommunity(
        [FromBody] CreateCommunityRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var userId = GetUserId();
            var response = await _communityService.CreateCommunityAsync(request, userId, cancellationToken);
            return CreatedAtAction(nameof(GetCommunityById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CommunityResponse>>> GetAllCommunities(
        CancellationToken cancellationToken)
    {
        var communities = await _communityService.GetAllCommunitiesAsync(cancellationToken);
        return Ok(communities);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommunityResponse>> GetCommunityById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var community = await _communityService.GetCommunityByIdAsync(id, cancellationToken);
        
        if (community is null)
        {
            return NotFound(new { error = "Community not found." });
        }

        return Ok(community);
    }

    [HttpPost("{id}/admins")]
    [Authorize]
    public async Task<IActionResult> AddAdmin(
        Guid id,
        [FromBody] AddCommunityAdminRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var userId = GetUserId();
            await _communityService.AddAdminAsync(id, request, userId, cancellationToken);
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

    [HttpDelete("{id}/admins/{userId}")]
    [Authorize]
    public async Task<IActionResult> RemoveAdmin(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken)
    {
        try
        {
            var requesterId = GetUserId();
            await _communityService.RemoveAdminAsync(id, userId, requesterId, cancellationToken);
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

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token.");
        }
        return userId;
    }
}

// Made with Bob