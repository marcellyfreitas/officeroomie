import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import { ScrollView, Text, View } from 'react-native';
import Button from '@/components/ui/Button';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { UserService } from '@/services/public/UserService';
import { useState } from 'react';
import { Ionicons } from '@expo/vector-icons';
import Toast from 'react-native-root-toast';
import { router } from 'expo-router';
import { useUserAuth } from '@/hooks/useUserAuth';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserService(client);

const DeleteAccount = () => {
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState('');
	const { logout } = useUserAuth();

	const onSubmit = async () => {
		try {
			setLoading(true);

			await service.deleteFromUserAsync();

			Toast.show('Operação realizada com sucesso!', {
				duration: Toast.durations.SHORT,
				position: Toast.positions.BOTTOM,
				animation: true,
			});
			await logout();
			router.dismissAll();
			router.replace('/auth/userlogin');

		} catch (error: any) {
			console.error('Erro de Authenticação', error);
			setError(error?.response?.data?.message ?? 'Ops! Não foi possível excluir sua conta');
		} finally {
			setLoading(false);
		}
	};

	return (
		<ThemedView className="flex-1 gap-5 p-5">
			{!!error && (
				<View className="flex flex-row items-center w-full gap-2 p-2 bg-red-200 rounded">
					<Ionicons name="information-circle-outline" size={20} className="text-red-500" />
					<Text className="text-red-500">{error}</Text>
				</View>
			)}
			<ScrollView keyboardShouldPersistTaps="handled" contentContainerStyle={{ flexGrow: 1 }}>
				<ThemedText className="text-xl font-bold text-red-600">
					Tem certeza de que deseja excluir sua conta?
				</ThemedText>
				<ThemedText className="mt-4 text-gray-700">
					A exclusão da sua conta é <ThemedText className="font-bold">permanente e irreversível</ThemedText>.
					Isso significa que todos os seus dados, histórico e informações associadas serão apagados
					<ThemedText className="font-bold"> para sempre</ThemedText>, sem possibilidade de recuperação.
				</ThemedText>
				<ThemedText className="mt-4 text-gray-700">
					Se estiver enfrentando algum problema, podemos ajudar! Nossa equipe está disponível para esclarecer dúvidas
					e encontrar a melhor solução para você.
				</ThemedText>
				<ThemedText className="mt-4 text-gray-700">
					Se ainda assim decidir continuar, saiba que sentiremos sua falta.
				</ThemedText>
				<ThemedText className="mt-4 text-gray-700">
					Deseja realmente prosseguir com a exclusão?
				</ThemedText>

			</ScrollView>

			<Button
				onPress={onSubmit}
				color="danger"
				className="w-full"
				disabled={loading}
			>
				<Text className="text-white">Sim, quero excluir minha conta</Text>
			</Button>

		</ThemedView>
	);
};

export default DeleteAccount;
