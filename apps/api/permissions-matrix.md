# Permissions Matrix

## Overview

This document defines the authorization rules and permissions for the BBXTournamentPH platform.

## User Roles

### System-wide Roles
1. **SuperAdmin** - Platform administrator with full access
2. **CommunityOwner** - User who created a community
3. **CommunityAdmin** - User with admin privileges in a community
4. **Judge** - User who can officiate matches and tournaments
5. **Player** - Regular user

## Community Management Permissions

| Action | SuperAdmin | Community Owner | Community Admin | Judge | Player | Anonymous |
|--------|------------|-----------------|-----------------|-------|--------|-----------|
| **Community Operations** |
| Create Community | ✅ | ✅ | ✅ | ✅ | ✅ | ❌ |
| View All Communities | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| View Community Details | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Update Community Info | ✅ | ✅ (own) | ❌ | ❌ | ❌ | ❌ |
| Delete Community | ✅ | ✅ (own) | ❌ | ❌ | ❌ | ❌ |
| Verify Community | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Unverify Community | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Activate/Deactivate Community | ✅ | ✅ (own) | ❌ | ❌ | ❌ | ❌ |
| **Admin Management** |
| Add Admin | ✅ | ✅ (own) | ❌ | ❌ | ❌ | ❌ |
| Remove Admin | ✅ | ✅ (own) | ❌ | ❌ | ❌ | ❌ |
| Update Admin Role | ✅ | ✅ (own) | ❌ | ❌ | ❌ | ❌ |
| View Admins | ✅ | ✅ | ✅ (same community) | ✅ (same community) | ✅ | ✅ |
| **Member Management** |
| View Members | ✅ | ✅ | ✅ (same community) | ✅ (same community) | ❌ | ❌ |
| Join Community | ✅ | ✅ | ✅ | ✅ | ✅ | ❌ |
| Leave Community | ✅ | ✅ | ✅ | ✅ | ✅ | ❌ |
| Remove Member | ✅ | ✅ (own) | ✅ (same community) | ❌ | ❌ | ❌ |

## Authentication Requirements

### Public Endpoints (No Authentication Required)
- `GET /communities` - List all communities
- `GET /communities/{id}` - Get community details
- `POST /auth/register` - User registration
- `POST /auth/login` - User login

### Protected Endpoints (Authentication Required)
- `POST /communities` - Create community
- `POST /communities/{id}/admins` - Add admin
- `DELETE /communities/{id}/admins/{userId}` - Remove admin
- `GET /auth/me` - Get current user

### SuperAdmin Only Endpoints (Future)
- `POST /communities/{id}/verify` - Verify community
- `POST /communities/{id}/unverify` - Unverify community
- `GET /users` - List all users
- `PUT /users/{id}` - Update user
- `DELETE /users/{id}` - Delete user

## Authorization Rules

### Community Creation
```
Rule: Any authenticated user can create a community
Implementation: JWT token required
Default State: IsVerified = false, IsActive = true
```

### Community Verification
```
Rule: Only SuperAdmin can verify communities
Implementation: Check user.Role == SuperAdmin
Effect: Sets IsVerified = true
```

### Admin Management
```
Rule: Only community owner can manage admins
Implementation: Check community.OwnerId == requesterId
Constraints:
  - User can only have one admin role per community
  - Owner cannot be removed as admin
  - Cannot add duplicate admins
```

### Member Viewing
```
Rule: Only owner and admins can view member list
Implementation: Check if user is owner or admin of the community
Future: May be expanded to allow members to view other members
```

## Role Hierarchy

```
SuperAdmin (Level 1)
    ↓
Community Owner (Level 2)
    ↓
Community Admin (Level 3)
    ↓
Judge (Level 4)
    ↓
Player (Level 5)
```

Higher levels inherit permissions from lower levels where applicable.

## Implementation Notes

### Current Implementation (Alpha)
- Basic role checking using JWT claims
- Owner validation for admin management
- Simple authorization in service layer

### Future Enhancements
- Policy-based authorization
- Resource-based authorization
- Fine-grained permissions
- Role-based access control (RBAC)
- Attribute-based access control (ABAC)
- Audit logging for sensitive operations

## Security Considerations

1. **JWT Token Validation**
   - All protected endpoints validate JWT token
   - Token contains user ID and role
   - Token expiration enforced

2. **Owner Verification**
   - Community owner ID checked against requester ID
   - Prevents unauthorized admin management

3. **Input Validation**
   - All inputs validated at controller level
   - Model validation using data annotations
   - Business rule validation in service layer

4. **SQL Injection Prevention**
   - Entity Framework Core parameterized queries
   - No raw SQL queries in current implementation

5. **Authorization Checks**
   - Performed before any data modification
   - Fail-safe: deny by default
   - Clear error messages for unauthorized access

## Error Responses

### 401 Unauthorized
- Missing or invalid JWT token
- Token expired

### 403 Forbidden
- Valid token but insufficient permissions
- Not the community owner when required

### 404 Not Found
- Resource doesn't exist
- User doesn't have permission to view

### 400 Bad Request
- Validation errors
- Business rule violations

## Testing Checklist

- [ ] Anonymous users cannot access protected endpoints
- [ ] Authenticated users can create communities
- [ ] Only owners can manage admins
- [ ] Only SuperAdmin can verify communities
- [ ] Duplicate admin prevention works
- [ ] Slug uniqueness is enforced
- [ ] JWT token validation works correctly
- [ ] Error messages don't leak sensitive information

// Made with Bob