export const validateCpf = (cpf: string) => {
	const cleaned = cpf.replace(/[^\d]/g, '');
	if (cleaned.length !== 11) return false;
	// Algoritmo de validação de dígitos verificadores
	let sum = 0;
	let rest;
	for (let i = 1; i <= 9; i++) sum += parseInt(cleaned.substring(i - 1, i)) * (11 - i);
	rest = (sum * 10) % 11;
	if (rest === 10 || rest === 11) rest = 0;
	if (rest !== parseInt(cleaned.substring(9, 10))) return false;
	sum = 0;
	for (let i = 1; i <= 10; i++) sum += parseInt(cleaned.substring(i - 1, i)) * (12 - i);
	rest = (sum * 10) % 11;
	if (rest === 10 || rest === 11) rest = 0;
	if (rest !== parseInt(cleaned.substring(10, 11))) return false;
	return true;
};