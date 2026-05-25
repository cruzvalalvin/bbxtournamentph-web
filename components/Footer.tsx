export default function Footer() {
  return (
    <footer className="relative py-6 sm:py-8 px-4 sm:px-6">
      <div className="max-w-7xl mx-auto text-center w-full">
        {/* Main brand text */}
        <p className="text-gray-400/60 text-xs sm:text-sm font-light tracking-wide mb-2">
          BBXTournamentPH © 2026
        </p>
        
        {/* Creator credit with hover effect */}
        <p className="text-gray-500/40 text-[10px] sm:text-xs font-light">
          Created by{" "}
          <span className="text-gray-400/50 hover:text-neon-green/80 hover:drop-shadow-[0_0_8px_rgba(0,255,157,0.4)] transition-all duration-300 cursor-default">
            MastervinviN
          </span>
        </p>
      </div>
    </footer>
  );
}

// Made with Bob
