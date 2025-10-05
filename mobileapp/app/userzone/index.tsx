import { router } from 'expo-router';
import { Suspense, useEffect } from 'react';



const HomeScreen = () => {
	async function redirectToHome() {
		await new Promise(resolve => setTimeout(resolve, 600));
		router.replace('/userzone/home');
	}

	useEffect(() => {
		redirectToHome();
	}, []);

	return (<Suspense />);
};

export default HomeScreen;