import { apiClient } from '../../../lib/api/apiClient';
import type { PagedResult } from '../../../types/pagination.types';
import type { 
  Order, 
  OrderDetailsExtended, 
  CreateOrderPayload, 
  UpdateOrderPayload, 
  OrderQueryFilter 
} from '../types/order.types';

export const ordersApi = {
  getAll: async (filter: OrderQueryFilter) => {
    const params = Object.fromEntries(
      // Cambiamos [_, v] por [key, v]
      Object.entries(filter).filter(([key, v]) => key !== undefined && v != null && v !== '')
    );
    const response = await apiClient.get<PagedResult<Order>>('/ordenes', { params });
    return response.data;
  },

  getById: async (id: string) => {
    const response = await apiClient.get<OrderDetailsExtended>(`/ordenes/${id}`);
    return response.data;
  },

  create: async (payload: CreateOrderPayload) => {
    const response = await apiClient.post<Order>('/ordenes', payload);
    return response.data;
  },

  update: async (id: string, payload: UpdateOrderPayload) => {
    // Si tu backend no retorna nada (NoContent 204), no tipeamos el response.data
    await apiClient.put(`/ordenes/${id}`, payload);
  },
};