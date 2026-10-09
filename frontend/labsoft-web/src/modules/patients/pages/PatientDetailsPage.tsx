// import React from 'react';
// import { useParams, useNavigate } from 'react-router-dom';
// import { ArrowLeft, User, Calendar, Phone, Activity, FileText } from 'lucide-react';
// import { ROUTES } from '../../../routes/routeConfig';
// import { usePatient } from '../hooks/usePatient';
// import { usePatientHistory } from '../hooks/usePatientHistory';

// export default function PatientDetailsPage() {
//   const { id } = useParams<{ id: string }>();
//   const navigate = useNavigate();

//   const { data: patient, isLoading: isLoadingPatient } = usePatient(id);
//   const { data: history, isLoading: isLoadingHistory } = usePatientHistory(id);

//   if (isLoadingPatient || isLoadingHistory) {
//     return <div className="p-8 text-center text-[#64768b]">Cargando expediente clínico...</div>;
//   }

//   if (!patient) {
//     return (
//       <div className="p-8 text-center text-red-500">
//         No se encontró el paciente. <br />
//         <button onClick={() => navigate(ROUTES.PATIENTS)} className="text-[#2a79b7] underline mt-2">Volver</button>
//       </div>
//     );
//   }

//   return (
//     <div className="max-w-6xl mx-auto space-y-6">
//       {/* Cabecera y Navegación */}
//       <div className="flex items-center gap-4">
//         <button
//           onClick={() => navigate(ROUTES.PATIENTS)}
//           className="p-2 text-[#64768b] hover:text-[#15243b] hover:bg-[#eef5fa] rounded-lg transition-colors"
//         >
//           <ArrowLeft className="w-5 h-5" />
//         </button>
//         <div>
//           <h1 className="text-2xl font-bold text-[#15243b]">Expediente Clínico</h1>
//           <p className="text-[#64768b] mt-1">Historial detallado y datos del paciente.</p>
//         </div>
//       </div>

//       <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
//         {/* Columna Izquierda: Tarjeta de Resumen del Paciente */}
//         <div className="lg:col-span-1 space-y-6">
//           <div className="bg-white border border-[#dce5ef] rounded-xl shadow-sm p-6">
//             <div className="flex items-center gap-4 mb-6 pb-6 border-b border-[#dce5ef]">
//               <div className="w-16 h-16 bg-[#eef5fa] rounded-full flex items-center justify-center text-[#2a79b7]">
//                 <User className="w-8 h-8" />
//               </div>
//               <div>
//                 <h2 className="text-xl font-bold text-[#15243b]">{patient.nombre} {patient.apellido}</h2>
//                 <p className="text-sm text-[#64768b]">C.I: {patient.cedula || 'N/A'}</p>
//                 {patient.esRecienNacido && (
//                   <span className="inline-block mt-1 text-[10px] uppercase tracking-wider font-bold bg-purple-100 text-purple-700 px-2 py-0.5 rounded">
//                     Recién Nacido
//                   </span>
//                 )}
//               </div>
//             </div>

//             <div className="space-y-4">
//               <div className="flex items-start gap-3">
//                 <Calendar className="w-5 h-5 text-[#94a3b8] mt-0.5" />
//                 <div>
//                   <p className="text-xs font-medium text-[#64768b] uppercase">Nacimiento</p>
//                   <p className="text-sm text-[#15243b]">{new Date(patient.fechaNacimiento).toLocaleDateString('es-VE', { timeZone: 'UTC' })}</p>
//                   <p className="text-sm text-[#15243b]">({patient.sexo})</p>
//                 </div>
//               </div>
              
//               <div className="flex items-start gap-3">
//                 <Phone className="w-5 h-5 text-[#94a3b8] mt-0.5" />
//                 <div>
//                   <p className="text-xs font-medium text-[#64768b] uppercase">Contacto</p>
//                   <p className="text-sm text-[#15243b]">{patient.telefono || 'Sin teléfono'}</p>
//                   <p className="text-sm text-[#15243b]">{patient.correo || 'Sin correo'}</p>
//                 </div>
//               </div>

//               {patient.notaMedica && (
//                 <div className="flex items-start gap-3 pt-4 border-t border-[#dce5ef]">
//                   <Activity className="w-5 h-5 text-red-400 mt-0.5" />
//                   <div>
//                     <p className="text-xs font-medium text-red-500 uppercase">Nota Médica / Alergias</p>
//                     <p className="text-sm text-[#15243b] mt-1">{patient.notaMedica}</p>
//                   </div>
//                 </div>
//               )}
//             </div>
//           </div>
//         </div>

