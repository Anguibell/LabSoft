// import { useState } from 'react';
// import { useNavigate } from 'react-router-dom';
// import { Plus, Search, Layers, Edit2, ChevronRight, ChevronDown, Activity, Trash2 } from 'lucide-react';
// import { AxiosError } from 'axios';
// import { usePanels } from '../hooks/usePanels';
// import { usePanelMutations } from '../hooks/usePanelMutations';
// import { ROUTES } from '../../../routes/routeConfig';
// import { ConfirmDialog } from '../../../components/ui/ConfirmDialog';
// import type { Panel } from '../types/panel.types';

// // Subcomponente
// const PanelGroup = ({ 
//   panel, 
//   searchTerm, 
//   onDeleteRequest 
// }: { 
//   panel: Panel; 
//   searchTerm: string;
//   onDeleteRequest: (id: string, nombre: string) => void;
// }) => {
//   const navigate = useNavigate();
//   const [isExpanded, setIsExpanded] = useState(searchTerm.length > 0);

//   return (
//     <div className="bg-white border border-[#dce5ef] rounded-xl shadow-sm overflow-hidden mb-4 transition-all">
//       <div className="flex items-center justify-between bg-[#f8fafc] hover:bg-[#eef5fa] px-6 py-4 border-b border-[#dce5ef] transition-colors">
//         <button 
//           onClick={() => setIsExpanded(!isExpanded)}
//           className="flex items-center gap-3 flex-1 text-left"
//         >
//           {isExpanded ? <ChevronDown className="w-5 h-5 text-[#2a79b7]" /> : <ChevronRight className="w-5 h-5 text-[#64768b]" />}
//           <Activity className={`w-5 h-5 ${panel.activo ? 'text-[#2a79b7]' : 'text-gray-400'}`} />
//           <h2 className="text-sm font-bold text-[#15243b] tracking-wider uppercase">
//             <span className="text-[#64768b] mr-2">{panel.codigo}</span>
//             {panel.nombre} 
//             <span className="text-[#64768b] font-medium ml-2 normal-case">({panel.examenes.length} exámenes)</span>
//           </h2>
//           {!panel.activo && (
//             <span className="ml-2 bg-red-100 text-red-700 text-[10px] px-2 py-0.5 rounded font-bold uppercase">Inactivo</span>
//           )}
//         </button>

//         <div className="flex items-center gap-2">
//           <button 
//             onClick={() => navigate(ROUTES.PANEL_EDIT(panel.id))}
//             className="flex items-center gap-1.5 text-[#2a79b7] hover:bg-[#dce5ef] px-3 py-1.5 rounded-lg font-medium text-xs transition-colors"
//           >
//             <Edit2 className="w-3.5 h-3.5" /> Editar
//           </button>
//           <button 
//             onClick={() => onDeleteRequest(panel.id, panel.nombre)}
//             className="flex items-center gap-1.5 text-red-500 hover:bg-red-50 px-3 py-1.5 rounded-lg font-medium text-xs transition-colors"
//             title="Eliminar Panel"
//           >
//             <Trash2 className="w-3.5 h-3.5" />
//           </button>
//         </div>
//       </div>

//       {isExpanded && (
//         <div className="overflow-x-auto animate-in fade-in slide-in-from-top-2 duration-200 bg-white p-4">
//           {panel.examenes.length === 0 ? (
//             <p className="text-sm text-[#64768b] text-center py-4 bg-gray-50 rounded border border-dashed border-gray-200">
//               Este panel no tiene exámenes asociados.
//             </p>
//           ) : (
//             <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-2">
//               {[...panel.examenes]
//                 .sort((a, b) => a.codigo - b.codigo)
//                 .map((examen) => (
//                 <div key={examen.id} className="flex flex-col justify-center text-sm text-[#15243b] bg-[#f8fafc] p-3 rounded border border-[#dce5ef] hover:border-[#2a79b7] transition-colors">
//                   <div className="flex items-start gap-2">
//                     <span className="text-xs font-bold text-[#64768b] mt-0.5">{examen.codigo}</span>
//                     <span className="font-medium leading-tight">{examen.nombre}</span>
//                   </div>
//                   {examen.especialidadNombre && (
//                     <div className="mt-2 ml-7">
//                       <span className="text-[9px] text-[#2a79b7] bg-[#eef5fa] px-1.5 py-0.5 rounded font-bold uppercase tracking-wider">
//                         {examen.especialidadNombre}
//                       </span>
//                     </div>
//                   )}
//                 </div>
//               ))}
//             </div>
//           )}
//         </div>
//       )}
//     </div>
//   );
// };

