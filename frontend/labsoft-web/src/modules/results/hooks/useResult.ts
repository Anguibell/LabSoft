import { useQuery } from '@tanstack/react-query';
import { resultsApi } from '../services/resultsApi';

export const useResultadosPorOrden = (ordenId: string | undefined) => {
  return useQuery({
    queryKey: ['resultados', 'orden', ordenId],
    queryFn: () => resultsApi.getByOrdenId(ordenId!),
    enabled: !!ordenId,
  });
};