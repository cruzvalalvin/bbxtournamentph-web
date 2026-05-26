# Ranking System Design

Comprehensive player ranking and statistics system for BBXTournamentPH.

## Overview

The ranking system provides fair, transparent, and competitive player rankings based on tournament performance, encouraging active participation and skill development.

---

## Ranking Philosophy

### Core Principles
1. **Performance-Based**: Rankings reflect actual tournament results
2. **Activity Rewarded**: Regular participation is encouraged
3. **Decay Over Time**: Inactive players gradually lose ranking
4. **Transparent**: Clear calculation methodology
5. **Fair**: No pay-to-win mechanics

### Goals
- Identify top players in the community
- Provide matchmaking data for tournaments
- Track player progression over time
- Create competitive incentives
- Enable league/division systems

---

## Ranking Calculation

### Base Point System

#### Tournament Placement Points
```
1st place:    100 points
2nd place:    75 points
3rd place:    50 points
4th place:    40 points
5th-8th:      30 points
9th-16th:     20 points
17th-32nd:    10 points
Participation: 5 points
```

#### Tournament Size Multiplier
```
Small (< 16 players):     0.5x
Medium (16-32 players):   1.0x
Large (32-64 players):    1.5x
Major (64+ players):      2.0x
```

#### Tournament Tier Multiplier
```
Community:     1.0x
Regional:      1.5x
National:      2.0x
Championship:  3.0x
```

### Final Points Formula
```
Points = Base Points × Size Multiplier × Tier Multiplier

Example:
1st place in Large Regional tournament
= 100 × 1.5 × 1.5
= 225 points
```

---

## ELO-Based Rating System

### Why ELO?
- Proven system used in chess, esports
- Accounts for opponent strength
- Self-correcting over time
- Predictive of match outcomes

### ELO Calculation
```typescript
function calculateELO(
  playerRating: number,
  opponentRating: number,
  result: 1 | 0.5 | 0, // win, draw, loss
  kFactor: number = 32
): number {
  // Expected score
  const expected = 1 / (1 + Math.pow(10, (opponentRating - playerRating) / 400));
  
  // New rating
  const newRating = playerRating + kFactor * (result - expected);
  
  return Math.round(newRating);
}
```

### K-Factor (Rating Volatility)
```
New players (< 30 matches):    K = 40
Established (30-100 matches):  K = 32
Veterans (100+ matches):       K = 24
```

### Starting Rating
- New players: 1200 ELO
- Provisional period: First 20 matches
- Rating more volatile during provisional

---

## Ranking Tiers

### Tier System
```
Master:        2000+ ELO
Diamond:       1800-1999 ELO
Platinum:      1600-1799 ELO
Gold:          1400-1599 ELO
Silver:        1200-1399 ELO
Bronze:        1000-1199 ELO
Beginner:      < 1000 ELO
```

### Tier Benefits
- **Master**: Exclusive tournaments, special badge
- **Diamond**: Priority tournament registration
- **Platinum**: Featured on leaderboards
- **Gold**: Community recognition
- **Silver**: Standard tier
- **Bronze**: Learning resources
- **Beginner**: Tutorial access, mentorship

---

## Seasonal Rankings

### Season Structure
```
Season Duration: 3 months
Seasons per year: 4

Season 1: January - March
Season 2: April - June
Season 3: July - September
Season 4: October - December
```

### Season Points
- Separate from overall ELO
- Reset each season
- Determines seasonal rewards
- Contributes to annual championship seeding

### Season Rewards
```
Top 1:     Championship invitation + trophy
Top 3:     Special badge + prizes
Top 8:     Season badge
Top 16:    Recognition on leaderboard
```

---

## Community Rankings

### Community-Specific Rankings
Each community/league has its own rankings:
- Local leaderboards
- Community champions
- Inter-community competitions
- Community pride and rivalry

### Community Points
- Earned only in community tournaments
- Separate from global rankings
- Used for community leagues
- Promotes local engagement

