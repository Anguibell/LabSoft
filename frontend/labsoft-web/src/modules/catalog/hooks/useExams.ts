import { useQuery } from '@tanstack/react-query';
import { catalogApi } from '../services/catalogApi';

// Hook para obtener todo el catálogo agrupado
export const useExams = () => {
  return useQuery({
    queryKey: ['exams'],
    queryFn: catalogApi.getAll,
  });
};

// Hook para obtener el detalle de un solo examen
export const useExam = (id: string | undefined) => {
  return useQuery({
    queryKey: ['exam', id],
    queryFn: () => catalogApi.getById(id!),
    enabled: !!id, // Solo se ejecuta si el id existe (ej. al editar)
  });
};