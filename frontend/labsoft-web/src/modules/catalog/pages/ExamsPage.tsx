// import { useState } from 'react';
// import { useNavigate } from 'react-router-dom';
// import { Plus, Search, FlaskConical, Edit2, AlertCircle, CornerDownRight } from 'lucide-react';
// import { useExams } from '../hooks/useExams';
// import { ROUTES } from '../../../routes/routeConfig'; 
// import type { Exam } from '../types/exam.types';

// // Subcomponente recursivo para renderizar filas y sus hijos indentados
// const ExamTableRow = ({ examen, nivel = 0 }: { examen: Exam; nivel?: number }) => {
//   const paddingLeft = nivel * 24; // Indentación en píxeles basada en la jerarquía

//   return (
//     <>
//       <tr className="hover:bg-[#f3f7fb] transition-colors">
//         <td className="px-6 py-3 font-medium text-[#4e6681]">
//           {examen.codigo !== 0 ? examen.codigo : ''}
//         </td>
//         <td className="px-6 py-3 font-medium text-[#15243b]" style={{ paddingLeft: `${paddingLeft + 24}px` }}>
//           <div className="flex items-center gap-2">
//             {nivel > 0 && <CornerDownRight className="w-4 h-4 text-[#94a3b8]" />}
//             {examen.nombre}
//             {examen.esOpcionalDinamico && (
//               <span className="text-[10px] bg-amber-100 text-amber-700 px-1.5 py-0.5 rounded uppercase font-bold">Opcional</span>
//             )}
//           </div>
//         </td>
//         <td className="px-6 py-3 text-[#64768b]">{examen.unidad || '-'}</td>
//         <td className="px-6 py-3 text-xs text-[#64768b] truncate max-w-50" title={examen.valoresReferencia || ''}>
//           {examen.valoresReferencia || '-'}
//         </td>
//         <td className="px-6 py-3 text-center">
//           <span className={`inline-flex items-center px-2 py-0.5 rounded text-xs font-medium ${
//             examen.activo ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
//           }`}>
//             {examen.activo ? 'Activo' : 'Inactivo'}
//           </span>
//         </td>
//         <td className="px-6 py-3 text-right">
//           <button className="p-1.5 text-[#2a79b7] hover:bg-[#eef5fa] rounded-lg transition-colors" title="Editar Examen">
//             <Edit2 className="w-4 h-4" />
//           </button>
//         </td>
//       </tr>
//       {/* Llamada recursiva para los hijos */}
//       {examen.subExamenes?.map((subExamen) => (
//         <ExamTableRow key={subExamen.id} examen={subExamen} nivel={nivel + 1} />
//       ))}
//     </>
//   );
// };

// export default function ExamsPage() {
//   const navigate = useNavigate();
//   const { data: groupedExams, isLoading, isError } = useExams();
//   const [searchTerm, setSearchTerm] = useState('');

//   if (isLoading) {
//     return <div className="p-8 text-center text-[#64768b]">Cargando catálogo de exámenes...</div>;
//   }

//   if (isError) {
//     return (
//       <div className="p-8 flex flex-col items-center justify-center text-red-500">
//         <AlertCircle className="w-8 h-8 mb-2" />
//         <p>Error al cargar el catálogo de exámenes.</p>
//       </div>
//     );
//   }

//   // Filtrado simple en el frontend
//   const filteredExams = groupedExams?.map(group => ({
//     ...group,
//     examenes: group.examenes.filter(exam => 
//       exam.nombre.toLowerCase().includes(searchTerm.toLowerCase()) || 
//       exam.codigo.toString().includes(searchTerm)
//     )
//   })).filter(group => group.examenes.length > 0);

//   return (
//     <div className="max-w-7xl mx-auto space-y-6">
//       {/* Cabecera de la Página */}
//       <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4 bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
//         <div>
//           <h1 className="text-2xl font-bold text-[#15243b] flex items-center gap-2">
//             <FlaskConical className="w-6 h-6 text-[#2a79b7]" />
//             Catálogo de Exámenes
//           </h1>
//           <p className="text-[#64768b] mt-1">Gestiona las pruebas, valores de referencia y unidades del laboratorio.</p>
//         </div>
        
