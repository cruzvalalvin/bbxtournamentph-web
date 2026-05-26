# Roles and Permissions

## User Roles

### SuperAdmin
Platform-level access.
- Full system administration
- Manage all communities, tournaments, and users
- Assign elevated roles

### CommunityOwner
Top-level owner of a specific community or league.
- Manage community settings
- Manage community administrators
- Oversee tournaments inside the community

### CommunityAdmin
Operational admin for community activities.
- Manage tournaments and event operations
- Manage players and local staff access
- Support judges and organizers

### Judge
Match and rules enforcement role.
- Submit match results
- Validate match outcomes
- Support tournament operations

### Player
Default user role.
- Register and log in
- Join tournaments when enabled
- View profile and competition data

## Current Authorization Foundation

The backend currently supports:
- JWT authentication
- Role claim issuance in tokens
- ASP.NET Core `[Authorize]` usage
- Future role-based policies through `Authorize(Roles = "...")`

## Current Default

If no role is provided during registration, the user defaults to:
- `Player`

## Notes for Startup Development

- Keep role checks at controller or service boundaries
- Avoid complex permission matrices early
- Start with role-based authorization and expand only when needed
- Reserve `SuperAdmin` creation for secure internal workflows or seeded data