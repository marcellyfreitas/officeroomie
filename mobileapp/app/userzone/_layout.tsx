import { Tabs } from 'expo-router';
import { AuthProvider } from '@/contexts/UserAuthenticationContext';
import Ionicons from '@expo/vector-icons/Ionicons';
import { HapticTab } from '@/components/ui/HapticTab';
import { colors } from '@/utils/constants';

export default function TabLayout() {
	return (
		<AuthProvider>
			<Tabs
				screenOptions={{
					headerShown: false,
					tabBarButton: HapticTab,
					tabBarStyle: {
						backgroundColor: colors.primary,
					},
					tabBarActiveTintColor: colors.secondary,
					tabBarInactiveTintColor: colors.gray,
				}}
			>
				<Tabs.Screen
					name="home"
					options={{
						title: 'Home',
						tabBarIcon: ({ color, size, focused }) => (
							<Ionicons
								name={focused ? 'home' : 'home-outline'}
								size={size}
								color={color}
							/>
						),
					}}
				/>

				<Tabs.Screen
					name="index"
					options={{
						title: 'Home',
						tabBarIcon: ({ color, size, focused }) => (
							<Ionicons
								name={focused ? 'home' : 'home-outline'}
								size={size}
								color={color}
							/>
						),
						href: null,
					}}
				/>

				<Tabs.Screen
					name="agendamentos"
					options={{
						title: 'Agendamentos',
						tabBarIcon: ({ color, size, focused }) => (
							<Ionicons
								name={focused ? 'calendar' : 'calendar-clear-outline'}
								size={size}
								color={color}
							/>
						),
					}}
				/>

				<Tabs.Screen
					name="perfil"
					options={{
						title: 'Perfil',
						tabBarIcon: ({ color, size, focused }) => (
							<Ionicons
								name={focused ? 'person' : 'person-outline'}
								size={size}
								color={color}
							/>
						),
					}}
				/>
			</Tabs>
		</AuthProvider>
	);
}