//         <div className="flex items-center gap-3 w-full md:w-auto">
//           <div className="relative flex-1 md:w-64">
//             <Search className="w-4 h-4 absolute left-3 top-1/2 -translate-y-1/2 text-[#94a3b8]" />
//             <input
//               type="text"
//               placeholder="Buscar por código o nombre..."
//               value={searchTerm}
//               onChange={(e) => setSearchTerm(e.target.value)}
//               className="w-full pl-9 pr-4 py-2 border border-[#dce5ef] rounded-lg text-sm focus:outline-none focus:border-[#2a79b7] focus:ring-1 focus:ring-[#2a79b7] transition-all"
//             />
//           </div>
//           <button
//             onClick={() => navigate(ROUTES.EXAM_NEW)}
//             className="flex items-center gap-2 px-4 py-2 bg-[#2a79b7] text-white text-sm font-medium rounded-lg hover:bg-[#206094] transition-colors whitespace-nowrap shadow-sm"
//           >
//             <Plus className="w-4 h-4" />
//             Nuevo Examen
//           </button>
//         </div>
//       </div>

//       {/* Lista Agrupada por Especialidad */}
//       <div className="space-y-8">
//         {filteredExams?.length === 0 ? (
//           <div className="text-center py-12 bg-white rounded-xl border border-[#dce5ef]">
//             <p className="text-[#64768b]">No se encontraron exámenes que coincidan con la búsqueda.</p>
//           </div>
//         ) : (
//           filteredExams?.map((grupo, idx) => (
//             <div key={idx} className="bg-white border border-[#dce5ef] rounded-xl shadow-sm overflow-hidden">
//               {/* Encabezado de la Especialidad */}
//               <div className="bg-[#f8fafc] px-6 py-3 border-b border-[#dce5ef]">
//                 <h2 className="text-sm font-bold text-[#2a79b7] tracking-wider uppercase">
//                   {grupo.nombreEspecialidad} ({grupo.examenes.length})
//                 </h2>
//               </div>

//               {/* Tabla de Exámenes */}
//               <div className="overflow-x-auto">
//                 <table className="w-full text-sm text-left whitespace-nowrap">
//                   <thead className="text-[#64768b] border-b border-[#dce5ef]">
//                     <tr>
//                       <th className="px-6 py-3 font-medium w-24">Código</th>
//                       <th className="px-6 py-3 font-medium">Nombre del Examen</th>
//                       <th className="px-6 py-3 font-medium">Unidad</th>
//                       <th className="px-6 py-3 font-medium">Valores de Referencia</th>
//                       <th className="px-6 py-3 font-medium text-center">Estado</th>
//                       <th className="px-6 py-3 font-medium text-right">Acciones</th>
//                     </tr>
//                   </thead>
//                   <tbody className="divide-y divide-[#dce5ef]">
//                     {grupo.examenes.map((examen) => (
//                       <ExamTableRow key={examen.id} examen={examen} />
//                     ))}
//                   </tbody>
//                   {/* <tbody className="divide-y divide-[#dce5ef]">
//                     {grupo.examenes.map((examen) => (
//                       <tr key={examen.id} className="hover:bg-[#f3f7fb] transition-colors">
//                         <td className="px-6 py-3 font-medium text-[#4e6681]">{examen.codigo}</td>
//                         <td className="px-6 py-3 font-medium text-[#15243b]">
//                           {examen.nombre}
//                           {examen.esOpcionalDinamico && (
//                             <span className="ml-2 text-[10px] bg-amber-100 text-amber-700 px-1.5 py-0.5 rounded uppercase font-bold">Opcional</span>
//                           )}
//                         </td>
//                         <td className="px-6 py-3 text-[#64768b]">{examen.unidad || '-'}</td>
//                         <td className="px-6 py-3 text-xs text-[#64768b] truncate max-w-50" title={examen.valoresReferencia || ''}>
//                           {examen.valoresReferencia || '-'}
//                         </td>
//                         <td className="px-6 py-3 text-center">
//                           <span className={`inline-flex items-center px-2 py-0.5 rounded text-xs font-medium ${
//                             examen.activo ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
//                           }`}>
//                             {examen.activo ? 'Activo' : 'Inactivo'}
//                           </span>
//                         </td>
//                         <td className="px-6 py-3 text-right">
//                           <button
//                             onClick={() => navigate(ROUTES.EXAM_EDIT(examen.id))}
//                             className="p-1.5 text-[#2a79b7] hover:bg-[#eef5fa] rounded-lg transition-colors"
//                             title="Editar Examen"
//                           >
//                             <Edit2 className="w-4 h-4" />
//                           </button>
//                         </td>
//                       </tr>
//                     ))}
//                   </tbody> */}
//                 </table>
//               </div>
//             </div>
//           ))
//         )}
//       </div>
//     </div>
//   );
// }