---

## Ranking Decay

### Inactivity Decay
To keep rankings current and encourage participation:

```typescript
function applyDecay(rating: number, daysSinceLastMatch: number): number {
  if (daysSinceLastMatch < 30) {
    return rating; // No decay
  }
  
  const monthsInactive = Math.floor(daysSinceLastMatch / 30);
  const decayRate = 0.02; // 2% per month
  
  const decayedRating = rating * Math.pow(1 - decayRate, monthsInactive);
  
  return Math.max(decayedRating, 1000); // Minimum 1000 ELO
}
```

### Decay Schedule
```
0-30 days:     No decay
31-60 days:    2% decay
61-90 days:    4% decay
91-120 days:   6% decay
120+ days:     8% decay (max)
```

### Reactivation
- First match back: No penalty
- Rating recalculated normally
- Provisional period if > 6 months inactive

---

## Statistics Tracking

### Player Statistics
```typescript
interface PlayerStats {
  // Overall
  totalMatches: number;
  wins: number;
  losses: number;
  draws: number;
  winRate: number;
  
  // Tournament
  tournamentsPlayed: number;
  tournamentsWon: number;
  topThreeFinishes: number;
  topEightFinishes: number;
  
  // Points
  totalPointsEarned: number;
  averagePointsPerTournament: number;
  highestPlacement: number;
  
  // Streaks
  currentWinStreak: number;
  longestWinStreak: number;
  
  // Head-to-Head
  favoriteOpponent: string;
  nemesis: string;
  
  // Activity
  lastMatchDate: Date;
  activeDays: number;
  averageMatchesPerMonth: number;
}
```

### Match Statistics
```typescript
interface MatchStats {
  // Finish Types
  burstFinishes: number;
  overFinishes: number;
  spinFinishes: number;
  
  // Performance
  averageMatchDuration: number;
  comebackWins: number;
  perfectGames: number;
  
  // Consistency
  standardDeviation: number;
  performanceRating: number;
}
```

---

## Leaderboards

### Global Leaderboard
- Top 100 players by ELO
- Updated in real-time
- Filterable by region
- Historical snapshots

### Seasonal Leaderboard
- Current season rankings
- Points-based
- Resets each season
- Determines season rewards

### Community Leaderboards
- Per-community rankings
- Local champions
- Community pride
- Inter-community comparisons

### Specialized Leaderboards
- Most Active Players
- Highest Win Rate (min 20 matches)
- Longest Win Streak
- Most Improved (monthly)
- Rising Stars (new players)

---

## Ranking Calculations

### Win Rate Calculation
```typescript
function calculateWinRate(wins: number, losses: number, draws: number): number {
  const total = wins + losses + draws;
  if (total === 0) return 0;
  
  // Draws count as 0.5 wins
  const effectiveWins = wins + (draws * 0.5);
  return (effectiveWins / total) * 100;
}
```

### Performance Rating
```typescript
function calculatePerformanceRating(
  recentMatches: Match[],
  playerELO: number
): number {
  // Average opponent ELO
  const avgOpponentELO = recentMatches.reduce((sum, match) => 
    sum + match.opponentELO, 0) / recentMatches.length;
  
  // Win rate in recent matches
  const wins = recentMatches.filter(m => m.result === 'win').length;
  const winRate = wins / recentMatches.length;
  
  // Performance rating
  return avgOpponentELO + (winRate - 0.5) * 400;
}
```

### Strength of Schedule
```typescript
function calculateStrengthOfSchedule(matches: Match[]): number {
  const opponentRatings = matches.map(m => m.opponentELO);
  const avgOpponentRating = opponentRatings.reduce((a, b) => a + b, 0) / opponentRatings.length;
  
  return avgOpponentRating;
}
```

---

## Data Models