//         {/* Columna Derecha: Historial Clínico (Órdenes y Resultados) */}
//         <div className="lg:col-span-2">
//           <div className="bg-white border border-[#dce5ef] rounded-xl shadow-sm overflow-hidden">
//             <div className="p-4 border-b border-[#dce5ef] bg-[#f8fafc] flex justify-between items-center">
//               <h3 className="font-semibold text-[#15243b] flex items-center gap-2">
//                 <FileText className="w-5 h-5 text-[#2a79b7]" />
//                 Historial de Órdenes ({history?.length || 0})
//               </h3>
//             </div>
            
//             <div className="p-6">
//               {!history || history.length === 0 ? (
//                 <div className="text-center py-8 text-[#64768b]">
//                   Este paciente aún no tiene historial de resultados procesados.
//                 </div>
//               ) : (
//                 <div className="space-y-8">
//                   {history.map((orden) => (
//                     <div key={orden.ordenId} className="relative pl-6 border-l-2 border-[#dce5ef]">
//                       <div className="absolute -left-1.5 top-0 w-3 h-3 bg-[#2a79b7] rounded-full border-4 border-white"></div>
                      
//                       <div className="mb-2">
//                         <span className="text-xs font-bold text-[#64768b]">
//                           {new Date(orden.fechaCreacion).toLocaleDateString()}
//                         </span>
//                         <h4 className="text-lg font-semibold text-[#15243b]">Orden #{orden.numeroOrden}</h4>
//                       </div>

//                       <div className="space-y-4 mt-4">
//                         {orden.especialidades.map((esp, idx) => (
//                           <div key={idx} className="bg-[#f8fafc] border border-[#dce5ef] rounded-lg p-4">
//                             <h5 className="font-semibold text-[#2a79b7] mb-3 text-sm uppercase tracking-wide">
//                               {esp.nombreEspecialidad}
//                             </h5>
                            
//                             <table className="w-full text-sm text-left">
//                               <thead className="text-[#64768b] border-b border-[#dce5ef]">
//                                 <tr>
//                                   <th className="pb-2 font-medium">Examen</th>
//                                   <th className="pb-2 font-medium">Resultado</th>
//                                   <th className="pb-2 font-medium">Ref.</th>
//                                 </tr>
//                               </thead>
//                               <tbody className="divide-y divide-[#dce5ef]">
//                                 {esp.examenes.map((examen, eIdx) => (
//                                   <React.Fragment key={eIdx}>
//                                     {/* Nivel 1: Perfil/Panel Padre (Ej. HEMATOLOGIA COMPLETA) */}
//                                     <tr className={examen.subExamenes?.length ? "bg-[#f3f7fb]" : ""}>
//                                       <td className="py-2 text-[#15243b] font-medium">{examen.examenNombre}</td>
//                                       <td className="py-2 font-medium text-[#15243b]">
//                                         {examen.valor} <span className="text-xs text-[#64768b]">{examen.unidad}</span>
//                                       </td>
//                                       <td className="py-2 text-xs text-[#64768b]">{examen.valoresReferencia}</td>
//                                     </tr>
                                    
//                                     {/* Nivel 2: Exámenes Hijos (Ej. Hemoglobina, Formula Leucocitaria) */}
//                                     {examen.subExamenes && examen.subExamenes.map((sub, sIdx) => (
//                                       <React.Fragment key={`${eIdx}-${sIdx}`}>
//                                         <tr className="bg-white">
//                                           <td className="py-1.5 pl-6 text-[#4e6681] text-sm flex items-center gap-2">
//                                             <div className="w-1.5 h-1.5 bg-[#94a3b8] rounded-full"></div>
//                                             {sub.examenNombre}
//                                           </td>
//                                           <td className="py-1.5 text-sm font-medium text-[#15243b]">
//                                             {sub.valor} <span className="text-xs text-[#64768b]">{sub.unidad}</span>
//                                           </td>
//                                           <td className="py-1.5 text-xs text-[#64768b]">{sub.valoresReferencia}</td>
//                                         </tr>

