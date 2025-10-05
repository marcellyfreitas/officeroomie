import { View, ViewProps } from 'react-native';
import clsx from 'clsx';

interface Props extends ViewProps {
	className?: string;
}

export const ThemedView = ({ children, className, ...rest }: Props) => {
	return (
		<View
			{...rest}
			className={clsx('bg-white', className)}
		>
			{children}
		</View>
	);
};