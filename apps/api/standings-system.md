# Tournament Standings System

## Overview

The Standings System tracks participant performance within tournament stages, calculates rankings, and determines advancement. It's essential for Swiss tournaments, Round Robin, and any format that requires performance tracking beyond simple win/loss.

## Standing Entity

Each standing represents one participant's performance in one stage.

### Core Fields

**Performance Metrics:**
- `Wins`: Number of matches won
- `Losses`: Number of matches lost
- `Draws`: Number of tied matches
- `MatchPoints`: Total points earned (3 for win, 1 for draw, 0 for loss)
- `PointDifference`: Total score differential (points scored - points against)

**Tiebreakers:**
- `Buchholz`: Sum of opponents' match points
- `MedianBuchholz`: Buchholz excluding highest and lowest
- `HeadToHeadScore`: Direct matchup results
- `Rank`: Final calculated ranking

### Relationships

- One standing per participant per stage
- Unique constraint: (TournamentStageId, ParticipantId)
- Updated after each match result

## Scoring System

### Match Points

Standard scoring (configurable in future):
- **Win**: 3 points
- **Draw**: 1 point
- **Loss**: 0 points

This is the primary metric for ranking.

### Point Difference

Also known as "goal difference" or "score differential":
```
PointDifference = Total Points Scored - Total Points Against
```

Example:
- Match 1: Win 10-5 → +5
- Match 2: Loss 3-8 → -5
- Match 3: Win 12-7 → +5
- Total: +5 point difference

Used as first tiebreaker when match points are equal.

## Tiebreaker System

When participants have the same match points, use tiebreakers in order:

### 1. Point Difference
Most straightforward tiebreaker.
```
Player A: 9 points, +15 difference
Player B: 9 points, +12 difference
Winner: Player A
```

### 2. Buchholz Score
Sum of all opponents' match points. Rewards playing against stronger opponents.

```
Player A opponents: 9, 6, 6, 3, 3 → Buchholz = 27
Player B opponents: 6, 6, 3, 3, 0 → Buchholz = 18
Winner: Player A (faced stronger opponents)
```

**When to Use:**
- Swiss tournaments
- When strength of schedule matters
- After point difference

### 3. Median Buchholz
Buchholz score excluding highest and lowest opponent scores. More stable than regular Buchholz.

```
Player A opponents: 9, 6, 6, 3, 3
Remove 9 and 3 → 6, 6, 3 → Median Buchholz = 15

Player B opponents: 6, 6, 3, 3, 0
Remove 6 and 0 → 6, 3, 3 → Median Buchholz = 12

Winner: Player A
```

**When to Use:**
- After Buchholz
- Reduces impact of outliers
- More fair for Swiss

### 4. Head-to-Head
Direct matchup result between tied players.

```
Player A vs Player B: Player A won
Winner: Player A
```

**When to Use:**
- Final tiebreaker
- Only works for 2-way ties
- Most decisive

### 5. Additional Tiebreakers (Future)
- Wins (if using different point system)
- Cumulative opponent scores
- Coin flip / random (last resort)

## Ranking Calculation

### Algorithm

1. Sort by Match Points (descending)
2. If tied, sort by Point Difference (descending)
3. If tied, sort by Buchholz (descending)
4. If tied, sort by Median Buchholz (descending)
5. If tied, sort by Head-to-Head (if applicable)
6. Assign ranks based on final order

### Example Calculation

```
Participants after 5 rounds:

ID  | MP | PD  | Buch | MBuch | Rank
----|----|----|------|-------|-----
A   | 15 | +20 | 45   | 36    | 1
B   | 15 | +18 | 42   | 33    | 2
C   | 12 | +15 | 39   | 30    | 3
D   | 12 | +15 | 36   | 27    | 4
E   | 12 | +10 | 36   | 27    | 5
F   | 9  | +5  | 33   | 24    | 6
```

**Explanation:**
- A and B tied on MP (15), A wins on PD (+20 > +18)
- C, D, E tied on MP (12) and PD (+15, +15, +10)
- C wins on Buchholz (39 > 36)
- D and E tied on Buchholz (36), D wins on MBuch (27 > 27)
  - If still tied, would use head-to-head

## Standing Updates

### After Match Completion

When a match is reported:

1. **Update Winner's Standing:**
   ```csharp
   standing.RecordWin(scoreFor, scoreAgainst);
   // Increments Wins
   // Adds 3 to MatchPoints
   // Updates PointDifference
   ```

2. **Update Loser's Standing:**
   ```csharp
   standing.RecordLoss(scoreFor, scoreAgainst);
   // Increments Losses
   // Updates PointDifference (negative)
   ```

3. **Update Draw (if applicable):**
   ```csharp
   standing.RecordDraw(scoreFor, scoreAgainst);
   // Increments Draws
   // Adds 1 to MatchPoints
   // Updates PointDifference
   ```

4. **Recalculate Tiebreakers:**
   ```csharp
   standing.UpdateTiebreakers(buchholz, medianBuchholz, h2h);
   ```

