import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';


export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen
				name="index"
				options={{
					headerShown: true,
					headerTitle: 'Meu perfil',
					headerStyle: {
						backgroundColor: colors.primary,
					},
					headerTintColor: colors.secondary,
				}}
			/>
			<Stack.Screen name="meusdados" options={{ headerShown: false }} />
			<Stack.Screen name="exclusao" options={{ headerShown: false }} />
		</Stack>
	);
}
