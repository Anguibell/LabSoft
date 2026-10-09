import { useQuery } from '@tanstack/react-query';
import { panelsApi } from '../services/panelsApi';

export const usePanels = () => {
  return useQuery({
    queryKey: ['panels'],
    queryFn: panelsApi.getAll,
  });
};

export const usePanel = (id: string | undefined) => {
  return useQuery({
    queryKey: ['panel', id],
    queryFn: () => panelsApi.getById(id!),
    enabled: !!id,
  });
};