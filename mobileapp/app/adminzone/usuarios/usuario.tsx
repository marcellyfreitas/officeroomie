import { useState } from 'react';
import { View, Text, KeyboardAvoidingView, Platform, ScrollView } from 'react-native';
import { useLocalSearchParams, useRouter } from 'expo-router';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import TextInput from '@/components/ui/TextInput';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import Ionicons from '@expo/vector-icons/Ionicons';
import { isCPF } from '@/utils/helpers';
import { HttpClient } from '@/services/restrict/HttpClient';
import { UserService } from '@/services/restrict/UserService';
import { z } from 'zod';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import Toast from 'react-native-root-toast';

const formSchema = z.object({
	name: z.string().min(1, 'Campo obrigatório'),
	email: z.string().min(1, 'Email é obrigatório').email('Email inválido'),
	cpf: z.string()
		.min(11, 'O CPF deve ter 11 caracteres')
		.max(14, 'O CPF deve ter no máximo 14 caracteres')
		.refine(isCPF, { message: 'CPF inválido' }),
});
type InnerFormData = z.infer<typeof formSchema>;

const cpfMask = [/\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '-', /\d/, /\d/];

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserService(client);

export default function UsuarioCreateScreen() {
	const params = useLocalSearchParams();
	const { ...routeParams } = params;

	const { control, handleSubmit, formState: { errors } } = useForm<InnerFormData>({
		resolver: zodResolver(formSchema),
		defaultValues: {
			name: (routeParams.name ?? '') as string,
			email: (routeParams.email ?? '') as string,
			cpf: (routeParams.cpf ?? '') as string,
		},
	});

	const router = useRouter();
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState('');

	const onSubmit = async (data: InnerFormData) => {
		try {
			setLoading(true);

			if (!routeParams.editable) {
				await service.addFromAdminAsync({ ...data, password: 'Senh@123' });
			}

			if (routeParams.editable) {
				await service.updateFromAdminAsync(routeParams.id as string, { ...data, password: 'Senh@123' });
			}

			Toast.show('Operação realizada com sucesso!', {
				duration: Toast.durations.SHORT,
				position: Toast.positions.BOTTOM,
				animation: true,
			});
			router.dismissAll();
			router.replace('/adminzone/usuarios');
		} catch (error: any) {
			console.error('Erro de Authenticação', error);
			if (error?.response?.status === 401) {
				setError('Usuário ou senha inválidos. Tente novamente!');
			}
			setError('Erro! Não conseguimos conectar com o servidor.');
		} finally {
			setLoading(false);
		}
	};

	return (
		<KeyboardAvoidingView
			behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
			keyboardVerticalOffset={Platform.OS === 'ios' ? 64 : 0}
			style={{ flex: 1 }}
		>
			<ScrollView keyboardShouldPersistTaps="handled" contentContainerStyle={{ flexGrow: 1 }}>
				<ThemedView className="items-center justify-center flex-1 p-8 bg-gray-500">
					<View className="flex flex-col w-full gap-5">
						{!!error && (
							<View className="flex flex-row items-center w-full gap-2 p-2 bg-red-200 rounded">
								<Ionicons name="information-circle-outline" size={20} className="text-red-500" />
								<Text className="text-red-500">{error}</Text>
							</View>
						)}

						<View className="flex flex-col w-full gap-4">
							<Controller
								control={control}
								name="name"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										autoCapitalize="words"
										returnKeyType="next"
										placeholder="name"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.name}
										errorMessage={errors.name?.message}
									/>
								)}
							/>

							<Controller
								control={control}
								name="email"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										keyboardType="email-address"
										returnKeyType="next"
										placeholder="Email"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.email}
										errorMessage={errors.email?.message}
									/>
								)}
							/>

							<Controller
								control={control}
								name="cpf"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="next"
										placeholder="CPF"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.cpf}
										errorMessage={errors.cpf?.message}
										mask={cpfMask}
									/>
								)}
							/>

							<Button
								onPress={handleSubmit(onSubmit)}
								color="primary"
								className="w-full"
								disabled={loading}
							>
								<Text className="font-bold text-slate-50">
									{routeParams.editable ? 'Atualizar' : 'Cadastrar'}
								</Text>
							</Button>
						</View>
					</View>
				</ThemedView>
			</ScrollView>
		</KeyboardAvoidingView>

	);
}