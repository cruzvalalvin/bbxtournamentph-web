export default function GridBackground() {
  return (
    <div className="fixed inset-0 -z-10 overflow-hidden">
      {/* Base dark metallic background */}
      <div className="absolute inset-0 bg-gradient-to-b from-dark-bg via-gunmetal/30 to-dark-bg" />
      
      {/* Arena grid pattern */}
      <div className="absolute inset-0 arena-grid" />
      
      {/* Scanning line effect - tournament broadcast style */}
      <div className="scan-line" />
      
      {/* Subtle arena spotlight effect */}
      <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_center,_var(--tw-gradient-stops))] from-amber-primary/8 via-transparent to-transparent" />
      
      {/* Top arena lighting */}
      <div className="absolute top-0 left-0 right-0 h-px bg-gradient-to-r from-transparent via-amber-primary/30 to-transparent" />
      
      {/* Metallic vignette for depth */}
      <div className="absolute inset-0 bg-gradient-to-b from-transparent via-transparent to-dark-bg/80" />
      
      {/* Corner accent lights - arena style */}
      <div className="absolute top-0 left-0 w-64 h-64 bg-gradient-to-br from-amber-primary/5 to-transparent blur-3xl" />
      <div className="absolute top-0 right-0 w-64 h-64 bg-gradient-to-bl from-amber-primary/5 to-transparent blur-3xl" />
    </div>
  );
}

// Made with Bob
