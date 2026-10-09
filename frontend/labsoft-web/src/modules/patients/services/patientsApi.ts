import { apiClient } from '../../../lib/api/apiClient';
import type { Paciente, PacienteQueryFilter, PagedResultDto, PacienteFormData, HistorialPaciente } from '../types/patient.types';

export const patientsApi = {
  // GET: /api/pacientes
  getAll: async (filter: PacienteQueryFilter): Promise<PagedResultDto<Paciente>> => {
    const response = await apiClient.get<PagedResultDto<Paciente>>('/pacientes', { params: filter });
    return response.data;
  },

  // GET: /api/pacientes/{id}
  getById: async (id: string): Promise<Paciente> => {
    const response = await apiClient.get<Paciente>(`/pacientes/${id}`);
    return response.data;
  },

  // POST: /api/pacientes
  create: async (data: PacienteFormData): Promise<Paciente> => {
    const response = await apiClient.post<Paciente>('/pacientes', data);
    return response.data;
  },

  // PUT: /api/pacientes/{id}
  update: async (id: string, data: PacienteFormData): Promise<void> => {
    // El backend espera el objeto completo con su ID para validar
    const response = await apiClient.put(`/pacientes/${id}`, { id, ...data });
    return response.data;
  },

  // GET: /api/pacientes/{id}/historial
  getHistory: async (id: string): Promise<HistorialPaciente[]> => {
    const response = await apiClient.get<HistorialPaciente[]>(`/pacientes/${id}/historial`);
    return response.data;
  }
  
};