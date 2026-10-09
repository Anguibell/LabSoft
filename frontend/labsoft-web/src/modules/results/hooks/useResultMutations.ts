import { useMutation, useQueryClient } from '@tanstack/react-query';
import { resultsApi } from '../services/resultsApi';
import type { RegistrarResultadosMasivosPayload } from '../types/result.types';

export const useResultMutations = () => {
  const queryClient = useQueryClient();

  const registrarMasivos = useMutation({
    mutationFn: (payload: RegistrarResultadosMasivosPayload) => resultsApi.registrarMasivos(payload),
    onSuccess: (_, variables) => {
      // Refrescamos las listas de pendientes y la orden específica
      queryClient.invalidateQueries({ queryKey: ['resultados', 'pendientes'] });
      queryClient.invalidateQueries({ queryKey: ['resultados', 'orden', variables.ordenId] });
      queryClient.invalidateQueries({ queryKey: ['orders'] }); // Refresca también el módulo de órdenes
    },
  });

  const validarOrden = useMutation({
    mutationFn: (ordenId: string) => resultsApi.validarOrden(ordenId),
    onSuccess: (_, ordenId) => {
      queryClient.invalidateQueries({ queryKey: ['resultados', 'pendientes'] });
      queryClient.invalidateQueries({ queryKey: ['resultados', 'orden', ordenId] });
      queryClient.invalidateQueries({ queryKey: ['orders'] });
    },
  });

  const invalidarResultado = useMutation({
    mutationFn: ({ id, motivo }: { id: string; motivo: string }) => 
      resultsApi.invalidarResultado(id, motivo),
    onSuccess: () => {
      // Recarga la orden para que el estado vuelva a "Procesado" visualmente
      queryClient.invalidateQueries({ queryKey: ['resultadosOrden'] }); 
    }
  });

  return {
    registrarMasivos,
    validarOrden,
    invalidarResultado
  };
};