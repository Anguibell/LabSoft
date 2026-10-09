import { useQuery } from '@tanstack/react-query';
import { patientsApi } from '../services/patientsApi';
import type { PacienteQueryFilter } from '../types/patient.types';

export const usePatients = (filter: PacienteQueryFilter) => {
  return useQuery({
    queryKey: ['patients', filter],
    queryFn: () => patientsApi.getAll(filter),
  });
};