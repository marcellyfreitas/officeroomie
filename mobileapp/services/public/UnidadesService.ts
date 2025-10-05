import { HttpClient } from '@/@types/IHttpClient';

export class UnidadesService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async getAllAsync(): Promise<any> {
		return this.client.get('/public/unidades');
	}
}
