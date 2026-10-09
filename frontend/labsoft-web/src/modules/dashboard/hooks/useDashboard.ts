import { useQuery } from '@tanstack/react-query';
import { dashboardApi } from '../services/dashboardApi';

export const useDashboardStats = () => {
  return useQuery({
    queryKey: ['dashboard', 'stats'],
    queryFn: dashboardApi.getStats,
    refetchInterval: 60000, // Refresca automáticamente cada 1 minuto (ideal para pantallas principales)
  });
};