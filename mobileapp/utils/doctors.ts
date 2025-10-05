// doctors.ts

// Função para normalizar texto, removendo acentos e convertendo para minúsculas
export const normalizeText = (text: string): string => {
	return text
	  .toLowerCase()
	  .normalize('NFD')
	  .replace(/[\u0300-\u036f]/g, '');
  };
  
// Interface que define a estrutura de um médico
export interface Doctor {
	id: string;
	name: string;
	cpf: string;
	specializationId: string;
	crm: string;
	email: string;
}

// Dados fictícios de médicos para testes
export const mockDoctors: Doctor[] = [
	{
		id: '1',
		name: 'Dr. João Silva',
		cpf: '123.456.789-00',
		specializationId: 'Cardiologia',
		crm: '123456',
		email: 'joao.silva@example.com',
	},
	{
		id: '2',
		name: 'Dra. Maria Santos',
		cpf: '987.654.321-00',
		specializationId: 'Pediatria',
		crm: '654321',
		email: 'maria.santos@example.com',
	},
];

// Função para encontrar um médico pelo ID
export const findDoctorById = (id: string): Doctor | undefined => {
	return mockDoctors.find(doctor => doctor.id === id);
};

// Função para filtrar médicos por especialidade
export const filterDoctorsBySpecialty = (specialty: string): Doctor[] => {
	return mockDoctors.filter(doctor => 
		doctor.specializationId.toLowerCase().includes(specialty.toLowerCase())
	);
};

// Função para buscar médicos com base em um termo de pesquisa
export const searchDoctors = (searchTerm: string): Doctor[] => {
	const term = searchTerm.toLowerCase();
	return mockDoctors.filter(doctor => 
		doctor.name.toLowerCase().includes(term) ||
		doctor.specializationId.toLowerCase().includes(term)
	);
};