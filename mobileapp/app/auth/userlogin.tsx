import { useUserAuth } from '@/hooks/useUserAuth';
import { View, Text, TouchableOpacity, KeyboardAvoidingView, Platform, ScrollView } from 'react-native';
import { Href, router } from 'expo-router';
import { ThemedView } from '@/components/ui/ThemedView';
import { ThemedText } from '@/components/ui/ThemedText';
import Button from '@/components/ui/Button';
import TextInput from '@/components/ui/TextInput';
import PasswordInput from '@/components/ui/PasswordInput';
import AuthHeader from '@/components/modules/auth/AuthHeader';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import Toast from 'react-native-root-toast';
import { z } from 'zod';
import { colors } from '@/utils/constants';

const formSchema = z.object({
	email: z.string().min(1, 'Email é obrigatório').email('Email inválido'),
	password: z.string().min(3, 'A senha deve ter pelo menos 6 caracteres'),
});

type InnerFormData = z.infer<typeof formSchema>;

export default function LoginScreen() {
	const { control, handleSubmit, formState: { errors } } = useForm<InnerFormData>({
		resolver: zodResolver(formSchema),
		defaultValues: {
			email: 'user@email.com',
			password: 'senha',
			// email: '',
			// password: '',
		},
	});

	const [loading, setLoading] = useState(false);
	const [error, setError] = useState('');

	const { login } = useUserAuth();

	const onSubmit = async (data: InnerFormData) => {


		try {
			setLoading(true);

			const { token } = await login(data.email, data.password);

			if (token) {
				Toast.show('Autenticado com sucesso!', {
					duration: Toast.durations.SHORT,
					position: Toast.positions.BOTTOM,
					animation: true,
				});

				router.replace('/userzone' as Href);
			}
		} catch (error: any) {
			console.error('Erro de Authenticação', error);
			if (error?.response?.status === 401) {
				setError('Usuário ou senha inválidos. Tente novamente!');
				return;
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
			<ScrollView
				keyboardShouldPersistTaps="handled"
				contentContainerStyle={{ flexGrow: 1 }}
			>
				<ThemedView className="relative items-center justify-center flex-1 p-8 bg-gray">
					<View className="flex flex-col w-full gap-5">
						<AuthHeader
							icon="lock-closed-outline"
							title="Authenticação"
							description="Insira os dados solicitados abaixo para autenticar no aplicativo."
						/>

						{!!error && (
							<View className="flex flex-row items-center w-full gap-2 p-2 bg-red-200 rounded">
								<Ionicons name="information-circle-outline" size={20} className="text-red-500" />
								<Text className="text-red-500">{error}</Text>
							</View>
						)}

						<View className="flex flex-col w-full gap-4">
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
								name="password"
								render={({ field: { onChange, onBlur, value } }) => (
									<PasswordInput
										placeholder="Senha"
										returnKeyType="done"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.password}
										errorMessage={errors.password?.message}
									/>
								)}
							/>

							<Button
								onPress={handleSubmit(onSubmit)}
								color="primary"
								className="w-full"
								disabled={loading}
							>
								<Text className="font-bold text-slate-50">Entrar</Text>
							</Button>



							<View className="flex flex-row items-center justify-center w-full gap-2 text-center">
								<ThemedText className="leading-10 align-middle">Não possui uma conta?</ThemedText>
								<TouchableOpacity
									className=""
									onPress={() => router.push('/auth/register')}
								>

									<Text className="font-semibold text-primary-500">Cadastre-se</Text>
								</TouchableOpacity>
							</View>
						</View>
					</View>
					<Button
						circular
						onPress={() => router.replace('/auth/adminlogin')}
						color="primary"
						disabled={loading}
						className="!w-[40px] !h-[40px] absolute bottom-0 right-0 m-5"
					>
						<Ionicons size={22} name="eye-off-outline" color={colors.secondary} />
					</Button>
				</ThemedView>
			</ScrollView>
		</KeyboardAvoidingView>

	);
}