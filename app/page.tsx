import GridBackground from "@/components/GridBackground";
import HeroSection from "@/components/HeroSection";
import FeatureCards from "@/components/FeatureCards";
import AlphaSection from "@/components/AlphaSection";
import Footer from "@/components/Footer";

export default function Home() {
  return (
    <main className="relative min-h-screen">
      {/* Animated Grid Background */}
      <GridBackground />

      {/* Hero Section */}
      <HeroSection />

      {/* Feature Cards */}
      <FeatureCards />

      {/* Closed Alpha Section */}
      <AlphaSection />

      {/* Footer */}
      <Footer />
    </main>
  );
}

// Made with Bob
