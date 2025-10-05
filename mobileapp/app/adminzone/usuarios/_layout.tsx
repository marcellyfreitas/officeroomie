import { DrawerToggleButton } from '@react-navigation/drawer';
import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';

export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen name="index" options={{
				headerShown: true,
				title: 'Lista de usuários',
				headerStyle: {
					backgroundColor: colors.primary,
				},
				headerTintColor: colors.secondary,
				headerLeft: ({ tintColor }) => (<DrawerToggleButton tintColor={tintColor} />),
			}}
			/>
			<Stack.Screen name="usuario" options={{
				title: 'Formulário usuário',
				headerStyle: {
					backgroundColor: colors.primary,
				},
				headerTintColor: colors.secondary,
			}}
			/>
		</Stack>
	);
}