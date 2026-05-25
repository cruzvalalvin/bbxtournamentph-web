export default function HeroSection() {
  return (
    <section className="relative min-h-screen flex items-center justify-center px-4 sm:px-6 py-20">
      <div className="max-w-5xl mx-auto text-center space-y-6 sm:space-y-8 w-full">
        {/* Main Title */}
        <div className="space-y-3 sm:space-y-4">
          <h1 className="text-4xl sm:text-5xl md:text-7xl lg:text-8xl font-bold tracking-tight text-glow break-words">
            <span className="bg-gradient-to-r from-neon-green via-neon-green-dark to-neon-green bg-clip-text text-transparent">
              BBXTournamentPH
            </span>
          </h1>
          
          <p className="text-base sm:text-lg md:text-xl lg:text-2xl text-gray-400 font-light tracking-wide px-2">
            Competitive Platform for Beyblade X
          </p>
        </div>

        {/* Status Badges */}
        <div className="flex flex-col sm:flex-row items-center justify-center gap-3 sm:gap-4 pt-6 sm:pt-8 w-full px-2">
          <div className="w-full sm:w-auto px-4 sm:px-6 py-3 bg-dark-card border border-neon-green/30 rounded-lg glow-green">
            <p className="text-neon-green font-mono text-xs sm:text-sm md:text-base font-semibold tracking-wider">
              UNDER CONSTRUCTION
            </p>
          </div>
          
          <div className="w-full sm:w-auto px-4 sm:px-6 py-3 bg-dark-card border border-neon-green/30 rounded-lg glow-green">
            <p className="text-neon-green font-mono text-xs sm:text-sm md:text-base font-semibold tracking-wider">
              CLOSED ALPHA COMING SOON
            </p>
          </div>
        </div>

        {/* Description */}
        <div className="pt-6 sm:pt-8 max-w-3xl mx-auto px-4">
          <p className="text-gray-300 text-sm sm:text-base md:text-lg leading-relaxed">
            Tournament management, rankings, communities, and competitive ecosystem
            for Beyblade X players in the Philippines.
          </p>
        </div>

        {/* Decorative line */}
        <div className="pt-8 sm:pt-12">
          <div className="h-px w-24 sm:w-32 mx-auto bg-gradient-to-r from-transparent via-neon-green to-transparent" />
        </div>
      </div>
    </section>
  );
}

// Made with Bob
