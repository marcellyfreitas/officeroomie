import { ThemedText } from '@/components/ui/ThemedText';
import { Text, View } from 'react-native';

interface Props {
	username?: string,
	email?: string,
}

const UserProfileHeader = ({ username = 'USER NAME', email }: Props) => {
	const splitedName = username.split(' ');
	const initialLetters = [splitedName.shift()?.charAt(0), splitedName.pop()?.charAt(0)].join('');

	return (
		<View className="w-full min-h-[80px] flex flex-row gap-4 items-center">
			<View className="w-[50px] h-[50px] rounded-full bg-gray-800 flex justify-center items-center">
				<View className="text-2xl font-semibold text-white">
					<Text className="font-bold text-white">{initialLetters}</Text>
				</View>
			</View>
			<View className="flex flex-col gap-2">
				<ThemedText className="font-bold">Olá,</ThemedText>

				<View className="">
					<ThemedText className="text-lg font-semibold">{username}</ThemedText>
					<ThemedText>{email}</ThemedText>
				</View>
			</View>
		</View>
	);
};

export default UserProfileHeader;