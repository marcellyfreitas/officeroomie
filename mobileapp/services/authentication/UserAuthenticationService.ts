import type { HttpClient, IAuthenticationService, IUser } from '@/@types';


export class UserAuthenticationService implements IAuthenticationService<IUser> {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async login(payload: any): Promise<{ token: string }> {
		try {
			const response = await this.client.post('/auth/user/login', {
				email: payload.username,
				password: payload.password,
			});


			const token = response.data.data.token ?? '';

			if (!token) {
				throw 'Não autorizado!';
			}

			return { token };
		} catch (error) {
			console.error(error);
			throw error;
		}
	}

	async userData() {
		try {
			const { data } = await this.client.get('/auth/user');

			if (!data?.data?.email) {
				throw 'Erro ao capturar dados de usuário';
			}

			const user: IUser = {
				id: data?.data?.id,
				email: data?.data?.email,
				name: data?.data?.name,
				cpf: data?.data?.cpf,
			};

			return user;
		} catch (error) {
			throw error;
		}

	}

	async register(data: any) {
		try {
			const response = await this.client.post('/auth/user/register', data);

			const token = response.data.data.token ?? '';

			if (!token) {
				throw 'Não autorizado!';
			}

			return { token };
		} catch (error) {
			console.error(error);
			throw '[Service]: Erro ao cadastrar usuário';
		}
	}

	async logout() {
		try {
			return this.client.post('/auth/user/logout');
		} catch (error) {
			console.error(error);
			throw '[Service]: Erro ao deslogar usuário';
		}
	}
};