# Monorepo Structure

This document explains the monorepo structure of BBXTournamentPH.

## Overview

BBXTournamentPH uses a monorepo architecture with npm workspaces to manage multiple applications and shared packages in a single repository.

## Directory Structure

```
bbxtournamentph/
├── apps/
│   ├── web/              # Frontend Next.js application
│   │   ├── app/          # Next.js app directory
│   │   ├── components/   # React components
│   │   ├── public/       # Static assets
│   │   ├── package.json  # Web app dependencies
│   │   └── tsconfig.json # Web app TypeScript config
│   │
│   └── api/              # Backend API application
│       ├── package.json  # API dependencies
│       ├── tsconfig.json # API TypeScript config
│       └── README.md     # API documentation
│
├── docs/                 # Project documentation
│   ├── README.md         # Documentation index
│   └── *.md              # Various documentation files
│
├── package.json          # Root package.json with workspaces
├── tsconfig.json         # Root TypeScript configuration
└── README.md             # Main project README
```

## Workspaces

The project uses npm workspaces defined in the root `package.json`:

```json
{
  "workspaces": [
    "apps/*"
  ]
}
```

This allows:
- Shared dependencies across apps
- Single `node_modules` at the root
- Unified scripts for all workspaces

## Running Applications

### Frontend (Web)

```bash
# From root directory
npm run dev:web

# Or directly in the web directory
cd apps/web
npm run dev
```

### API (When Implemented)

```bash
# From root directory
npm run dev:api

# Or directly in the api directory
cd apps/api
npm run dev
```

### All Applications

```bash
# Install all dependencies
npm install

# Build all apps
npm run build

# Lint all apps
npm run lint
```

## Benefits of Monorepo Structure

1. **Code Sharing**: Easy to share code between frontend and backend
2. **Unified Versioning**: Single version for the entire project
3. **Atomic Changes**: Changes across multiple apps in single commit
4. **Simplified Dependencies**: Shared dependencies managed at root level
5. **Better Developer Experience**: Single repository to clone and manage

## Adding New Apps

To add a new application:

1. Create a new directory under `apps/`
2. Add `package.json` with name `@bbxtournamentph/[app-name]`
3. Add to workspace scripts in root `package.json`
4. Install dependencies from root: `npm install`

## Migration Notes

The project was restructured from a single Next.js app to a monorepo:

- Original files moved to `apps/web/`
- Root configuration updated for monorepo
- Workspace scripts added for convenience
- API placeholder created for future development

## Best Practices

1. **Keep apps independent**: Each app should be self-contained
2. **Use workspace scripts**: Run commands from root when possible
3. **Shared code**: Consider creating a `packages/` directory for shared utilities
4. **Documentation**: Keep docs updated as structure evolves
5. **Consistent naming**: Use `@bbxtournamentph/` prefix for all packages

## Future Enhancements

- Add `packages/` directory for shared libraries
- Implement shared UI component library
- Add shared TypeScript types package
- Set up shared ESLint and Prettier configs