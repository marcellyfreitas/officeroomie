import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import { useAdminAuth } from '@/hooks/useAdminAuth';
// import { Image } from 'react-native';

const AdminIndexScreen = () => {
	const { userData } = useAdminAuth();
	return (
		<ThemedView className="items-center justify-center flex-1">
			<ThemedText className="text-xl font-bold">Seja bem vindo, </ThemedText>
			<ThemedText className="text-xl font-bold">{userData?.name}</ThemedText>
			<ThemedText className="text-xl font-bold">ao</ThemedText>

			<ThemedText>
				[LOGO IMAGE]
			</ThemedText>
			{/* <Image
				source={require('@/assets/images/logo_white.png')}
				style={{
					width: '50%',
					height: 200,
					resizeMode: 'contain',
				}}
			/> */}
		</ThemedView>
	);
};

export default AdminIndexScreen;