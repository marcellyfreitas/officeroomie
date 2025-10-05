import { Stack } from 'expo-router';
import { StatusBar } from 'expo-status-bar';
import { AuthProvider as UserAuthenticationProvider } from '@/contexts/UserAuthenticationContext';
import { AuthProvider as AdminAuthenticationProvider } from '@/contexts/AdminAuthenticationContext';
import '@/assets/styles/global.css';
import { SafeAreaProvider, SafeAreaView } from 'react-native-safe-area-context';
import { colors } from '@/utils/constants';

export default function AuthLayout() {

	return (
		<SafeAreaProvider>
			<SafeAreaView className="flex flex-1">
				<AdminAuthenticationProvider>
					<UserAuthenticationProvider>
						<Stack initialRouteName="userlogin">
							<Stack.Screen name="userlogin" options={{ headerShown: false }} />
							<Stack.Screen name="register" options={{ headerShown: false }} />
							<Stack.Screen name="adminlogin" options={{ headerShown: false }} />
							<Stack.Screen
								name="termos"
								options={{
									headerShown: true,
									headerTitle: 'Termos de uso',
									headerStyle: {
										backgroundColor: colors.primary,
									},
									headerTintColor: colors.secondary,
								}}
							/>
						</Stack>
						<StatusBar style="auto" />
					</UserAuthenticationProvider>
				</AdminAuthenticationProvider>
			</SafeAreaView>
		</SafeAreaProvider>
	);
}