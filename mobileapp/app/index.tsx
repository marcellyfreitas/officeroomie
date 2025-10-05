import { useEffect } from 'react';
import { router } from 'expo-router';
import LoadingLine from '@/components/ui/LoadingLine';
import { ThemedView } from '@/components/ui/ThemedView';

const SplashScreen = () => {
	useEffect(() => {
		setTimeout(() => {
			router.replace('/auth/userlogin');
		}, 600);
	}, []);

	return (
		<ThemedView className="items-center justify-center flex-1">
			<LoadingLine />
		</ThemedView>
	);
};
export default SplashScreen;