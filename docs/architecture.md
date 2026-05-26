# Architecture Overview

BBXTournamentPH uses a monorepo architecture with clear separation between frontend and backend.

## System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                         Frontend                             │
│                    (Next.js 15 + React)                      │
│                                                              │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │   App    │  │Components│  │ Features │  │ Services │   │
│  │  Router  │  │   (UI)   │  │(Business)│  │   (API)  │   │
│  └──────────┘  └──────────┘  └──────────┘  └──────────┘   │
└─────────────────────────────────────────────────────────────┘
                            │
                            │ REST API / WebSocket
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                         Backend                              │
│                  (ASP.NET Core 8.0)                          │
│                                                              │
│  ┌──────────────────────────────────────────────────────┐  │
│  │                    API Layer                          │  │
│  │              (Controllers, Middleware)                │  │
│  └──────────────────────────────────────────────────────┘  │
│                            │                                 │
│  ┌──────────────────────────────────────────────────────┐  │
│  │               Application Layer                       │  │
│  │          (Services, DTOs, Use Cases)                  │  │
│  └──────────────────────────────────────────────────────┘  │
│                            │                                 │
│  ┌──────────────────────────────────────────────────────┐  │
│  │                 Domain Layer                          │  │
│  │         (Entities, Business Rules)                    │  │
│  └──────────────────────────────────────────────────────┘  │
│                            │                                 │
│  ┌──────────────────────────────────────────────────────┐  │
│  │            Infrastructure Layer                       │  │
│  │      (Database, Repositories, External APIs)          │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
                    ┌───────────────┐
                    │   SQL Server  │
                    └───────────────┘
```

## Frontend Architecture

### Technology Stack
- **Framework**: Next.js 15 (App Router)
- **UI Library**: React 19
- **Styling**: TailwindCSS 4
- **Language**: TypeScript
- **State Management**: React hooks + Context API (when needed)

### Directory Structure
```
apps/web/
├── app/                    # Next.js App Router
│   ├── layout.tsx         # Root layout
│   ├── page.tsx           # Home page
│   └── globals.css        # Global styles
├── components/            # Shared UI components
├── features/              # Feature modules
│   ├── tournaments/
│   ├── rankings/
│   └── judges/
├── services/              # API clients
├── hooks/                 # Custom React hooks
├── types/                 # TypeScript definitions
└── styles/                # Additional styles
```

### Design Principles
1. **Component-Based**: Reusable, composable UI components
2. **Feature-First**: Organize by feature, not by type
3. **Type-Safe**: Full TypeScript coverage
4. **Server Components**: Use RSC where appropriate
5. **Client Components**: Only when interactivity needed

## Backend Architecture

### Clean Architecture Lite

#### 1. Domain Layer (Core)
- **Purpose**: Business entities and rules
- **Dependencies**: None
- **Contains**:
  - Entities (Tournament, Match, Player)
  - Value Objects
  - Domain Events
  - Business Rules

#### 2. Application Layer
- **Purpose**: Business logic and use cases
- **Dependencies**: Domain only
- **Contains**:
  - Service interfaces
  - DTOs
  - Use case implementations
  - Business workflows

#### 3. Infrastructure Layer
- **Purpose**: External concerns
- **Dependencies**: Application, Domain
- **Contains**:
  - Database context
  - Repositories
  - External API clients
  - File storage
  - Email services

#### 4. API Layer (Presentation)
- **Purpose**: HTTP interface
- **Dependencies**: Application, Infrastructure
- **Contains**:
  - Controllers
  - Request/Response models
  - Middleware
  - API configuration

### Technology Stack
- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server
- **Authentication**: JWT Bearer
- **API Docs**: Swagger/OpenAPI

## Data Flow

### Request Flow (Frontend → Backend)
```
User Action
    ↓
React Component
    ↓
Service/API Client
    ↓
HTTP Request
    ↓
API Controller
    ↓
Application Service
    ↓
Repository
    ↓
Database
```

### Response Flow (Backend → Frontend)
```
Database
    ↓
Repository
    ↓
