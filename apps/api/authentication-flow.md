# Authentication Flow

## Overview

BBXTournamentPH backend uses a simple JWT-based authentication foundation built with Clean Architecture Lite.

## Register

**Endpoint:** `POST /auth/register`

### Request
```json
{
  "displayName": "Juan Dela Cruz",
  "email": "juan@example.com",
  "password": "StrongPassword123!",
  "role": "Player",
  "region": "NCR",
  "province": "Metro Manila",
  "city": "Quezon City"
}
```

### Behavior
- Validates required fields
- Validates email format
- Defaults role to `Player`
- Hashes password before saving
- Creates the user in SQL Server through EF Core
- Returns a JWT token and basic user profile

## Login

**Endpoint:** `POST /auth/login`

### Request
```json
{
  "email": "juan@example.com",
  "password": "StrongPassword123!"
}
```

### Behavior
- Validates credentials
- Verifies hashed password
- Rejects inactive users
- Returns a JWT token and basic user profile

## Current User

**Endpoint:** `GET /users/me`

### Behavior
- Requires `Authorization: Bearer {token}`
- Reads user ID from JWT claims
- Returns the current authenticated user profile

## JWT Claims

Current token generation includes:
- User ID
- Display name
- Email
- Role

## Startup Notes

- SQL Server connection is configured through `ConnectionStrings:DefaultConnection`
- JWT is configured through `JwtSettings`
- Database migrations are applied on startup with `Database.Migrate()`

## Next Suggested Steps

- Add refresh token strategy if needed
- Add email verification later
- Add forgot/reset password flow later
- Add audit logging for sign-in events