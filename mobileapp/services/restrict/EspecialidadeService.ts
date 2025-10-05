import { HttpClient } from '@/@types/IHttpClient';

export class EspecialidadeService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async addFromAdminAsync(data: any): Promise<any> {
		return this.client.post('/especializacoes', data);
	}

	async getAllFromAdminAsync(): Promise<any> {
		return this.client.get('/especializacoes');
	}

	async getByIdFromAdminAsync(id: number): Promise<any> {
		return this.client.get(`/especializacoes/${id}`);
	}

	async updateFromAdminAsync(id: number, data: any) {
		return this.client.put(`/especializacoes/${id}`, data);
	}

	async deleteFromAdminAsync(id: number) {
		return this.client.delete(`/especializacoes/${id}`);
	}
}