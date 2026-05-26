# Types

Shared TypeScript type definitions and interfaces.

## Purpose

- Global type definitions
- API response types
- Domain models
- Utility types

## Structure

```
types/
├── index.ts           # Main exports
├── tournament.ts      # Tournament-related types
├── player.ts          # Player-related types
├── match.ts           # Match-related types
└── api.ts             # API response types
```

## Example

```typescript
// types/tournament.ts
export interface Tournament {
  id: string;
  name: string;
  format: 'swiss' | 'round-robin' | 'single-elimination';
  status: 'upcoming' | 'active' | 'completed';
  startDate: Date;
  endDate: Date;
}

export type TournamentFormat = Tournament['format'];
```

## Guidelines

- Use interfaces for object shapes
- Use types for unions and complex types
- Export all types through index.ts
- Document complex types with JSDoc comments