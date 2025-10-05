import React from 'react';
import { View, Text, ViewStyle, TextStyle } from 'react-native';
import clsx from 'clsx';
import Button from './Button'; // ou o caminho relativo correto

interface AlertProps {
	title?: string;
	message?: string;
	variant?: 'primary' | 'info' | 'success' | 'warning' | 'danger' | 'default';
	onClose?: () => void;
	style?: ViewStyle;
	textStyle?: TextStyle;
	className?: string;
}

const Alert = ({
	title,
	message,
	variant = 'default',
	onClose,
	style,
	textStyle,
	className,
}: AlertProps) => {
	const baseColor = {
		primary: 'bg-primary-100 border-primary-500 text-primary-900',
		info: 'bg-blue-100 border-blue-500 text-blue-900',
		success: 'bg-green-100 border-green-500 text-green-900',
		warning: 'bg-yellow-100 border-yellow-500 text-yellow-900',
		danger: 'bg-red-100 border-red-500 text-red-900',
		default: 'bg-gray-100 border-gray-300 text-gray-900 dark:bg-gray-800 dark:text-white',
	}[variant];

	return (
		<View
			className={clsx(
				'border rounded-md p-4 flex-row justify-between items-start space-x-2',
				baseColor,
				className,
			)}
			style={style}
		>
			<View className="flex-1">
				{title && <Text className={clsx('font-bold mb-1', textStyle)}>{title}</Text>}
				{message && <Text className={clsx('text-xs', textStyle)}>{message}</Text>}
			</View>

			{onClose && (
				<Button
					title="X"
					onPress={onClose}
					color={variant}
					circular
					className="items-center justify-center w-6 h-6 p-0"
					textStyle={{ fontSize: 12, fontWeight: 'bold' }}
				/>
			)}
		</View>
	);
};

export default Alert;
