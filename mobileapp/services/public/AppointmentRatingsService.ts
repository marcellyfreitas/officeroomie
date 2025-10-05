import { HttpClient } from '@/@types/IHttpClient';

export class AppointmentRatingsService {
  private client: HttpClient;

  constructor(client: HttpClient) {
    this.client = client;
  }

  async postEvaluationAsync(payload: {
    rating: number;
    comment: string;
    appointmentId: number;
    userId: number;
  }) {
    return this.client.post('/agendamento-avaliacoes', payload);
  }

  async getAllEvaluationsAsync(params?: any) {
    return this.client.get('/agendamento-avaliacoes', { params });
  }

  async getEvaluationByIdAsync(id: number | string) {
    return this.client.get(`/agendamento-avaliacoes/${id}`);
  }

  async putEvaluationAsync(
    id: number | string,
    payload: { rating: number; comment: string }
  ) {
    return this.client.put(`/agendamento-avaliacoes/${id}`, payload);
  }

  async deleteEvaluationAsync(id: number | string) {
    return this.client.delete(`/agendamento-avaliacoes/${id}`);
  }

  async getAppointmentDetailsAsync(id: number | string) {
    return this.client.get(`/public/agendamentos/${id}`);
  }

}
