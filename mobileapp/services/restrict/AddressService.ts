import { HttpClient } from '@/@types/IHttpClient';

export class AddressService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async search(cep: string): Promise<any> {
		const API_URL = process.env.EXPO_PUBLIC_APICEP_URL ?? '';
		const REPLACED = API_URL?.replace('{cep}', cep);
		return this.client.get(REPLACED);
	}

	async estados(): Promise<any> {
		const API_URL = process.env.EXPO_PUBLIC_IBGE_URL ?? '';
		return this.client.get(`${API_URL}/localidades/estados`);
	}

	async addFromAdminAsync(data: any): Promise<any> {
		return this.client.post('/enderecos', data);
	}

	async getAllFromAdminAsync(): Promise<any> {
		return this.client.get('/enderecos');
	}

	async getByIdFromAdminAsync(id: number): Promise<any> {
		return this.client.get(`/enderecos/${id}`);
	}

	async updateFromAdminAsync(id: number, data: any) {
		return this.client.put(`/enderecos/${id}`, data);
	}

	async deleteFromAdminAsync(id: number) {
		return this.client.delete(`/enderecos/${id}`);
	}
};