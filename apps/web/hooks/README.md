# Hooks

Shared React hooks for the application.

## Purpose

- Reusable custom hooks
- State management hooks
- Data fetching hooks
- UI interaction hooks

## Example Hooks

```typescript
// hooks/useTournament.ts
export function useTournament(id: string) {
  const [tournament, setTournament] = useState(null);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    // Fetch tournament data
  }, [id]);
  
  return { tournament, loading };
}
```

## Guidelines

- Follow React hooks naming convention (use prefix)
- Keep hooks focused and single-purpose
- Document parameters and return values
- Handle loading and error states