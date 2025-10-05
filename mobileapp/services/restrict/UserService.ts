import { HttpClient } from '@/@types/IHttpClient';

const BASE_URL = '/private/usuarios';

export class UserService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async addFromAdminAsync(data: any): Promise<any> {
		return this.client.post(BASE_URL, data);
	}

	async getAllFromAdminAsync(): Promise<any> {
		return this.client.get(BASE_URL);
	}

	async getByIdFromAdminAsync(id: string): Promise<any> {
		return this.client.get(`${BASE_URL}/${id}`);
	}

	async updateFromAdminAsync(id: string, data: any) {
		return this.client.put(`${BASE_URL}/${id}`, data);
	}

	async deleteFromAdminAsync(id: string) {
		return this.client.delete(`${BASE_URL}/${id}`);
	}
};