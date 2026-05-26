# Frontend Development Rules

Guidelines and best practices for Next.js/React frontend development.

## General Principles

### 1. Component-First Thinking
- Build reusable, composable components
- Keep components focused and single-purpose
- Prefer composition over inheritance

### 2. Type Safety
- Use TypeScript for everything
- Define proper interfaces and types
- Avoid `any` type unless absolutely necessary

### 3. Performance
- Use Server Components by default
- Client Components only when needed
- Optimize images and assets
- Lazy load when appropriate

---

## Project Structure

### Directory Organization
```
apps/web/
├── app/                    # Next.js App Router
│   ├── (auth)/            # Route groups
│   ├── tournaments/       # Feature routes
│   ├── layout.tsx         # Root layout
│   └── page.tsx           # Home page
├── components/            # Shared UI components
│   ├── ui/               # Base UI components
│   └── layout/           # Layout components
├── features/              # Feature modules
│   ├── tournaments/
│   │   ├── components/
│   │   ├── hooks/
│   │   ├── types/
│   │   └── index.ts
├── services/              # API clients
├── hooks/                 # Shared hooks
├── types/                 # Shared types
└── styles/                # Global styles
```

---

## Component Guidelines

### Server Components (Default)
```tsx
// ✅ DO: Use Server Components by default
export default async function TournamentPage({ params }: { params: { id: string } }) {
  const tournament = await getTournament(params.id);
  
  return (
    <div>
      <h1>{tournament.name}</h1>
      <TournamentDetails tournament={tournament} />
    </div>
  );
}

// ✅ DO: Fetch data directly in Server Components
async function getTournament(id: string) {
  const res = await fetch(`${process.env.API_URL}/tournaments/${id}`, {
    cache: 'no-store' // or 'force-cache' for static data
  });
  return res.json();
}
```

### Client Components
```tsx
// ✅ DO: Use 'use client' only when needed
'use client';

import { useState } from 'react';

export function TournamentRegistration({ tournamentId }: { tournamentId: string }) {
  const [isRegistered, setIsRegistered] = useState(false);
  
  const handleRegister = async () => {
    // Handle registration
    setIsRegistered(true);
  };
  
  return (
    <button onClick={handleRegister}>
      {isRegistered ? 'Registered' : 'Register'}
    </button>
  );
}

// ❌ DON'T: Use client components unnecessarily
'use client'; // Not needed if no interactivity

export function TournamentCard({ tournament }) {
  return <div>{tournament.name}</div>;
}
```

### Component Naming
```tsx
// ✅ DO: PascalCase for components
export function TournamentCard() {}
export function MatchList() {}
export function PlayerProfile() {}

// ✅ DO: Descriptive names
export function TournamentRegistrationForm() {}
export function MatchResultInput() {}

// ❌ DON'T: Generic names
export function Card() {} // Too generic
export function Form() {} // Too generic
```

### Component Structure
```tsx
// ✅ DO: Consistent component structure
interface TournamentCardProps {
  tournament: Tournament;
  onSelect?: (id: string) => void;
}

export function TournamentCard({ tournament, onSelect }: TournamentCardProps) {
  // Hooks at the top
  const [isHovered, setIsHovered] = useState(false);
  
  // Event handlers
  const handleClick = () => {
    onSelect?.(tournament.id);
  };
  
  // Render helpers (if needed)
  const formatDate = (date: Date) => {
    return new Intl.DateTimeFormat('en-PH').format(date);
  };
  
  // Return JSX
  return (
    <div 
      onClick={handleClick}
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
    >
      <h3>{tournament.name}</h3>
      <p>{formatDate(tournament.startDate)}</p>
    </div>
  );
}
```

---

## TypeScript Best Practices

