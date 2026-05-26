export default function AlphaSection() {
  return (
    <section className="relative py-16 sm:py-20 md:py-24 px-4 sm:px-6 lg:px-8">
      <div className="max-w-5xl mx-auto w-full">
        {/* Premium Tournament Container */}
        <div className="relative bg-gradient-to-br from-gunmetal via-dark-card to-gunmetal border border-amber-primary/25 rounded-xl p-8 sm:p-10 md:p-14 overflow-hidden">
          {/* Top metallic accent line */}
          <div className="absolute top-0 left-0 right-0 h-px bg-gradient-to-r from-transparent via-amber-primary/60 to-transparent" />
          
          {/* Alpha Badge - Premium Championship Style */}
          <div className="absolute -top-4 sm:-top-5 left-0 right-0 flex justify-center z-10">
            <div className="relative px-6 sm:px-8 py-3 sm:py-3.5 bg-gradient-to-r from-amber-dark via-amber-primary to-amber-dark text-dark-bg font-[family-name:var(--font-rajdhani)] font-bold text-sm sm:text-base tracking-[0.15em] rounded-md shadow-xl whitespace-nowrap uppercase">
              <span className="relative z-10 flex items-center justify-center gap-2 sm:gap-2.5">
                <span className="w-1.5 h-1.5 rounded-full bg-dark-bg/60 animate-pulse" />
                CLOSED ALPHA • 2026
                <span className="w-1.5 h-1.5 rounded-full bg-dark-bg/60 animate-pulse" />
              </span>
              <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/10 to-transparent" />
            </div>
          </div>

          {/* Content */}
          <div className="text-center space-y-6 sm:space-y-8 pt-6 sm:pt-8 relative z-10">
            {/* Main Headline - Tournament Style */}
            <h2 className="font-[family-name:var(--font-rajdhani)] text-2xl sm:text-3xl md:text-4xl lg:text-5xl font-bold text-white px-4 leading-tight">
              <span className="block text-amber-primary">FORGED FOR THE COMMUNITY,</span>
              <span className="block mt-2 sm:mt-3">BY THE COMMUNITY.</span>
            </h2>

            {/* Description */}
            <p className="font-[family-name:var(--font-inter)] text-gray-300 text-sm sm:text-base md:text-lg leading-relaxed max-w-2xl mx-auto px-4">
              We're building the ultimate competitive platform for Beyblade X players
              in the Philippines. Join us in shaping the future of competitive Beyblade.
            </p>

            {/* Arena Decorative Elements */}
            <div className="flex flex-col sm:flex-row items-center justify-center gap-4 sm:gap-6 pt-6 sm:pt-8">
              <div className="hidden sm:flex items-center gap-2">
                <div className="h-px w-16 md:w-24 bg-gradient-to-r from-transparent to-amber-primary/50" />
                <div className="w-1.5 h-1.5 rounded-full bg-amber-primary shadow-[0_0_8px_rgba(255,176,32,0.4)] animate-pulse" />
              </div>
              
              <div className="px-5 py-2 border border-amber-primary/25 rounded-full">
                <span className="font-[family-name:var(--font-rajdhani)] text-xs sm:text-sm text-amber-primary uppercase tracking-wide font-semibold">
                  Championship Series
                </span>
              </div>
              
              <div className="hidden sm:flex items-center gap-2">
                <div className="w-1.5 h-1.5 rounded-full bg-amber-primary shadow-[0_0_8px_rgba(255,176,32,0.4)] animate-pulse" />
                <div className="h-px w-16 md:w-24 bg-gradient-to-l from-transparent to-amber-primary/50" />
              </div>
            </div>
          </div>

          {/* Background metallic gradient overlay */}
          <div className="absolute inset-0 rounded-xl bg-gradient-to-br from-amber-primary/3 via-transparent to-amber-primary/3 pointer-events-none" />
          
          {/* Corner accent lights - Subtle */}
          <div className="absolute top-0 left-0 w-40 h-40 bg-gradient-to-br from-amber-primary/8 to-transparent blur-3xl pointer-events-none" />
          <div className="absolute bottom-0 right-0 w-40 h-40 bg-gradient-to-tl from-amber-primary/8 to-transparent blur-3xl pointer-events-none" />
          
          {/* Bottom metallic accent line */}
          <div className="absolute bottom-0 left-0 right-0 h-px bg-gradient-to-r from-transparent via-metal-steel/20 to-transparent" />
        </div>
      </div>
    </section>
  );
}

// Made with Bob
