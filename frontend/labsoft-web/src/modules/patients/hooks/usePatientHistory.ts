import { useQuery } from '@tanstack/react-query';
import { patientsApi } from '../services/patientsApi';

export const usePatientHistory = (id: string | undefined) => {
  return useQuery({
    queryKey: ['patientHistory', id],
    queryFn: () => patientsApi.getHistory(id!),
    enabled: !!id, 
  });
};