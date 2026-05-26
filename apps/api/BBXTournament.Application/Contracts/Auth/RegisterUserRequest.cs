using BBXTournament.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BBXTournament.Application.Contracts.Auth;

public class RegisterUserRequest
{
    [Required]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Player;

    public string? Region { get; set; }
    public string? Province { get; set; }
    public string? City { get; set; }
}

// Made with Bob
