import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "BBXTournamentPH - Competitive Beyblade X Platform",
  description: "Tournament management, rankings, communities, and competitive ecosystem for Beyblade X players in the Philippines.",
  keywords: ["Beyblade X", "Tournament", "Philippines", "Competitive", "Esports"],
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
      className={`${geistSans.variable} ${geistMono.variable} h-full antialiased overflow-x-hidden`}
    >
      <body className="min-h-full flex flex-col bg-dark-bg text-foreground overflow-x-hidden">{children}</body>
    </html>
  );
}

// Made with Bob
