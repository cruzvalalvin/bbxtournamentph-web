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
    <section className="relative py-20 px-4">
      <div className="max-w-7xl mx-auto">
        {/* Section Header */}
        <div className="text-center mb-16">
          <h2 className="text-4xl md:text-5xl font-bold text-neon-green mb-4">
            Feature Preview
          </h2>
          <p className="text-gray-400 text-lg">
            Built for competitive Beyblade X players
          </p>
        </div>

        {/* Feature Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {features.map((feature, index) => (
            <div
              key={index}
              className="group relative bg-dark-card border border-dark-border rounded-xl p-6 hover-lift hover:border-neon-green/50 transition-all duration-300"
            >
              {/* Icon */}
              <div className="text-5xl mb-4 group-hover:scale-110 transition-transform duration-300">
                {feature.icon}
              </div>

              {/* Title */}
              <h3 className="text-xl font-bold text-neon-green mb-3 group-hover:text-glow transition-all duration-300">
                {feature.title}
              </h3>

              {/* Description */}
              <p className="text-gray-400 leading-relaxed">
                {feature.description}
              </p>

              {/* Hover glow effect */}
              <div className="absolute inset-0 rounded-xl bg-neon-green/5 opacity-0 group-hover:opacity-100 transition-opacity duration-300 -z-10" />
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

// Made with Bob
