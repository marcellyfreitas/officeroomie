import { Stack } from 'expo-router';
import { DrawerToggleButton } from '@react-navigation/drawer';
import { colors } from '@/utils/constants';

export default function AddressLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen name="index" options={{
				headerShown: true,
				title: 'Unidades de saúde',
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