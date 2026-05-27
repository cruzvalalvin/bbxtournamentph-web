# Tournament Architecture

## Overview

The Tournament Foundation provides a scalable, flexible structure for managing competitive gaming tournaments in BBXTournamentPH. This is the foundational layer that supports multiple tournament formats without implementing specific bracket generation logic.

## Core Entities

### Tournament
The main tournament entity that represents a competitive event.

**Key Properties:**
- `Id`: Unique identifier
- `CommunityId`: Links tournament to a community
- `Name`: Tournament name
- `Description`: Tournament details
- `Status`: Current state (Draft, RegistrationOpen, Ongoing, Finished, Cancelled)
- `MaxParticipants`: Maximum number of participants allowed
- `Location`: Region, Province, City (optional)
- `CreatedById`: User who created the tournament
- `Timestamps`: CreatedAt, UpdatedAt

**Relationships:**
- Belongs to one Community
- Created by one User
- Has many Stages
- Has many Participants

### TournamentStage
Represents a phase or bracket within a tournament. Tournaments can have multiple stages (e.g., Swiss rounds followed by single elimination playoffs).

**Key Properties:**
- `Id`: Unique identifier
- `TournamentId`: Parent tournament
- `Name`: Stage name (e.g., "Swiss Rounds", "Top 8 Playoffs")
- `StageOrder`: Sequence number (must be unique per tournament)
- `FormatType`: SingleElimination, DoubleElimination, RoundRobin, Swiss
- `NumberOfRounds`: How many rounds in this stage
- `GroupCount`: For group stages (optional)
- `AdvanceCount`: How many advance to next stage (optional)
- `HasThirdPlaceMatch`: For elimination brackets
- `HasGrandFinalReset`: For double elimination
- `Status`: Current state

**Relationships:**
- Belongs to one Tournament
- Has many Matches
- Has many Standings

### TournamentParticipant
Represents a player or team registered for a tournament.

**Key Properties:**
- `Id`: Unique identifier
- `TournamentId`: Parent tournament
- `UserId`: Linked user account (nullable for manual entries)
- `DisplayName`: Participant display name
- `TeamName`: Team name (optional)
- `IsManualEntry`: True if added manually by organizer
- `IsPaid`: Payment status
- `Seed`: Seeding position (optional)
- `CheckedIn`: Check-in status
- `CreatedAt`: Registration timestamp

**Relationships:**
- Belongs to one Tournament
- Optionally linked to one User

### Match
Represents a single match/game between participants.

**Key Properties:**
- `Id`: Unique identifier
- `TournamentStageId`: Parent stage
- `RoundNumber`: Which round this match belongs to
- `MatchNumber`: Match identifier within the round
- `Player1Id`, `Player2Id`: Participant references
- `WinnerId`, `LoserId`: Match result
- `Score1`, `Score2`: Match scores
- `IsBye`: True if this is a bye match
- `Status`: Pending, Ongoing, Completed
- `CreatedAt`: Creation timestamp

**Relationships:**
- Belongs to one TournamentStage
- References TournamentParticipants for players and results

### Standing
Tracks participant performance within a stage. Used for Swiss, Round Robin, and group stages.

**Key Properties:**
- `Id`: Unique identifier
- `TournamentStageId`: Parent stage
- `ParticipantId`: Participant reference
- `Wins`, `Losses`, `Draws`: Match record
- `MatchPoints`: Total points earned
- `PointDifference`: Score differential
- `Buchholz`: Tiebreaker for Swiss
- `MedianBuchholz`: Tiebreaker for Swiss
- `HeadToHeadScore`: Direct matchup tiebreaker
- `Rank`: Current ranking

**Relationships:**
- Belongs to one TournamentStage
- References one TournamentParticipant

## Tournament Status Flow

```
Draft
  ↓
RegistrationOpen
  ↓
Ongoing
  ↓
Finished

(Can be Cancelled from any state except Finished)
```

### Status Descriptions

**Draft**
- Initial state when tournament is created
- Organizers can configure settings
- Not visible to public (future feature)
- Can add stages and configure format

