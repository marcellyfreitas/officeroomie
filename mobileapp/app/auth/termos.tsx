import { ScrollView, Linking } from 'react-native';
import { ThemedView } from '@/components/ui/ThemedView';
import { ThemedText } from '@/components/ui/ThemedText';

export default function TermsPrivacyScreen() {
	const handleOpenLink = (url: string) => {
		Linking.openURL(url);
	};

	return (
		<ScrollView className="flex-1 p-6 bg-secondary-500">
			<ThemedView className="mb-8">
				<ThemedText className="text-2xl font-bold ">
					Termos de Uso e Política de Privacidade
				</ThemedText>
				<ThemedText className="mt-2">
					Última atualização: {new Date().toLocaleDateString('pt-BR')}
				</ThemedText>
			</ThemedView>

			{/* Termos de Uso */}
			<ThemedView className="mb-8">
				<ThemedText className="mb-3 text-xl font-semibold">
					1. Termos de Uso do Mediconnect
				</ThemedText>
				<ThemedText className="text-base leading-6">
					Bem-vindo ao Mediconnect! Ao usar nosso aplicativo, você concorda com:
					{'\n\n'}• Agendamentos devem ser feitos apenas para fins médicos legítimos.
					{'\n'}• Cancelamentos devem ocorrer com pelo menos 24h de antecedência.
					{'\n'}• Dados falsos ou de terceiros resultarão em suspensão da conta.
				</ThemedText>
			</ThemedView>

			{/* Política de Privacidade */}
			<ThemedView className="mb-8">
				<ThemedText className="mb-3 text-xl font-semibold">
					2. Política de Privacidade
				</ThemedText>
				<ThemedText className="text-base leading-6 text-gray-700">
					Nós coletamos:
					{'\n\n'}• Informações cadastrais (nome, e-mail, telefone).
					{'\n'}• Histórico de consultas para personalizar seu atendimento.
					{'\n\n'}Seus dados são protegidos conforme a{' '}
					<ThemedText
						className="text-blue-500 underline"
						onPress={() => handleOpenLink('https://mediconnect.com/lgpd')}
					>
						LGPD
					</ThemedText> e nunca serão compartilhados sem consentimento.
				</ThemedText>
			</ThemedView>
		</ScrollView>
	);
}