using BBXTournament.Domain.Enums;

namespace BBXTournament.Domain.Entities;

public class Tournament
{
    public Guid Id { get; private set; }
    public string PublicCode { get; private set; } = string.Empty;
    public Guid CommunityId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public TournamentStatus Status { get; private set; }
    public int MaxParticipants { get; private set; }
    public string? Region { get; private set; }
    public string? Province { get; private set; }
    public string? City { get; private set; }
    public Guid CreatedById { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public Community Community { get; private set; } = null!;
    public User CreatedBy { get; private set; } = null!;
    public ICollection<TournamentStage> Stages { get; private set; } = new List<TournamentStage>();
    public ICollection<TournamentParticipant> Participants { get; private set; } = new List<TournamentParticipant>();

    private Tournament()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public Tournament(
        Guid communityId,
        string name,
        string description,
        int maxParticipants,
        Guid createdById,
        string? region = null,
        string? province = null,
        string? city = null)
    {
        Id = Guid.NewGuid();
        PublicCode = string.Empty; // Will be set after save to get proper sequence
        CommunityId = communityId;
        Name = name.Trim();
        Description = description.Trim();
        Status = TournamentStatus.Draft;
        MaxParticipants = maxParticipants;
        Region = NormalizeLocation(region);
        Province = NormalizeLocation(province);
        City = NormalizeLocation(city);
        CreatedById = createdById;
        IsDeleted = false;
        DeletedAt = null;
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
        int maxParticipants,
        string? region,
        string? province,
        string? city)
    {
        Name = name.Trim();
        Description = description.Trim();
        MaxParticipants = maxParticipants;
        Region = NormalizeLocation(region);
        Province = NormalizeLocation(province);
        City = NormalizeLocation(city);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(TournamentStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void OpenRegistration()
    {
        if (Status != TournamentStatus.Draft)
        {
            throw new InvalidOperationException("Can only open registration from Draft status.");
        }
        Status = TournamentStatus.RegistrationOpen;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Start()
    {
        if (Status != TournamentStatus.RegistrationOpen)
        {
            throw new InvalidOperationException("Can only start tournament from RegistrationOpen status.");
        }
        Status = TournamentStatus.Ongoing;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Finish()
    {
        if (Status != TournamentStatus.Ongoing)
        {
            throw new InvalidOperationException("Can only finish tournament from Ongoing status.");
        }
        Status = TournamentStatus.Finished;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == TournamentStatus.Finished)
        {
            throw new InvalidOperationException("Cannot cancel a finished tournament.");
        }
        if (IsDeleted)
        {
            throw new InvalidOperationException("Cannot cancel a deleted tournament.");
        }
        Status = TournamentStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        if (IsDeleted)
        {
            throw new InvalidOperationException("Tournament is already deleted.");
        }
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string? NormalizeLocation(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}

// Made with Bob