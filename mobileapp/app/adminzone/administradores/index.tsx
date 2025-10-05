import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useCallback, useEffect, useState } from 'react';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { ActivityIndicator, FlatList, RefreshControl, Text } from 'react-native';
import CardCrud from '@/components/ui/CardCrud';
import { AdminService } from '@/services/restrict/AdminService';
import { useActionSheet } from '@expo/react-native-action-sheet';
import Toast from 'react-native-root-toast';
import { colors } from '@/utils/constants';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new AdminService(client);

export default function AdminListScreen() {
	const router = useRouter();
	const [list, setList] = useState<any[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | null>(null);
	const { showActionSheetWithOptions } = useActionSheet();

	function transformCollection(data: { email: string }[]) {
		data = data.filter((item) => (!['admin@email.com'].includes(item.email)));
		return data;
	}

	const fetchAddresses = useCallback(async () => {
		try {
			setLoading(true);
			const { data: response } = await service.getAllFromAdminAsync();
			setList(transformCollection(response.data));
			setError(null);
		} catch (err) {
			console.error('Erro ao buscar dados:', err);
			setList([]);
			setError('Erro ao carregar dados.');
		} finally {
			setLoading(false);
		}

		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, []);

	const onRefresh = useCallback(() => {
		fetchAddresses();
	}, [fetchAddresses]);

	useEffect(() => {
		fetchAddresses();
	}, [fetchAddresses]);

	const deleteItem = useCallback(async (item: any) => {
		try {
			await service.deleteFromAdminAsync(item.id);
			Toast.show('Operação realizada com sucesso!', {
				duration: Toast.durations.SHORT,
				position: Toast.positions.BOTTOM,
				animation: true,
			});
			fetchAddresses();
		} catch (err) {
			console.error('Erro ao deletar item:', err);
			setError('Erro ao deletar item.');
		}
	}, [fetchAddresses]);

	const onPressDelete = useCallback((item: any) => {
		const options = ['Deletar', 'Cancelar'];
		const destructiveButtonIndex = 0;
		const cancelButtonIndex = 1;

		showActionSheetWithOptions({
			options,
			cancelButtonIndex,
			destructiveButtonIndex,
		}, (selected) => {
			if (selected === destructiveButtonIndex) {
				deleteItem(item);
			}
		});
	}, [deleteItem, showActionSheetWithOptions]);

	function handleEdit(item: any) {

		router.push({
			pathname: '/adminzone/administradores/administrador',
			params: { ...item, editable: true },
		});
	}

	return (
		<ThemedView className="relative flex-1 w-full p-5">
			<ThemedView className="flex flex-col flex-1 w-full gap-4">
				{loading && <ActivityIndicator size="large" color="#007BFF" />}

				{error && <ThemedText className="text-center">{error}</ThemedText>}

				<FlatList
					data={list}
					keyExtractor={(item) => item.id.toString()}
					renderItem={({ item }) => (
						<CardCrud item={item} onDelete={onPressDelete} onEdit={handleEdit}>
							<Text className="text-lg font-semibold uppercase text-slate-600">{item.name}</Text>
							<Text className="text-sm font-semibold text-slate-600">{item.email}</Text>
						</CardCrud>
					)}
					contentContainerStyle={{ paddingBottom: 50, width: '100%', gap: 10 }}
					refreshControl={
						<RefreshControl
							refreshing={loading}
							onRefresh={onRefresh}
							colors={['#007BFF']}
							progressBackgroundColor="#FFFFFF"
						/>
					}
				/>
			</ThemedView>

			<Button
				onPress={() => router.push('/adminzone/administradores/administrador')}
				circular
				color="primary"
				className="!w-[50px] !h-[50px] absolute right-0 bottom-0 m-5 shadow"
			>
				<Ionicons size={25} name="add" color={colors.secondary} />
			</Button>
		</ThemedView>
	);
};