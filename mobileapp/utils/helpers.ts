export function isEmpty(v: any): boolean {
	if (v === undefined) return true;
	if (v === null) return true;
	if (v === '') return true;
	if (v === Object(v) && !Object.entries(v).length) return true;
	return Array.isArray(v) && !v.length;
}

export function isEmail(email: string): boolean {
	if (isEmpty(email)) return false;
	const expression =
		// eslint-disable-next-line no-control-regex
		/(?!.*\.{2})^([a-z\d!#$%&'*+\-/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([\t]*\r\n)?[\t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([\t]*\r\n)?[\t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
	return expression.test(String(email).toLowerCase());
}

export function onlyNumbers(string: string): string {
	return String(string).replace(/\D+/g, '');
}

export function isCPF(cpf: any): boolean {
	cpf = onlyNumbers(cpf);

	if (cpf.length !== 11 || !!cpf.match(/(\d)\1{10}/)) return false;

	cpf = cpf.split('').map((el: any) => +el);
	const rest = (count: number) =>
		((cpf.slice(0, count - 12)
			.reduce((soma: any, el: any, index: any) => soma + el * (count - index), 0) * 10) % 11) % 10;
	return rest(10) === cpf[9] && rest(11) === cpf[10];
}

export function isCNPJ(cnpj: string): boolean {
	cnpj = onlyNumbers(cnpj);

	if (cnpj.length !== 14 || !!cnpj.match(/(\d)\1{13}/)) return false;

	let tamanho = cnpj.length - 2,
		numeros = cnpj.slice(0, tamanho),
		soma = 0,
		pos = tamanho - 7,
		// eslint-disable-next-line prefer-const
		digitos = cnpj.slice(tamanho);

	for (let i = tamanho; i >= 1; i--) {
		soma += Number(numeros.charAt(tamanho - i)) * pos--;
		if (pos < 2) pos = 9;
	}

	let resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);

	if (resultado !== Number(digitos.charAt(0))) return false;

	tamanho = tamanho + 1;
	numeros = cnpj.slice(0, tamanho);
	soma = 0;
	pos = tamanho - 7;

	for (let i = tamanho; i >= 1; i--) {
		soma += Number(numeros.charAt(tamanho - i)) * pos--;
		if (pos < 2) pos = 9;
	}

	resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);

	if (resultado !== Number(digitos.charAt(1))) return false;

	return true;
}

export function formatCPF(cpf: string): string {
	cpf = onlyNumbers(cpf);
	if (cpf.length < 11) return cpf;
	cpf = cpf.slice(0, 11);
	cpf = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
	return cpf;
}

export function formatCNPJ(cnpj: string): string {
	cnpj = onlyNumbers(cnpj);
	if (cnpj.length < 14) return cnpj;
	cnpj = cnpj.slice(0, 14);
	cnpj = cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
	return cnpj;
}

export function formatPhoneNumber(fn: string): string {
	let match: any = '';
	fn = onlyNumbers(fn);
	match = fn.length === 8 ? fn.match(/^(\d{4})(\d{4})$/) : match;
	match = fn.length === 9 ? fn.match(/^(\d{5})(\d{4})$/) : match;
	match = fn.length === 10 ? fn.match(/^(\d{2})(\d{4})(\d{4})$/) : match;
	match = fn.length === 11 ? fn.match(/^(\d{2})(\d{5})(\d{4})$/) : match;
	if (match.length > 0) return `(${match[1]}) ${match[2]}-${match[3]}`;
	return match;
}

export function isPhoneNumber(phoneNumber: string): boolean {
	if (isEmpty(phoneNumber)) return false;
	phoneNumber = formatPhoneNumber(phoneNumber);
	// eslint-disable-next-line no-useless-escape
	const expression = /(\(?\d{2}\)?\s)?(\d{4,5}\-\d{4})/g;
	return expression.test(String(phoneNumber).toLowerCase());
}

export function randNumber(min: number, max: number) {
	return Math.floor(Math.random() * (max - min + 1)) + min;
}

export function groupBy(items: [], key: string): [] {
	if (isEmpty(items)) return [];
	if (isEmpty(key)) return [];

	let res: any = [];
	const reduce = items.reduce(
		(result, item) => ({
			...result,
			[item[key]]: [...(result[item[key]] || []), item],
		}),
		{},
	);

	Object.entries(reduce).map(([, v]) => {
		res = [...res, v];
	});

	return res;
}