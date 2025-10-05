import { View, Button, Platform, Text } from 'react-native';
import DateTimePicker from '@react-native-community/datetimepicker';
import { useState } from 'react';

export default function DateInput({
	value,
	onChange,
	error,
}: {
	value: string;
	onChange: (dateString: string) => void;
	error?: string;
}) {
	const [showPicker, setShowPicker] = useState(false);
	const [date, setDate] = useState(() => {
		const [d, m, y] = value.split('/');
		return value ? new Date(`${y}-${m}-${d}`) : new Date();
	});

	const handleChange = (_event: any, selectedDate?: Date) => {
		setShowPicker(false);
		if (selectedDate) {
			const day = selectedDate.getDate().toString().padStart(2, '0');
			const month = (selectedDate.getMonth() + 1).toString().padStart(2, '0');
			const year = selectedDate.getFullYear();
			const formatted = `${day}/${month}/${year}`;
			onChange(formatted);
			setDate(selectedDate);
		}
	};

	return (
		<View>
			<Button title={value || 'Escolher data'} onPress={() => setShowPicker(true)} />
			{showPicker && (
				<DateTimePicker
					value={date}
					mode="date"
					display={Platform.OS === 'ios' ? 'spinner' : 'default'}
					onChange={handleChange}
				/>
			)}
			{error && <Text style={{ color: 'red' }}>{error}</Text>}
		</View>
	);
}
