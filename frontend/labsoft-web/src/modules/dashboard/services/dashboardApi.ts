import { apiClient } from '../../../lib/api/apiClient';
import type { DashboardStats } from '../types/dashboard.types';

export const dashboardApi = {
  getStats: async (): Promise<DashboardStats> => {
    const { data } = await apiClient.get('/dashboard/resumen');
    return data;
  },
};