5. **Recalculate All Rankings:**
   ```csharp
   // Sort all standings
   // Assign new ranks
   standing.UpdateRank(newRank);
   ```

## Format-Specific Usage

### Swiss Tournaments

**Primary Use Case:**
- Track performance across rounds
- Determine pairings for next round
- Calculate final rankings

**Key Metrics:**
- Match Points (most important)
- Buchholz (strength of schedule)
- Median Buchholz (stable tiebreaker)

**Example:**
```
After Round 3:
1. Player A: 9 points (3-0)
2. Player B: 6 points (2-1)
3. Player C: 6 points (2-1)
4. Player D: 3 points (1-2)

Round 4 Pairings:
- A vs B (top 2)
- C vs D (bottom 2)
```

### Round Robin

**Primary Use Case:**
- Track league-style standings
- Determine group winners
- Calculate advancement

**Key Metrics:**
- Match Points
- Point Difference
- Head-to-Head (for 2-way ties)

**Example:**
```
Group A Final Standings:
1. Team A: 12 points (4-0-0)
2. Team B: 9 points (3-1-0)
3. Team C: 6 points (2-2-0)
4. Team D: 3 points (1-3-0)
5. Team E: 0 points (0-4-0)

Top 2 advance to playoffs
```

### Elimination Brackets

**Limited Use:**
- Not typically needed for single/double elimination
- May track for seeding purposes
- Used if bracket has group stage first

## API Operations

### Get Standings
```http
GET /tournaments/{id}/stages/{stageId}/standings
```

Returns standings sorted by rank.

### Recalculate Rankings
```http
POST /tournaments/{id}/stages/{stageId}/standings/recalculate
```

Manually trigger ranking recalculation (usually automatic).

## Calculation Examples

### Example 1: Simple Swiss

**Scenario:** 8 players, 3 rounds

```
Round 1 Results:
A beats B (10-5)
C beats D (8-3)
E beats F (12-7)
G beats H (9-6)

Standings after Round 1:
Rank | Player | MP | PD
-----|--------|----|----|
1    | A      | 3  | +5
2    | C      | 3  | +5
3    | E      | 3  | +5
4    | G      | 3  | +3
5    | B      | 0  | -5
6    | D      | 0  | -5
7    | F      | 0  | -5
8    | H      | 0  | -3
```

### Example 2: Complex Tiebreaker

**Scenario:** 3-way tie at 9 points

```
Player X:
- Opponents: A(12), B(9), C(6) → Buchholz = 27
- Scores: 10-5, 8-7, 12-3 → PD = +15

Player Y:
- Opponents: D(12), E(6), F(6) → Buchholz = 24
- Scores: 11-4, 9-5, 10-6 → PD = +15

Player Z:
- Opponents: G(9), H(9), I(6) → Buchholz = 24
- Scores: 8-3, 7-4, 15-5 → PD = +18

Final Ranking:
1. Z (PD: +18)
2. X (Buchholz: 27)
3. Y (Buchholz: 24)
```

## Best Practices

### Updating Standings

**Immediate Updates:**
- Update after each match result
- Recalculate tiebreakers
- Update rankings

**Batch Updates:**
- Can defer ranking calculation
- Update all at once after round
- More efficient for large tournaments

### Tiebreaker Calculation

**When to Calculate:**
- After each match (real-time)
- After each round (batch)
- On-demand (when viewing standings)

**Optimization:**
- Cache Buchholz calculations
- Only recalculate affected standings
- Use database queries for efficiency

### Display

**Essential Information:**
- Rank
- Player name
- Match Points
- Win-Loss-Draw record
- Point Difference

**Additional Information:**
- Buchholz (for Swiss)
- Recent form (last 3 matches)
- Upcoming opponent

## Future Enhancements

### Custom Scoring
- Configurable point values
- Different systems per format
- Bonus points for achievements

### Advanced Tiebreakers
- Cumulative scores
- Progressive scores
- Opponent win percentage
- Sonneborn-Berger

### Real-time Updates
- Live standing updates
- WebSocket notifications
- Automatic recalculation

### Analytics
- Performance trends
- Strength of schedule analysis
- Prediction algorithms
- Historical comparisons

### Visualization
- Standing tables
- Progress charts
- Tiebreaker breakdowns
- What-if scenarios

## Common Issues

### Tied Rankings
**Problem:** Multiple players with same rank
**Solution:** Apply all tiebreakers, use head-to-head as final

### Buchholz Calculation
**Problem:** Opponent withdrew/forfeited
**Solution:** Use average opponent score or exclude

### Point Difference Overflow
**Problem:** Blowout scores skew rankings
**Solution:** Cap maximum point difference per match

### Late Match Results
**Problem:** Results entered out of order
**Solution:** Recalculate all standings after any update

## Integration

### With Match System
- Match completion triggers standing update
- Score validation before updating
- Rollback on match dispute

### With Stage System
- One set of standings per stage
- Reset between stages
- Carry over for advancement

### With Tournament System
- Final standings determine tournament winner
- Used for prize distribution
- Historical record keeping

// Made with Bob