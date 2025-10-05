import { HttpClient } from '@/@types/IHttpClient';

export class UnitService {
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
		return this.client.post('/unidades', data);
	}

	async getAllFromAdminAsync(): Promise<any> {
		return this.client.get('/unidades');
	}

	async getByIdFromAdminAsync(id: number): Promise<any> {
		return this.client.get(`/unidades/${id}`);
	}

	async updateFromAdminAsync(id: number, data: any) {
		return this.client.put(`/unidades/${id}`, data);
	}

	async deleteFromAdminAsync(id: number) {
		return this.client.delete(`/unidades/${id}`);
	}
};