import type { AxiosInstance } from 'axios';

export interface IMedic {
  id?: number;
  name: string;
  cpf: string;
  email: string;
  crm: string;
  specializationId: number;
  specialization?: any;
  createdAt?: string;
  updatedAt?: string;
  units: number[];
}

export class MedicService {
  constructor(private readonly client: AxiosInstance) {
    this.client = client;
  }

  async createAsync(data: any) {
    const response = await this.client.post('/medicos', { ...data });
    if (response.data?.data) {
      return response.data.data;
    }
    return response.data; // Fallback to direct response if not nested
  }
  
  async updateAsync(id: number, data: any) {
    try {
      // Garantir que o specializationId é um número válido antes de enviar
      const specializationId = Number(data.specializationId);
      if (isNaN(specializationId)) {
        throw new Error('ID de especialização inválido');
      }
      
      const payload = {
        id,
        name: data.name,
        cpf: data.cpf,
        email: data.email,
        crm: data.crm,
        specializationId: specializationId,
        units: data.units || []
      };

      console.log("Payload antes da atualização:", JSON.stringify(payload, null, 2));

      const response = await this.client.put(`/medicos/${id}`, payload, {
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        }
      });

      // Adicione logs mais detalhados
      console.log("Payload antes da atualização:", JSON.stringify(payload, null, 2));
      console.log("Response completo:", JSON.stringify(response, null, 2));

      // Log detalhado da resposta
      console.log("Status da resposta:", response.status);
      console.log("Headers da resposta:", response.headers);
      console.log("Dados da resposta:", JSON.stringify(response.data, null, 2));

      if (response.status === 200 || response.status === 204) {
        return response.data?.data || response.data;
      }

      throw new Error(`Falha na atualização: ${response.status}`);
    } catch (error: any) {
      console.error("Erro detalhado:", {
        message: error.message,
        response: error.response?.data,
        status: error.response?.status
      });
      throw error;
    }
  }

  // Use uma ferramenta como Postman para fazer a requisição diretamente
  // PUT /medicos/{id}
  // Body: {"name": "...", "specializationId": 123, ...}
  async deleteAsync(id: number) {
    try {
      console.log(`Tentando excluir médico com ID: ${id}`);
      const response = await this.client.delete(`/medicos/${id}`);
      console.log(`Médico excluído com sucesso. Status: ${response.status}`);
      return response.data;
    } catch (error: any) {
      console.error('Erro ao excluir médico:', {
        message: error.message,
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        url: error.config?.url,
        method: error.config?.method
      });
      
      // Throw a more descriptive error
      if (error.response?.status === 400) {
        throw new Error('Não é possível excluir este médico. Verifique se não há agendamentos associados.');
      } else if (error.response?.status === 404) {
        throw new Error('Médico não encontrado.');
      } else if (error.response?.status === 500) {
        throw new Error('Erro interno do servidor. Tente novamente mais tarde.');
      } else if (!error.response) {
        throw new Error('Erro de conexão. Verifique sua conexão com a internet.');
      }
      
      throw error;
    }
  }

  async getByIdAsync(id: number, params?: Record<string, any>) {
    try {
      const response = await this.client.get(`/medicos/${id}`, { params });
      
      // Add detailed debugging to see the exact response structure
      console.log("Full API Response:", JSON.stringify(response));
      console.log("Response data:", JSON.stringify(response.data));
      
      // Check if we have data in the expected location
      if (response.data) {
        // If the data is nested in a data property, use that
        const medicData = response.data.data || response.data;
        console.log("Extracted medic data:", JSON.stringify(medicData));
        
        // Ensure CPF is properly formatted if it exists
        if (medicData.cpf) {
          medicData.cpf = medicData.cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
        }
        
        // Garantir que a especialização esteja presente
        if (medicData.specializationId && !medicData.specialization) {
          // Se temos apenas o ID da especialização mas não o objeto completo,
          // vamos buscar os detalhes da especialização
          try {
            const specializationResponse = await this.client.get(`/especializacoes/${medicData.specializationId}`);
            if (specializationResponse.data?.data) {
              medicData.specialization = specializationResponse.data.data;
            }
          } catch (error) {
            console.error('Error fetching specialization details:', error);
          }
        }
        
        return medicData;
      }
      return null;
    } catch (error) {
      console.error('Error fetching doctor details:', error);
      throw error;
    }
  }

  async getAllAsync(params?: Record<string, any>) {
    const response = await this.client.get('/medicos', { params });
    if (response.data?.data) {
      // Format CPF for each doctor in the list and ensure specialization data
      const medicos = await Promise.all(response.data.data.map(async (medico: IMedic) => {
        const formattedMedico = {
          ...medico,
          cpf: medico.cpf ? medico.cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4") : medico.cpf
        };

        // Fetch specialization data if not included
        if (medico.specializationId && !medico.specialization) {
          try {
            const specializationResponse = await this.client.get(`/especializacoes/${medico.specializationId}`);
            if (specializationResponse.data?.data) {
              formattedMedico.specialization = specializationResponse.data.data;
            }
          } catch (error) {
            console.error('Error fetching specialization:', error);
          }
        }

        return formattedMedico;
      }));

      return medicos;
    }
    return [];
  }
}
