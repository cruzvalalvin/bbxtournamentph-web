import type { Metadata } from "next";
import { Rajdhani, Inter, Geist_Mono } from "next/font/google";
import "./globals.css";

// Modern esports-style headline font - more readable than Orbitron
const rajdhani = Rajdhani({
  variable: "--font-rajdhani",
  subsets: ["latin"],
  weight: ["300", "400", "500", "600", "700"],
  display: "swap",
});

// Clean readable UI font for descriptions and buttons
const inter = Inter({
  variable: "--font-inter",
  subsets: ["latin"],
  weight: ["300", "400", "500", "600", "700"],
  display: "swap",
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "BBXTournamentPH - Official Competitive Beyblade X Platform",
  description: "Official competitive tournament ecosystem for Beyblade X players in the Philippines. Tournament management, rankings, and competitive community.",
  keywords: ["Beyblade X", "Tournament", "Philippines", "Competitive", "Esports", "Championship", "Arena"],
  viewport: "width=device-width, initial-scale=1, maximum-scale=5",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html
      lang="en"
      className={`${rajdhani.variable} ${inter.variable} ${geistMono.variable} h-full antialiased overflow-x-hidden`}
    >
      <body className="min-h-full flex flex-col bg-dark-bg text-foreground overflow-x-hidden">{children}</body>
    </html>
  );
}

// Made with Bob
