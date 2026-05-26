export default function Footer() {
  return (
    <footer className="relative py-10 sm:py-12 px-4 sm:px-6 lg:px-8 border-t border-dark-border/40">
      <div className="max-w-7xl mx-auto text-center w-full">
        {/* Decorative top line */}
        <div className="absolute top-0 left-1/2 transform -translate-x-1/2 w-40 sm:w-48 h-px bg-gradient-to-r from-transparent via-amber-primary/25 to-transparent" />

        {/* Main brand text */}
        <p className="font-[family-name:var(--font-rajdhani)] text-metal-silver text-sm sm:text-base font-semibold tracking-wide mb-2 uppercase">
          BBXTournamentPH © 2026
        </p>

        {/* Creator credit with hover effect */}
        <p className="font-[family-name:var(--font-inter)] text-gray-500/60 text-xs sm:text-sm font-light">
          Created by{" "}
          <a
            href="https://www.facebook.com/mastervinvin.fb"
            target="_blank"
            rel="noopener noreferrer"
            className="text-gray-400/70 hover:text-amber-primary hover:drop-shadow-[0_0_6px_rgba(255,176,32,0.3)] transition-all duration-300 cursor-pointer font-medium"
          >
            MastervinviN
          </a>
        </p>

        {/* Subtle metallic accent */}
        <div className="mt-5 flex items-center justify-center gap-2">
          <div className="w-1 h-1 rounded-full bg-amber-primary/30" />
          <div className="w-1 h-1 rounded-full bg-amber-primary/30" />
          <div className="w-1 h-1 rounded-full bg-amber-primary/30" />
        </div>
      </div>
    </footer>
  );
}

// Made with Bob
