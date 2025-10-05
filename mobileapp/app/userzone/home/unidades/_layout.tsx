import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';


export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen
				name="index"
				options={{
					headerShown: true,
					headerTitle: 'Unidades',
					headerStyle: {
						backgroundColor: colors.primary,
					},
					headerTintColor: colors.secondary,
				}}
			/>
		</Stack>
	);
}
