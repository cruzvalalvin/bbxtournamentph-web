# BBXTournamentPH Landing Page

A modern, futuristic landing page for BBXTournamentPH - a competitive platform for Beyblade X tournaments in the Philippines.

## 🎨 Design Features

- **Dark Esports Aesthetic**: Professional dark theme inspired by competitive gaming
- **Neon Green Accents**: Vibrant neon green (#00ff41) accent colors throughout
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

```
bbxtournamentph/
├── app/
│   ├── globals.css          # Global styles and animations
│   ├── layout.tsx           # Root layout with metadata
│   └── page.tsx             # Main landing page
├── components/
│   ├── GridBackground.tsx   # Animated grid background
│   ├── HeroSection.tsx      # Hero section with title and status
│   ├── FeatureCards.tsx     # Feature preview cards
│   ├── AlphaSection.tsx     # Closed alpha announcement
│   └── Footer.tsx           # Footer with Discord button
└── public/                  # Static assets
```

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

2. Install dependencies:
```bash
npm install
```

3. Run the development server:
```bash
npm run dev
```

4. Open [http://localhost:3000](http://localhost:3000) in your browser

### Build for Production

```bash
npm run build
npm start
```

## 🎨 Color Palette

- **Background**: `#0a0a0a` (Dark)
- **Card Background**: `#111111` (Slightly lighter dark)
- **Border**: `#1a1a1a` (Subtle border)
- **Neon Green**: `#00ff41` (Primary accent)
- **Neon Green Dark**: `#00cc33` (Hover state)
- **Foreground**: `#ededed` (Light text)

## ✨ Custom Animations

- **Grid Pulse**: Subtle pulsing animation on the background grid
- **Hover Lift**: Cards lift up on hover with shadow
- **Glow Effects**: Neon green glow on interactive elements
- **Text Glow**: Subtle glow on important text

## 📱 Responsive Breakpoints

- **Mobile**: < 768px
- **Tablet**: 768px - 1024px
- **Desktop**: > 1024px

## 🔧 Customization

### Changing Colors
Edit the CSS variables in `app/globals.css`:
```css
:root {
  --neon-green: #00ff41;
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
