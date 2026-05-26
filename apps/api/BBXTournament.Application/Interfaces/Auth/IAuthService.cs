using BBXTournament.Application.Contracts.Auth;

namespace BBXTournament.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<UserResponse?> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default);
}

// Made with Bob
