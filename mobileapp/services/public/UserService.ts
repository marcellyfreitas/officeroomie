import { HttpClient } from '@/@types/IHttpClient';

export class UserService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async updateFromUserAsync(data: any) {
		return this.client.put('/public/usuario', data);
	}

	async deleteFromUserAsync() {
		return this.client.delete('/public/usuario');
	}
};