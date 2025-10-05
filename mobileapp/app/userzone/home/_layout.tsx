import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';

export default function RootLayout() {
	return (
		<Stack initialRouteName="index" >
			<Stack.Screen name="index"
				options={{
					headerShown: true,
					headerTitle: '',
					headerStyle: {
						backgroundColor: colors.primary,
					},
					headerTintColor: colors.secondary,
				}}
			/>

			<Stack.Screen name="exames"
				options={{ headerShown: false }}
			/>

			<Stack.Screen name="unidades"
				options={{ headerShown: false }}
			/>

			<Stack.Screen name="convenios"
				options={{ headerShown: false }}
			/>
		</Stack>
	);
}