//                                         {/* Nivel 3: Sub-exámenes Nietos (Ej. Neutrófilos dentro de Fórmula Leucocitaria) */}
//                                         {sub.subExamenes && sub.subExamenes.map((nieto, nIdx) => (
//                                           <tr key={`${eIdx}-${sIdx}-${nIdx}`} className="bg-white">
//                                             <td className="py-1 pl-12 text-[#64768b] text-xs flex items-center gap-2">
//                                               <div className="w-1 h-1 bg-[#cbd5e1] rounded-full"></div>
//                                               {nieto.examenNombre}
//                                             </td>
//                                             <td className="py-1 text-xs font-medium text-[#4e6681]">
//                                               {nieto.valor} <span className="text-[10px] text-[#94a3b8]">{nieto.unidad}</span>
//                                             </td>
//                                             <td className="py-1 text-[10px] text-[#94a3b8]">{nieto.valoresReferencia}</td>
//                                           </tr>
//                                         ))}
//                                       </React.Fragment>
//                                     ))}
//                                   </React.Fragment>
//                                 ))}
//                               </tbody>
//                             </table>
//                           </div>
//                         ))}
//                       </div>
//                     </div>
//                   ))}
//                 </div>
//               )}
//             </div>
//           </div>
//         </div>
//       </div>
//     </div>
//   );
// }




import { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, User, Calendar, Phone, Activity, FileText, Eye } from 'lucide-react';
import { ROUTES } from '../../../routes/routeConfig';
import { usePatient } from '../hooks/usePatient';
import { usePatientHistory } from '../hooks/usePatientHistory';
import type { HistorialPaciente } from '../types/patient.types';
import PdfViewerModal from '../components/PdfViewerModal';

