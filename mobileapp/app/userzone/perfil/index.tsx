import { ThemedView } from '@/components/ui/ThemedView';
import UserProfileHeader from '@/components/modules/user/UserProfileHeader';
import { useUserAuth } from '@/hooks/useUserAuth';
import UserLogoutButton from '@/components/ui/UserLogoutButton';
import ScreenOptionButton from '@/components/ui/ScreenOptionButton';
import { View } from 'react-native';
import { router } from 'expo-router';

const UserScreen = () => {
	const { userData } = useUserAuth();

	return (
		<ThemedView className="flex-1 w-full gap-5 p-5">
			<UserProfileHeader username={userData?.name} email={userData?.email} />

			<View className="flex-1">
				<ScreenOptionButton
					title="Meus dados"
					description="Atualize seus dados"
					icon="person-outline"
					onPress={() => router.push('/userzone/perfil/meusdados')}
				/>

				<ScreenOptionButton
					title="Excluir conta"
					description="Solicitar exclusão de conta"
					icon="archive-outline"
					onPress={() => router.push('/userzone/perfil/exclusao')}
				/>

			</View>

			<UserLogoutButton />
		</ThemedView>
	);
};

export default UserScreen;