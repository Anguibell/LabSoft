import { useQuery } from '@tanstack/react-query';
import { resultsApi } from '../services/resultsApi';

export const useResultadosPendientes = () => {
  return useQuery({
    queryKey: ['resultados', 'pendientes'],
    queryFn: resultsApi.getPendientes,
  });
};