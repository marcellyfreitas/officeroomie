import React, { useRef, useEffect } from 'react';
import { View, StyleSheet, Animated } from 'react-native';
import { colors } from '@/utils/constants';

const LoadingLine = () => {
	const progressAnim = useRef(new Animated.Value(0)).current;

	useEffect(() => {
		const animate = () => {
			Animated.loop(
				Animated.sequence([
					Animated.timing(progressAnim, {
						toValue: 1,
						duration: 1300,
						useNativeDriver: false,
					}),
					Animated.timing(progressAnim, {
						toValue: 0,
						duration: 0,
						useNativeDriver: false,
					}),
				]),
			).start();
		};

		animate();
	}, [progressAnim]);

	const progressWidth = progressAnim.interpolate({
		inputRange: [0, 1],
		outputRange: ['0%', '100%'],
	});

	return (
		<View style={styles.container}>
			<View style={styles.line}>
				<Animated.View
					style={[
						styles.progress,
						{
							width: progressWidth,
						},
					]}
				/>
			</View>
		</View>
	);
};

const styles = StyleSheet.create({
	container: {
		width: '100%',
		paddingHorizontal: 20,
	},
	line: {
		height: 4,
		backgroundColor: '#e0e0e0',
		borderRadius: 2,
		overflow: 'hidden',
	},
	progress: {
		height: '100%',
		backgroundColor: colors.green,
		borderRadius: 2,
	},
});

export default LoadingLine;