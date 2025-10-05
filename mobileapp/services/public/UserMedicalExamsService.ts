import { HttpClient } from '@/@types/IHttpClient';

export class UserMedicalExamsService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async getAllMedicalExamsAsync(params: any) {
		return this.client.get('/public/exames', { params });
	}

	async getMedicalExamsByIdAsync(id: string, params?: any) {
		return this.client.get(`/public/exames/${id}`, { params });
	}

	async getAllMedicosAsync(params: any) {
		return this.client.get('/public/medicos', { params });
	}

	async postMedicalExamsAsync(payload: any) {
		return this.client.post('/api/v1/exames/', payload);
	}

	async putUpdateMedicalExamsAsync(id: string, payload: any) {
		return this.client.put(`/api/v1/exames/${id}`, payload);
	}

	async getAllSpecializationsAsync() {
		return this.client.get('/public/especializacoes');
	}

	async getDoctorByIdAsync(id: string | number) {
		return this.client.get(`/public/medicos/${id}`);
	}
};