### Type Definitions
```tsx
// ✅ DO: Define proper interfaces
interface Tournament {
  id: string;
  name: string;
  format: 'swiss' | 'round-robin' | 'single-elimination';
  status: 'upcoming' | 'active' | 'completed';
  startDate: Date;
  endDate: Date;
}

// ✅ DO: Use type for unions and complex types
type TournamentFormat = Tournament['format'];
type TournamentStatus = Tournament['status'];

// ✅ DO: Props interfaces
interface TournamentListProps {
  tournaments: Tournament[];
  onTournamentSelect: (id: string) => void;
  isLoading?: boolean;
}

// ❌ DON'T: Use any
function processTournament(data: any) { } // Don't do this

// ✅ DO: Use unknown and type guards
function processTournament(data: unknown) {
  if (isTournament(data)) {
    // Now TypeScript knows data is Tournament
  }
}
```

### Type Guards
```tsx
// ✅ DO: Create type guards
function isTournament(obj: unknown): obj is Tournament {
  return (
    typeof obj === 'object' &&
    obj !== null &&
    'id' in obj &&
    'name' in obj &&
    'format' in obj
  );
}
```

---

## State Management

### Local State
```tsx
// ✅ DO: Use useState for component state
function TournamentFilter() {
  const [format, setFormat] = useState<TournamentFormat>('swiss');
  const [status, setStatus] = useState<TournamentStatus>('active');
  
  return (
    <div>
      <select value={format} onChange={(e) => setFormat(e.target.value as TournamentFormat)}>
        <option value="swiss">Swiss</option>
        <option value="round-robin">Round Robin</option>
      </select>
    </div>
  );
}
```

### Server State
```tsx
// ✅ DO: Use React Query or SWR for server state
'use client';

import useSWR from 'swr';

export function TournamentList() {
  const { data, error, isLoading } = useSWR<Tournament[]>(
    '/api/tournaments',
    fetcher
  );
  
  if (isLoading) return <LoadingSpinner />;
  if (error) return <ErrorMessage error={error} />;
  
  return (
    <div>
      {data?.map(tournament => (
        <TournamentCard key={tournament.id} tournament={tournament} />
      ))}
    </div>
  );
}
```

### Context (When Needed)
```tsx
// ✅ DO: Use Context for truly global state
'use client';

import { createContext, useContext, useState } from 'react';

interface AuthContextType {
  user: User | null;
  login: (credentials: Credentials) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  
  const login = async (credentials: Credentials) => {
    // Login logic
  };
  
  const logout = () => {
    setUser(null);
  };
  
  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
}
```

---

## Custom Hooks

### Hook Guidelines
```tsx
// ✅ DO: Prefix with 'use'
export function useTournament(id: string) {
  const [tournament, setTournament] = useState<Tournament | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);
  
  useEffect(() => {
    fetchTournament(id)
      .then(setTournament)
      .catch(setError)
      .finally(() => setLoading(false));
  }, [id]);
  
  return { tournament, loading, error };
}

// ✅ DO: Return object for multiple values
export function useForm<T>(initialValues: T) {
  const [values, setValues] = useState(initialValues);
  const [errors, setErrors] = useState<Partial<Record<keyof T, string>>>({});
  
  return { values, errors, setValues, setErrors };
}

// ✅ DO: Keep hooks focused
export function useDebounce<T>(value: T, delay: number): T {
  const [debouncedValue, setDebouncedValue] = useState(value);
  
  useEffect(() => {
    const handler = setTimeout(() => setDebouncedValue(value), delay);
    return () => clearTimeout(handler);
  }, [value, delay]);
  
  return debouncedValue;
}
```

---

## Styling

