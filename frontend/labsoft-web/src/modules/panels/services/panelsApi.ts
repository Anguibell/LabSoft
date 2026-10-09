import { apiClient } from '../../../lib/api/apiClient';
import type { Panel, CreateUpdatePanelPayload } from '../types/panel.types';

export const panelsApi = {
  getAll: async () => {
    const response = await apiClient.get<Panel[]>('/paneles');
    return response.data;
  },

  getById: async (id: string) => {
    const response = await apiClient.get<Panel>(`/paneles/${id}`);
    return response.data;
  },

  create: async (payload: CreateUpdatePanelPayload) => {
    const response = await apiClient.post('/paneles', payload);
    return response.data;
  },

  update: async (id: string, payload: CreateUpdatePanelPayload) => {
    const response = await apiClient.put(`/paneles/${id}`, payload);
    return response.data;
  },

  delete: async (id: string) => {
    const response = await apiClient.delete(`/paneles/${id}`);
    return response.data;
  },
};