//------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------


// import { useState } from 'react';
// import { useNavigate } from 'react-router-dom';
// import { Plus, Search, FlaskConical, Edit2, ChevronRight, ChevronDown, Folder, FolderOpen } from 'lucide-react';
// import { useExams } from '../hooks/useExams';
// import { ROUTES } from '../../../routes/routeConfig';
// import type { ExamGroup } from '../types/exam.types';

// // Componente para cada bloque de especialidad (Colapsable)
// const SpecialtyGroup = ({ grupo, searchTerm }: { grupo: ExamGroup; searchTerm: string }) => {

//   const navigate = useNavigate();
  
//   // Si hay búsqueda, expandimos por defecto. Si no, colapsado.
//   const [isExpanded, setIsExpanded] = useState(searchTerm.length > 0);
  
//   // Filtramos para mostrar SOLO LOS PADRES (los que no tienen examenPadreId)
//   const parentExams = grupo.examenes.filter((examen) => !examen.examenPadreId);

//   // Si después de filtrar (por búsqueda y por ser padre) no hay exámenes, no renderizamos el bloque
//   if (parentExams.length === 0) return null;

//   return (
//     <div className="bg-white border border-[#dce5ef] rounded-xl shadow-sm overflow-hidden mb-4 transition-all">
//       {/* Cabecera Colapsable */}
//       <button 
//         onClick={() => setIsExpanded(!isExpanded)}
//         className="w-full flex items-center gap-3 bg-[#f8fafc] hover:bg-[#eef5fa] px-6 py-4 border-b border-[#dce5ef] transition-colors"
//       >
//         {isExpanded ? <ChevronDown className="w-5 h-5 text-[#2a79b7]" /> : <ChevronRight className="w-5 h-5 text-[#64768b]" />}
//         {isExpanded ? <FolderOpen className="w-5 h-5 text-[#2a79b7]" /> : <Folder className="w-5 h-5 text-[#64768b]" />}
//         <h2 className="text-sm font-bold text-[#15243b] tracking-wider uppercase">
//           {grupo.nombreEspecialidad} <span className="text-[#64768b] font-medium ml-1">({parentExams.length} principales)</span>
//         </h2>
//       </button>

//       {/* Tabla (Solo visible si está expandido) */}
//       {isExpanded && (
//         <div className="overflow-x-auto animate-in fade-in slide-in-from-top-2 duration-200">
//           <table className="w-full text-sm text-left whitespace-nowrap">
//             <thead className="text-[#64768b] border-b border-[#dce5ef] bg-white">
//               <tr>
//                 <th className="px-6 py-3 font-medium w-24">Código</th>
//                 <th className="px-6 py-3 font-medium">Nombre del Examen</th>
//                 <th className="px-6 py-3 font-medium text-center">Estado</th>
//                 <th className="px-6 py-3 font-medium text-right">Acciones</th>
//               </tr>
//             </thead>
//             <tbody className="divide-y divide-[#dce5ef]">
//               {parentExams.map((examen) => (
//                 <tr key={examen.id} className="hover:bg-[#f3f7fb] transition-colors">
//                   <td className="px-6 py-3 font-medium text-[#4e6681]">{examen.codigo}</td>
//                   <td className="px-6 py-3 font-medium text-[#15243b]">{examen.nombre}</td>
//                   <td className="px-6 py-3 text-center">
//                     <span className={`inline-flex items-center px-2 py-0.5 rounded text-xs font-medium ${
//                       examen.activo ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
//                     }`}>
//                       {examen.activo ? 'Activo' : 'Inactivo'}
//                     </span>
//                   </td>
//                   <td className="px-6 py-3 text-right">
//                     <button 
//                         onClick={() => navigate(ROUTES.EXAM_EDIT(examen.id))}
//                         className="flex items-center justify-end gap-2 text-[#2a79b7] hover:text-[#206094] font-medium w-full text-xs"
//                     >
//                         <Edit2 className="w-3.5 h-3.5" /> EDITAR
//                     </button>
//                   </td>
//                 </tr>
//               ))}
//             </tbody>
//           </table>
//         </div>
//       )}
//     </div>
//   );
// };

