import { HttpClient } from '@/@types/IHttpClient';

export class AdminAppointmentsService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async getAllMedicosAsync(params: any) {
		return this.client.get('/agendamentos/medicos', { params });
	}

	async getAllAppointmentAsync(params: any) {
		return this.client.get('/agendamentos', { params });
	}

	async getAppointmentByIdAsync(id: string, params?: any) {
		return this.client.get(`/agendamentos/${id}`, { params });
	}

	async getReservedHoursAsync(params: any) {
		return this.client.get('/agendamentos/horarios', { params });
	}

	async putUpdateAppointmentAsync(id: string, payload: any) {
		return this.client.put(`/agendamentos/${id}`, payload);
	}

	async putCancelAppointmentAsync(id: string) {
		return this.client.put(`/agendamentos/${id}/cancelar`);
	}

	async getUserByIdAsync(id: string | number) {
		return this.client.get(`/usuarios/${id}`);
	}

	async getMedicalCenterByIdAsync(id: string | number) {
		return this.client.get(`/unidades/${id}`);
	}

	async postAppointmentAsync(payload: any) {
		return this.client.post('/agendamentos', payload);
	}

	async getDoctorByIdAsync(id: string | number) {
		return this.client.get(`/medicos/${id}`);
	}

	async getAllUsuariosAsync(params: any) {
		return this.client.get('/usuarios/pesquisar', { params });
	}

	async getAllSpecializationsAsync() {
		return this.client.get('/especializacoes');
	}
};