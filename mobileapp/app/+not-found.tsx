import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';

const NotFoundScrren = () => {
	return (
		<ThemedView className="items-center justify-center flex-1">
			<ThemedText>Oops! Não conseguimos encontrar a página.</ThemedText>
		</ThemedView>
	);
};

export default NotFoundScrren;