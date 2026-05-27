# Tournament Stage System

## Overview

The Stage System allows tournaments to be divided into multiple phases, each with its own format and rules. This enables complex tournament structures like "Swiss rounds followed by single elimination playoffs."

## Stage Concept

A **Stage** is a distinct phase within a tournament that:
- Has its own format type (Swiss, Single Elimination, etc.)
- Contains its own set of matches
- Maintains separate standings
- Can advance participants to the next stage

## Stage Properties

### Core Fields

**Name**
- Human-readable stage identifier
- Examples: "Swiss Rounds", "Top 8 Playoffs", "Grand Finals"
- Used for display and organization

**StageOrder**
- Integer sequence number
- Must be unique per tournament
- Determines execution order
- Stage 1 runs first, then Stage 2, etc.

**FormatType**
- Enum: SingleElimination, DoubleElimination, RoundRobin, Swiss
- Determines how matches are structured
- Cannot be changed after stage starts

**NumberOfRounds**
- How many rounds in this stage
- For Swiss: typically 4-7 rounds
- For Single Elimination: log2(participants)
- For Round Robin: participants - 1

### Optional Configuration

**GroupCount**
- Used for group stages
- Divides participants into groups
- Each group runs independently
- Example: 32 players → 4 groups of 8

**AdvanceCount**
- How many participants advance to next stage
- Used for multi-stage tournaments
- Example: Top 8 from Swiss advance to playoffs

**HasThirdPlaceMatch**
- Boolean flag for elimination brackets
- Creates match between semi-final losers
- Common in traditional tournaments

**HasGrandFinalReset**
- Boolean flag for double elimination
- If loser bracket winner beats winner bracket winner
- They play one more match (the "reset")

## Stage Formats

### Single Elimination

**Characteristics:**
- One loss and you're eliminated
- Fast and decisive
- Bracket size should be power of 2
- Uses byes if participant count isn't power of 2

**Configuration:**
```json
{
  "formatType": "SingleElimination",
  "numberOfRounds": 4,  // For 16 participants
  "hasThirdPlaceMatch": true
}
```

**Match Structure:**
- Round 1: 16 → 8 (8 matches)
- Round 2: 8 → 4 (4 matches)
- Round 3: 4 → 2 (2 matches)
- Round 4: 2 → 1 (1 match)
- Optional: Third place match

**When to Use:**
- Time-constrained tournaments
- Clear winner needed quickly
- Traditional bracket feel

### Double Elimination

**Characteristics:**
- Two brackets: Winners and Losers
- Must lose twice to be eliminated
- More forgiving than single elimination
- Takes longer to complete

**Configuration:**
```json
{
  "formatType": "DoubleElimination",
  "numberOfRounds": 7,  // For 8 participants
  "hasGrandFinalReset": true
}
```

**Match Structure:**
- Winners Bracket: Standard elimination
- Losers Bracket: Receives losers from winners bracket
- Grand Finals: Winner of each bracket
- Optional: Grand Final Reset if loser bracket wins

**When to Use:**
- Want to give players second chance
- More matches for participants
- Fairer determination of skill

### Round Robin

**Characteristics:**
- Everyone plays everyone
- No elimination
- Best for small groups
- Very fair but time-consuming

**Configuration:**
```json
{
  "formatType": "RoundRobin",
  "numberOfRounds": 7,  // For 8 participants (n-1)
  "groupCount": null
}
```

**Match Structure:**
- Each participant plays every other participant once
- Total matches: n(n-1)/2
- For 8 participants: 28 matches
- Can be split into groups to reduce matches

**When to Use:**
- Small participant count (≤16)
- Group stages before playoffs
- League-style competition
- Fairness is priority

### Swiss System

**Characteristics:**
- Pair players with similar records
- No elimination
- Efficient for large tournaments
- Uses tiebreakers for final ranking

**Configuration:**
```json
{
  "formatType": "Swiss",
  "numberOfRounds": 5,  // Typically 4-7
  "advanceCount": 8  // Top 8 to playoffs
}
```

**Match Structure:**
- Round 1: Random or seeded pairing
- Round 2+: Pair players with same record
- Avoid repeat matchups
- Standings determine final placement

**When to Use:**
- Large participant count (32+)
- Want many matches without elimination
- Preliminary rounds before playoffs
- Chess-style tournament

## Multi-Stage Tournaments

### Common Patterns

**Swiss → Single Elimination**
```
Stage 1: Swiss (5 rounds, 64 players)
  ↓ Top 8 advance
Stage 2: Single Elimination (3 rounds, 8 players)
```

**Group Stage → Playoffs**
```
Stage 1: Round Robin Groups (4 groups of 8)
  ↓ Top 2 from each group advance
Stage 2: Single Elimination (3 rounds, 8 players)
```

**Double Elimination → Grand Finals**
```
Stage 1: Double Elimination (Main bracket)
  ↓ Top 2 advance
Stage 2: Best of 5 Grand Finals
```

