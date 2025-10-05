import { Ionicons } from '@expo/vector-icons';
import { Text, View } from 'react-native';

const EmptyList = () => {
	return (
		<View className="flex items-center justify-center flex-1 py-10">
			<Ionicons name="search-outline" size={32} color="#6B7280" />
			<Text className="mt-2 text-gray-500">Nenhum dado encontrado</Text>
		</View>
	);
};

export default EmptyList;