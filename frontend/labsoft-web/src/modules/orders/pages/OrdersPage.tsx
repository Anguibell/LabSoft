import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Plus } from 'lucide-react';
import PageHeader from '../../../components/common/PageHeader';
import OrderTable from '../components/OrderTable';
import { useOrders } from '../hooks/useOrders';
import SearchInput from '../../../components/common/SearchInput';

export default function OrdersPage() {
  const navigate = useNavigate();
  
  // 1. Un solo estado que se actualiza en tiempo real
  const [searchTerm, setSearchTerm] = useState('');

  // 2. Le pasamos el texto directamente a TanStack Query. 
  // Al teclear, React Query hará la petición automáticamente.
  const { data: pagedOrders, isLoading } = useOrders({
    pageNumber: 1,
    pageSize: 50,
    terminoPaciente: searchTerm || undefined,
  });

  const orders = pagedOrders?.items || [];

  return (
    <div className="space-y-6 animate-in fade-in duration-300">
      
      <PageHeader 
        title="Órdenes Médicas" 
        description="Gestión de órdenes de laboratorio, recepción de muestras y estado de los resultados."
        action={
          <button 
            onClick={() => navigate('/ordenes/nueva')}
            className="flex items-center justify-center gap-2 px-5 py-2.5 bg-[#2a79b7] text-white text-sm font-bold rounded-lg hover:bg-[#206094] transition-colors shadow-sm h-10.5"
            //className="flex items-center gap-2 px-4 py-2 bg-[#2a79b7] text-white rounded-lg hover:bg-[#206094] transition-colors font-medium shadow-sm"
          >
            <Plus className="w-5 h-5" />
            Nueva Orden
          </button>
        }
      />

      {/* Nuestro nuevo componente universal de búsqueda */}
      <SearchInput 
        value={searchTerm} 
        onChange={setSearchTerm} 
        placeholder="Buscar por cédula o nombre del paciente..." 
      />

      <OrderTable orders={orders} isLoading={isLoading} />
    </div>
  );
}