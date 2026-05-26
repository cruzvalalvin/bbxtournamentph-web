# BBXTournamentPH

> **An official competitive Beyblade X tournament and league platform for the Philippines**

[![Status](https://img.shields.io/badge/status-pre--alpha-yellow)](https://github.com/yourusername/bbxtournamentph)
[![License](https://img.shields.io/badge/license-UNLICENSED-red)](LICENSE)

A professional esports-level tournament management platform featuring dark metallic design with championship gold accents, built for the Philippine Beyblade X competitive community.

## 🎯 Overview

BBXTournamentPH provides a complete ecosystem for competitive Beyblade X tournaments, including:

- **Tournament Management**: Swiss, Round Robin, and Single Elimination formats
- **Live Match Tracking**: Real-time bracket updates and results
- **Player Rankings**: ELO-based ranking system with seasonal leaderboards
- **Judge System**: Mobile-optimized match input interface
- **Community Features**: League management and community rankings

## 🏗️ Architecture

This project uses a **monorepo structure** with clear separation between frontend and backend:

```
bbxtournamentph/
├── apps/
│   ├── web/                    # Next.js frontend
│   │   ├── app/               # Next.js App Router
│   │   ├── components/        # Shared UI components
│   │   ├── features/          # Feature modules
│   │   ├── services/          # API clients
│   │   ├── hooks/             # Custom React hooks
│   │   ├── types/             # TypeScript definitions
│   │   └── styles/            # Global styles
│   │
│   └── api/                   # ASP.NET Core backend
│       ├── BBXTournament.Api/           # API layer
│       ├── BBXTournament.Application/   # Business logic
│       ├── BBXTournament.Domain/        # Domain models
│       └── BBXTournament.Infrastructure/ # Data access
│
├── docs/                      # Comprehensive documentation
└── package.json              # Monorepo configuration
```

## 🚀 Tech Stack

### Frontend
- **Next.js 15** - React framework with App Router
- **TypeScript** - Type-safe development
- **TailwindCSS 4** - Utility-first CSS framework
- **React 19** - Latest React features

### Backend
- **ASP.NET Core 8.0** - High-performance web framework
- **Entity Framework Core** - ORM for database access
- **SQL Server** - Relational database
- **Clean Architecture** - Maintainable, scalable structure

## 🎨 Design System

- **Dark Metallic Theme**: Professional competitive arena aesthetic
- **Championship Gold**: Amber/gold accents (#f59e0b) for primary actions
- **Modern Typography**: Geist Sans and Geist Mono fonts
- **Responsive Design**: Mobile-first approach
- **Smooth Animations**: Professional transitions and hover effects

See [Branding Guide](docs/branding.md) for complete design specifications.

## 📚 Documentation

Comprehensive documentation is available in the `/docs` directory:

- **[Architecture](docs/architecture.md)** - System design and technical decisions
- **[Roadmap](docs/roadmap.md)** - Development phases and milestones
- **[Backend Rules](docs/backend-rules.md)** - ASP.NET Core best practices
- **[Frontend Rules](docs/frontend-rules.md)** - Next.js/React guidelines
- **[Tournament System](docs/tournament-system.md)** - Tournament formats and management
- **[Ranking System](docs/ranking-system.md)** - ELO ratings and leaderboards
- **[Branding Guide](docs/branding.md)** - Visual identity and design system

## ✨ Features

### Current (Landing Page)
- ✅ Professional dark metallic design
- ✅ Championship gold accents
- ✅ Responsive mobile-first layout
- ✅ Animated grid background
- ✅ Feature preview cards
- ✅ Alpha status messaging

### Planned (See [Roadmap](docs/roadmap.md))
- 🚧 Tournament management (Swiss, Round Robin, Single Elimination)
- 🚧 Real-time match tracking
- 🚧 Judge interface for mobile match input
- 🚧 Player rankings and statistics
- 🚧 Community and league management
- 🚧 Authentication and authorization

## 🛠️ Getting Started

### Prerequisites
- **Node.js 18+** - For frontend development
- **.NET 8.0 SDK** - For backend development (optional)
- **SQL Server** - For database (optional, for backend)

### Frontend Development

```bash
# Install dependencies
npm install

# Run development server
npm run dev

# Build for production
npm run build

# Start production server
npm start
```

The frontend will be available at [http://localhost:3000](http://localhost:3000)

### Backend Development

```bash
# Navigate to API directory
cd apps/api

# Restore dependencies
dotnet restore

# Run the API
dotnet run --project BBXTournament.Api
```

The API will be available at:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5001`
- Swagger: `https://localhost:7001/swagger`

See [apps/api/README.md](apps/api/README.md) for detailed backend setup.

## 🎨 Design System

### Color Palette
- **Championship Gold**: `#f59e0b` - Primary actions and highlights
- **Dark Metallic**: `#0a0a0a` - Main background
- **Card Background**: `#111111` - Elevated surfaces
- **Border**: `#1a1a1a` - Subtle dividers
- **Text Primary**: `#ededed` - Main text color

See [docs/branding.md](docs/branding.md) for complete design specifications.

## 🚀 Deployment

### Frontend (Vercel)
The frontend is deployed on Vercel and automatically deploys from the main branch.

```bash
# Vercel will automatically detect Next.js and deploy from apps/web
```

### Backend (Future)
Backend deployment will be configured for Azure App Service or AWS Elastic Beanstalk.

## 🤝 Contributing

We welcome contributions from the community!

### Development Workflow
1. Read the [Architecture](docs/architecture.md) documentation
2. Follow [Frontend Rules](docs/frontend-rules.md) or [Backend Rules](docs/backend-rules.md)
3. Check the [Roadmap](docs/roadmap.md) for current priorities
4. Submit pull requests with clear descriptions

### Code Standards
- TypeScript for frontend
- C# for backend
- Follow established patterns
- Write clean, maintainable code
- Document complex logic

## 📄 License

© 2026 BBXTournamentPH. All rights reserved.

## 🔗 Links

- **Documentation**: [/docs](docs/)
- **Frontend**: [/apps/web](apps/web/)
- **Backend**: [/apps/api](apps/api/)
- **Live Site**: [https://bbxtournamentph.vercel.app](https://bbxtournamentph.vercel.app)

## 📞 Contact

For questions, suggestions, or feedback:
- Open an issue on GitHub
- Join our Discord community (coming soon)
- Email: contact@bbxtournamentph.com (coming soon)

---

**Built with ⚡ for the Philippine Beyblade X competitive community**

*Empowering bladers through fair competition and professional tournament management*
