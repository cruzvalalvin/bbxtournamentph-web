# BBXTournamentPH Branding Guide

Official branding guidelines for the BBXTournamentPH platform.

## Brand Identity

### Mission
To provide the premier competitive Beyblade X tournament platform for the Philippines, fostering a professional esports ecosystem for players, judges, and tournament organizers.

### Vision
Become the official standard for competitive Beyblade X tournaments in the Philippines, recognized for fairness, professionalism, and community engagement.

### Values
- **Competitive Excellence**: Promoting skill and fair play
- **Community First**: Built by and for the community
- **Transparency**: Open and honest operations
- **Professionalism**: Esports-level presentation
- **Accessibility**: Welcoming to all skill levels

---

## Visual Identity

### Brand Evolution
The platform has evolved from a cyberpunk aesthetic to a professional competitive tournament identity:

**Previous (Deprecated)**:
- Neon green (#00ff41)
- Razer-like gaming aesthetic
- Cyberpunk wording and style

**Current (Official)**:
- Amber/gold championship accents
- Dark metallic competitive theme
- Professional esports tournament atmosphere
- Official league/tournament ecosystem style

---

## Color Palette

### Primary Colors

#### Championship Gold
```
Primary:   #f59e0b (rgb(245, 158, 11))
Dark:      #d97706 (rgb(217, 119, 6))
Light:     #fbbf24 (rgb(251, 191, 36))
```

**Usage**:
- Primary CTAs and buttons
- Championship badges and awards
- Important highlights
- Interactive elements
- Success states

#### Dark Metallic
```
Background:      #0a0a0a (rgb(10, 10, 10))
Card Background: #111111 (rgb(17, 17, 17))
Border:          #1a1a1a (rgb(26, 26, 26))
Elevated:        #1f1f1f (rgb(31, 31, 31))
```

**Usage**:
- Main backgrounds
- Card containers
- Borders and dividers
- Elevated surfaces

### Secondary Colors

#### Text Colors
```
Primary Text:    #ededed (rgb(237, 237, 237))
Secondary Text:  #a3a3a3 (rgb(163, 163, 163))
Muted Text:      #737373 (rgb(115, 115, 115))
Disabled:        #525252 (rgb(82, 82, 82))
```

#### Semantic Colors
```
Success:  #10b981 (rgb(16, 185, 129))
Warning:  #f59e0b (rgb(245, 158, 11))
Error:    #ef4444 (rgb(239, 68, 68))
Info:     #3b82f6 (rgb(59, 130, 246))
```

### Gradients

#### Championship Gradient
```css
background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
```

#### Metallic Gradient
```css
background: linear-gradient(180deg, #1f1f1f 0%, #0a0a0a 100%);
```

#### Glow Effect
```css
box-shadow: 0 0 20px rgba(245, 158, 11, 0.3);
```

---

## Typography

### Font Families

#### Primary Font: Geist Sans
```css
font-family: 'Geist', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
```

**Usage**: Body text, UI elements, general content

#### Monospace Font: Geist Mono
```css
font-family: 'Geist Mono', 'Courier New', monospace;
```

**Usage**: Code, technical data, match scores, timestamps

### Font Scales

#### Headings
```css
h1: 3rem (48px)    - font-weight: 700
h2: 2.25rem (36px) - font-weight: 700
h3: 1.875rem (30px) - font-weight: 600
h4: 1.5rem (24px)  - font-weight: 600
h5: 1.25rem (20px) - font-weight: 600
h6: 1rem (16px)    - font-weight: 600
```

#### Body Text
```css
Large:   1.125rem (18px) - font-weight: 400
Base:    1rem (16px)     - font-weight: 400
Small:   0.875rem (14px) - font-weight: 400
Tiny:    0.75rem (12px)  - font-weight: 400
```

### Text Styles

#### Championship Title
```css
font-size: 3rem;
font-weight: 700;
background: linear-gradient(135deg, #f59e0b 0%, #fbbf24 100%);
-webkit-background-clip: text;
-webkit-text-fill-color: transparent;
text-shadow: 0 0 30px rgba(245, 158, 11, 0.5);
```

#### Section Heading
```css
font-size: 2.25rem;
font-weight: 700;
color: #ededed;
letter-spacing: -0.025em;
```

---

## Logo & Branding

### Logo Usage

#### Primary Logo
- Full color on dark backgrounds
- Minimum size: 120px width
- Clear space: 20px on all sides

#### Logo Variations
- **Full Logo**: BBXTournamentPH with icon
- **Icon Only**: For small spaces (favicon, app icon)
- **Text Only**: For horizontal layouts

### Logo Don'ts
❌ Don't change colors
❌ Don't rotate or distort
❌ Don't add effects or shadows
❌ Don't place on busy backgrounds
❌ Don't use old neon green version

---

## UI Components

### Buttons

#### Primary Button
```css
background: #f59e0b;
color: #0a0a0a;
padding: 0.75rem 1.5rem;
border-radius: 0.5rem;
font-weight: 600;
transition: all 0.2s;

hover:
  background: #d97706;
  box-shadow: 0 0 20px rgba(245, 158, 11, 0.4);
```

#### Secondary Button
```css
background: transparent;
border: 2px solid #f59e0b;
color: #f59e0b;
padding: 0.75rem 1.5rem;
border-radius: 0.5rem;
font-weight: 600;

hover:
  background: rgba(245, 158, 11, 0.1);
```

#### Ghost Button
```css
background: transparent;
color: #ededed;
padding: 0.75rem 1.5rem;

hover:
  background: rgba(255, 255, 255, 0.05);
```

### Cards

#### Tournament Card
```css
background: #111111;
border: 1px solid #1a1a1a;
border-radius: 0.75rem;
padding: 1.5rem;
transition: all 0.3s;

hover:
  border-color: #f59e0b;
  box-shadow: 0 4px 20px rgba(245, 158, 11, 0.15);
  transform: translateY(-4px);
```

### Badges

#### Status Badge
```css
/* Active */
background: rgba(16, 185, 129, 0.1);
color: #10b981;
border: 1px solid rgba(16, 185, 129, 0.3);

/* Upcoming */
background: rgba(245, 158, 11, 0.1);
color: #f59e0b;
border: 1px solid rgba(245, 158, 11, 0.3);

/* Completed */
background: rgba(163, 163, 163, 0.1);
color: #a3a3a3;
border: 1px solid rgba(163, 163, 163, 0.3);
```

#### Tier Badge
```css
/* Master */
background: linear-gradient(135deg, #f59e0b 0%, #fbbf24 100%);
color: #0a0a0a;

/* Diamond */
background: linear-gradient(135deg, #3b82f6 0%, #60a5fa 100%);
color: #ffffff;

/* Platinum */
background: linear-gradient(135deg, #8b5cf6 0%, #a78bfa 100%);
color: #ffffff;
```

---

## Iconography

### Icon Style
- **Style**: Outline/stroke icons
- **Weight**: 2px stroke
- **Size**: 24px default
- **Color**: Inherit from parent or #ededed

### Icon Usage
```tsx
// Championship/Trophy icons
🏆 - Tournament winners
👑 - Champions
⭐ - Featured/Important

// Status icons
✓ - Success/Completed
⚡ - Active/Live
⏱️ - Upcoming/Scheduled
📊 - Statistics

// Action icons
➕ - Add/Create
✏️ - Edit
🗑️ - Delete
👁️ - View
```

---

## Animation & Motion

### Transition Timing
```css
/* Fast: UI feedback */
transition: all 0.15s ease;

/* Standard: Most interactions */
transition: all 0.2s ease;

/* Slow: Page transitions */
transition: all 0.3s ease;
```

### Hover Effects

#### Lift Effect
```css
transform: translateY(-4px);
box-shadow: 0 8px 24px rgba(0, 0, 0, 0.3);
```

#### Glow Effect
```css
box-shadow: 0 0 20px rgba(245, 158, 11, 0.4);
```

#### Scale Effect
```css
transform: scale(1.05);
```

### Loading States
- Skeleton screens for content loading
- Spinner for actions
- Progress bars for uploads
- Shimmer effect for placeholders

---

## Voice & Tone

### Brand Voice
- **Professional**: Esports-level presentation
- **Competitive**: Emphasize skill and achievement
- **Welcoming**: Inclusive to all skill levels
- **Authoritative**: Official tournament platform
- **Energetic**: Exciting and engaging

### Writing Style

#### Do's ✅
- Use active voice
- Be concise and clear
- Emphasize competition and achievement
- Use tournament/esports terminology
- Maintain professional tone

#### Don'ts ❌
- Don't use slang excessively
- Don't be overly casual
- Don't use outdated gaming terms
- Don't be condescending
- Don't use jargon without explanation

### Example Copy

#### Good ✅
```
"Compete in official Beyblade X tournaments. 
Track your rankings. Claim your championship."

"Join the premier competitive platform for 
Philippine Beyblade X players."
```

#### Bad ❌
```
"Yo! Come battle and stuff! It's gonna be lit! 🔥"

"The most epic, insane, crazy Beyblade thing ever!"
```

---

## Platform-Specific Guidelines

### Web Application
- Dark metallic theme throughout
- Championship gold for primary actions
- Smooth transitions and hover effects
- Responsive design (mobile-first)
- Accessible contrast ratios

### Mobile Application (Future)
- Simplified navigation
- Larger touch targets (44px minimum)
- Bottom navigation bar
- Swipe gestures
- Optimized for one-handed use

### Social Media
- Use championship gold in graphics
- Dark backgrounds for posts
- Tournament highlights and results
- Player spotlights
- Community engagement

### Print Materials (Future)
- High contrast for readability
- Championship gold as accent
- Professional tournament aesthetic
- Clear hierarchy
- QR codes for digital integration

---

## Accessibility

### Color Contrast
- Text on dark background: Minimum 4.5:1 ratio
- Large text: Minimum 3:1 ratio
- Interactive elements: Clear focus states

### Focus States
```css
:focus-visible {
  outline: 2px solid #f59e0b;
  outline-offset: 2px;
}
```

### Screen Reader Support
- Semantic HTML
- ARIA labels where needed
- Alt text for images
- Descriptive link text

---

## Brand Assets

### Required Assets
- [ ] Logo (SVG, PNG)
- [ ] Favicon (ICO, PNG)
- [ ] Social media images
- [ ] Tournament badges
- [ ] Tier badges
- [ ] Achievement icons

### Asset Specifications
```
Logo:
- SVG: Vector format
- PNG: 512x512px, transparent background

Favicon:
- ICO: 32x32px, 16x16px
- PNG: 192x192px, 512x512px

Social Media:
- Open Graph: 1200x630px
- Twitter Card: 1200x675px
```

---

## Implementation

### CSS Variables
```css
:root {
  /* Colors */
  --championship-gold: #f59e0b;
  --championship-gold-dark: #d97706;
  --championship-gold-light: #fbbf24;
  
  --dark-bg: #0a0a0a;
  --card-bg: #111111;
  --border: #1a1a1a;
  --elevated: #1f1f1f;
  
  --text-primary: #ededed;
  --text-secondary: #a3a3a3;
  --text-muted: #737373;
  
  /* Spacing */
  --spacing-xs: 0.25rem;
  --spacing-sm: 0.5rem;
  --spacing-md: 1rem;
  --spacing-lg: 1.5rem;
  --spacing-xl: 2rem;
  
  /* Border Radius */
  --radius-sm: 0.25rem;
  --radius-md: 0.5rem;
  --radius-lg: 0.75rem;
  --radius-xl: 1rem;
  
  /* Shadows */
  --shadow-sm: 0 1px 2px rgba(0, 0, 0, 0.05);
  --shadow-md: 0 4px 6px rgba(0, 0, 0, 0.1);
  --shadow-lg: 0 10px 15px rgba(0, 0, 0, 0.2);
  --shadow-glow: 0 0 20px rgba(245, 158, 11, 0.3);
}
```

### Tailwind Configuration
```javascript
// tailwind.config.js
module.exports = {
  theme: {
    extend: {
      colors: {
        championship: {
          gold: '#f59e0b',
          'gold-dark': '#d97706',
          'gold-light': '#fbbf24',
        },
        dark: {
          bg: '#0a0a0a',
          card: '#111111',
          border: '#1a1a1a',
          elevated: '#1f1f1f',
        },
      },
    },
  },
};
```

---

## Brand Checklist

### Design Review
- [ ] Uses championship gold for primary actions
- [ ] Dark metallic theme applied
- [ ] Proper contrast ratios
- [ ] Consistent spacing
- [ ] Smooth transitions
- [ ] Accessible focus states
- [ ] Responsive design
- [ ] Professional appearance

### Content Review
- [ ] Professional tone
- [ ] Clear and concise
- [ ] Competitive language
- [ ] No deprecated terms (neon green, cyberpunk)
- [ ] Proper grammar and spelling
- [ ] Consistent terminology

---

## Version History

### v2.0 (Current) - May 2026
- Rebranded to championship gold theme
- Professional esports aesthetic
- Dark metallic competitive theme
- Removed neon green/cyberpunk elements

### v1.0 (Deprecated) - 2025
- Neon green (#00ff41) primary color
- Razer-like gaming aesthetic
- Cyberpunk styling

---

**Official Tagline**: "The Premier Competitive Platform for Philippine Beyblade X"

**Brand Essence**: Championship • Competition • Community