Application Service (maps to DTO)
    ↓
API Controller
    ↓
HTTP Response
    ↓
Service/API Client
    ↓
React Component (updates state)
    ↓
UI Update
```

## Key Architectural Decisions

### 1. Monorepo Structure
**Decision**: Use npm workspaces for monorepo
**Rationale**: 
- Simplified dependency management
- Atomic commits across frontend/backend
- Easier code sharing
- Single repository to maintain

### 2. Clean Architecture for Backend
**Decision**: Use Clean Architecture Lite (4 layers)
**Rationale**:
- Clear separation of concerns
- Testable business logic
- Flexible infrastructure
- Not over-engineered for startup scale

### 3. Next.js App Router
**Decision**: Use Next.js 15 with App Router
**Rationale**:
- Modern React patterns (RSC)
- Built-in routing
- Excellent performance
- Great developer experience
- Easy Vercel deployment

### 4. Service Layer Pattern
**Decision**: Use service layer for business logic
**Rationale**:
- Keep controllers thin
- Reusable business logic
- Easier testing
- Clear responsibility boundaries

### 5. JWT Authentication
**Decision**: Use JWT for authentication
**Rationale**:
- Stateless authentication
- Works well with SPA
- Easy to implement
- Industry standard

## Scalability Considerations

### Current Scale (Alpha/Beta)
- Single database instance
- Monolithic API
- Simple caching strategy
- Direct database queries

### Future Scale (Production)
- Database read replicas
- Redis caching layer
- CDN for static assets
- Horizontal API scaling
- Background job processing

## Security Architecture

### Frontend Security
- Environment variables for sensitive data
- HTTPS only in production
- XSS protection via React
- CSRF tokens for mutations
- Input validation

### Backend Security
- JWT token validation
- Role-based authorization
- SQL injection prevention (EF Core)
- Rate limiting
- CORS configuration
- Secure password hashing

## Deployment Architecture

### Frontend (Vercel)
```
GitHub Push
    ↓
Vercel Build
    ↓
Deploy to Edge Network
    ↓
Global CDN Distribution
```

### Backend (Azure/AWS)
```
GitHub Push
    ↓
CI/CD Pipeline
    ↓
Build & Test
    ↓
Deploy to App Service
    ↓
Database Migration
```

## Development Workflow

### Local Development
1. Frontend: `npm run dev:web` (port 3000)
2. Backend: `dotnet run` (port 5001/7001)
3. Database: SQL Server LocalDB

### Testing Strategy
- **Unit Tests**: Business logic in Application layer
- **Integration Tests**: API endpoints
- **E2E Tests**: Critical user flows (future)

## Monitoring & Observability

### Logging
- Frontend: Console + Error tracking service
- Backend: Structured logging (Serilog)
- Database: Query performance monitoring

### Metrics (Future)
- API response times
- Error rates
- User activity
- Tournament statistics

## Technology Choices Rationale

### Why Next.js?
- Best-in-class React framework
- Excellent performance
- Great DX
- Easy deployment
- Strong community

### Why ASP.NET Core?
- High performance
- Mature ecosystem
- Strong typing with C#
- Excellent tooling
- Enterprise-ready when needed

### Why SQL Server?
- Relational data fits tournament structure
- Strong consistency guarantees
- Excellent tooling
- Azure integration
- Familiar to .NET developers

### Why Clean Architecture?
- Maintainable codebase
- Testable business logic
- Flexible infrastructure
- Industry best practice
- Not over-engineered

## Anti-Patterns to Avoid

❌ **Don't**:
- Over-engineer for scale you don't have
- Add microservices prematurely
- Use complex state management unnecessarily
- Create abstractions without clear benefit
- Optimize prematurely

✅ **Do**:
- Keep it simple and maintainable
- Write clean, readable code
- Test critical business logic
- Document architectural decisions
- Refactor when patterns emerge

---

This architecture is designed to be:
- **Simple**: Easy to understand and maintain
- **Scalable**: Can grow with the platform
- **Practical**: Fits startup development workflow
- **Modern**: Uses current best practices