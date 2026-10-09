import { useMutation, useQueryClient } from '@tanstack/react-query';
import { ordersApi } from '../services/ordersApi';
import type { CreateOrderPayload, UpdateOrderPayload } from '../types/order.types';

export const useOrderMutations = () => {
  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: (payload: CreateOrderPayload) => ordersApi.create(payload),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['orders'] });
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, payload }: { id: string; payload: UpdateOrderPayload }) =>
      ordersApi.update(id, payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['orders'] });
      queryClient.invalidateQueries({ queryKey: ['order', variables.id] });
    },
  });

  return {
    createOrder: createMutation,
    updateOrder: updateMutation,
  };
};