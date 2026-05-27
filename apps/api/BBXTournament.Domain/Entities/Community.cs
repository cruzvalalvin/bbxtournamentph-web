namespace BBXTournament.Domain.Entities;

public class Community
{
    public Guid Id { get; private set; }
    public string PublicCode { get; private set; } = string.Empty;
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string Description { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? BannerUrl { get; private set; }
    public string? Region { get; private set; }
    public string? Province { get; private set; }
    public string? City { get; private set; }
    public Guid OwnerId { get; private set; }
    public bool IsVerified { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public User Owner { get; private set; } = null!;
    public ICollection<CommunityAdmin> Admins { get; private set; } = new List<CommunityAdmin>();
    public ICollection<CommunityMember> Members { get; private set; } = new List<CommunityMember>();

    private Community()
    {
        Name = string.Empty;
        Slug = string.Empty;
        Description = string.Empty;
    }

    public Community(
        string name,
        string slug,
        string description,
        Guid ownerId,
        string? logoUrl = null,
        string? bannerUrl = null,
        string? region = null,
        string? province = null,
        string? city = null)
    {
        Id = Guid.NewGuid();
        PublicCode = string.Empty; // Will be set after save to get proper sequence
        Name = name.Trim();
        Slug = slug.Trim().ToLowerInvariant();
        Description = description.Trim();
        OwnerId = ownerId;
        LogoUrl = NormalizeUrl(logoUrl);
        BannerUrl = NormalizeUrl(bannerUrl);
        Region = NormalizeLocation(region);
        Province = NormalizeLocation(province);
        City = NormalizeLocation(city);
        IsVerified = false; // Default to unverified
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPublicCode(string publicCode)
    {
        if (string.IsNullOrWhiteSpace(publicCode))
        {
            throw new ArgumentException("Public code cannot be empty.", nameof(publicCode));
        }
        PublicCode = publicCode.Trim();
    }

    public void Update(
        string name,
        string description,
        string? logoUrl,
        string? bannerUrl,
        string? region,
        string? province,
        string? city)
    {
        Name = name.Trim();
        Description = description.Trim();
        LogoUrl = NormalizeUrl(logoUrl);
        BannerUrl = NormalizeUrl(bannerUrl);
        Region = NormalizeLocation(region);
        Province = NormalizeLocation(province);
        City = NormalizeLocation(city);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Verify()
    {
        IsVerified = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unverify()
    {
        IsVerified = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string? NormalizeLocation(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static string? NormalizeUrl(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}

// Made with Bob