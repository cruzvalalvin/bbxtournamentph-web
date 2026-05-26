# BBXTournamentPH Landing Page

An official competitive Beyblade tournament and league platform for the Philippines.

## 🎨 Design Features

- **Dark Metallic Theme**: Premium competitive arena-inspired UI with professional esports presentation
- **Amber/Gold Championship Accents**: Prestigious gold and amber tones representing tournament excellence
- **Animated Grid Background**: Subtle pulsing grid pattern for depth
- **Smooth Animations**: Hover effects and transitions on all interactive elements
- **Mobile-First Responsive**: Fully responsive design that works on all devices
- **Modern Typography**: Clean, professional fonts with proper hierarchy

## 🚀 Tech Stack

- **Next.js 15** - React framework with App Router
- **TypeScript** - Type-safe development
- **TailwindCSS** - Utility-first CSS framework
- **Geist Font** - Modern sans-serif and monospace fonts

## 📦 Project Structure

This project uses a **monorepo structure** with npm workspaces:

```
bbxtournamentph/
├── apps/
│   ├── web/                 # Frontend Next.js application
│   │   ├── app/            # Next.js app directory
│   │   ├── components/     # React components
│   │   ├── public/         # Static assets
│   │   └── package.json    # Web app dependencies
│   │
│   └── api/                # Backend API (placeholder)
│       ├── package.json    # API dependencies
│       └── README.md       # API documentation
│
├── docs/                   # Project documentation
│   ├── README.md          # Documentation index
│   └── monorepo-structure.md  # Monorepo details
│
├── package.json           # Root monorepo configuration
└── README.md             # This file
```

For detailed information about the monorepo structure, see [docs/monorepo-structure.md](docs/monorepo-structure.md).

## 🎯 Sections

### Hero Section
- Main title: "BBXTournamentPH"
- Subtitle: "Competitive Platform for Beyblade X"
- Status badges: "UNDER CONSTRUCTION" and "CLOSED ALPHA COMING SOON"
- Description of the platform

### Feature Preview Cards
Six feature cards showcasing:
1. Live Tournament Brackets
2. Judge Match System
3. Community Rankings
4. Player Profiles
5. Tournament Seasons
6. Real-Time Match Tracking

### Closed Alpha Section
- "Forged for the community, by the community" message
- Alpha build badge
- Community-focused messaging

### Footer
- Brand information
- Discord button (placeholder)
- Copyright information

## 🛠️ Getting Started

### Prerequisites
- Node.js 18+ installed
- npm or yarn package manager

### Installation

1. Navigate to the project directory:
```bash
cd bbxtournamentph
```

2. Install dependencies (installs for all workspaces):
```bash
npm install
```

3. Run the development server:
```bash
# Run frontend (web app)
npm run dev
# or specifically
npm run dev:web
```

4. Open [http://localhost:3000](http://localhost:3000) in your browser

### Build for Production

```bash
# Build all apps
npm run build

# Start frontend
npm run start
# or specifically
npm run start:web
```

### Monorepo Commands

```bash
# Run frontend dev server
npm run dev:web

# Run API dev server (when implemented)
npm run dev:api

# Build specific app
npm run build:web
npm run build:api

# Lint all workspaces
npm run lint
```

For more details on the monorepo structure, see [docs/monorepo-structure.md](docs/monorepo-structure.md).

## 🎨 Color Palette

- **Background**: `#0a0a0a` (Dark metallic base)
- **Card Background**: `#111111` (Slightly lighter dark)
- **Border**: `#1a1a1a` (Subtle border)
- **Championship Gold**: `#f59e0b` (Primary amber/gold accent)
- **Championship Gold Dark**: `#d97706` (Hover state)
- **Foreground**: `#ededed` (Light text)

## ✨ Custom Animations

- **Grid Pulse**: Subtle pulsing animation on the background grid
- **Hover Lift**: Cards lift up on hover with shadow
- **Glow Effects**: Championship gold glow on interactive elements
- **Text Glow**: Subtle amber glow on important text

## 📱 Responsive Breakpoints

- **Mobile**: < 768px
- **Tablet**: 768px - 1024px
- **Desktop**: > 1024px

## 🔧 Customization

### Changing Colors
Edit the CSS variables in `app/globals.css`:
```css
:root {
  --championship-gold: #f59e0b;
  --dark-bg: #0a0a0a;
  /* ... other variables */
}
```

### Adding New Features
Add new feature cards in `components/FeatureCards.tsx`:
```typescript
const features: Feature[] = [
  {
    title: "Your Feature",
    description: "Feature description",
    icon: "🎯",
  },
  // ... more features
];
```

## 📄 License

© 2026 BBXTournamentPH. All rights reserved.

## 🤝 Contributing

This is a community-driven project. Contributions are welcome!

---

Made with ⚡ for the Philippine Beyblade X community
