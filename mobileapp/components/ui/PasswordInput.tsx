import React, { useState } from 'react';
import { TextInput as RNTextInput, TextInputProps, View, TouchableOpacity, Text } from 'react-native';
import clsx from 'clsx';
import Ionicons from '@expo/vector-icons/Ionicons';

interface PasswordInputProps extends TextInputProps {
	placeholder?: string;
	value?: string;
	onChangeText?: (text: string) => void;
	className?: string;
	error?: boolean;
	errorMessage?: string;
	disabled?: boolean;
}

const PasswordInput = ({
	placeholder,
	value,
	onChangeText,
	className,
	error = false,
	errorMessage,
	style,
	disabled = false,
	...rest
}: PasswordInputProps) => {
	const [showPassword, setShowPassword] = useState(true);

	const togglePasswordVisibility = () => {
		if (!disabled) {
			setShowPassword((prev) => !prev);
		}
	};

	return (
		<View className="w-full">
			<View
				className={clsx(
					'w-full flex-row items-center h-[40px] bg-white border border-gray-300 rounded focus:border-blue-500',
					error && 'border-red-500 dark:border-red-500',
					disabled && 'opacity-50',
					className,
				)}
			>
				<RNTextInput
					autoCorrect={false}
					autoComplete="off"
					placeholder={placeholder}
					value={value}
					onChangeText={disabled ? undefined : onChangeText}
					secureTextEntry={showPassword}
					className={clsx(
						'flex-1 h-full px-4 text-black placeholder-gray-400',
					)}
					style={style}
					editable={!disabled}
					{...rest}
				/>
				<TouchableOpacity onPress={togglePasswordVisibility} className="mr-2" disabled={disabled}>
					<Ionicons
						name={!showPassword ? 'eye-off' : 'eye'}
						size={24}
						color="#6B7280"
					/>
				</TouchableOpacity>
			</View>
			{error && errorMessage && (
				<Text className="mt-1 text-sm text-red-500">{errorMessage}</Text>
			)}
		</View>
	);
};

export default PasswordInput;