import { HttpClient } from '@/@types/IHttpClient';

export class MedicalAgreementService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async getAllFromAdminAsync(): Promise<any> {
		return this.client.get('/convenios');
	}

	async getByIdFromAdminAsync(id: number): Promise<any> {
		return this.client.get(`/convenios/${id}`);
	}

	async addFromAdminAsync(data: any): Promise<any> {
		return this.client.post('/convenios', data);
	}

	async updateFromAdminAsync(id: number, data: any) {
		return this.client.put(`/convenios/${id}`, data);
	}

	async deleteFromAdminAsync(id: number) {
		return this.client.delete(`/convenios/${id}`);
	}
};