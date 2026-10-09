import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft } from 'lucide-react';
import PageHeader from '../../../components/common/PageHeader';
import OrderForm from '../components/OrderForm';
import { useOrder } from '../hooks/useOrder';
import { useOrderMutations } from '../hooks/useOrderMutations';
import type { OrderFormData } from '../schemas/orderSchema';
import type { OrderDetailsExtended, PanelSolicitudPayload } from '../types/order.types';

// Definimos lo que esperamos recibir del backend para editar sin usar 'any'
type EditOrderData = OrderDetailsExtended & {
  pacienteId?: string;
  examenIds?: string[];
  panelesSolicitados?: PanelSolicitudPayload[];
};

export default function EditOrderPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  
  const { data: orderResponse, isLoading, isError } = useOrder(id);
  const { updateOrder } = useOrderMutations();

  // Parseo seguro indicándole exactamente qué propiedades puede tener
  const typedResponse = orderResponse as { data?: EditOrderData };
  const orderData = (typedResponse?.data || orderResponse) as EditOrderData;

  if (isLoading) {
    return <div className="p-8 text-center text-[#64768b]">Cargando orden para editar...</div>;
  }

  if (isError || !orderData) {
    return (
      <div className="p-8 text-center text-red-500 bg-red-50 border border-red-200 rounded-lg">
        Error al cargar la orden. No se puede editar.
      </div>
    );
  }

  // MAPEO DE DATOS: 
  // NOTA PARA EL DESARROLLADOR: 'orderData' debe contener pacienteId, examenIds y panelesSolicitados.
  // Si tu GET de C# no los devuelve, estos campos quedarán en blanco en el formulario.
  const initialFormValues: Partial<OrderFormData> = {
    pacienteId: orderData.pacienteId || '', 
    examenIds: orderData.examenIds || [], 
    panelesSolicitados: orderData.panelesSolicitados || [], 
  };

  const handleSubmit = (data: OrderFormData) => {
    // Al actualizar, inyectamos el estado actual para no perderlo, 
    // y mapeamos el payload a lo que espera C# (UpdateOrderPayload)
    updateOrder.mutate(
      { 
        id: id!, 
        payload: {
          estado: orderData.estado || 'Pendiente',
          examenIds: data.examenIds,
          panelesSolicitados: data.panelesSolicitados
        }
      },
      {
        onSuccess: () => {
          navigate('/ordenes'); // Volvemos a la tabla al terminar
        },
        onError: (error: unknown) => {
          const typedError = error as { response?: { data?: { error?: string, mensaje?: string } }, message?: string };
          const errorMessage = typedError.response?.data?.error || typedError.response?.data?.mensaje || typedError.message || "Error desconocido";
          alert(`Error en el servidor: ${errorMessage}`);
          console.error("Error backend:", error);
        }
      }
    );
  };

  return (
    <div className="space-y-6 animate-in fade-in duration-300 max-w-5xl mx-auto">
      
      <div className="flex items-center justify-between mb-2">
        <button 
          onClick={() => navigate('/ordenes')}
          className="flex items-center gap-2 text-sm font-medium text-[#64768b] hover:text-[#2a79b7] transition-colors"
        >
          <ArrowLeft className="w-4 h-4" />
          Cancelar y volver
        </button>
      </div>

      <PageHeader 
        title={`Editar Orden #${orderData.numeroOrden ? orderData.numeroOrden.toString().padStart(4, '0') : '0000'}`} 
        description="Modifica los exámenes solicitados para este paciente."
      />

      {/* Reutilizamos el OrderForm pasándole los initialValues */}
      <div className="bg-white rounded-xl shadow-sm border border-[#dce5ef] overflow-hidden">
        <div className="p-6">
          <OrderForm 
            initialValues={initialFormValues}
            onSubmit={handleSubmit}
            onCancel={() => navigate('/ordenes')}
            isLoading={updateOrder.isPending}
            isEditing={true}
            pacienteNombre={orderData.pacienteNombre}
          />
        </div>
      </div>

    </div>
  );
}