### Player Ranking Entity
```typescript
interface PlayerRanking {
  playerId: string;
  
  // ELO Rating
  currentELO: number;
  peakELO: number;
  peakELODate: Date;
  
  // Tier
  tier: RankingTier;
  
  // Points
  totalPoints: number;
  seasonPoints: number;
  
  // Statistics
  totalMatches: number;
  wins: number;
  losses: number;
  draws: number;
  
  // Activity
  lastMatchDate: Date;
  matchesThisMonth: number;
  
  // Rankings
  globalRank: number;
  seasonRank: number;
  communityRank: number;
  
  // Metadata
  updatedAt: Date;
}
```

### Ranking History Entity
```typescript
interface RankingHistory {
  id: string;
  playerId: string;
  
  date: Date;
  elo: number;
  rank: number;
  tier: RankingTier;
  
  // Context
  tournamentId: string | null;
  change: number;
  reason: string;
}
```

---

## Ranking Updates

### Update Triggers
1. **Tournament Completion**: Award placement points
2. **Match Result**: Update ELO ratings
3. **Season End**: Calculate season rankings
4. **Daily**: Apply decay to inactive players
5. **Weekly**: Recalculate global rankings

### Update Process
```typescript
async function updateRankings(tournamentId: string) {
  // 1. Get tournament results
  const results = await getTournamentResults(tournamentId);
  
  // 2. Award placement points
  for (const result of results) {
    await awardPoints(result.playerId, result.points);
  }
  
  // 3. Update ELO for all matches
  const matches = await getTournamentMatches(tournamentId);
  for (const match of matches) {
    await updateELO(match);
  }
  
  // 4. Recalculate rankings
  await recalculateGlobalRankings();
  
  // 5. Update player statistics
  await updatePlayerStats(tournamentId);
  
  // 6. Create ranking snapshots
  await createRankingSnapshot();
}
```

---

## Anti-Cheat Measures

### Ranking Integrity
1. **Match Validation**: Judges verify results
2. **Anomaly Detection**: Flag suspicious patterns
3. **Manual Review**: Admin review of flagged matches
4. **Penalties**: Point deductions for violations
5. **Bans**: Temporary or permanent for serious violations

### Suspicious Patterns
- Unusual win rate spikes
- Consistent wins against same opponent
- Sandbagging (intentional losses)
- Match fixing
- Multiple accounts

---

## Future Enhancements

### Advanced Features
- [ ] Machine learning predictions
- [ ] Player style analysis
- [ ] Matchup predictions
- [ ] Form tracking (hot/cold streaks)
- [ ] Peak performance analysis
- [ ] Career milestones
- [ ] Achievement system
- [ ] Ranking simulations
- [ ] "What-if" scenarios
- [ ] Comparative analysis

### Integration Features
- [ ] Discord bot for rankings
- [ ] Mobile app rankings view
- [ ] Social media sharing
- [ ] Ranking badges/flair
- [ ] Email notifications for rank changes

---

## Implementation Checklist

### Phase 4: Rankings & Statistics
- [ ] Basic ELO calculation
- [ ] Tournament points system
- [ ] Global leaderboard
- [ ] Player statistics
- [ ] Ranking tiers

### Phase 5: Community Features
- [ ] Community rankings
- [ ] Seasonal rankings
- [ ] Specialized leaderboards
- [ ] Ranking history

### Phase 6: Advanced Features
- [ ] Decay system
- [ ] Performance ratings
- [ ] Advanced statistics
- [ ] Ranking predictions

---

## Performance Considerations

### Optimization
- Cache leaderboards (5-minute TTL)
- Batch ranking updates
- Index on ELO, rank, tier
- Denormalize frequently accessed stats
- Background jobs for heavy calculations

### Scalability
- Support 10,000+ ranked players
- Handle 100+ concurrent tournaments
- Real-time ranking updates
- Historical data retention

---

This ranking system is designed to be:
- **Fair**: Rewards skill and consistency
- **Transparent**: Clear calculation methods
- **Engaging**: Encourages participation
- **Competitive**: Drives improvement
- **Scalable**: Grows with the community