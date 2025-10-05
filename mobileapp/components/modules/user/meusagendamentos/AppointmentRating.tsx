import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import { useState } from 'react';
import Button from '@/components/ui/Button';
import { Text } from 'react-native';
import { MaterialCommunityIcons } from '@expo/vector-icons';

const AppointmentRating = () => {
	const [hasRating, setHasRating] = useState<boolean>(false);

	if (!hasRating) {
		return (
			<Button color="primary" className="flex flex-row items-center flex-1 w-full gap-2" onPress={() => { }}>
				<MaterialCommunityIcons name="text-box-search-outline" className="text-slate-50" size={16} />
				<Text className="font-bold text-slate-50">Avaliar</Text>
			</Button>
		);
	}

	return (
		<ThemedView className="">
			<ThemedText>AppointmentRating</ThemedText>
		</ThemedView>
	);
};

export default AppointmentRating;