### Tailwind CSS
```tsx
// ✅ DO: Use Tailwind utility classes
export function TournamentCard({ tournament }: TournamentCardProps) {
  return (
    <div className="rounded-lg border border-gray-800 bg-gray-900 p-6 hover:border-amber-500 transition-colors">
      <h3 className="text-xl font-bold text-white">{tournament.name}</h3>
      <p className="text-gray-400 mt-2">{tournament.format}</p>
    </div>
  );
}

// ✅ DO: Use conditional classes with clsx
import clsx from 'clsx';

export function Button({ variant = 'primary', children }: ButtonProps) {
  return (
    <button
      className={clsx(
        'px-4 py-2 rounded-lg font-medium transition-colors',
        {
          'bg-amber-500 hover:bg-amber-600 text-black': variant === 'primary',
          'bg-gray-800 hover:bg-gray-700 text-white': variant === 'secondary',
        }
      )}
    >
      {children}
    </button>
  );
}

// ❌ DON'T: Inline styles (unless dynamic)
<div style={{ color: 'red' }}>Don't do this</div>

// ✅ DO: Use inline styles for dynamic values
<div style={{ width: `${progress}%` }}>Progress bar</div>
```

### CSS Modules (When Needed)
```tsx
// styles/TournamentCard.module.css
.card {
  border-radius: 0.5rem;
  padding: 1.5rem;
}

.card:hover {
  border-color: var(--color-amber);
}

// Component
import styles from './TournamentCard.module.css';

export function TournamentCard() {
  return <div className={styles.card}>...</div>;
}
```

---

## Data Fetching

### Server Components
```tsx
// ✅ DO: Fetch in Server Components
export default async function TournamentsPage() {
  const tournaments = await fetch(`${process.env.API_URL}/tournaments`, {
    next: { revalidate: 60 } // Revalidate every 60 seconds
  }).then(res => res.json());
  
  return <TournamentList tournaments={tournaments} />;
}
```

### Client Components
```tsx
// ✅ DO: Use SWR or React Query
'use client';

import useSWR from 'swr';

const fetcher = (url: string) => fetch(url).then(r => r.json());

export function TournamentList() {
  const { data, error, mutate } = useSWR<Tournament[]>(
    '/api/tournaments',
    fetcher,
    {
      refreshInterval: 30000, // Refresh every 30 seconds
      revalidateOnFocus: true
    }
  );
  
  return (
    <div>
      {data?.map(tournament => (
        <TournamentCard key={tournament.id} tournament={tournament} />
      ))}
    </div>
  );
}
```

### API Routes
```tsx
// app/api/tournaments/route.ts
import { NextResponse } from 'next/server';

export async function GET() {
  try {
    const res = await fetch(`${process.env.API_URL}/tournaments`);
    const data = await res.json();
    
    return NextResponse.json(data);
  } catch (error) {
    return NextResponse.json(
      { error: 'Failed to fetch tournaments' },
      { status: 500 }
    );
  }
}

export async function POST(request: Request) {
  try {
    const body = await request.json();
    
    // Validate body
    if (!body.name) {
      return NextResponse.json(
        { error: 'Name is required' },
        { status: 400 }
      );
    }
    
    // Create tournament
    const res = await fetch(`${process.env.API_URL}/tournaments`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body)
    });
    
    const data = await res.json();
    return NextResponse.json(data, { status: 201 });
  } catch (error) {
    return NextResponse.json(
      { error: 'Failed to create tournament' },
      { status: 500 }
    );
  }
}
```

---

## Performance Optimization

### Image Optimization
```tsx
// ✅ DO: Use Next.js Image component
import Image from 'next/image';

export function PlayerAvatar({ src, alt }: { src: string; alt: string }) {
  return (
    <Image
      src={src}
      alt={alt}
      width={64}
      height={64}
      className="rounded-full"
    />
  );
}
```

### Code Splitting
```tsx
// ✅ DO: Lazy load heavy components
import dynamic from 'next/dynamic';

const TournamentBracket = dynamic(
  () => import('@/components/TournamentBracket'),
  {
    loading: () => <LoadingSpinner />,
    ssr: false // Disable SSR if needed
  }
);
```

