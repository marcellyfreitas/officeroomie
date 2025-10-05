import { HttpClient } from '@/@types/IHttpClient';

export class UserAppointmentsService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async getAllAppointmentAsync(params: any) {
		return this.client.get('/public/agendamentos', { params });
	}

	async getAppointmentByIdAsync(id: string, params?: any) {
		return this.client.get(`/public/agendamentos/${id}`, { params });
	}

	async getAllMedicosAsync(params: any) {
		return this.client.get('/public/agendamentos/medicos', { params });
	}

	async getReservedHoursAsync(date: string) {
		return this.client.get('/public/agendamentos/horarios', { params: { date } });
	}

	async postAppointmentAsync(payload: any) {
		return this.client.post('/public/agendamentos/', payload);
	}

	async putUpdateAppointmentAsync(id: string, payload: any) {
		return this.client.put(`/public/agendamentos/${id}`, payload);
	}

	async putCancelAppointmentAsync(id: string) {
		return this.client.put(`/public/agendamentos/${id}/cancelar`);
	}

	async getAllSpecializationsAsync() {
		return this.client.get('/public/especializacoes');
	}

	async getDoctorByIdAsync(id: string | number) {
		return this.client.get(`/public/medicos/${id}`);
	}

	async getMedicalCenterByIdAsync(id: string | number) {
		return this.client.get(`/public/unidades/${id}`);
	}
};