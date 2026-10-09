import { useQuery } from '@tanstack/react-query';
import { patientsApi } from '../services/patientsApi';

export const usePatient = (id: string | undefined) => {
  return useQuery({
    queryKey: ['patient', id],
    queryFn: () => patientsApi.getById(id!),
    enabled: !!id, 
  });
};