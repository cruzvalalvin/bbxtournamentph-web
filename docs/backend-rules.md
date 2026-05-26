# Backend Development Rules

Guidelines and best practices for ASP.NET Core backend development.

## General Principles

### 1. Keep It Simple
- Don't over-engineer solutions
- Write code for humans first, machines second
- Prefer clarity over cleverness
- YAGNI (You Aren't Gonna Need It)

### 2. Clean Architecture
- Respect layer boundaries
- Dependencies flow inward (toward Domain)
- Domain layer has zero dependencies
- Use interfaces for abstraction

### 3. Consistency
- Follow established patterns
- Use consistent naming conventions
- Maintain code style throughout

---

## Project Structure Rules

### Domain Layer
```csharp
// ✅ DO: Pure business entities
public class Tournament
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public TournamentStatus Status { get; private set; }
    
    public void Start()
    {
        if (Status != TournamentStatus.Pending)
            throw new InvalidOperationException("Tournament already started");
        
        Status = TournamentStatus.Active;
    }
}

// ❌ DON'T: Reference infrastructure concerns
public class Tournament
{
    [Required] // Don't use data annotations
    public string Name { get; set; }
    
    public DbSet<Match> Matches { get; set; } // Don't reference EF Core
}
```

### Application Layer
```csharp
// ✅ DO: Service interfaces and DTOs
public interface ITournamentService
{
    Task<TournamentDto> GetByIdAsync(Guid id);
    Task<TournamentDto> CreateAsync(CreateTournamentDto dto);
}

public class TournamentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
}

// ✅ DO: Keep services focused
public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _repository;
    
    public async Task<TournamentDto> CreateAsync(CreateTournamentDto dto)
    {
        // Validate
        // Create entity
        // Save
        // Return DTO
    }
}

// ❌ DON'T: God services
public class TournamentService
{
    // Handles tournaments, matches, players, rankings, etc.
}
```

### Infrastructure Layer
```csharp
// ✅ DO: Implement repository interfaces
public class TournamentRepository : ITournamentRepository
{
    private readonly ApplicationDbContext _context;
    
    public async Task<Tournament> GetByIdAsync(Guid id)
    {
        return await _context.Tournaments
            .Include(t => t.Matches)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}

// ✅ DO: Configure entities
public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
{
    public void Configure(EntityTypeBuilder<Tournament> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
    }
}
```

### API Layer
```csharp
// ✅ DO: Thin controllers
[ApiController]
[Route("api/[controller]")]
public class TournamentsController : ControllerBase
{
    private readonly ITournamentService _service;
    
    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentDto>> GetById(Guid id)
    {
        var tournament = await _service.GetByIdAsync(id);
        if (tournament == null)
            return NotFound();
        
        return Ok(tournament);
    }
}

// ❌ DON'T: Business logic in controllers
[HttpPost]
public async Task<ActionResult> Create(CreateTournamentRequest request)
{
    // Don't do validation here
    // Don't do business logic here
    // Don't access repository directly
}
```

---

## Naming Conventions

### Classes
```csharp
// Entities (Domain)
public class Tournament { }
public class Match { }
public class Player { }

// Services (Application)
public class TournamentService { }
public interface ITournamentService { }

// Repositories (Infrastructure)
public class TournamentRepository { }
public interface ITournamentRepository { }

// Controllers (API)
public class TournamentsController { }

// DTOs
public class TournamentDto { }
public class CreateTournamentDto { }
public class UpdateTournamentDto { }
```

### Methods
```csharp
// ✅ DO: Descriptive async method names
public async Task<Tournament> GetByIdAsync(Guid id)
public async Task<List<Tournament>> GetAllAsync()
public async Task<Tournament> CreateAsync(Tournament tournament)
public async Task UpdateAsync(Tournament tournament)
public async Task DeleteAsync(Guid id)

// ❌ DON'T: Unclear names
public async Task<Tournament> Get(Guid id) // Missing Async suffix
public async Task<Tournament> DoStuff(Guid id) // Vague
```

### Properties
```csharp
// ✅ DO: PascalCase for public properties
public string Name { get; set; }
public DateTime CreatedAt { get; set; }

// ✅ DO: camelCase for private fields
private readonly ITournamentRepository _repository;
private string _internalState;
```

---

## Error Handling

### Use Exceptions for Exceptional Cases
```csharp
// ✅ DO: Throw for business rule violations
public void Start()
{
    if (Status != TournamentStatus.Pending)
        throw new InvalidOperationException("Tournament already started");
    
    Status = TournamentStatus.Active;
}

// ✅ DO: Use custom exceptions
public class TournamentNotFoundException : Exception
{
    public TournamentNotFoundException(Guid id) 
        : base($"Tournament with ID {id} not found") { }
}

// ❌ DON'T: Use exceptions for flow control
public Tournament GetById(Guid id)
{
    try
    {
        return _repository.GetById(id);
    }
    catch (Exception)
    {
        return null; // Don't do this
    }
}
```

### Global Exception Handling
```csharp
// ✅ DO: Use middleware for global handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        
        var error = context.Features.Get<IExceptionHandlerFeature>();
        if (error != null)
        {
            await context.Response.WriteAsJsonAsync(new
            {
                error = error.Error.Message
            });
        }
    });
});
```

---

## Async/Await Best Practices

```csharp
// ✅ DO: Use async all the way
public async Task<Tournament> GetByIdAsync(Guid id)
{
    return await _repository.GetByIdAsync(id);
}

// ✅ DO: Use ConfigureAwait(false) in libraries
public async Task<Tournament> GetByIdAsync(Guid id)
{
    return await _repository.GetByIdAsync(id).ConfigureAwait(false);
}

// ❌ DON'T: Mix sync and async
public Tournament GetById(Guid id)
{
    return _repository.GetByIdAsync(id).Result; // Deadlock risk!
}

// ❌ DON'T: Async void (except event handlers)
public async void ProcessTournament() // Don't do this
{
    await _service.ProcessAsync();
}
```

---

## Dependency Injection

```csharp
// ✅ DO: Constructor injection
public class TournamentService
{
    private readonly ITournamentRepository _repository;
    private readonly ILogger<TournamentService> _logger;
    
    public TournamentService(
        ITournamentRepository repository,
        ILogger<TournamentService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}

// ✅ DO: Register services properly
services.AddScoped<ITournamentService, TournamentService>();
services.AddScoped<ITournamentRepository, TournamentRepository>();

// ❌ DON'T: Service locator pattern
public class TournamentService
{
    public void DoSomething()
    {
        var repository = ServiceLocator.Get<ITournamentRepository>(); // Anti-pattern
    }
}
```

---

## Database Access

### Use Repository Pattern
```csharp
// ✅ DO: Abstract database access
public interface ITournamentRepository
{
    Task<Tournament> GetByIdAsync(Guid id);
    Task<List<Tournament>> GetAllAsync();
    Task AddAsync(Tournament tournament);
    Task UpdateAsync(Tournament tournament);
    Task DeleteAsync(Guid id);
}

// ❌ DON'T: Use DbContext directly in services
public class TournamentService
{
    private readonly ApplicationDbContext _context; // Don't do this
    
    public async Task<Tournament> GetById(Guid id)
    {
        return await _context.Tournaments.FindAsync(id);
    }
}
```

### Optimize Queries
```csharp
// ✅ DO: Use Include for related data
var tournament = await _context.Tournaments
    .Include(t => t.Matches)
    .ThenInclude(m => m.Players)
    .FirstOrDefaultAsync(t => t.Id == id);

// ✅ DO: Use AsNoTracking for read-only queries
var tournaments = await _context.Tournaments
    .AsNoTracking()
    .ToListAsync();

// ❌ DON'T: N+1 queries
var tournaments = await _context.Tournaments.ToListAsync();
foreach (var tournament in tournaments)
{
    var matches = await _context.Matches
        .Where(m => m.TournamentId == tournament.Id)
        .ToListAsync(); // N+1 problem!
}
```

---

## API Design

### RESTful Conventions
```csharp
// ✅ DO: Follow REST conventions
[HttpGet]                          // GET /api/tournaments
[HttpGet("{id}")]                  // GET /api/tournaments/{id}
[HttpPost]                         // POST /api/tournaments
[HttpPut("{id}")]                  // PUT /api/tournaments/{id}
[HttpDelete("{id}")]               // DELETE /api/tournaments/{id}

// ✅ DO: Return appropriate status codes
return Ok(tournament);             // 200
return Created($"/api/tournaments/{id}", tournament); // 201
return NoContent();                // 204
return NotFound();                 // 404
return BadRequest(errors);         // 400
```

### Request/Response Models
```csharp
// ✅ DO: Separate request/response models from DTOs
public class CreateTournamentRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    
    [Required]
    public string Format { get; set; }
}

public class TournamentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

---

## Security

### Authentication & Authorization
```csharp
// ✅ DO: Use [Authorize] attribute
[Authorize]
[HttpPost]
public async Task<ActionResult> Create(CreateTournamentRequest request)
{
    // Only authenticated users can access
}

// ✅ DO: Role-based authorization
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<ActionResult> Delete(Guid id)
{
    // Only admins can delete
}

// ✅ DO: Validate user permissions
public async Task<ActionResult> Update(Guid id, UpdateTournamentRequest request)
{
    var tournament = await _service.GetByIdAsync(id);
    if (tournament.CreatorId != User.GetUserId())
        return Forbid();
    
    // Update tournament
}
```

### Input Validation
```csharp
// ✅ DO: Validate at API layer
[HttpPost]
public async Task<ActionResult> Create([FromBody] CreateTournamentRequest request)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);
    
    // Process request
}

