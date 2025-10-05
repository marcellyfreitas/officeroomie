export interface IAuthenticationService<T> {
	login: (data: any) => Promise<{ token: string }>,
	userData: () => Promise<T>,
	register: (data: { name: string, cpf: string, email: string, password: string }) => Promise<any>,
	logout: () => Promise<any>,
}