// export default function PanelsPage() {
//   const navigate = useNavigate();
//   const { data: panels, isLoading, isError } = usePanels();
//   const { deletePanel } = usePanelMutations();
//   const [searchTerm, setSearchTerm] = useState('');
  
//   // Estado para el modal de confirmación
//   const [panelToDelete, setPanelToDelete] = useState<{ id: string; nombre: string } | null>(null);

//   if (isLoading) return <div className="p-8 text-center text-[#64768b]">Cargando perfiles y paneles...</div>;
//   if (isError) return <div className="p-8 text-center text-red-500">Error al cargar los paneles.</div>;

//   const filteredPanels = panels
//     ?.filter(panel => {
//       const searchLower = searchTerm.toLowerCase();
//       return panel.nombre.toLowerCase().includes(searchLower) ||
//              panel.codigo.toString().includes(searchLower) ||
//              panel.examenes.some(e => e.nombre.toLowerCase().includes(searchLower) || e.codigo.toString().includes(searchLower));
//     })
//     .sort((a, b) => a.codigo - b.codigo); 

//   const confirmDelete = () => {
//     if (panelToDelete) {
//       deletePanel.mutate(panelToDelete.id, {
//         onSuccess: () => {
//           setPanelToDelete(null);
//         },
//         onError: (error: unknown) => {
//           setPanelToDelete(null); // Cerramos el modal pase lo que pase
          
//           let errorMessage = 'Ocurrió un error al eliminar el panel.';
//           if (error instanceof AxiosError) {
//             errorMessage = error.response?.data?.message || error.message;
//           } else if (error instanceof Error) {
//             errorMessage = error.message;
//           }
          
//           alert(`Error al eliminar: ${errorMessage}`);
//         }
//       });
//     }
//   };

//   return (
//     <div className="max-w-7xl mx-auto space-y-6">
//       <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4 bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
//         <div>
//           <h1 className="text-2xl font-bold text-[#15243b] flex items-center gap-2">
//             <Layers className="w-6 h-6 text-[#2a79b7]" />
//             Paneles y Perfiles
//           </h1>
//           <p className="text-[#64768b] mt-1">Agrupa múltiples exámenes para agilizar la creación de órdenes.</p>
//         </div>
        
//         <div className="flex items-center gap-3 w-full md:w-auto">
//           <div className="relative flex-1 md:w-64">
//             <Search className="w-4 h-4 absolute left-3 top-1/2 -translate-y-1/2 text-[#94a3b8]" />
//             <input
//               type="text"
//               placeholder="Buscar panel o examen..."
//               value={searchTerm}
//               onChange={(e) => setSearchTerm(e.target.value)}
//               className="w-full pl-9 pr-4 py-2 border border-[#dce5ef] rounded-lg text-sm focus:outline-none focus:border-[#2a79b7]"
//             />
//           </div>
//           <button
//             onClick={() => navigate(ROUTES.PANEL_NEW)}
//             className="flex items-center gap-2 px-4 py-2 bg-[#2a79b7] text-white text-sm font-medium rounded-lg hover:bg-[#206094] transition-colors whitespace-nowrap shadow-sm"
//           >
//             <Plus className="w-4 h-4" />
//             Nuevo Panel
//           </button>
//         </div>
//       </div>

//       <div className="space-y-2">
//         {filteredPanels?.length === 0 ? (
//            <div className="text-center py-12 bg-white rounded-xl border border-[#dce5ef]">
//              <p className="text-[#64768b]">No se encontraron paneles que coincidan con la búsqueda.</p>
//            </div>
//         ) : (
//           filteredPanels?.map((panel) => (
//             <PanelGroup 
//               key={panel.id} 
//               panel={panel} 
//               searchTerm={searchTerm} 
//               onDeleteRequest={(id, nombre) => setPanelToDelete({ id, nombre })}
//             />
//           ))
//         )}
//       </div>

