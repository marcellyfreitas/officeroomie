import { HttpClient } from '@/@types/IHttpClient';

const baseApi = process.env.EXPO_PUBLIC_GITHUBURL ?? '';
const token = process.env.EXPO_PUBLIC_GITHUBTOKEN ?? '';
const headers = { 'X-Github-Token': token, 'Content-Type': 'application/json' };

export class UserAddressService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async search(cep: string): Promise<any> {
		const API_URL = process.env.EXPO_PUBLIC_APICEP_URL ?? '';
		const REPLACED = API_URL?.replace('{cep}', cep);
		return this.client.get(REPLACED);
	}

	async addFromUserAsync(data: any): Promise<any> {
		return this.client.post(baseApi, data, { headers });
	}

	async getAllFromUserAsync(): Promise<any> {
		return this.client.get(baseApi, { headers });
	}

	async getByIdFromUserAsync(id: number): Promise<any> {
		return this.client.get(baseApi, { headers });
	}

	async updateFromUserAsync(id: number, data: any) {
		return this.client.put(baseApi, data, { headers });
	}

	async deleteFromUserAsync(id: number) {
		return this.client.delete(baseApi, { headers });
	}
};