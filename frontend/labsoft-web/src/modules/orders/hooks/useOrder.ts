import { useQuery } from '@tanstack/react-query';
import { ordersApi } from '../services/ordersApi';

export const useOrder = (id: string | undefined) => {
  return useQuery({
    queryKey: ['order', id],
    queryFn: () => ordersApi.getById(id!),
    enabled: !!id,
  });
};