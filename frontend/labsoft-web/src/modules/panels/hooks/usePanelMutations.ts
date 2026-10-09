import { useMutation, useQueryClient } from '@tanstack/react-query';
import { panelsApi } from '../services/panelsApi';
import type { CreateUpdatePanelPayload } from '../types/panel.types';

export const usePanelMutations = () => {
  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: (payload: CreateUpdatePanelPayload) => panelsApi.create(payload),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['panels'] });
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, payload }: { id: string; payload: CreateUpdatePanelPayload }) =>
      panelsApi.update(id, payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ['panels'] });
      queryClient.invalidateQueries({ queryKey: ['panel', variables.id] });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => panelsApi.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['panels'] });
    },
  });

  return {
    createPanel: createMutation,
    updatePanel: updateMutation,
    deletePanel: deleteMutation, 
  };
};