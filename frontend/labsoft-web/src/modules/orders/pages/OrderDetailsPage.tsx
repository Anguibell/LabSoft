// import { useParams, useNavigate } from 'react-router-dom';
// import { ArrowLeft, Printer, FlaskConical, XCircle } from 'lucide-react';
// import { useOrder } from '../hooks/useOrder';
// import PageHeader from '../../../components/common/PageHeader';
// import OrderHeader from '../components/OrderHeader';
// import type { OrderDetailsExtended } from '../types/order.types';

// export default function OrderDetailsPage() {
//   const { id } = useParams<{ id: string }>();
//   const navigate = useNavigate();
  
//   // Usamos el hook que ya tienes definido
//   const { data: orderResponse, isLoading, isError } = useOrder(id);

//   // Usamos aserción estructurada para complacer a ESLint y TypeScript
//   const typedResponse = orderResponse as { data?: OrderDetailsExtended };
//   const order = (typedResponse?.data || orderResponse) as OrderDetailsExtended;

//   if (isLoading) {
//     return <div className="p-8 text-center text-[#64768b]">Cargando detalles de la orden...</div>;
//   }

//   if (isError || !order) {
//     return (
//       <div className="p-8 text-center text-red-500 bg-red-50 border border-red-200 rounded-lg">
//         Error al cargar la orden. Es posible que no exista o haya un problema de conexión.
//       </div>
//     );
//   }

//   const handlePrint = () => {
//     // Aquí conectaremos luego la lógica de PDF
//     alert("Función de impresión en desarrollo (Fase 10)");
//   };

//   const handleCancelOrder = () => {
//     // Aquí conectaremos la mutación para cancelar
//     if(confirm("¿Estás seguro de que deseas cancelar esta orden?")) {
//         alert("Función de cancelación pendiente de conectar con API");
//     }
//   };

//   return (
//     <div className="space-y-6 animate-in fade-in duration-300 max-w-5xl mx-auto">
      
//       {/* 1. NAVEGACIÓN Y ACCIONES GLOBALES */}
//       <div className="flex items-center justify-between mb-2">
//         <button 
//           onClick={() => navigate('/ordenes')}
//           className="flex items-center gap-2 text-sm font-medium text-[#64768b] hover:text-[#2a79b7] transition-colors"
//         >
//           <ArrowLeft className="w-4 h-4" />
//           Volver a Órdenes
//         </button>
//       </div>

//       <PageHeader 
//         title="Detalles de la Orden" 
//         action={
//           <div className="flex gap-3">
//             {order.estado?.toLowerCase() !== 'cancelada' && (
//               <button
//                 onClick={handleCancelOrder}
//                 className="flex items-center gap-2 px-4 py-2 bg-white text-red-600 border border-red-200 rounded-lg hover:bg-red-50 transition-colors font-medium text-sm"
//               >
//                 <XCircle className="w-4 h-4" />
//                 Cancelar Orden
//               </button>
//             )}
//             <button
//               onClick={handlePrint}
//               className="flex items-center gap-2 px-4 py-2 bg-white text-[#4e6681] border border-[#dce5ef] rounded-lg hover:bg-[#f3f7fb] transition-colors font-medium shadow-sm text-sm"
//             >
//               <Printer className="w-4 h-4" />
//               Imprimir Recibo
//             </button>
//           </div>
//         }
//       />

//       {/* 2. TARJETA DE RESUMEN (El componente que hicimos en el Paso 1) */}
//       <OrderHeader order={order} />

//       {/* 3. LISTA DE EXÁMENES SOLICITADOS */}
//       <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden">
//         <div className="px-6 py-4 border-b border-[#dce5ef] bg-[#f8fafc]">
//           <h3 className="text-sm font-bold text-[#2a79b7] uppercase flex items-center gap-2">
//             <FlaskConical className="w-4 h-4" />
//             Exámenes Solicitados
//           </h3>
//         </div>
        
//         {/* Usamos detallesPlanos si existe (OrderDetailsExtended), si no, caemos a detalles normales */}
//         <ul className="divide-y divide-[#dce5ef]">
//           {(order.detallesPlanos || order.detalles || []).map((detalle) => (
//             <li key={detalle.id} className="p-4 sm:px-6 hover:bg-[#f3f7fb] transition-colors flex items-center justify-between">
//               <div className="flex items-center gap-3">
//                 <div className="w-2 h-2 rounded-full bg-[#2a79b7]"></div>
//                 <div>
//                   <p className="text-sm font-bold text-[#15243b]">{detalle.itemNombre}</p>
//                   <p className="text-xs font-medium text-[#64768b] uppercase mt-0.5">{detalle.tipo}</p>
//                 </div>
//               </div>
//               <div>
//                 <span className={`text-xs font-bold px-2 py-1 rounded-md ${
//                   detalle.estado?.toLowerCase() === 'completado' ? 'bg-green-100 text-green-700' : 'bg-yellow-100 text-yellow-700'
//                 }`}>
//                   {detalle.estado || 'PENDIENTE'}
//                 </span>
//               </div>
//             </li>
//           ))}
//         </ul>
        
