import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';

export default function RootLayout() {

	return (
		<Stack initialRouteName="index">
			<Stack.Screen
				name="index"
				options={{
					headerShown: true,
					title: 'Meus dados',
					headerStyle: {
						backgroundColor: colors.primary,
					},
					headerTintColor: colors.secondary,
				}}
			/>
		</Stack>
	);
}