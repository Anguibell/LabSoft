// import { useParams, useNavigate } from 'react-router-dom';
// import { CheckCircle, ArrowLeft, ShieldCheck, FileCheck2 } from 'lucide-react';
// import PageHeader from '../../../components/common/PageHeader';
// import ResultDetailsRow from '../components/ResultDetailsRow';
// import { useResultadosPorOrden } from '../hooks/useResult';
// import { useResultMutations } from '../hooks/useResultMutations';
// import PrintResultButton from '../components/PrintResultButton';

// export default function ResultDetailsPage() {
//   const { id } = useParams<{ id: string }>();
//   const navigate = useNavigate();
  
//   const { data: orden, isLoading, isError } = useResultadosPorOrden(id);
//   const { validarOrden } = useResultMutations();

//   if (isLoading) return <div className="p-8 text-center text-[#64768b]">Cargando vista de validación...</div>;

//   if (isError || !orden) {
//     return <div className="p-8 text-center text-red-500 bg-red-50 rounded-lg">Error al cargar la orden.</div>;
//   }

//   const handleValidar = () => {
//     // Busca si hay al menos un examen procesado en el árbol para poder validarlo
//     const tieneProcesados = orden.especialidades?.some(grupo => 
//       grupo.examenes.some(detalle => 
//         JSON.stringify(detalle.arbol).includes('"estado":"Procesado"')
//       )
//     );

//     if (!tieneProcesados) {
//       alert("No hay exámenes en estado 'Procesado' listos para validar en esta orden.");
//       return;
//     }

//     if (window.confirm("¿Estás seguro de que deseas VALIDAR Y FIRMAR los resultados procesados de esta orden? Esta acción bloqueará la edición de dichos valores y generará el reporte oficial.")) {
//       validarOrden.mutate(id!, {
//         onSuccess: () => {
//           alert("Resultados validados y firmados exitosamente.");
//           navigate('/resultados');
//         }
//       });
//     }
//   };

//   return (
//     <div className="space-y-6 animate-in fade-in duration-300 max-w-6xl mx-auto">
      
//       <div className="flex items-center justify-between mb-2">
//         <button 
//           onClick={() => navigate(-1)} // Vuelve a la pantalla anterior
//           className="flex items-center gap-2 text-sm font-medium text-[#64768b] hover:text-[#2a79b7] transition-colors"
//         >
//           <ArrowLeft className="w-4 h-4" />
//           Volver
//         </button>
//       </div>

//       {/* <PageHeader 
//         title="Validación Oficial de Resultados" 
//         description="Revisión clínica de los valores procesados antes de la emisión del reporte oficial."
//         action={
//           <button
//             onClick={handleValidar}
//             disabled={validarOrden.isPending}
//             className="flex items-center gap-2 px-6 py-2.5 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors font-bold shadow-sm disabled:opacity-50"
//           >
//             {validarOrden.isPending ? <ShieldCheck className="w-5 h-5 animate-pulse" /> : <FileCheck2 className="w-5 h-5" />}
//             {validarOrden.isPending ? 'Firmando...' : 'VALIDAR Y FIRMAR ORDEN'}
//           </button>
//         }
//       /> */}

//       <PageHeader 
//         title="Validación Oficial de Resultados" 
//         description="Revisión clínica de los valores procesados antes de la emisión del reporte oficial."
//         action={
//           <div className="flex items-center gap-3">
//             {/* Solo mostramos el botón de imprimir si la orden ya tiene al menos una parte procesada o validada */}
//             <PrintResultButton ordenId={id!} />

//             <button
//               onClick={handleValidar}
//               disabled={validarOrden.isPending || orden.estadoOrden === 'Validado'}
//               className="flex items-center gap-2 px-6 py-2.5 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors font-bold shadow-sm disabled:opacity-50"
//             >
//               {validarOrden.isPending ? <ShieldCheck className="w-5 h-5 animate-pulse" /> : <FileCheck2 className="w-5 h-5" />}
//               {orden.estadoOrden === 'Validado' ? 'ORDEN FIRMADA' : 'VALIDAR Y FIRMAR ORDEN'}
//             </button>
//           </div>
//         }
//       />

