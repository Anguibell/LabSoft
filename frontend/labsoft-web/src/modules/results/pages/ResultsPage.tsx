import { useState, useMemo } from 'react';
import { Search } from 'lucide-react';
import PageHeader from '../../../components/common/PageHeader';
import ResultTable from '../components/ResultTable';
import { useResultadosPendientes } from '../hooks/useResults';

export default function ResultsPage() {
  const [searchTerm, setSearchTerm] = useState('');
  
  // Obtenemos todas las órdenes pendientes directamente del backend
  const { data: ordenesPendientes, isLoading } = useResultadosPendientes();

  // Filtrado rápido en el cliente (por nombre, cédula o número de orden)
  const filteredOrdenes = useMemo(() => {
    if (!ordenesPendientes) return [];
    if (!searchTerm) return ordenesPendientes;

    const lowerSearch = searchTerm.toLowerCase();
    return ordenesPendientes.filter(o => 
      o.paciente.toLowerCase().includes(lowerSearch) ||
      (o.cedula && o.cedula.includes(lowerSearch)) ||
      (o.numeroOrden && o.numeroOrden.toString().includes(lowerSearch))
    );
  }, [ordenesPendientes, searchTerm]);

  return (
    <div className="space-y-6 animate-in fade-in duration-300">
      
      <PageHeader 
        title="Procesamiento de Resultados" 
        description="Selecciona una orden médica pendiente para cargar y validar los resultados de laboratorio."
      />

      {/* BUSCADOR */}
      <div className="bg-white p-4 rounded-xl border border-[#dce5ef] shadow-sm">
        <div className="relative max-w-md">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-[#94a3b8]" />
          <input
            type="text"
            placeholder="Buscar por cédula o nombre del paciente..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none font-medium"
          />
        </div>
      </div>

      {/* TABLA DE DATOS */}
      <ResultTable ordenes={filteredOrdenes} isLoading={isLoading} />

    </div>
  );
}