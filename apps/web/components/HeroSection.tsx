export default function HeroSection() {
  return (
    <section className="relative min-h-screen flex items-center justify-center px-4 sm:px-6 lg:px-8 py-16 sm:py-20">
      <div className="max-w-6xl mx-auto text-center space-y-8 sm:space-y-10 w-full">
        {/* Tournament Broadcast Style Header */}
        <div className="space-y-5 sm:space-y-7">
          {/* Main Title - Unified Brand Name with Responsive Scaling */}
          <h1 className="font-[family-name:var(--font-rajdhani)] font-bold tracking-tight">
            {/* Unified brand name - scales responsively */}
            <span className="block text-4xl sm:text-5xl md:text-6xl lg:text-7xl xl:text-8xl leading-tight">
              <span className="bg-gradient-to-r from-amber-bright via-amber-primary to-amber-dark bg-clip-text text-transparent text-glow-amber">
                BBXTournamentPH
              </span>
            </span>
          </h1>
          
          {/* Subtitle with cleaner styling */}
          <div className="relative inline-block px-4">
            <p className="font-[family-name:var(--font-rajdhani)] text-lg sm:text-xl md:text-2xl lg:text-3xl text-amber-primary font-semibold tracking-wide uppercase">
              Official Competitive Platform
            </p>
          </div>
          
          <p className="font-[family-name:var(--font-inter)] text-base sm:text-lg md:text-xl text-gray-300 font-normal leading-relaxed px-4 max-w-2xl mx-auto">
            Beyblade X Championship Ecosystem
          </p>
        </div>

        {/* Description - More prominent */}
        <div className="pt-4 sm:pt-6 max-w-3xl mx-auto px-6">
          <p className="font-[family-name:var(--font-inter)] text-gray-300 text-base sm:text-lg md:text-xl leading-relaxed">
            Tournament management, rankings, communities, and competitive ecosystem
            for Beyblade X players in the Philippines.
          </p>
        </div>

        {/* Status Badges - Cleaner Premium Style */}
        <div className="flex flex-col sm:flex-row items-center justify-center gap-4 sm:gap-5 pt-8 sm:pt-10 w-full px-4">
          <div className="relative w-full sm:w-auto min-w-[220px] px-7 py-3.5 bg-gradient-to-br from-gunmetal via-dark-card to-gunmetal border border-amber-primary/30 rounded-lg overflow-hidden group hover:border-amber-primary/50 transition-all duration-300">
            <div className="absolute inset-0 bg-gradient-to-r from-amber-primary/0 via-amber-primary/8 to-amber-primary/0 opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
            <p className="relative text-amber-primary font-[family-name:var(--font-rajdhani)] text-base sm:text-lg font-bold tracking-wider uppercase">
              Under Construction
            </p>
          </div>
          
          <div className="relative w-full sm:w-auto min-w-[220px] px-7 py-3.5 bg-gradient-to-br from-gunmetal via-dark-card to-gunmetal border border-amber-primary/30 rounded-lg overflow-hidden group hover:border-amber-primary/50 transition-all duration-300">
            <div className="absolute inset-0 bg-gradient-to-r from-amber-primary/0 via-amber-primary/8 to-amber-primary/0 opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
            <p className="relative text-amber-primary font-[family-name:var(--font-rajdhani)] text-base sm:text-lg font-bold tracking-wider uppercase">
              Closed Alpha Coming Soon
            </p>
          </div>
        </div>

        {/* Decorative Arena Elements */}
        <div className="pt-10 sm:pt-12 flex items-center justify-center gap-4 sm:gap-5">
          <div className="h-px w-16 sm:w-24 bg-gradient-to-r from-transparent to-amber-primary/50" />
          <div className="w-2 h-2 rounded-full bg-amber-primary shadow-[0_0_12px_rgba(255,176,32,0.5)] animate-pulse" />
          <div className="h-px w-16 sm:w-24 bg-gradient-to-l from-transparent to-amber-primary/50" />
        </div>

        {/* Championship Badge - Premium Style */}
        <div className="pt-4 sm:pt-5">
          <div className="inline-flex items-center gap-2.5 px-6 sm:px-7 py-2.5 border border-amber-primary/25 rounded-full bg-gradient-to-r from-gunmetal/50 via-dark-card/50 to-gunmetal/50">
            <div className="w-1.5 h-1.5 rounded-full bg-amber-primary shadow-[0_0_6px_rgba(255,176,32,0.4)]" />
            <span className="font-[family-name:var(--font-rajdhani)] text-sm sm:text-base text-amber-primary uppercase tracking-wider font-semibold">
              Philippine Championship Series
            </span>
            <div className="w-1.5 h-1.5 rounded-full bg-amber-primary shadow-[0_0_6px_rgba(255,176,32,0.4)]" />
          </div>
        </div>
      </div>
    </section>
  );
}

// Made with Bob
