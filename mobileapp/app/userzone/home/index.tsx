import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import { Href, router, useLocalSearchParams, useNavigation } from 'expo-router';
import { useEffect } from 'react';
import { useUserAuth } from '@/hooks/useUserAuth';
import { FlatList, Text, View } from 'react-native';
import Card from '@/components/ui/Card';
import { Fontisto, Ionicons, MaterialCommunityIcons } from '@expo/vector-icons';
import { isEmpty } from '@/utils/helpers';

export default function HomeIndex() {
	const params = useLocalSearchParams();
	const navigation = useNavigation();
	const { userData } = useUserAuth();

	useEffect(() => {
		if (params.status === 'registered') {
			router.dismissAll();
			router.replace({
				pathname: '/userzone/agendamentos',
				params,
			});
		}
	}, [params.status, params]);

	useEffect(() => {
		navigation.setOptions({
			headerTitle: `Olá, ${userData?.name}`,
		});
	}, [userData?.name, navigation]);

	const data = [
		{ id: '1', rota: '/userzone/agendamentos', title: 'Agendamentos', icon: <Ionicons name="calendar-outline" size={22} /> },
		{ id: '2', rota: '/userzone/home/unidades', title: 'Unidades', icon: <Ionicons name="map-outline" size={22} /> },
		{ id: '3', rota: '/userzone/home/convenios', title: 'Convênios', icon: <MaterialCommunityIcons name="handshake-outline" size={22} /> },
		{ id: '4', rota: '/userzone/home/exames', title: 'Exames', icon: <Fontisto name="laboratory" size={22} /> },
	];

	return (
		<ThemedView className="flex-1 gap-5 p-5">
			<ThemedText className="text-xl font-bold">O que você precisa hoje?</ThemedText>

			<FlatList
				data={data}
				keyExtractor={(item) => item.id}
				numColumns={2}
				columnWrapperStyle={{ gap: 15 }}
				renderItem={({ item }) => (
					<Card onPress={() => {
						if (!isEmpty(item.rota)) {
							router.push(item.rota as Href);
						}
					}}
					>
						<View className="gap-5 p-5">
							{item.icon}
							<Text className="font-semibold">{item.title}</Text>
						</View>
					</Card>
				)}
				contentContainerStyle={{
					gap: 15,
				}}
			/>
		</ThemedView>
	);
};