import { StyleSheet } from 'react-native';
import { colors } from '@/utils/constants';

export const datepickerStyles = StyleSheet.create({
	container: {
		flex: 1,
		padding: 24,
	},
	calendar: {
		backgroundColor: 'transparent',
	},
	selected: {
		color: '#fff',
		fontSize: 16,
		marginTop: 42,
	},
	dayText: {
		color: colors.gray,
	},
	disabled: {
		color: '#717171',
	},
	today: {
		color: colors.gray,
		fontWeight: 'bold',
	},
	daySelected: {
		backgroundColor: '#22c55e',
	},
	day: {
		width: 30,
		height: 30,
		alignItems: 'center',
		justifyContent: 'center',
		borderRadius: 7,
	},

	disabledDay: {
		opacity: 0.5,
	},
});