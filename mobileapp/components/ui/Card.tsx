import { TouchableOpacity, type ViewProps } from 'react-native';
import { colors } from '@/utils/constants';

interface Props extends ViewProps {
	onPress?: () => void
}

const Card = (props: Props) => {
	return (
		<TouchableOpacity
			className="flex-1 rounded bg-gray"
			style={{ backgroundColor: colors.gray }}
			onPress={props.onPress}
		>
			{props.children}
		</TouchableOpacity>
	);
};

export default Card;