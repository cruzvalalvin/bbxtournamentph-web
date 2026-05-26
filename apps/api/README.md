# BBXTournamentPH API

ASP.NET Core Web API for the BBXTournamentPH platform.

## Architecture

Clean Architecture Lite with four layers:

### 1. **BBXTournament.Api** (Presentation Layer)
- Controllers and endpoints
- Request/response models
- API configuration
- Middleware

### 2. **BBXTournament.Application** (Application Layer)
- Business logic and services
- Use cases and commands
- DTOs and mapping
- Interfaces for infrastructure

### 3. **BBXTournament.Domain** (Domain Layer)
- Core business entities
- Domain models
- Business rules
- Domain events

### 4. **BBXTournament.Infrastructure** (Infrastructure Layer)
- Database context and repositories
- External service integrations
- Authentication implementation
- Data persistence

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code with C# extension

### Running the API

```bash
# Navigate to API directory
cd apps/api

# Restore dependencies
dotnet restore

# Run the API
dotnet run --project BBXTournament.Api
```

The API will be available at:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5001`
- Swagger UI: `https://localhost:7001/swagger`

### Database Setup

```bash
# Add Entity Framework tools (if not installed)
dotnet tool install --global dotnet-ef

# Create initial migration (when ready)
dotnet ef migrations add InitialCreate --project BBXTournament.Infrastructure --startup-project BBXTournament.Api

# Update database
dotnet ef database update --project BBXTournament.Infrastructure --startup-project BBXTournament.Api
```

## Project Structure

```
apps/api/
├── BBXTournament.Api/
│   ├── Controllers/          # API endpoints
│   ├── Program.cs           # Application entry point
│   └── appsettings.json     # Configuration
│
├── BBXTournament.Application/
│   ├── Services/            # Business logic services
│   ├── DTOs/                # Data transfer objects
│   └── Interfaces/          # Service contracts
│
├── BBXTournament.Domain/
│   ├── Entities/            # Domain models
│   ├── Enums/               # Domain enumerations
│   └── ValueObjects/        # Value objects
│
└── BBXTournament.Infrastructure/
    ├── Data/                # DbContext and configurations
    ├── Repositories/        # Data access implementations
    └── Services/            # External service implementations
```

## Features to Implement

### Phase 1: Core Tournament System
- [ ] Tournament CRUD operations
- [ ] Tournament formats (Swiss, Round Robin, Single Elimination)
- [ ] Tournament stages and progression
- [ ] Basic player registration

### Phase 2: Match Management
- [ ] Match creation and scheduling
- [ ] Judge match input system
- [ ] Match results and scoring
- [ ] Real-time match updates

### Phase 3: Rankings & Communities
- [ ] Player rankings calculation
- [ ] Community/league management
- [ ] Seasonal rankings
- [ ] Leaderboards

### Phase 4: Authentication & Authorization
- [ ] JWT authentication
- [ ] Role-based access (Player, Judge, Admin)
- [ ] User registration and login
- [ ] Password management

## API Endpoints (Planned)

### Tournaments
- `GET /api/tournaments` - List all tournaments
- `GET /api/tournaments/{id}` - Get tournament details
- `POST /api/tournaments` - Create tournament
- `PUT /api/tournaments/{id}` - Update tournament
- `DELETE /api/tournaments/{id}` - Delete tournament

### Matches
- `GET /api/matches` - List matches
- `GET /api/matches/{id}` - Get match details
- `POST /api/matches` - Create match
- `PUT /api/matches/{id}/result` - Submit match result

### Players
- `GET /api/players` - List players
- `GET /api/players/{id}` - Get player profile
- `GET /api/players/{id}/stats` - Get player statistics

### Rankings
- `GET /api/rankings` - Get current rankings
- `GET /api/rankings/community/{id}` - Get community rankings

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BBXTournamentPH;..."
  },
  "JwtSettings": {
    "Secret": "your-secret-key",
    "Issuer": "BBXTournamentPH",
    "Audience": "BBXTournamentPH",
    "ExpirationInMinutes": 60
  }
}
```

### Environment Variables (Production)
- `ConnectionStrings__DefaultConnection`
- `JwtSettings__Secret`

## Development Guidelines

### Service Layer Pattern
- Keep controllers thin
- Business logic in Application layer services
- Use dependency injection
- Return DTOs, not domain entities

### Repository Pattern
- Abstract data access in Infrastructure
- Use interfaces in Application layer
- Keep repositories focused and simple

### Error Handling
- Use global exception middleware
- Return consistent error responses
- Log errors appropriately

### Testing
- Unit tests for business logic
- Integration tests for API endpoints
- Use in-memory database for testing

## Deployment

The API can be deployed to:
- Azure App Service
- AWS Elastic Beanstalk
- Docker containers
- Traditional IIS hosting

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server
- **Authentication**: JWT Bearer tokens
- **API Documentation**: Swagger/OpenAPI
- **Logging**: Built-in ASP.NET Core logging

## Contributing

Follow clean architecture principles:
1. Domain layer has no dependencies
2. Application layer depends only on Domain
3. Infrastructure implements Application interfaces
4. API layer orchestrates everything

---

Built for the Philippine Beyblade X competitive community