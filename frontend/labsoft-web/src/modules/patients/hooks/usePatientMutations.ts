import { useMutation, useQueryClient } from '@tanstack/react-query';
import { patientsApi } from '../services/patientsApi';
import type { PacienteFormData } from '../types/patient.types';

export const usePatientMutations = () => {
  const queryClient = useQueryClient();

  const createPatient = useMutation({
    mutationFn: (data: PacienteFormData) => patientsApi.create(data),
    onSuccess: () => {
      // Refresca la tabla de pacientes automáticamente
      queryClient.invalidateQueries({ queryKey: ['patients'] });
    },
  });

  const updatePatient = useMutation({
    mutationFn: ({ id, data }: { id: string; data: PacienteFormData }) => patientsApi.update(id, data),
    onSuccess: (_, variables) => {
      // Refresca la tabla general y el caché de este paciente en específico
      queryClient.invalidateQueries({ queryKey: ['patients'] });
      queryClient.invalidateQueries({ queryKey: ['patient', variables.id] });
    },
  });

  return {
    createPatient,
    updatePatient,
  };
};