### Stage Transitions

**Advancement Rules:**
- Use `AdvanceCount` to specify how many move forward
- Based on final standings from previous stage
- Can be top N, or specific criteria

**Data Flow:**
- Participants carry over to next stage
- Previous stage results preserved
- New matches created for new stage
- Fresh standings for new stage

## Stage Status

Stages inherit tournament status but can have their own state:

**Draft**
- Stage is configured but not started
- Can modify settings
- No matches created yet

**Ongoing**
- Matches are being played
- Results being recorded
- Cannot modify format

**Finished**
- All matches complete
- Final standings calculated
- Ready for next stage or tournament end

## Match Organization

### Round Structure

Matches are organized by rounds within a stage:

```
Stage: "Swiss Rounds"
  Round 1: Matches 1-32
  Round 2: Matches 33-64
  Round 3: Matches 65-96
  ...
```

### Match Numbering

- `RoundNumber`: Which round (1, 2, 3, ...)
- `MatchNumber`: Identifier within round (1, 2, 3, ...)
- Combination must be unique within stage

### Bye Handling

When participant count isn't even:
- Create bye matches
- Assign one participant
- Auto-complete with that participant as winner
- Counts as win in standings

## Standings Per Stage

Each stage maintains its own standings:

**Purpose:**
- Track performance within that stage
- Determine advancement
- Calculate tiebreakers
- Generate final rankings

**Reset Between Stages:**
- New stage = fresh standings
- Previous stage results preserved
- Allows different scoring systems per stage

## Configuration Examples

### Small Local Tournament (16 players)
```json
{
  "stages": [
    {
      "name": "Main Bracket",
      "stageOrder": 1,
      "formatType": "SingleElimination",
      "numberOfRounds": 4,
      "hasThirdPlaceMatch": true
    }
  ]
}
```

### Medium Regional (64 players)
```json
{
  "stages": [
    {
      "name": "Swiss Rounds",
      "stageOrder": 1,
      "formatType": "Swiss",
      "numberOfRounds": 6,
      "advanceCount": 8
    },
    {
      "name": "Top 8 Playoffs",
      "stageOrder": 2,
      "formatType": "DoubleElimination",
      "numberOfRounds": 6,
      "hasGrandFinalReset": true
    }
  ]
}
```

### Large Championship (128 players)
```json
{
  "stages": [
    {
      "name": "Group Stage",
      "stageOrder": 1,
      "formatType": "RoundRobin",
      "numberOfRounds": 7,
      "groupCount": 16,
      "advanceCount": 32
    },
    {
      "name": "Swiss Rounds",
      "stageOrder": 2,
      "formatType": "Swiss",
      "numberOfRounds": 5,
      "advanceCount": 8
    },
    {
      "name": "Finals",
      "stageOrder": 3,
      "formatType": "SingleElimination",
      "numberOfRounds": 3,
      "hasThirdPlaceMatch": true
    }
  ]
}
```

## API Operations

### Create Stage
```http
POST /tournaments/{id}/stages
{
  "name": "Swiss Rounds",
  "stageOrder": 1,
  "formatType": "Swiss",
  "numberOfRounds": 5,
  "advanceCount": 8
}
```

### List Stages
```http
GET /tournaments/{id}/stages
```

Returns stages ordered by `stageOrder`.

## Validation Rules

### Stage Creation
- StageOrder must be unique per tournament
- FormatType is required
- NumberOfRounds must be > 0
- Can only create in Draft tournament status

### Stage Order
- Must be sequential (1, 2, 3, ...)
- Gaps are allowed but not recommended
- Order determines execution sequence

### Format-Specific Rules
- Single/Double Elimination: Consider power of 2
- Round Robin: NumberOfRounds = participants - 1
- Swiss: Typically 4-7 rounds
- Groups: GroupCount must divide participants evenly

## Future Enhancements

### Automatic Configuration
- Suggest optimal rounds based on participant count
- Validate format feasibility
- Auto-calculate advancement numbers

### Stage Templates
- Pre-configured stage setups
- Common tournament structures
- One-click stage creation

### Dynamic Stages
- Add stages during tournament
- Modify stages based on results
- Conditional stage activation

### Advanced Features
- Stage-specific rules
- Custom scoring per stage
- Stage-level prizes
- Parallel stages (multiple brackets)

## Best Practices

### Planning Stages
1. Know your participant count
2. Consider time constraints
3. Choose appropriate formats
4. Plan advancement criteria
5. Test with smaller numbers first

### Stage Ordering
- Start with qualification/seeding stages
- Progress to elimination stages
- End with finals/championship stage

### Format Selection
- Small (≤16): Single Elimination or Round Robin
- Medium (17-64): Swiss → Single Elimination
- Large (65+): Groups → Swiss → Elimination

### Advancement
- Be generous with advancement (top 25-50%)
- Consider tiebreaker scenarios
- Communicate criteria clearly to participants

// Made with Bob