export default function PatientDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [selectedPdf, setSelectedPdf] = useState<{ ordenId: string; numeroOrden: string, nombrePaciente: string, fechaOrden: string } | null>(null);

  const { data: patient, isLoading: isLoadingPatient } = usePatient(id);
  const { data: history, isLoading: isLoadingHistory } = usePatientHistory(id);

  if (isLoadingPatient || isLoadingHistory) {
    return <div className="p-8 text-center text-[#64768b]">Cargando expediente clínico...</div>;
  }

  if (!patient) {
    return (
      <div className="p-8 text-center text-red-500">
        No se encontró el paciente. <br />
        <button onClick={() => navigate(ROUTES.PATIENTS)} className="text-[#2a79b7] underline mt-2">Volver</button>
      </div>
    );
  }

  return (
    <div className="max-w-6xl mx-auto space-y-6">
      {/* Cabecera */}
      <div className="flex items-center gap-4">
        <button
          onClick={() => navigate(ROUTES.PATIENTS)}
          className="p-2 text-[#64768b] hover:text-[#15243b] hover:bg-[#eef5fa] rounded-lg transition-colors"
        >
          <ArrowLeft className="w-5 h-5" />
        </button>
        <div>
          <h1 className="text-2xl font-bold text-[#15243b]">Expediente Clínico</h1>
          <p className="text-[#64768b] mt-1">Historial simplificado y datos del paciente.</p>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Columna Izquierda: Resumen del Paciente */}
        <div className="lg:col-span-1 space-y-6">
          <div className="bg-white border border-[#dce5ef] rounded-xl shadow-sm p-6">
            <div className="flex items-center gap-4 mb-6 pb-6 border-b border-[#dce5ef]">
              <div className="w-16 h-16 bg-[#eef5fa] rounded-full flex items-center justify-center text-[#2a79b7]">
                <User className="w-8 h-8" />
              </div>
              <div>
                <h2 className="text-xl font-bold text-[#15243b]">
              {patient?.nombre} {patient?.apellido}
            </h2>
            <p className="text-sm text-[#64768b]">C.I: {patient?.cedula || 'N/A'}</p>

            {patient.esRecienNacido && (
                    <div className="mt-1">
                      <span className={`inline-block text-[10px] uppercase tracking-wider font-bold px-2 py-0.5 rounded ${
                        patient.edadFormateada?.includes('meses') || patient.edadFormateada?.includes('días')  
                          ? 'bg-purple-100 text-purple-700' 
                          : 'bg-orange-100 text-orange-700'
                      }`}>
                        {patient.edadFormateada?.includes('meses') || patient.edadFormateada?.includes('días') 
                          ? 'Recién Nacido' 
                          : 'Menor sin ID'}
                      </span>
                    </div>
                  )}

            {/* Añade la edad formateada aquí */}
             <div className="mt-4 flex items-center gap-2 text-sm text-[#4e6681]">
                <Calendar className="w-4 h-4 text-[#94a3b8]" />
                {patient?.edadFormateada} ({patient?.sexo})
             </div>

            {/* Añade el titular aquí */}
            {patient?.titularNombre && (
              <div className="mt-3 pt-3 border-t border-[#f1f5f9]">
                <p className="text-xs font-bold text-[#2a79b7] uppercase mb-1">Representante Legal</p>
                <p className="text-sm text-[#15243b]">{patient.titularNombre}</p>
                <p className="text-xs text-[#64768b]">C.I: {patient.titularCedula || 'N/A'}</p>
              </div>
            )}
              </div>
            </div>

            <div className="space-y-4">
              <div className="flex items-start gap-3">
                <Calendar className="w-5 h-5 text-[#94a3b8] mt-0.5" />
                <div>
                  <p className="text-xs font-medium text-[#64768b] uppercase">Nacimiento</p>
                  <p className="text-sm text-[#15243b]">{new Date(patient.fechaNacimiento).toLocaleDateString('es-VE', { timeZone: 'UTC' })} ({patient.sexo})</p>
                </div>
              </div>
              
              <div className="flex items-start gap-3">
                <Phone className="w-5 h-5 text-[#94a3b8] mt-0.5" />
                <div>
                  <p className="text-xs font-medium text-[#64768b] uppercase">Contacto</p>
                  <p className="text-sm text-[#15243b]">{patient.telefono || 'Sin teléfono'}</p>
                  <p className="text-sm text-[#15243b]">{patient.correo || 'Sin correo'}</p>
                </div>
              </div>

              {patient.notaMedica && (
                <div className="flex items-start gap-3 pt-4 border-t border-[#dce5ef]">
                  <Activity className="w-5 h-5 text-red-400 mt-0.5" />
                  <div>
                    <p className="text-xs font-medium text-red-500 uppercase">Nota Médica / Alergias</p>
                    <p className="text-sm text-[#15243b] mt-1">{patient.notaMedica}</p>
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>

        {/* Columna Derecha: Tarjetas de Órdenes e Historial */}
        <div className="lg:col-span-2">
          <div className="bg-white border border-[#dce5ef] rounded-xl shadow-sm overflow-hidden">
            <div className="p-4 border-b border-[#dce5ef] bg-[#f8fafc] flex justify-between items-center">
              <h3 className="font-semibold text-[#15243b] flex items-center gap-2">
                <FileText className="w-5 h-5 text-[#2a79b7]" />
                Historial de Órdenes y Resultados ({history?.length || 0})
              </h3>
            </div>
            
            <div className="p-6 space-y-4">
              {!history || history.length === 0 ? (
                <div className="text-center py-8 text-[#64768b]">
                  Este paciente aún no registra órdenes procesadas.
                </div>
              ) : (
                history.map((orden: HistorialPaciente) => (
                  <div key={orden.ordenId} className="border border-[#dce5ef] rounded-xl p-5 hover:border-[#2a79b7] transition-all bg-[#fcfdff] flex flex-col gap-4">
                    <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
                      <div className="space-y-1 flex-1">
                        <div className="flex items-center gap-3">
                          <span className="text-sm font-bold text-[#2a79b7]">Orden #{orden.numeroOrden}</span>
                          <span className="text-xs text-[#64768b] bg-[#eef5fa] px-2 py-0.5 rounded-full">
                            {new Date(orden.fechaCreacion).toLocaleDateString('es-VE', { timeZone: 'UTC' })}
                          </span>
                        </div>
                        <div className="flex flex-wrap gap-1.5 pt-2">
                          {orden.especialidades?.map((esp: { nombreEspecialidad: string }, idx: number) => (
                            <span key={idx} className="text-xs font-medium bg-gray-100 text-[#4e6681] px-2.5 py-1 rounded-md border border-gray-200">
                              {esp.nombreEspecialidad}
                            </span>
                          ))}
                        </div>
                      </div>

                      <button
                        onClick={() => setSelectedPdf({
                          ordenId: orden.ordenId,
                          numeroOrden: orden.numeroOrden,
                          nombrePaciente: `${patient.nombre} ${patient.apellido}`,
                          fechaOrden: new Date(orden.fechaCreacion).toLocaleDateString()
                        })}
                        className="flex items-center gap-2 px-4 py-2 bg-[#eef5fa] text-[#2a79b7] hover:bg-[#2a79b7] hover:text-white rounded-lg text-sm font-medium transition-colors whitespace-nowrap self-end md:self-center"
                      >
                        <Eye className="w-4 h-4" />
                        Ver Resultados
                      </button>
                    </div>
                  </div>
                ))
              )}
            </div>
          </div>
        </div>
      </div>

      {/* Modal Visor de PDF */}
      {selectedPdf && (
        <PdfViewerModal
          isOpen={!!selectedPdf}
          onClose={() => setSelectedPdf(null)}
          ordenId={selectedPdf.ordenId}
          numeroOrden={selectedPdf.numeroOrden}
          nombrePaciente={selectedPdf.nombrePaciente}
          fechaOrden={selectedPdf.fechaOrden}
        />
      )}
    </div>
  );
}