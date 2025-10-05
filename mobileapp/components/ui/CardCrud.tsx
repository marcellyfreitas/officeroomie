import { Text, TouchableOpacity, View, ViewProps } from 'react-native';
import Ionicons from '@expo/vector-icons/Ionicons';
import { colors } from '@/utils/constants';

interface Props extends ViewProps {
	item: any
	onDelete: (item: any) => void,
	onEdit: (item: any) => void,
	disabledEdit?: boolean,
	disabledDelete?: boolean,
}

const CardCrud = ({ item, onDelete, onEdit, children, disabledEdit = false, disabledDelete = false }: Props) => {
	return (
		<View className="flex w-full p-0 rounded-lg" style={{ backgroundColor: colors.cardBg, borderColor: colors.cardBorder }}>
			<View className="flex flex-1 gap-2 p-4">
				{children}
			</View>

			<View className="flex flex-row border-t border-slate-200">
				<TouchableOpacity
					disabled={disabledEdit}
					className="flex flex-row w-full flex-1 gap-2 h-[40px] justify-center items-center border-r border-slate-200"
					onPress={() => onEdit(item)}
				>
					<Ionicons name="pencil-outline" className="!text-slate-600" size={14} />
					<Text className="font-semibold text-slate-600">Editar</Text>
				</TouchableOpacity>

				<TouchableOpacity
					disabled={disabledDelete}
					className="flex flex-row w-full flex-1 gap-2 h-[40px] justify-center items-center"
					onPress={() => onDelete(item)}
				>
					<Ionicons name="trash-outline" className="!text-red-500" size={14} />
					<Text className="font-semibold text-red-500">Remover</Text>
				</TouchableOpacity>
			</View>
		</View>
	);
};

export default CardCrud;