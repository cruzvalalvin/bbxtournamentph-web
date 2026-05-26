# Services

API clients and external service integrations.

## Purpose

- API communication layer
- External service wrappers
- Data fetching utilities
- WebSocket connections

## Example Structure

```typescript
// services/api/tournaments.ts
export const tournamentsApi = {
  getAll: () => fetch('/api/tournaments'),
  getById: (id: string) => fetch(`/api/tournaments/${id}`),
  create: (data: TournamentData) => fetch('/api/tournaments', { method: 'POST', body: JSON.stringify(data) })
};
```

## Guidelines

- Keep services stateless
- Handle errors consistently
- Use TypeScript for type safety
- Export clean, documented APIs