import { Text, TextProps } from 'react-native';
import clsx from 'clsx';

interface Props extends TextProps {
	className?: string;
}

export const ThemedText = ({ children, className, ...rest }: Props) => {
	return (
		<Text
			{...rest}
			className={clsx('text-black', className)}
		>
			{children}
		</Text>
	);
};