//       <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm flex justify-between items-center">
//         <div>
//           <h2 className="text-xl font-black text-[#15243b]">{orden.paciente}</h2>
//           <p className="text-sm text-[#64768b]">C.I: {orden.cedula || 'N/A'} • Orden generada el {new Date(orden.fechaCreacion).toLocaleDateString('es-ES')}</p>
//         </div>
//         <div className="text-right">
//           <p className="text-xs font-bold text-[#94a3b8] uppercase mb-1">Estado General</p>
//           <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-sm font-bold border bg-blue-50 text-blue-700 border-blue-200">
//             {orden.estadoOrden === 'Validado' && <CheckCircle className="w-4 h-4" />}
//             {orden.estadoOrden}
//           </span>
//         </div>
//       </div>

//       {orden.especialidades?.map((grupo, idx) => (
//         <div key={idx} className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden">
//           <div className="px-6 py-4 border-b border-[#dce5ef] bg-[#f8fafc]">
//             <h3 className="text-sm font-bold text-[#2a79b7] uppercase tracking-wider">
//               {grupo.nombreEspecialidad}
//             </h3>
//           </div>
          
//           <div className="overflow-x-auto">
//             <table className="w-full text-left">
//               <thead className="text-xs text-[#64768b] uppercase border-b border-[#dce5ef] bg-white">
//                 <tr>
//                   <th className="px-6 py-3 font-bold w-[35%]">Examen / Parámetro</th>
//                   <th className="px-4 py-3 font-bold w-[20%]">Resultado Final</th>
//                   <th className="px-4 py-3 font-bold w-[15%]">Unidad</th>
//                   <th className="px-4 py-3 font-bold w-[20%]">Rango de Referencia</th>
//                   <th className="px-4 py-3 font-bold w-[10%] text-center">Estado</th>
//                 </tr>
//               </thead>
//               <tbody>
//                 {grupo.examenes?.map((item) => (
//                   <ResultDetailsRow 
//                     key={item.ordenDetalleId}
//                     nodo={item.arbol}
//                   />
//                 ))}
//               </tbody>
//             </table>
//           </div>
//         </div>
//       ))}

//     </div>
//   );
// }









import { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, ShieldCheck, FileCheck2, X } from 'lucide-react';
import PageHeader from '../../../components/common/PageHeader';
import ResultDetailsRow from '../components/ResultDetailsRow';
import PrintResultButton from '../components/PrintResultButton';
import { useResultadosPorOrden } from '../hooks/useResult';
import { useResultMutations } from '../hooks/useResultMutations';
import type { EspecialidadGrupo } from '../types/result.types';

