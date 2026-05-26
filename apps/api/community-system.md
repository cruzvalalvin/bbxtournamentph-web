# Community Management System

## Overview

The Community Management System allows users to create and manage gaming communities within BBXTournamentPH. Communities serve as organizational units for tournaments, events, and player groups.

## Core Concepts

### Community
A community represents a gaming organization or group. Each community has:
- **Owner**: The user who created the community
- **Admins**: Users with elevated permissions to manage the community
- **Members**: Regular users who belong to the community
- **Verification Status**: Communities must be verified by SuperAdmin before being fully active

### Roles

#### Community Roles
- **Owner**: Full control over the community (assigned at creation)
- **Admin**: Can manage community settings and members
- **Judge**: Can officiate tournaments and matches

#### User Roles (System-wide)
- **SuperAdmin**: Can verify/unverify communities
- **Player**: Regular user who can create communities

## Features

### Community Creation
- Any authenticated user can create a community
- Communities are created in an **unverified** state by default
- Only SuperAdmin can verify communities
- Required fields: Name, Slug, Description
- Optional fields: Logo, Banner, Location (Region, Province, City)

### Community Management
- **Owner** can:
  - Update community information
  - Add/remove admins
  - Assign roles to admins (Admin, Judge)
  - View all members
  - Deactivate the community

### Slug System
- Each community has a unique slug (URL-friendly identifier)
- Slugs are automatically converted to lowercase
- Used for SEO-friendly URLs

## API Endpoints

### Public Endpoints
```
GET /communities
GET /communities/{id}
```

### Protected Endpoints (Requires Authentication)
```
POST /communities
POST /communities/{id}/admins
DELETE /communities/{id}/admins/{userId}
```

## Database Schema

### Communities Table
- Id (GUID, Primary Key)
- Name (string, required, max 200)
- Slug (string, required, unique, max 200)
- Description (string, required, max 1000)
- LogoUrl (string, optional, max 500)
- BannerUrl (string, optional, max 500)
- Region (string, optional, max 100)
- Province (string, optional, max 100)
- City (string, optional, max 100)
- OwnerId (GUID, Foreign Key to Users)
- IsVerified (boolean, default: false)
- IsActive (boolean, default: true)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)

### CommunityAdmins Table
- Id (GUID, Primary Key)
- CommunityId (GUID, Foreign Key to Communities)
- UserId (GUID, Foreign Key to Users)
- Role (string: Owner, Admin, Judge)
- CreatedAt (DateTime)
- Unique constraint: (CommunityId, UserId)

### CommunityMembers Table
- Id (GUID, Primary Key)
- CommunityId (GUID, Foreign Key to Communities)
- UserId (GUID, Foreign Key to Users)
- JoinedAt (DateTime)
- Unique constraint: (CommunityId, UserId)

## Business Rules

1. **Community Creation**
   - Any authenticated user can create a community
   - New communities default to unverified status
   - Owner is automatically set to the creator

2. **Verification**
   - Only SuperAdmin can verify communities
   - Unverified communities are visible but may have limited features

3. **Admin Management**
   - Only the community owner can add/remove admins
   - A user can only have one admin role per community
   - Owner cannot be removed as admin

4. **Slug Uniqueness**
   - Slugs must be unique across all communities
   - Validation occurs at creation time

## Future Enhancements (Not in Alpha)

- Community member management (join/leave)
- Community settings and preferences
- Community statistics and analytics
- Community tournaments and events
- Community rankings and leaderboards
- Community social features (posts, announcements)
- Community moderation tools

## Alpha Development Notes

This is a foundational implementation focused on:
- Core community CRUD operations
- Basic admin management
- Simple authorization rules
- Clean, maintainable code structure

Advanced features will be added in future iterations based on user feedback and requirements.

// Made with Bob