# Tournament System Design

Comprehensive design for the BBXTournamentPH tournament management system.

## Overview

The tournament system supports multiple formats and provides a complete lifecycle management for competitive Beyblade X tournaments in the Philippines.

---

## Tournament Formats

### 1. Swiss System
**Best for**: Large tournaments with time constraints

**How it works**:
- Players are paired based on similar records
- No elimination - everyone plays all rounds
- Wins determine final standings
- Typically 4-7 rounds depending on player count

**Pairing Algorithm**:
```
Round 1: Random pairing
Round 2+: Pair players with same record
  - Avoid repeat pairings
  - Handle odd numbers with bye
  - Top half plays top half when possible
```

**Advantages**:
- Fair for all skill levels
- Everyone gets multiple matches
- Faster than round robin
- Clear winner determination

**Implementation Priority**: High (Phase 2)

### 2. Round Robin
**Best for**: Smaller tournaments, league play

**How it works**:
- Every player plays every other player once
- Most wins determines winner
- Tiebreakers: head-to-head, point differential

**Match Count Formula**:
```
Total Matches = n(n-1)/2
where n = number of players

Example: 8 players = 28 matches
```

**Advantages**:
- Most fair format
- Complete data for rankings
- No luck factor

**Disadvantages**:
- Time consuming
- Not scalable for large events

**Implementation Priority**: Medium (Phase 2)

### 3. Single Elimination
**Best for**: Finals, playoffs, quick tournaments

**How it works**:
- Bracket-style tournament
- Lose once, you're out
- Seeding determines initial matchups
- Bracket size must be power of 2

**Bracket Sizes**:
- 4, 8, 16, 32, 64 players
- Byes for non-power-of-2 counts

**Advantages**:
- Fast and exciting
- Clear bracket visualization
- Easy to understand

**Disadvantages**:
- One bad match eliminates you
- Less data for rankings
- Seeding heavily impacts results

**Implementation Priority**: Medium (Phase 2)

### 4. Double Elimination (Future)
**Best for**: Competitive finals

**How it works**:
- Winners bracket and losers bracket
- Must lose twice to be eliminated
- Grand finals: winners vs losers champion

**Implementation Priority**: Low (Phase 6)

---

## Tournament Lifecycle

### 1. Creation Phase
```
Admin creates tournament
  ↓
Set format, rules, dates
  ↓
Configure registration
  ↓
Publish tournament
```

**Required Information**:
- Tournament name
- Format (Swiss, Round Robin, Single Elimination)
- Start date and time
- Registration deadline
- Max participants
- Entry requirements
- Rules and regulations

### 2. Registration Phase
```
Tournament published
  ↓
Players register
  ↓
Admin reviews registrations
  ↓
Confirm participants
```

**Registration Features**:
- Open/closed registration
- Approval required option
- Waitlist support
- Registration deadline
- Entry fee tracking (future)

### 3. Check-in Phase
```
Registration closed
  ↓
Check-in opens (1-2 hours before)
  ↓
Players check in
  ↓
Finalize participant list
```

**Check-in Features**:
- QR code check-in
- Mobile-friendly interface
- No-show handling
- Last-minute substitutions

### 4. Active Tournament Phase
```
Tournament starts
  ↓
Generate pairings/bracket
  ↓
Matches played
  ↓
Results submitted
  ↓
Next round generated
  ↓
Repeat until complete
```

**Active Features**:
- Live bracket updates
- Real-time standings
- Match scheduling
- Judge assignments
- Dispute resolution

### 5. Completion Phase
```
Final match completed
  ↓
Calculate final standings
  ↓
Award prizes/points
  ↓
Update rankings
  ↓
Archive tournament
```

**Post-Tournament**:
- Final standings published
- Statistics generated
- Ranking points awarded
- Tournament report
- Photo/video gallery (future)

---

## Match Management

### Match States
```typescript
enum MatchStatus {
  Scheduled = 'scheduled',
  InProgress = 'in_progress',
  Completed = 'completed',
  Disputed = 'disputed',
  Cancelled = 'cancelled'
}
```

### Match Flow
```
Match created
  ↓
Assigned to judge
  ↓
Players called
  ↓
Match starts
  ↓
Judge records results
  ↓
Results submitted
  ↓
Match completed
```

### Match Data Structure
```typescript
interface Match {
  id: string;
  tournamentId: string;
  round: number;
  matchNumber: number;
  
  player1Id: string;
  player2Id: string;
  
  player1Score: number;
  player2Score: number;
  
  winnerId: string | null;
  
  judgeId: string | null;
  tableNumber: number | null;
  
  status: MatchStatus;
  startTime: Date | null;
  endTime: Date | null;
  
  notes: string | null;
}
```

### Judge Interface Requirements
- **Mobile-optimized**: Quick input on phones
- **Offline support**: Works without internet
- **Quick entry**: Minimal taps to record result
- **Validation**: Prevent invalid scores
- **Undo**: Ability to correct mistakes

---

## Scoring System

### Match Scoring
- **Best of 3**: First to 2 points wins
- **Best of 5**: First to 3 points wins (finals)
- **Point types**:
  - Burst Finish: 2 points
  - Over Finish: 1 point
  - Spin Finish: 1 point

### Tournament Points (for rankings)
```
1st place: 100 points
2nd place: 75 points
3rd place: 50 points
4th place: 40 points
5th-8th: 30 points
9th-16th: 20 points
17th-32nd: 10 points
Participation: 5 points
```

**Scaling by tournament size**:
- Small (< 16): 0.5x multiplier
- Medium (16-32): 1.0x multiplier
- Large (32-64): 1.5x multiplier
- Major (64+): 2.0x multiplier

---

## Pairing Algorithms