export default function ResultDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [showModal, setShowModal] = useState(false);
  
  const { data: orden, isLoading, isError } = useResultadosPorOrden(id);
  const { validarOrden } = useResultMutations();

  if (isLoading) return <div className="p-8 text-center text-[#64768b]">Cargando vista de validación...</div>;
  if (isError || !orden) return <div className="p-8 text-center text-red-500 bg-red-50 rounded-lg">Error al cargar la orden.</div>;

  const numOrdenStr = orden.numeroOrden ? orden.numeroOrden.toString().padStart(6, '0') : '000000';
  const nombreArchivo = `RESULTADO ORD-${numOrdenStr} ${orden.paciente}`.toUpperCase();

  // NUEVA LÓGICA: ¿Hay al menos un examen validado en todo el árbol?
  const tieneValidados = orden.especialidades?.some((grupo: EspecialidadGrupo) => 
    grupo.examenes.some((detalle) => 
      JSON.stringify(detalle.arbol).includes('"estado":"Validado"')
    )
  );

  const handleValidar = () => {
    validarOrden.mutate(id!, {
      onSuccess: () => {
        setShowModal(false);
      }
    });
  };

  return (
    <div className="space-y-6 animate-in fade-in duration-300 max-w-6xl mx-auto relative">
      
      {/* Modal Profesional de Confirmación */}
      {showModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#0f172a]/40 backdrop-blur-sm">
          <div className="bg-white rounded-xl shadow-2xl max-w-md w-full p-6 animate-in zoom-in-95 duration-200">
            <div className="flex items-start justify-between mb-4">
              <div className="p-3 bg-blue-50 text-blue-600 rounded-full">
                <ShieldCheck className="w-6 h-6" />
              </div>
              <button onClick={() => setShowModal(false)} className="text-[#94a3b8] hover:text-[#15243b]">
                <X className="w-5 h-5" />
              </button>
            </div>
            <h3 className="text-lg font-black text-[#15243b] mb-2">Firma y Validación Oficial</h3>
            <p className="text-sm text-[#4e6681] mb-6">
              Estás a punto de emitir y firmar electrónicamente los resultados procesados de <strong>{orden.paciente}</strong>. Esta acción genera el reporte final y sella los registros por motivos legales.
            </p>
            <div className="flex items-center gap-3 justify-end">
              <button onClick={() => setShowModal(false)} className="px-4 py-2 text-sm font-bold text-[#64768b] hover:bg-[#f8fafc] rounded-lg transition-colors">
                Cancelar
              </button>
              <button 
                onClick={handleValidar} 
                disabled={validarOrden.isPending}
                className="flex items-center gap-2 px-6 py-2 bg-green-600 text-white text-sm font-bold rounded-lg hover:bg-green-700 transition-colors"
              >
                {validarOrden.isPending ? 'Procesando firma...' : 'Firmar y Emitir Reporte'}
              </button>
            </div>
          </div>
        </div>
      )}

      <div className="flex items-center justify-between mb-2">
        <button onClick={() => navigate(-1)} className="flex items-center gap-2 text-sm font-medium text-[#64768b] hover:text-[#2a79b7] transition-colors">
          <ArrowLeft className="w-4 h-4" /> Volver
        </button>
      </div>

      <PageHeader 
        title="Validación Oficial de Resultados" 
        description="Revisión clínica de los valores procesados."
        action={
          <div className="flex items-center gap-3">
            
            {/* El botón solo se renderiza si tieneValidados es TRUE */}
            {tieneValidados && (
              <PrintResultButton 
                ordenId={id!} 
                nombreArchivo={nombreArchivo}
              />
            )}

            <button
              onClick={() => setShowModal(true)}
              disabled={validarOrden.isPending || orden.estadoOrden === 'Validado'}
              className={`flex items-center gap-2 px-6 py-2.5 text-white rounded-lg transition-colors font-bold shadow-sm disabled:opacity-50 ${orden.estadoOrden === 'Validado' ? 'bg-blue-600' : 'bg-green-600 hover:bg-green-700'}`}
            >
              <FileCheck2 className="w-5 h-5" />
              {orden.estadoOrden === 'Validado' ? 'ORDEN FIRMADA Y SELLADA' : 'VALIDAR Y FIRMAR'}
            </button>
          </div>
        }
      />

      {/* Resto del código de la tabla que ya tienes... */}
      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm flex justify-between items-center">
        <div>
          <h2 className="text-xl font-black text-[#15243b]">{orden.paciente}</h2>
          <p className="text-sm text-[#64768b]">C.I: {orden.cedula || 'N/A'} • Orden: #ORD-{numOrdenStr}</p>
        </div>
      </div>

      {orden.especialidades?.map((grupo, idx) => (
        <div key={idx} className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden mb-6">
          <div className="px-6 py-4 border-b border-[#dce5ef] bg-[#f8fafc]">
            <h3 className="text-sm font-bold text-[#2a79b7] uppercase tracking-wider">{grupo.nombreEspecialidad}</h3>
          </div>
          <table className="w-full text-left">
             <thead className="text-xs text-[#64768b] uppercase border-b border-[#dce5ef] bg-white">
                <tr>
                  <th className="px-6 py-3 font-bold w-[35%]">Examen / Parámetro</th>
                  <th className="px-4 py-3 font-bold w-[20%]">Resultado Final</th>
                  <th className="px-4 py-3 font-bold w-[15%]">Unidad</th>
                  <th className="px-4 py-3 font-bold w-[20%]">Rango</th>
                  <th className="px-4 py-3 font-bold w-[10%] text-center">Estado</th>
                </tr>
              </thead>
              <tbody>
                {grupo.examenes?.map((item) => (
                  <ResultDetailsRow key={item.ordenDetalleId} nodo={item.arbol} />
                ))}
              </tbody>
          </table>
        </div>
      ))}
    </div>
  );
}