import { Stack } from 'expo-router';
import { DrawerToggleButton } from '@react-navigation/drawer';
import { colors } from '@/utils/constants';

type RouteParams = {
	specializationName?: string;
};

export default function MedicosLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen
				name="index"
				options={{
					headerShown: true,
					title: 'Médicos Credenciados',
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
