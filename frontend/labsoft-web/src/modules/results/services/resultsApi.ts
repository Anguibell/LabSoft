import { apiClient } from '../../../lib/api/apiClient';
import type { 
  OrdenPendienteResultado, 
  RegistrarResultadosMasivosPayload 
} from '../types/result.types';

export const resultsApi = {
  // Obtiene la lista de órdenes pendientes por procesar
  getPendientes: async (): Promise<OrdenPendienteResultado[]> => {
    const { data } = await apiClient.get('/resultados/pendientes');
    return data;
  },

  // Obtiene los resultados y la estructura de una orden específica
  getByOrdenId: async (ordenId: string): Promise<OrdenPendienteResultado> => {
    const { data } = await apiClient.get(`/resultados/orden/${ordenId}`);
    return data;
  },

  // Envía todos los valores procesados al backend
  registrarMasivos: async (payload: RegistrarResultadosMasivosPayload): Promise<{ mensaje: string }> => {
    const { data } = await apiClient.post('/resultados/masivos', payload);
    return data;
  },

  // Sella la orden como "Validada"
  validarOrden: async (ordenId: string): Promise<boolean> => {
    const { data } = await apiClient.put(`/resultados/validar-orden/${ordenId}`);
    return data;
  },

  // Invalida algun resultado
  invalidarResultado: async (id: string, motivo: string) => {
    const { data } = await apiClient.post(`/resultados/${id}/invalidar`, { motivo });
    return data;
  },
};