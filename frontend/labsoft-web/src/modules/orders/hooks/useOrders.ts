import { useQuery } from '@tanstack/react-query';
import { ordersApi } from '../services/ordersApi';
import type { OrderQueryFilter } from '../types/order.types';

export const useOrders = (filter: OrderQueryFilter) => {
  return useQuery({
    queryKey: ['orders', filter],
    queryFn: () => ordersApi.getAll(filter),
  });
};