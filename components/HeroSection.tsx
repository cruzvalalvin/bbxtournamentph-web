export default function HeroSection() {
  return (
    <section className="relative min-h-screen flex items-center justify-center px-4 py-20">
      <div className="max-w-5xl mx-auto text-center space-y-8">
        {/* Main Title */}
        <div className="space-y-4">
          <h1 className="text-6xl md:text-8xl font-bold tracking-tight text-glow">
            <span className="bg-gradient-to-r from-neon-green via-neon-green-dark to-neon-green bg-clip-text text-transparent">
              BBXTournamentPH
            </span>
          </h1>
          
          <p className="text-xl md:text-2xl text-gray-400 font-light tracking-wide">
            Competitive Platform for Beyblade X
          </p>
        </div>

        {/* Status Badges */}
        <div className="flex flex-col sm:flex-row items-center justify-center gap-4 pt-8">
          <div className="px-6 py-3 bg-dark-card border border-neon-green/30 rounded-lg glow-green">
            <p className="text-neon-green font-mono text-sm md:text-base font-semibold tracking-wider">
              UNDER CONSTRUCTION
            </p>
          </div>
          
          <div className="px-6 py-3 bg-dark-card border border-neon-green/30 rounded-lg glow-green">
            <p className="text-neon-green font-mono text-sm md:text-base font-semibold tracking-wider">
              CLOSED ALPHA COMING SOON
            </p>
          </div>
        </div>

        {/* Description */}
        <div className="pt-8 max-w-3xl mx-auto">
          <p className="text-gray-300 text-base md:text-lg leading-relaxed">
            Tournament management, rankings, communities, and competitive ecosystem 
            for Beyblade X players in the Philippines.
          </p>
        </div>

        {/* Decorative line */}
        <div className="pt-12">
          <div className="h-px w-32 mx-auto bg-gradient-to-r from-transparent via-neon-green to-transparent" />
        </div>
      </div>
    </section>
  );
}

// Made with Bob
