
import { HttpClient } from '@/@types/IHttpClient';

export class MedicalAgreementService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async getAllAsync(params: any) {
		return this.client.get('/public/convenios', { params });
	}

};