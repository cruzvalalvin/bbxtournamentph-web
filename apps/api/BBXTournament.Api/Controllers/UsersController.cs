using BBXTournament.Application.Contracts.Auth;
using BBXTournament.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BBXTournament.Api.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;

    public UsersController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserResponse>> Me(CancellationToken cancellationToken)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized(new { error = "Invalid user token." });
        }

        var user = await _authService.GetCurrentUserAsync(userId, cancellationToken);

        if (user is null)
        {
            return NotFound(new { error = "User not found." });
        }

        return Ok(user);
    }
}

// Made with Bob
