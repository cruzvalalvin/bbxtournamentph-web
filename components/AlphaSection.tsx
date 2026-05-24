export default function AlphaSection() {
  return (
    <section className="relative py-20 px-4">
      <div className="max-w-4xl mx-auto">
        {/* Container with border and glow */}
        <div className="relative bg-dark-card border border-neon-green/30 rounded-2xl p-8 md:p-12 glow-green">
          {/* Alpha Badge */}
          <div className="absolute -top-4 left-1/2 transform -translate-x-1/2">
            <div className="px-6 py-2 bg-neon-green text-dark-bg font-mono font-bold text-sm tracking-wider rounded-full shadow-lg">
              ALPHA BUILD
            </div>
          </div>

          {/* Content */}
          <div className="text-center space-y-6 pt-4">
            <h2 className="text-3xl md:text-4xl font-bold text-neon-green">
              Forged for the community,
              <br />
              by the community.
            </h2>

            <p className="text-gray-300 text-lg leading-relaxed max-w-2xl mx-auto">
              We're building the ultimate competitive platform for Beyblade X players 
              in the Philippines. Join us in shaping the future of competitive Beyblade.
            </p>

            {/* Decorative elements */}
            <div className="flex items-center justify-center gap-4 pt-6">
              <div className="h-px w-16 bg-gradient-to-r from-transparent to-neon-green" />
              <div className="w-2 h-2 bg-neon-green rounded-full animate-pulse" />
              <div className="h-px w-16 bg-gradient-to-l from-transparent to-neon-green" />
            </div>
          </div>

          {/* Background decoration */}
          <div className="absolute inset-0 rounded-2xl bg-gradient-to-br from-neon-green/5 to-transparent pointer-events-none" />
        </div>
      </div>
    </section>
  );
}

// Made with Bob
