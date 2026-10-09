import { useMutation, useQueryClient } from '@tanstack/react-query';
import { catalogApi } from '../services/catalogApi';
import type { CreateExamPayload, UpdateExamPayload } from '../types/exam.types';

export const useExamMutations = () => {
  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: (payload: CreateExamPayload) => catalogApi.create(payload),
    onSuccess: () => {
      // Refresca la lista general al crear uno nuevo
      queryClient.invalidateQueries({ queryKey: ['exams'] });
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, payload }: { id: string; payload: UpdateExamPayload }) =>
      catalogApi.update(id, payload),
    onSuccess: (_, variables) => {
      // Refresca tanto la lista general como el caché individual de este examen
      queryClient.invalidateQueries({ queryKey: ['exams'] });
      queryClient.invalidateQueries({ queryKey: ['exam', variables.id] });
    },
  });

  return {
    createExam: createMutation,
    updateExam: updateMutation,
  };
};