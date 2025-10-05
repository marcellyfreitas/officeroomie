'use client';

import HomeBeneficios from './home-beneficios';
import HomeFooter from './home-footer';
import HomeHero from './home-hero';
import HomeMobileapp from './home-mobileapp';
import HomeNavbar from './home-navbar';
import HomeSobre from './home-sobre';

export default function HomeIndex() {
	return (
		<main className="flex min-h-screen flex-col">
			<HomeNavbar />

			<HomeHero />

			<HomeMobileapp />

			<HomeBeneficios />

			<HomeSobre />

			<HomeFooter />
		</main>
	);
}