### Swiss Pairing
```typescript
function generateSwissPairings(
  players: Player[],
  round: number,
  previousMatches: Match[]
): Pairing[] {
  // Sort by current record
  const sorted = sortByRecord(players);
  
  // Group by points
  const groups = groupByPoints(sorted);
  
  // Pair within groups
  const pairings: Pairing[] = [];
  
  for (const group of groups) {
    // Avoid repeat pairings
    const validPairs = findValidPairs(group, previousMatches);
    pairings.push(...validPairs);
  }
  
  // Handle bye if odd number
  if (players.length % 2 === 1) {
    const byePlayer = findByePlayer(players, previousMatches);
    pairings.push({ player1: byePlayer, player2: null });
  }
  
  return pairings;
}
```

### Single Elimination Seeding
```typescript
function generateBracket(players: Player[]): Bracket {
  // Determine bracket size (next power of 2)
  const bracketSize = nextPowerOf2(players.length);
  
  // Seed players
  const seeded = seedPlayers(players);
  
  // Generate first round matchups
  const firstRound = generateFirstRound(seeded, bracketSize);
  
  return {
    size: bracketSize,
    rounds: calculateRounds(bracketSize),
    matches: firstRound
  };
}

function seedPlayers(players: Player[]): Player[] {
  // Sort by ranking
  return players.sort((a, b) => b.ranking - a.ranking);
}
```

---

## Tournament Stages

### Multi-Stage Tournaments
Some tournaments may have multiple stages:

**Example: Championship Tournament**
```
Stage 1: Swiss (4 rounds)
  ↓
Top 8 advance
  ↓
Stage 2: Single Elimination (Top 8)
  ↓
Champion determined
```

**Stage Configuration**:
```typescript
interface TournamentStage {
  id: string;
  tournamentId: string;
  stageNumber: number;
  name: string;
  format: TournamentFormat;
  advancementCriteria: {
    type: 'top_n' | 'min_record' | 'all';
    value: number;
  };
}
```

---

## Tiebreakers

### Swiss System Tiebreakers
1. **Match Points**: Total wins
2. **Opponent Match Win %**: Strength of schedule
3. **Game Win %**: Individual game record
4. **Opponent Game Win %**: Opponents' game records
5. **Head-to-Head**: If only 2 tied

### Round Robin Tiebreakers
1. **Head-to-Head Record**: Direct matchup
2. **Point Differential**: Total points scored vs allowed
3. **Points Scored**: Total offensive points
4. **Playoff Match**: If still tied

---

## Special Cases

### Byes
- Awarded to lowest-ranked player without bye
- Counts as automatic win
- No match played
- Used for odd-numbered tournaments

### No-Shows
- 10-minute grace period
- After grace period: automatic loss
- Repeated no-shows: disqualification
- Affects future tournament eligibility

### Disputes
```
Dispute raised
  ↓
Match paused
  ↓
Head judge reviews
  ↓
Decision made
  ↓
Match continues or result adjusted
```

### Withdrawals
- Before tournament: removed from bracket
- During tournament: all matches become losses
- Affects pairings for remaining rounds

---

## Data Models

### Tournament Entity
```typescript
interface Tournament {
  id: string;
  name: string;
  description: string;
  
  format: TournamentFormat;
  status: TournamentStatus;
  
  startDate: Date;
  endDate: Date;
  registrationDeadline: Date;
  
  maxParticipants: number;
  currentParticipants: number;
  
  location: string;
  organizerId: string;
  
  rules: string;
  prizePool: string | null;
  
  createdAt: Date;
  updatedAt: Date;
}
```

### Participant Entity
```typescript
interface Participant {
  id: string;
  tournamentId: string;
  playerId: string;
  
  status: 'registered' | 'checked_in' | 'active' | 'eliminated' | 'withdrawn';
  seed: number | null;
  
  wins: number;
  losses: number;
  draws: number;
  
  pointsScored: number;
  pointsAllowed: number;
  
  finalPlacement: number | null;
  pointsEarned: number;
  
  registeredAt: Date;
  checkedInAt: Date | null;
}
```

---

## Performance Considerations

### Optimization Strategies
1. **Caching**: Cache tournament brackets and standings
2. **Pagination**: Paginate large participant lists
3. **Indexing**: Index on tournamentId, status, dates
4. **Denormalization**: Store calculated standings
5. **Background Jobs**: Generate pairings asynchronously

### Scalability
- Support up to 128 players per tournament
- Handle 10+ concurrent tournaments
- Real-time updates for active matches
- Archive completed tournaments

---

## Future Enhancements

### Phase 7+ Features
- [ ] Team tournaments
- [ ] Tag team format
- [ ] Custom tournament rules
- [ ] Tournament templates
- [ ] Automated scheduling
- [ ] Live streaming integration
- [ ] Spectator mode
- [ ] Tournament analytics dashboard
- [ ] Prize pool management
- [ ] Sponsorship integration

---

## Implementation Checklist

### Phase 2: Core Tournament System
- [ ] Tournament CRUD operations
- [ ] Swiss system implementation
- [ ] Round Robin implementation
- [ ] Single Elimination implementation
- [ ] Match management
- [ ] Basic pairing algorithms

### Phase 3: Match System
- [ ] Judge interface
- [ ] Real-time updates
- [ ] Match result validation
- [ ] Dispute handling

### Phase 4: Advanced Features
- [ ] Multi-stage tournaments
- [ ] Advanced tiebreakers
- [ ] Tournament statistics
- [ ] Historical data

---

This tournament system is designed to be:
- **Flexible**: Supports multiple formats
- **Scalable**: Handles various tournament sizes
- **Fair**: Implements proper pairing and tiebreakers
- **User-friendly**: Easy for organizers and players