import { Edit, FileText, FlaskConical } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import type { Order } from '../types/order.types';
import OrderStatusBadge from './OrderStatusBadge';

interface OrderTableProps {
  orders: Order[];
  isLoading: boolean;
}

export default function OrderTable({ orders, isLoading }: OrderTableProps) {
  const navigate = useNavigate();

  if (isLoading) {
    return <div className="p-8 text-center text-[#64768b]">Cargando órdenes...</div>;
  }

  if (!orders || orders.length === 0) {
    return (
      <div className="p-8 text-center text-[#64768b] bg-white border border-[#dce5ef] rounded-lg">
        No se encontraron órdenes con los filtros actuales.
      </div>
    );
  }

  return (
    <div className="overflow-x-auto bg-white rounded-lg border border-[#dce5ef]">
      <table className="w-full text-sm text-left">
        <thead className="text-xs text-[#64768b] uppercase bg-[#f8fafc] border-b border-[#dce5ef]">
          <tr>
            <th className="px-6 py-4 font-bold">Nro. Orden</th>
            <th className="px-6 py-4 font-bold">Paciente</th>
            <th className="px-6 py-4 font-bold">Fecha</th>
            <th className="px-6 py-4 font-bold">Exámenes</th>
            <th className="px-6 py-4 font-bold">Estado</th>
            <th className="px-6 py-4 font-bold text-right">Acciones</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.id} className="border-b border-[#dce5ef] hover:bg-[#f3f7fb] transition-colors">
              <td className="px-6 py-4 font-bold text-[#2a79b7]">
                #ORD-{order.numeroOrden.toString().padStart(6, '0')}
              </td>
              <td className="px-6 py-4 font-medium text-[#15243b]">
                {order.pacienteNombre}
              </td>
              <td className="px-6 py-4 text-[#64768b]">
                {new Date(order.fechaCreacion).toLocaleDateString('es-ES')}
              </td>
              <td className="px-6 py-4 text-[#64768b]">
                <div className="flex items-center gap-1">
                  <FlaskConical className="w-3 h-3" />
                  {order.detalles?.length || 0} items
                </div>
              </td>
              <td className="px-6 py-4">
                <OrderStatusBadge status={order.estado} />
              </td>
              <td className="px-6 py-4 text-right">
                <div className="flex items-center justify-end gap-2">
                  <button
                    onClick={() => navigate(`/resultados/${order.id}`)}
                    className="p-2 text-[#64768b] hover:text-[#2a79b7] hover:bg-[#eef5fa] rounded-md transition-colors"
                    title="Ver Resultados / Validar"
                  >
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M14.5 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V7.5L14.5 2z"/><polyline points="14 2 14 8 20 8"/><path d="m9 15 2 2 4-4"/></svg>
                  </button>
                  <button 
                    onClick={() => navigate(`/ordenes/${order.id}`)}
                    className="p-1.5 text-[#64768b] hover:text-[#2a79b7] hover:bg-[#eef5fa] rounded transition-colors"
                    title="Ver Detalles"
                  >
                    <FileText className="w-4 h-4" />
                  </button>
                  <button 
                    onClick={() => navigate(`/ordenes/${order.id}/editar`)}
                    className="p-1.5 text-[#64768b] hover:text-[#2a79b7] hover:bg-[#eef5fa] rounded transition-colors"
                    title="Editar Orden"
                  >
                    <Edit className="w-4 h-4" />
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}