### Memoization
```tsx
// ✅ DO: Memoize expensive computations
import { useMemo } from 'react';

export function TournamentStats({ matches }: { matches: Match[] }) {
  const stats = useMemo(() => {
    return calculateStats(matches); // Expensive calculation
  }, [matches]);
  
  return <div>{stats.totalMatches}</div>;
}

// ✅ DO: Memoize callbacks
import { useCallback } from 'react';

export function TournamentList() {
  const handleSelect = useCallback((id: string) => {
    console.log('Selected:', id);
  }, []);
  
  return (
    <div>
      {tournaments.map(t => (
        <TournamentCard key={t.id} onSelect={handleSelect} />
      ))}
    </div>
  );
}
```

---

## Error Handling

### Error Boundaries
```tsx
// ✅ DO: Use error boundaries
'use client';

import { Component, ReactNode } from 'react';

interface Props {
  children: ReactNode;
  fallback?: ReactNode;
}

interface State {
  hasError: boolean;
}

export class ErrorBoundary extends Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.state = { hasError: false };
  }
  
  static getDerivedStateFromError() {
    return { hasError: true };
  }
  
  componentDidCatch(error: Error, errorInfo: any) {
    console.error('Error caught:', error, errorInfo);
  }
  
  render() {
    if (this.state.hasError) {
      return this.props.fallback || <div>Something went wrong</div>;
    }
    
    return this.props.children;
  }
}
```

### Error States
```tsx
// ✅ DO: Handle loading and error states
export function TournamentList() {
  const { data, error, isLoading } = useSWR<Tournament[]>('/api/tournaments');
  
  if (isLoading) {
    return <LoadingSpinner />;
  }
  
  if (error) {
    return (
      <ErrorMessage 
        title="Failed to load tournaments"
        message={error.message}
        retry={() => mutate()}
      />
    );
  }
  
  if (!data || data.length === 0) {
    return <EmptyState message="No tournaments found" />;
  }
  
  return (
    <div>
      {data.map(tournament => (
        <TournamentCard key={tournament.id} tournament={tournament} />
      ))}
    </div>
  );
}
```

---

## Accessibility

```tsx
// ✅ DO: Use semantic HTML
<button onClick={handleClick}>Register</button>
<nav>...</nav>
<main>...</main>
<article>...</article>

// ✅ DO: Add ARIA labels
<button aria-label="Close modal" onClick={onClose}>
  <XIcon />
</button>

// ✅ DO: Keyboard navigation
<div
  role="button"
  tabIndex={0}
  onClick={handleClick}
  onKeyDown={(e) => e.key === 'Enter' && handleClick()}
>
  Click me
</div>

// ✅ DO: Alt text for images
<Image src={src} alt="Tournament bracket visualization" />
```

---

## Testing

### Component Tests
```tsx
// ✅ DO: Test component behavior
import { render, screen, fireEvent } from '@testing-library/react';

describe('TournamentCard', () => {
  it('renders tournament name', () => {
    const tournament = { id: '1', name: 'Test Tournament' };
    render(<TournamentCard tournament={tournament} />);
    
    expect(screen.getByText('Test Tournament')).toBeInTheDocument();
  });
  
  it('calls onSelect when clicked', () => {
    const onSelect = jest.fn();
    const tournament = { id: '1', name: 'Test Tournament' };
    
    render(<TournamentCard tournament={tournament} onSelect={onSelect} />);
    fireEvent.click(screen.getByText('Test Tournament'));
    
    expect(onSelect).toHaveBeenCalledWith('1');
  });
});
```

---

## Code Review Checklist

Before submitting code:
- [ ] TypeScript types are properly defined
- [ ] Server Components used where possible
- [ ] Client Components only when needed
- [ ] Loading and error states handled
- [ ] Accessibility considerations met
- [ ] Performance optimizations applied
- [ ] Responsive design implemented
- [ ] Code is properly formatted
- [ ] No console.logs in production code
- [ ] Components are properly tested

---

## Resources

- [Next.js Documentation](https://nextjs.org/docs)
- [React Documentation](https://react.dev)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)