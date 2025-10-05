import { HttpClient } from '@/@types/IHttpClient';

export class AdminMedicalExamsService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async getAllMedicalExamsAsync(params: any) {
		return this.client.get('/exames', { params });
	}

	async getMedicalExamsByIdAsync(id: string, params?: any) {
		return this.client.get(`/exames/${id}`, { params });
	}

	async postMedicalExamsAsync(payload: any) {
		return this.client.post('/exames', payload);
	}

	async putUpdateMedicalExamsAsync(id: string | Number, payload: any) {
		return this.client.put(`/exames/${id}`, payload);
	}

	async putDeleteMedicalExamsAsync(id: string) {
		return this.client.delete(`/exames/${id}`);
	}

	async getAllUsuariosAsync(params: any) {
		return this.client.get('/usuarios/pesquisar', { params });
	}

	async getUserByIdAsync(id: string | number) {
		return this.client.get(`/usuarios/${id}`);
	}

	async getAllMedicosAsync(params: any) {
		return this.client.get('/agendamentos/medicos', { params });
	}

	async getDoctorByIdAsync(id: string | number) {
		return this.client.get(`/medicos/${id}`);
	}

	async getAllSpecializationsAsync() {
		return this.client.get('/especializacoes');
	}
};