namespace BBXTournament.Application.Contracts.Auth;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public UserResponse User { get; set; } = new();
}

// Made with Bob