// export default function ExamsPage() {
//   const navigate = useNavigate();
//   const { data: groupedExams, isLoading, isError } = useExams();
//   const [searchTerm, setSearchTerm] = useState('');

//   if (isLoading) return <div className="p-8 text-center text-[#64768b]">Cargando catálogo...</div>;
//   if (isError) return <div className="p-8 text-center text-red-500">Error al cargar el catálogo.</div>;

//   // Filtrado general por búsqueda
//   const filteredExams = groupedExams?.map(group => ({
//     ...group,
//     examenes: group.examenes.filter(exam => 
//       exam.nombre.toLowerCase().includes(searchTerm.toLowerCase()) || 
//       exam.codigo.toString().includes(searchTerm)
//     )
//   }));

//   return (
//     <div className="max-w-7xl mx-auto space-y-6">
//       <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4 bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
//         <div>
//           <h1 className="text-2xl font-bold text-[#15243b] flex items-center gap-2">
//             <FlaskConical className="w-6 h-6 text-[#2a79b7]" />
//             Catálogo de Exámenes
//           </h1>
//           <p className="text-[#64768b] mt-1">Gestiona las pruebas y parámetros por especialidad.</p>
//         </div>
        
//         <div className="flex items-center gap-3 w-full md:w-auto">
//           <div className="relative flex-1 md:w-64">
//             <Search className="w-4 h-4 absolute left-3 top-1/2 -translate-y-1/2 text-[#94a3b8]" />
//             <input
//               type="text"
//               placeholder="Buscar por código o nombre..."
//               value={searchTerm}
//               onChange={(e) => setSearchTerm(e.target.value)}
//               className="w-full pl-9 pr-4 py-2 border border-[#dce5ef] rounded-lg text-sm focus:outline-none focus:border-[#2a79b7]"
//             />
//           </div>
//           <button
//             onClick={() => navigate(ROUTES.EXAM_NEW)}
//             className="flex items-center gap-2 px-4 py-2 bg-[#2a79b7] text-white text-sm font-medium rounded-lg hover:bg-[#206094] transition-colors whitespace-nowrap shadow-sm"
//           >
//             <Plus className="w-4 h-4" />
//             Nuevo Examen
//           </button>
//         </div>
//       </div>

//       <div className="space-y-2">
//         {filteredExams?.map((grupo, idx) => (
//           <SpecialtyGroup key={idx} grupo={grupo} searchTerm={searchTerm} />
//         ))}
//       </div>
//     </div>
//   );
// }



//------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------




import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Plus, Folder, FolderOpen, ChevronRight, ChevronDown, Edit2 } from 'lucide-react';
import { useExams } from '../hooks/useExams';
import { ROUTES } from '../../../routes/routeConfig';
import type { ExamGroup } from '../types/exam.types';
import PageHeader from '../../../components/common/PageHeader';
import SearchInput from '../../../components/common/SearchInput';
import Button from '../../../components/ui/Button';
import { TableHeader, Th } from '../../../components/ui/Table';

