import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../../../lib/api/apiClient';

export interface Specialty {
  id: string;
  nombre: string;
}

export const useSpecialties = () => {
  return useQuery({
    queryKey: ['specialties'],
    queryFn: async () => {
      const response = await apiClient.get<Specialty[]>('/especialidades');
      return response.data;
    },
  });
};