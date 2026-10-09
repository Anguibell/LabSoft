import { apiClient } from '../../../lib/api/apiClient';
import type { Exam, ExamGroup, CreateExamPayload, UpdateExamPayload } from '../types/exam.types';

export const catalogApi = {
  // GET: /api/examenes (Devuelve agrupado por especialidad)
  getAll: async (): Promise<ExamGroup[]> => {
    const response = await apiClient.get<ExamGroup[]>('/examenes');
    return response.data;
  },

  // GET: /api/examenes/{id}
  getById: async (id: string): Promise<Exam> => {
    const response = await apiClient.get<Exam>(`/examenes/${id}`);
    return response.data;
  },

  // POST: /api/examenes
  create: async (payload: CreateExamPayload): Promise<Exam> => {
    const response = await apiClient.post<Exam>('/examenes', payload);
    return response.data;
  },

  // PUT: /api/examenes/{id}
  update: async (id: string, payload: UpdateExamPayload): Promise<void> => {
    await apiClient.put(`/examenes/${id}`, payload);
  },
};