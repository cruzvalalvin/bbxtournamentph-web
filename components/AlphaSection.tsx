export default function AlphaSection() {
  return (
    <section className="relative py-12 sm:py-16 md:py-20 px-4 sm:px-6">
      <div className="max-w-4xl mx-auto w-full">
        {/* Container with border and glow */}
        <div className="relative bg-dark-card border border-neon-green/30 rounded-xl sm:rounded-2xl p-6 sm:p-8 md:p-12 glow-green">
          {/* Alpha Badge */}
          <div className="absolute -top-3 sm:-top-4 left-1/2 transform -translate-x-1/2">
            <div className="px-4 sm:px-6 py-1.5 sm:py-2 bg-neon-green text-dark-bg font-mono font-bold text-xs sm:text-sm tracking-wider rounded-full shadow-lg whitespace-nowrap">
              ALPHA BUILD
            </div>
          </div>

          {/* Content */}
          <div className="text-center space-y-4 sm:space-y-6 pt-4 sm:pt-4">
            <h2 className="text-2xl sm:text-3xl md:text-4xl font-bold text-neon-green px-2">
              Forged for the community,
              <br />
              by the community.
            </h2>

            <p className="text-gray-300 text-sm sm:text-base md:text-lg leading-relaxed max-w-2xl mx-auto px-2">
              We're building the ultimate competitive platform for Beyblade X players
              in the Philippines. Join us in shaping the future of competitive Beyblade.
            </p>

            {/* Decorative elements */}
            <div className="flex items-center justify-center gap-3 sm:gap-4 pt-4 sm:pt-6">
              <div className="h-px w-12 sm:w-16 bg-gradient-to-r from-transparent to-neon-green" />
              <div className="w-2 h-2 bg-neon-green rounded-full animate-pulse" />
              <div className="h-px w-12 sm:w-16 bg-gradient-to-l from-transparent to-neon-green" />
            </div>
          </div>

          {/* Background decoration */}
          <div className="absolute inset-0 rounded-xl sm:rounded-2xl bg-gradient-to-br from-neon-green/5 to-transparent pointer-events-none" />
        </div>
      </div>
    </section>
  );
}

// Made with Bob