//         {(order.detallesPlanos || order.detalles || []).length === 0 && (
//           <div className="p-8 text-center text-[#64768b] text-sm">
//             No hay exámenes registrados para esta orden.
//           </div>
//         )}
//       </div>

//     </div>
//   );
// }


























import { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, Printer, FlaskConical, XCircle } from 'lucide-react';
import { useOrder } from '../hooks/useOrder';
import { useOrderMutations } from '../hooks/useOrderMutations';
import PageHeader from '../../../components/common/PageHeader';
import OrderHeader from '../components/OrderHeader';
import { ConfirmDialog } from '../../../components/ui/ConfirmDialog';
import type { OrderDetailsExtended } from '../types/order.types';

export default function OrderDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  
  const { data: orderResponse, isLoading, isError } = useOrder(id);
  const { updateOrder } = useOrderMutations();

  // Estado para controlar nuestro nuevo Modal Profesional
  const [isCancelModalOpen, setIsCancelModalOpen] = useState(false);

  // Parseo seguro (Solución al error de ESLint 'any')
  const typedResponse = orderResponse as { data?: OrderDetailsExtended };
  const orderData = typedResponse?.data || orderResponse;
  const order = orderData as OrderDetailsExtended;

  if (isLoading) {
    return <div className="p-8 text-center text-[#64768b]">Cargando detalles de la orden...</div>;
  }

  if (isError || !order) {
    return (
      <div className="p-8 text-center text-red-500 bg-red-50 border border-red-200 rounded-lg">
        Error al cargar la orden. Es posible que no exista o haya un problema de conexión.
      </div>
    );
  }

  const handlePrint = () => {
    alert("Función de impresión en desarrollo (Fase 10)");
  };

  // Esta función se ejecuta solo cuando el usuario confirma en el modal bonito
  const confirmCancelOrder = () => {
    updateOrder.mutate(
      { 
        id: id!, 
        payload: {
          estado: 'Cancelada',
          examenIds: [], // Enviamos vacío temporalmente o requerimos que C# maneje cancelaciones con PATCH
          panelesSolicitados: []
        }
      },
      {
        onSuccess: () => {
          setIsCancelModalOpen(false);
          // Opcional: mostrar un SuccessMessage tipo toast
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
          Volver a Órdenes
        </button>
      </div>

      <PageHeader 
        title="Detalles de la Orden" 
        action={
          <div className="flex gap-3">
            {order.estado?.toLowerCase() !== 'cancelada' && (
              <button
                onClick={() => setIsCancelModalOpen(true)} // Abrimos el modal en lugar del alert nativo
                className="flex items-center gap-2 px-4 py-2 bg-white text-red-600 border border-red-200 rounded-lg hover:bg-red-50 transition-colors font-medium text-sm"
              >
                <XCircle className="w-4 h-4" />
                Cancelar Orden
              </button>
            )}
            <button
              onClick={handlePrint}
              className="flex items-center gap-2 px-4 py-2 bg-white text-[#4e6681] border border-[#dce5ef] rounded-lg hover:bg-[#f3f7fb] transition-colors font-medium shadow-sm text-sm"
            >
              <Printer className="w-4 h-4" />
              Imprimir Recibo
            </button>
          </div>
        }
      />

      <OrderHeader order={order} />

      <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden">
        <div className="px-6 py-4 border-b border-[#dce5ef] bg-[#f8fafc]">
          <h3 className="text-sm font-bold text-[#2a79b7] uppercase flex items-center gap-2">
            <FlaskConical className="w-4 h-4" />
            Exámenes Solicitados
          </h3>
        </div>
        
        <ul className="divide-y divide-[#dce5ef]">
          {(order.detallesPlanos || order.detalles || []).map((detalle) => (
            <li key={detalle.id} className="p-4 sm:px-6 hover:bg-[#f3f7fb] transition-colors flex items-center justify-between">
              <div className="flex items-center gap-3">
                <div className="w-2 h-2 rounded-full bg-[#2a79b7]"></div>
                <div>
                  <p className="text-sm font-bold text-[#15243b]">{detalle.itemNombre}</p>
                  <p className="text-xs font-medium text-[#64768b] uppercase mt-0.5">{detalle.tipo}</p>
                </div>
              </div>
              <div>
                <span className={`text-xs font-bold px-2 py-1 rounded-md ${
                  detalle.estado?.toLowerCase() === 'completado' ? 'bg-green-100 text-green-700' : 'bg-yellow-100 text-yellow-700'
                }`}>
                  {detalle.estado || 'PENDIENTE'}
                </span>
              </div>
            </li>
          ))}
        </ul>
      </div>

      {/* MODAL DE CONFIRMACIÓN PROFESIONAL */}
      <ConfirmDialog
        isOpen={isCancelModalOpen}
        title="Cancelar Orden Médica"
        message={`¿Estás seguro de que deseas cancelar esta orden? Esta acción cambiará el estado a "Cancelada" y no podrás procesar resultados.`}
        confirmText={updateOrder.isPending ? "Cancelando..." : "Sí, Cancelar Orden"}
        cancelText="Mantener Orden"
        onConfirm={confirmCancelOrder}
        onCancel={() => setIsCancelModalOpen(false)}
        isLoading={updateOrder.isPending}
      />
    </div>
  );
}