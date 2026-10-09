import { Microscope, Beaker } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import type { OrdenPendienteResultado } from '../types/result.types';

interface ResultTableProps {
  ordenes: OrdenPendienteResultado[];
  isLoading: boolean;
}

export default function ResultTable({ ordenes, isLoading }: ResultTableProps) {
  const navigate = useNavigate();

  if (isLoading) {
    return <div className="p-8 text-center text-[#64768b]">Buscando órdenes pendientes...</div>;
  }

  if (!ordenes || ordenes.length === 0) {
    return (
      <div className="p-12 text-center text-[#64768b] bg-white border border-[#dce5ef] rounded-lg">
        <Beaker className="w-12 h-12 mx-auto text-[#dce5ef] mb-3" />
        <h3 className="text-lg font-bold text-[#15243b]">Al día</h3>
        <p>No hay órdenes pendientes de procesamiento en este momento.</p>
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
            <th className="px-6 py-4 font-bold">Fecha de Ingreso</th>
            <th className="px-6 py-4 font-bold text-center">Estado</th>
            <th className="px-6 py-4 font-bold text-right">Acciones</th>
          </tr>
        </thead>
        <tbody>
          {ordenes.map((orden) => {
            // Protección contra el número de orden en 0 si falta mapearlo en C#
            const numOrdenStr = orden.numeroOrden && orden.numeroOrden > 0
              ? orden.numeroOrden.toString().padStart(4, '0') 
              : (orden.ordenId ? orden.ordenId.split('-').pop()?.substring(0, 4).toUpperCase() : '0000');

            return (
              <tr key={orden.ordenId} className="border-b border-[#dce5ef] hover:bg-[#f3f7fb] transition-colors">
                <td className="px-6 py-4 font-bold text-[#2a79b7]">
                  #ORD-{numOrdenStr}
                </td>
                <td className="px-6 py-4">
                  <p className="font-bold text-[#15243b]">{orden.paciente}</p>
                  <p className="text-xs text-[#64768b]">C.I: {orden.cedula || 'N/A'}</p>
                </td>
                <td className="px-6 py-4 text-[#64768b] font-medium">
                  {new Date(orden.fechaCreacion).toLocaleDateString('es-ES', { 
                    day: '2-digit', month: 'short', year: 'numeric', hour: '2-digit', minute: '2-digit' 
                  })}
                </td>
                <td className="px-6 py-4 text-center">
                  <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-bold border bg-yellow-50 text-yellow-700 border-yellow-200">
                    PENDIENTE
                  </span>
                </td>
                <td className="px-6 py-4 text-right">
                  <button 
                    onClick={() => navigate(`/resultados/${orden.ordenId}/procesar`)}
                    className="inline-flex items-center gap-2 px-4 py-2 text-sm font-bold text-white bg-[#2a79b7] hover:bg-[#206094] rounded-lg transition-colors shadow-sm"
                  >
                    <Microscope className="w-4 h-4" />
                    Procesar
                  </button>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}