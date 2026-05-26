using BBXTournament.Domain.Entities;

namespace BBXTournament.Application.Interfaces.Auth;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}

// Made with Bob