const SpecialtyGroup = ({ grupo, searchTerm }: { grupo: ExamGroup; searchTerm: string }) => {
  const navigate = useNavigate();
  const [isExpanded, setIsExpanded] = useState(searchTerm.length > 0);
  
  const parentExams = grupo.examenes.filter((examen) => !examen.examenPadreId);
  if (parentExams.length === 0) return null;

  return (
    <div className="bg-white border border-[#dce5ef] rounded-xl shadow-sm overflow-hidden mb-4 transition-all">
      <button 
        onClick={() => setIsExpanded(!isExpanded)}
        className="w-full flex items-center gap-3 bg-[#f8fafc] hover:bg-[#eef5fa] px-6 py-4 border-b border-[#dce5ef] transition-colors"
      >
        {isExpanded ? <ChevronDown className="w-5 h-5 text-[#2a79b7]" /> : <ChevronRight className="w-5 h-5 text-[#64768b]" />}
        {isExpanded ? <FolderOpen className="w-5 h-5 text-[#2a79b7]" /> : <Folder className="w-5 h-5 text-[#64768b]" />}
        <h2 className="text-sm font-bold text-[#15243b] tracking-wider uppercase">
          {grupo.nombreEspecialidad} <span className="text-[#64768b] font-medium ml-1">({parentExams.length} principales)</span>
        </h2>
      </button>

      {isExpanded && (
        <div className="overflow-x-auto animate-in fade-in slide-in-from-top-2 duration-200">
          <table className="w-full text-sm text-left whitespace-nowrap">
            {/* Cabecera Estandarizada */}
            <TableHeader>
              <Th className="w-24">Código</Th>
              <Th>Nombre del Examen</Th>
              <Th className="text-center">Estado</Th>
              <Th className="text-right">Acciones</Th>
            </TableHeader>
            <tbody className="divide-y divide-[#dce5ef]">
              {parentExams.map((examen) => (
                <tr key={examen.id} className="hover:bg-[#f3f7fb] transition-colors">
                  <td className="px-6 py-4 font-medium text-[#4e6681]">{examen.codigo}</td>
                  <td className="px-6 py-4 font-bold text-[#15243b]">{examen.nombre}</td>
                  <td className="px-6 py-4 text-center">
                    <span className={`inline-flex items-center px-2 py-0.5 rounded text-[10px] font-bold uppercase tracking-wider ${
                      examen.activo ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
                    }`}>
                      {examen.activo ? 'Activo' : 'Inactivo'}
                    </span>
                  </td>
                  <td className="px-6 py-4 text-right">
                    <button 
                        onClick={() => navigate(ROUTES.EXAM_EDIT(examen.id))}
                        className="inline-flex items-center p-2 text-[#64768b] hover:text-[#2a79b7] hover:bg-[#eef5fa] rounded transition-colors"
                        title="Editar Examen"
                    >
                        <Edit2 className="w-4 h-4" /> 
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default function ExamsPage() {
  const navigate = useNavigate();
  const { data: groupedExams, isLoading, isError } = useExams();
  const [searchTerm, setSearchTerm] = useState('');

  // Spinner estandarizado
  if (isLoading) return (
    <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden p-12 flex justify-center">
      <div className="text-center text-[#64768b] font-medium flex items-center gap-2">
        <div className="w-4 h-4 border-2 border-[#2a79b7] border-t-transparent rounded-full animate-spin"></div>
        Cargando catálogo...
      </div>
    </div>
  );
  if (isError) return <div className="p-8 text-center text-red-500 bg-red-50 rounded-xl border border-red-100">Error al cargar el catálogo.</div>;

  const filteredExams = groupedExams?.map(group => ({
    ...group,
    examenes: group.examenes.filter(exam => 
      exam.nombre.toLowerCase().includes(searchTerm.toLowerCase()) || 
      exam.codigo.toString().includes(searchTerm)
    )
  }));

  return (
    <div className="space-y-6 animate-in fade-in duration-300">
      
      {/* Encabezado Universal */}
      <PageHeader 
        title="Catálogo de Exámenes" 
        description="Gestiona las pruebas y parámetros por especialidad."
        action={
          <Button icon={Plus} onClick={() => navigate(ROUTES.EXAM_NEW)}>
            Nuevo Examen
          </Button>
        }
      />

      {/* Buscador Universal */}
      <SearchInput 
        value={searchTerm} 
        onChange={setSearchTerm} 
        placeholder="Buscar por código o nombre..." 
      />

      <div className="space-y-2">
        {filteredExams?.map((grupo, idx) => (
          <SpecialtyGroup key={idx} grupo={grupo} searchTerm={searchTerm} />
        ))}
      </div>
    </div>
  );
}