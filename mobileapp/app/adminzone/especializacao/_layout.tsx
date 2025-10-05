import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';
import { DrawerToggleButton } from '@react-navigation/drawer';

export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen
				name="index"
				options={{
					headerShown: true,
					title: 'Especializações',
					headerStyle: {
						backgroundColor: colors.primary,
					},
					headerTintColor: colors.secondary,
					headerLeft: ({ tintColor }) => (<DrawerToggleButton tintColor={tintColor} />),
				}}
			/>
		</Stack>
	);
}