import { useFonts } from 'expo-font';
import { Stack } from 'expo-router';
import * as SplashScreen from 'expo-splash-screen';
import { StatusBar } from 'expo-status-bar';
import { Suspense, useEffect } from 'react';
import { ActivityIndicator } from 'react-native';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { ActionSheetProvider } from '@expo/react-native-action-sheet';
import { RootSiblingParent } from 'react-native-root-siblings';
import 'react-native-reanimated';
import '@/assets/styles/global.css';

SplashScreen.preventAutoHideAsync();

export default function RootLayout() {
	const [loaded] = useFonts({
		SpaceMono: require('@/assets/fonts/SpaceMono-Regular.ttf'),
	});

	useEffect(() => {
		if (loaded) {
			SplashScreen.hideAsync();
		}
	}, [loaded]);

	if (!loaded) {
		return null;
	}

	return (
		<Suspense fallback={<ActivityIndicator size={'small'} />}>

			<StatusBar style="auto" />
			<SafeAreaProvider>
				<RootSiblingParent>
					<ActionSheetProvider>
						<Stack initialRouteName="index">
							<Stack.Screen name="index" options={{ headerShown: false }} />
							<Stack.Screen name="auth" options={{ headerShown: false }} />
							<Stack.Screen name="userzone" options={{ headerShown: false }} />
							<Stack.Screen name="adminzone" options={{ headerShown: false }} />
							<Stack.Screen name="+not-found" />
						</Stack>
					</ActionSheetProvider>
				</RootSiblingParent>
			</SafeAreaProvider>
		</Suspense>

	);
}
