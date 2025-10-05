import { DrawerItemList } from '@react-navigation/drawer';
import { Drawer } from 'expo-router/drawer';
import { ScrollView, View } from 'react-native';
import { GestureHandlerRootView } from 'react-native-gesture-handler';
import { SafeAreaView } from 'react-native-safe-area-context';
import AdminDrawerContent from '@/components/ui/AdminDrawerContent';
import { AuthProvider } from '@/contexts/AdminAuthenticationContext';
import AdminLogoutButton from '@/components/ui/AdminLogoutButton';
import { Fontisto, Ionicons, MaterialCommunityIcons } from '@expo/vector-icons';
import { colors } from '@/utils/constants';

export default function Layout() {
	return (
		<AuthProvider>
			<GestureHandlerRootView style={{ flex: 1 }}>
				<Drawer
					drawerContent={(props) => (
						<View className="flex-1">
							<SafeAreaView className="flex-1 gap-4 !p-5">
								<AdminDrawerContent />

								<View className="flex-1">
									<ScrollView className="flex-1">
										<DrawerItemList {...props} />
									</ScrollView>
								</View>

								<AdminLogoutButton />
							</SafeAreaView>
						</View>
					)}
					screenOptions={{
						drawerStyle: {
							backgroundColor: colors.gray,
						},
						drawerActiveBackgroundColor: colors.primary,
						drawerActiveTintColor: colors.secondary,
						drawerInactiveTintColor: colors.primary,
					}}
				>
					<Drawer.Screen
						name="index"
						options={{
							title: 'Início',
							drawerLabel: 'Início',
							headerStyle: {
								backgroundColor: colors.primary,
							},
							headerTintColor: colors.secondary,
							drawerIcon: ({ color, size }) => (<Ionicons name="home-outline" size={size} color={color} />),
						}}
					/>
					<Drawer.Screen
						name="administradores"
						options={{
							headerShown: false,
							drawerLabel: 'Administradores',
							drawerIcon: ({ color, size }) => (<Ionicons name="people-outline" size={size} color={color} />),
						}}
					/>
					<Drawer.Screen
						name="usuarios"
						options={{
							headerShown: false,
							drawerLabel: 'Usuários',
							drawerIcon: ({ color, size }) => (<Ionicons name="person-outline" size={size} color={color} />),
						}}
					/>
					<Drawer.Screen
						name="agendamentos"
						options={{
							headerShown: false,
							drawerLabel: 'Agendamentos',
							drawerIcon: ({ color, size }) => (<Ionicons name="calendar-outline" size={size} color={color} />),
						}}
					/>
					<Drawer.Screen
						name="medicos"
						options={{
							headerShown: false,
							drawerLabel: 'Médicos',
							drawerIcon: ({ color, size }) => (<Ionicons name="medical-outline" size={size} color={color} />),
						}}
					/>
					<Drawer.Screen
						name="exames"
						options={{
							headerShown: false,
							drawerLabel: 'Exames',
							drawerIcon: ({ color, size }) => (<Fontisto name="laboratory" size={size} color={color} />),
						}}
					/>
					<Drawer.Screen
						name="convenios"
						options={{
							headerShown: false,
							drawerLabel: 'Convênios',
							drawerIcon: ({ color, size }) => (<MaterialCommunityIcons name="handshake-outline" size={size} color={color} />),
						}}
					/>
					<Drawer.Screen
						name="unidades"
						options={{
							headerShown: false,
							drawerLabel: 'Unidades médicas',
							drawerIcon: ({ color, size }) => (<Ionicons name="medkit-outline" size={size} color={color} />),
						}}
					/>

					<Drawer.Screen
						name="especializacao"
						options={{
							headerShown: false,
							drawerLabel: 'Especializações',
							drawerIcon: ({ color, size }) => (<MaterialCommunityIcons name="card-account-details-star-outline" size={size} color={color} />),
						}}
					/>
				</Drawer>
			</GestureHandlerRootView>
		</AuthProvider>
	);
}