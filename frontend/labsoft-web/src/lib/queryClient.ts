import { QueryClient } from '@tanstack/react-query';

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false, // Evita llamadas extra a la API al cambiar de ventana
      retry: 1, // Si falla la red, reintenta automáticamente 1 vez
      staleTime: 1000 * 60 * 5, // Mantén los datos "frescos" en caché por 5 minutos
    },
  },
});