//       <ConfirmDialog
//         isOpen={!!panelToDelete}
//         title="Eliminar Panel"
//         message={`¿Estás seguro de que deseas eliminar el panel "${panelToDelete?.nombre}"? Esta acción no se puede deshacer.`}
//         confirmText="Sí, eliminar"
//         cancelText="Cancelar"
//         onConfirm={confirmDelete}
//         onCancel={() => setPanelToDelete(null)}
//         isLoading={deletePanel.isPending}
//       />
//     </div>
//   );
// }



import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Plus, Activity, Edit2, ChevronRight, ChevronDown, Trash2 } from 'lucide-react';
import { AxiosError } from 'axios';
import { usePanels } from '../hooks/usePanels';
import { usePanelMutations } from '../hooks/usePanelMutations';
import { ROUTES } from '../../../routes/routeConfig';
import { ConfirmDialog } from '../../../components/ui/ConfirmDialog';
import type { Panel } from '../types/panel.types';
import PageHeader from '../../../components/common/PageHeader';
import SearchInput from '../../../components/common/SearchInput';
import Button from '../../../components/ui/Button';

const PanelGroup = ({ 
  panel, 
  searchTerm, 
  onDeleteRequest 
}: { 
  panel: Panel; 
  searchTerm: string;
  onDeleteRequest: (id: string, nombre: string) => void;
}) => {
  const navigate = useNavigate();
  const [isExpanded, setIsExpanded] = useState(searchTerm.length > 0);

  return (
    <div className="bg-white border border-[#dce5ef] rounded-xl shadow-sm overflow-hidden mb-4 transition-all">
      <div className="flex items-center justify-between bg-[#f8fafc] hover:bg-[#eef5fa] px-6 py-4 border-b border-[#dce5ef] transition-colors">
        <button 
          onClick={() => setIsExpanded(!isExpanded)}
          className="flex items-center gap-3 flex-1 text-left"
        >
          {isExpanded ? <ChevronDown className="w-5 h-5 text-[#2a79b7]" /> : <ChevronRight className="w-5 h-5 text-[#64768b]" />}
          <Activity className={`w-5 h-5 ${panel.activo ? 'text-[#2a79b7]' : 'text-gray-400'}`} />
          <h2 className="text-sm font-bold text-[#15243b] tracking-wider uppercase">
            <span className="text-[#64768b] mr-2">{panel.codigo}</span>
            {panel.nombre} 
            <span className="text-[#64768b] font-medium ml-2 normal-case">({panel.examenes.length} exámenes)</span>
          </h2>
          {!panel.activo && (
            <span className="ml-2 bg-red-100 text-red-700 text-[10px] px-2 py-0.5 rounded font-bold uppercase tracking-wider">Inactivo</span>
          )}
        </button>

        <div className="flex items-center gap-2">
          <button 
            onClick={() => navigate(ROUTES.PANEL_EDIT(panel.id))}
            className="flex items-center gap-1.5 text-[#64768b] hover:text-[#2a79b7] hover:bg-[#eef5fa] px-3 py-1.5 rounded-lg font-bold text-xs transition-colors"
            title="Editar Panel"
          >
            <Edit2 className="w-3.5 h-3.5" /> Editar
          </button>
          <button 
            onClick={() => onDeleteRequest(panel.id, panel.nombre)}
            className="flex items-center gap-1.5 text-[#64768b] hover:text-red-600 hover:bg-red-50 px-3 py-1.5 rounded-lg font-bold text-xs transition-colors"
            title="Eliminar Panel"
          >
            <Trash2 className="w-3.5 h-3.5" /> Eliminar
          </button>
        </div>
      </div>

      {isExpanded && (
        <div className="overflow-x-auto animate-in fade-in slide-in-from-top-2 duration-200 bg-white p-4">
          {panel.examenes.length === 0 ? (
            <p className="text-sm text-[#64768b] text-center py-4 bg-gray-50 rounded-lg border border-dashed border-gray-200">
              Este panel no tiene exámenes asociados.
            </p>
          ) : (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-2">
              {[...panel.examenes]
                .sort((a, b) => a.codigo - b.codigo)
                .map((examen) => (
                <div key={examen.id} className="flex flex-col justify-center text-sm text-[#15243b] bg-[#f8fafc] p-3 rounded-lg border border-[#dce5ef] hover:border-[#2a79b7] transition-colors">
                  <div className="flex items-start gap-2">
                    <span className="text-xs font-bold text-[#64768b] mt-0.5">{examen.codigo}</span>
                    <span className="font-medium leading-tight">{examen.nombre}</span>
                  </div>
                  {examen.especialidadNombre && (
                    <div className="mt-2 ml-7">
                      <span className="text-[9px] text-[#2a79b7] bg-[#eef5fa] px-1.5 py-0.5 rounded font-bold uppercase tracking-wider">
                        {examen.especialidadNombre}
                      </span>
                    </div>
                  )}
                </div>
              ))}
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default function PanelsPage() {
  const navigate = useNavigate();
  const { data: panels, isLoading, isError } = usePanels();
  const { deletePanel } = usePanelMutations();
  const [searchTerm, setSearchTerm] = useState('');
  
  const [panelToDelete, setPanelToDelete] = useState<{ id: string; nombre: string } | null>(null);

  // Spinner estandarizado
  if (isLoading) return (
    <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden p-12 flex justify-center">
      <div className="text-center text-[#64768b] font-medium flex items-center gap-2">
        <div className="w-4 h-4 border-2 border-[#2a79b7] border-t-transparent rounded-full animate-spin"></div>
        Cargando perfiles y paneles...
      </div>
    </div>
  );
  if (isError) return <div className="p-8 text-center text-red-500 bg-red-50 rounded-xl border border-red-100">Error al cargar los paneles.</div>;

  const filteredPanels = panels
    ?.filter(panel => {
      const searchLower = searchTerm.toLowerCase();
      return panel.nombre.toLowerCase().includes(searchLower) ||
             panel.codigo.toString().includes(searchLower) ||
             panel.examenes.some(e => e.nombre.toLowerCase().includes(searchLower) || e.codigo.toString().includes(searchLower));
    })
    .sort((a, b) => a.codigo - b.codigo); 

  const confirmDelete = () => {
    if (panelToDelete) {
      deletePanel.mutate(panelToDelete.id, {
        onSuccess: () => {
          setPanelToDelete(null);
        },
        onError: (error: unknown) => {
          setPanelToDelete(null);
          let errorMessage = 'Ocurrió un error al eliminar el panel.';
          if (error instanceof AxiosError) {
            errorMessage = error.response?.data?.message || error.message;
          } else if (error instanceof Error) {
            errorMessage = error.message;
          }
          alert(`Error al eliminar: ${errorMessage}`);
        }
      });
    }
  };

  return (
    <div className="space-y-6 animate-in fade-in duration-300">
      
      {/* Encabezado Universal */}
      <PageHeader 
        title="Paneles y Perfiles" 
        description="Agrupa múltiples exámenes para agilizar la creación de órdenes."
        action={
          <Button icon={Plus} onClick={() => navigate(ROUTES.PANEL_NEW)}>
            Nuevo Panel
          </Button>
        }
      />

      {/* Buscador Universal */}
      <SearchInput 
        value={searchTerm} 
        onChange={setSearchTerm} 
        placeholder="Buscar panel o examen..." 
      />

      <div className="space-y-2">
        {filteredPanels?.length === 0 ? (
           <div className="text-center py-12 bg-white rounded-xl border border-[#dce5ef] shadow-sm">
             <p className="text-[#64768b] font-medium">No se encontraron paneles que coincidan con la búsqueda.</p>
           </div>
        ) : (
          filteredPanels?.map((panel) => (
            <PanelGroup 
              key={panel.id} 
              panel={panel} 
              searchTerm={searchTerm} 
              onDeleteRequest={(id, nombre) => setPanelToDelete({ id, nombre })}
            />
          ))
        )}
      </div>

      <ConfirmDialog
        isOpen={!!panelToDelete}
        title="Eliminar Panel"
        message={`¿Estás seguro de que deseas eliminar el panel "${panelToDelete?.nombre}"? Esta acción no se puede deshacer.`}
        confirmText="Sí, eliminar"
        cancelText="Cancelar"
        onConfirm={confirmDelete}
        onCancel={() => setPanelToDelete(null)}
        isLoading={deletePanel.isPending}
      />
    </div>
  );
}