// ✅ DO: Sanitize user input
public async Task<ActionResult> Search([FromQuery] string query)
{
    query = query?.Trim();
    if (string.IsNullOrEmpty(query))
        return BadRequest("Query cannot be empty");
    
    // Search
}
```

---

## Testing

### Unit Tests
```csharp
// ✅ DO: Test business logic
[Fact]
public void Start_WhenPending_ShouldChangeStatusToActive()
{
    // Arrange
    var tournament = new Tournament("Test Tournament");
    
    // Act
    tournament.Start();
    
    // Assert
    Assert.Equal(TournamentStatus.Active, tournament.Status);
}

// ✅ DO: Test edge cases
[Fact]
public void Start_WhenAlreadyActive_ShouldThrowException()
{
    // Arrange
    var tournament = new Tournament("Test Tournament");
    tournament.Start();
    
    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => tournament.Start());
}
```

---

## Performance

### Caching
```csharp
// ✅ DO: Cache frequently accessed data
public async Task<List<Tournament>> GetActiveAsync()
{
    var cacheKey = "active-tournaments";
    
    if (_cache.TryGetValue(cacheKey, out List<Tournament> tournaments))
        return tournaments;
    
    tournaments = await _repository.GetActiveAsync();
    _cache.Set(cacheKey, tournaments, TimeSpan.FromMinutes(5));
    
    return tournaments;
}
```

### Pagination
```csharp
// ✅ DO: Paginate large result sets
[HttpGet]
public async Task<ActionResult<PagedResult<TournamentDto>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
{
    var result = await _service.GetPagedAsync(page, pageSize);
    return Ok(result);
}
```

---

## Logging

```csharp
// ✅ DO: Log important events
_logger.LogInformation("Tournament {TournamentId} created by user {UserId}", 
    tournament.Id, userId);

// ✅ DO: Log errors with context
_logger.LogError(ex, "Failed to create tournament. Request: {@Request}", request);

// ❌ DON'T: Log sensitive data
_logger.LogInformation("User password: {Password}", password); // Never do this!
```

---

## Code Review Checklist

Before submitting code:
- [ ] Follows layer boundaries
- [ ] Uses async/await properly
- [ ] Has appropriate error handling
- [ ] Includes input validation
- [ ] Has meaningful names
- [ ] Is properly tested
- [ ] Has no security vulnerabilities
- [ ] Follows established patterns
- [ ] Is documented where needed
- [ ] Performs efficiently

---

## Resources

- [ASP.NET Core Best Practices](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/best-practices)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Entity Framework Core Best Practices](https://docs.microsoft.com/en-us/ef/core/performance/)