# Features

Feature-based modules for the BBXTournamentPH platform.

## Structure

Each feature should be self-contained with its own components, hooks, and logic:

```
features/
├── tournaments/
│   ├── components/
│   ├── hooks/
│   ├── types/
│   └── index.ts
├── rankings/
├── judges/
└── communities/
```

## Guidelines

- Keep features independent and modular
- Export public API through index.ts
- Co-locate related code within feature folders
- Use shared hooks/services from parent directories when needed