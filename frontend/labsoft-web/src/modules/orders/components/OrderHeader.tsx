import { Calendar, User, Hash } from 'lucide-react';
import type { OrderDetailsExtended } from '../types/order.types';
import OrderStatusBadge from './OrderStatusBadge';

interface OrderHeaderProps {
  order: OrderDetailsExtended;
}

export default function OrderHeader({ order }: OrderHeaderProps) {
  // 1. Protección contra orden vacía
  if (!order) return null;

  // 2. Protección en la fecha (Si viene vacía, no fallará)
  const formattedDate = order.fechaCreacion 
    ? new Date(order.fechaCreacion).toLocaleDateString('es-ES', {
        weekday: 'long', year: 'numeric', month: 'long',
        day: 'numeric', hour: '2-digit', minute: '2-digit'
      })
    : 'Fecha no disponible';

  // 3. Protección en el número de orden
  // Mostrará 0000 si C# no envía el número correcto, para evidenciar que falta mapearlo en el backend
  const numOrdenStr = order.numeroOrden && order.numeroOrden > 0
    ? order.numeroOrden.toString().padStart(4, '0') 
    : '0000';

  return (
    <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm p-6 mb-6">
      <div className="flex flex-col md:flex-row md:items-start justify-between gap-4">
        
        <div className="space-y-4">
          <div className="flex items-center gap-3">
            <div className="p-2 bg-[#eef5fa] text-[#2a79b7] rounded-lg">
              <Hash className="w-5 h-5" />
            </div>
            <div>
              <p className="text-xs font-bold text-[#64768b] uppercase tracking-wider">Número de Orden</p>
              <h2 className="text-xl font-bold text-[#15243b]">
                #ORD-{numOrdenStr}
              </h2>
            </div>
            <div className="ml-4">
               <OrderStatusBadge status={order.estado || 'Pendiente'} />
            </div>
          </div>

          <div className="flex flex-col sm:flex-row gap-6 mt-4">
            <div className="flex items-center gap-2 text-[#4e6681]">
              <User className="w-4 h-4 text-[#94a3b8]" />
              <span className="text-sm font-medium">{order.pacienteNombre || 'Paciente Desconocido'}</span>
            </div>
            <div className="flex items-center gap-2 text-[#4e6681]">
              <Calendar className="w-4 h-4 text-[#94a3b8]" />
              <span className="text-sm font-medium capitalize">{formattedDate}</span>
            </div>
          </div>
        </div>

        <div className="bg-[#f8fafc] p-4 rounded-lg border border-[#dce5ef] min-w-50 text-center">
            <p className="text-xs font-bold text-[#64768b] uppercase mb-1">Total Exámenes</p>
            <p className="text-2xl font-bold text-[#2a79b7]">
              {order.detallesPlanos?.length || order.detalles?.length || 0}
            </p>
        </div>

      </div>
    </div>
  );
}