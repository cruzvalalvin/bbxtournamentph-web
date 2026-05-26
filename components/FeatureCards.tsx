interface Feature {
  title: string;
  description: string;
  icon: string;
}

const features: Feature[] = [
  {
    title: "Live Tournament Brackets",
    description: "Real-time bracket updates and match progression tracking",
    icon: "🏆",
  },
  {
    title: "Judge Match System",
    description: "Streamlined judging interface for tournament officials",
    icon: "⚖️",
  },
  {
    title: "Community Rankings",
    description: "Comprehensive leaderboards and player statistics",
    icon: "📊",
  },
  {
    title: "Player Profiles",
    description: "Detailed profiles with match history and achievements",
    icon: "👤",
  },
  {
    title: "Tournament Seasons",
    description: "Organized seasonal competitions with rewards",
    icon: "🎯",
  },
  {
    title: "Real-Time Match Tracking",
    description: "Live match updates and score tracking",
    icon: "⚡",
  },
];

export default function FeatureCards() {
  return (
    <section className="relative py-16 sm:py-20 md:py-24 px-4 sm:px-6 lg:px-8">
      <div className="max-w-7xl mx-auto w-full">
        {/* Section Header - Tournament Broadcast Style */}
        <div className="text-center mb-12 sm:mb-14 md:mb-16 px-4">
          <div className="inline-block relative mb-5">
            <h2 className="font-[family-name:var(--font-rajdhani)] text-3xl sm:text-4xl md:text-5xl font-bold text-amber-primary mb-3 uppercase tracking-tight">
              Feature Preview
            </h2>
            <div className="absolute -bottom-2 left-0 right-0 h-0.5 bg-gradient-to-r from-transparent via-amber-primary/60 to-transparent" />
          </div>
          <p className="font-[family-name:var(--font-inter)] text-metal-silver text-sm sm:text-base md:text-lg font-light">
            Built for competitive Beyblade X players
          </p>
        </div>

        {/* Feature Grid - Metallic Cards */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-5 sm:gap-6">
          {features.map((feature, index) => (
            <div
              key={index}
              className="group relative bg-gradient-to-br from-gunmetal to-dark-card border border-dark-border rounded-lg p-6 sm:p-7 hover-lift-arena hover:border-amber-primary/40 transition-all duration-300 card-shine overflow-hidden"
            >
              {/* Metallic top accent */}
              <div className="absolute top-0 left-0 right-0 h-px bg-gradient-to-r from-transparent via-amber-primary/20 to-transparent" />
              
              {/* Icon with subtle glow */}
              <div className="text-4xl sm:text-5xl mb-4 group-hover:scale-105 transition-transform duration-300 filter drop-shadow-[0_0_6px_rgba(255,176,32,0.2)]">
                {feature.icon}
              </div>

              {/* Title - Rajdhani Font */}
              <h3 className="font-[family-name:var(--font-rajdhani)] text-xl sm:text-2xl font-semibold text-amber-primary mb-3 group-hover:text-amber-bright transition-colors duration-300 uppercase tracking-wide">
                {feature.title}
              </h3>

              {/* Description - Inter Font */}
              <p className="font-[family-name:var(--font-inter)] text-gray-400 text-sm sm:text-base leading-relaxed">
                {feature.description}
              </p>

              {/* Hover metallic glow effect */}
              <div className="absolute inset-0 rounded-lg bg-gradient-to-br from-amber-primary/3 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300 pointer-events-none" />
              
              {/* Bottom metallic accent */}
              <div className="absolute bottom-0 left-0 right-0 h-px bg-gradient-to-r from-transparent via-metal-steel/15 to-transparent" />
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

// Made with Bob
