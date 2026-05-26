using BBXTournament.Domain.Enums;

namespace BBXTournament.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string DisplayName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public string? Region { get; private set; }
    public string? Province { get; private set; }
    public string? City { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private User()
    {
        DisplayName = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
    }

    public User(
        string displayName,
        string email,
        string passwordHash,
        UserRole role = UserRole.Player,
        string? region = null,
        string? province = null,
        string? city = null)
    {
        Id = Guid.NewGuid();
        DisplayName = displayName.Trim();
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = role;
        Region = NormalizeLocation(region);
        Province = NormalizeLocation(province);
        City = NormalizeLocation(city);
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(
        string displayName,
        UserRole role,
        string? region,
        string? province,
        string? city,
        bool isActive)
    {
        DisplayName = displayName.Trim();
        Role = role;
        Region = NormalizeLocation(region);
        Province = NormalizeLocation(province);
        City = NormalizeLocation(city);
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string? NormalizeLocation(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}

// Made with Bob