**RegistrationOpen**
- Participants can register
- Tournament details are locked
- Can still modify participant list manually
- Can close registration to move to Ongoing

**Ongoing**
- Tournament is in progress
- Matches are being played
- Results are being recorded
- Cannot add new participants

**Finished**
- Tournament is complete
- All matches resolved
- Final standings calculated
- Cannot be modified

**Cancelled**
- Tournament was cancelled
- Can be set from any state except Finished
- Preserves data for record-keeping

## Stage Format Types

### SingleElimination
- One loss and you're out
- Bracket size must be power of 2 (or use byes)
- Fast tournament format
- Optional third-place match

### DoubleElimination
- Two brackets: Winners and Losers
- Need to lose twice to be eliminated
- Optional grand final reset
- More forgiving format

### RoundRobin
- Everyone plays everyone
- Best for small groups
- Fair but time-consuming
- Can be used for group stages

### Swiss
- Pair players with similar records
- No elimination
- Efficient for large tournaments
- Uses tiebreakers for final ranking

## Database Schema

### Unique Constraints
- `TournamentStages`: (TournamentId, StageOrder) must be unique
- `Standings`: (TournamentStageId, ParticipantId) must be unique

### Cascade Behavior
- Deleting a Tournament cascades to Stages and Participants
- Deleting a Stage cascades to Matches and Standings
- User and Community deletions are restricted if referenced

### Indexes
- TournamentId on Stages, Participants
- TournamentStageId on Matches, Standings
- UserId on Participants (for quick lookups)

## API Endpoints

### Tournament Management
```
POST   /tournaments              - Create tournament
GET    /tournaments              - List all tournaments
GET    /tournaments/{id}         - Get tournament details
```

### Participant Management
```
POST   /tournaments/{id}/participants  - Add participant
GET    /tournaments/{id}/participants  - List participants
```

### Stage Management
```
POST   /tournaments/{id}/stages  - Create stage
GET    /tournaments/{id}/stages  - List stages
```

## Business Rules

### Tournament Creation
- Must belong to a verified community
- Creator must be community owner or admin
- Starts in Draft status
- MaxParticipants must be > 0

### Participant Registration
- Cannot exceed MaxParticipants
- User can only register once per tournament
- Manual entries don't require user account
- Registration only allowed in RegistrationOpen status

### Stage Creation
- StageOrder must be unique per tournament
- Can only create stages in Draft status
- FormatType is required
- NumberOfRounds must be > 0

### Match Management
- Matches belong to a stage
- Can only report scores in Ongoing status
- Winner/Loser determined by scores
- Bye matches auto-complete

### Standing Calculation
- One standing per participant per stage
- Updated after each match result
- Tiebreakers calculated on demand
- Rankings updated after standings change

## Future Enhancements (Not in Foundation)

### Bracket Generation
- Automatic bracket creation based on format
- Seeding algorithms
- Bye distribution

### Swiss Pairing
- Automatic pairing based on standings
- Avoid repeat matchups
- Color balancing

### Round Robin Scheduling
- Generate all matches upfront
- Optimize for venue/time constraints

### Advanced Features
- Live scoring
- Match scheduling
- Bracket visualization
- Real-time updates
- Tournament templates
- Prize distribution
- Statistics and analytics

## Alpha Development Notes

This foundation focuses on:
- **Data Structure**: Solid entity relationships
- **Flexibility**: Support multiple formats
- **Scalability**: Can add features without breaking changes
- **Simplicity**: No complex algorithms yet
- **Manual Control**: Organizers can manage everything

The goal is to have a working structure that can be enhanced with automated features in future iterations.

## Integration Points

### Community System
- Tournaments belong to communities
- Community admins can manage tournaments
- Community verification affects tournament visibility

### User System
- Users can create tournaments
- Users can register as participants
- User roles determine permissions

### Future Systems
- Ranking system will use tournament results
- Notification system for match updates
- Payment system for entry fees
- Streaming integration for live matches

// Made with Bob