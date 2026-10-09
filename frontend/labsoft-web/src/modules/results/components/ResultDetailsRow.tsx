// import { ChevronRight, AlertTriangle, FileText } from 'lucide-react';
// import type { NodoExamen } from '../types/result.types';
// import { isOutOfRange } from '../../../utils/validation';

// interface ResultDetailsRowProps {
//   nodo: NodoExamen;
//   nivel?: number;
// }

// export default function ResultDetailsRow({ nodo, nivel = 0 }: ResultDetailsRowProps) {
//   const isPadre = nodo.subExamenes && nodo.subExamenes.length > 0;
//   const paddingIzquierdo = `${nivel * 2 + 1}rem`;
  
//   // Evaluamos si el resultado se sale de los parámetros de referencia
//   const hasPanicValue = isOutOfRange(nodo.valorResultado, nodo.valoresReferencia);
//   const isPendiente = !nodo.valorResultado && !isPadre;

//   return (
//     <>
//       <tr className={`border-b border-[#dce5ef] ${isPadre ? 'bg-[#f8fafc]' : 'hover:bg-[#f3f7fb]'} transition-colors`}>
        
//         {/* Nombre del Examen */}
//         <td className="py-3 pr-4" style={{ paddingLeft: paddingIzquierdo }}>
//           <div className="flex items-center gap-2">
//             {isPadre && <ChevronRight className="w-4 h-4 text-[#94a3b8]" />}
//             <span className={`${isPadre ? 'font-bold text-[#15243b]' : 'font-medium text-[#4e6681]'} text-sm`}>
//               {nodo.nombre}
//             </span>
//             {nodo.notaClinica && (
//                 <span title="Contiene nota clínica">
//                     <FileText className="w-3 h-3 text-[#2a79b7]" />
//                 </span>
//             )}
//           </div>
//         </td>

//         {/* Valor del Resultado (Con alerta de pánico) */}
//         <td className="px-4 py-3 font-medium">
//           {!isPadre ? (
//             isPendiente ? (
//               <span className="text-xs text-[#94a3b8] italic">Sin procesar</span>
//             ) : (
//               <div className={`flex items-center gap-1.5 ${hasPanicValue ? 'text-red-600 font-bold' : 'text-[#15243b]'}`}>
//                 {hasPanicValue && <AlertTriangle className="w-3 h-3" />}
//                 {nodo.valorResultado}
//                 {hasPanicValue && '*'}
//               </div>
//             )
//           ) : null}
//         </td>

//         <td className="px-4 py-3 text-sm text-[#64768b]">
//           {!isPadre && nodo.unidad}
//         </td>

//         <td className="px-4 py-3 text-sm text-[#64768b] whitespace-pre-line">
//           {!isPadre && nodo.valoresReferencia}
//         </td>

//         <td className="px-4 py-3 text-center">
//           <span className={`inline-flex items-center px-2 py-0.5 rounded text-[10px] font-bold uppercase tracking-wider ${
//             nodo.estado === 'Validado' ? 'bg-blue-100 text-blue-700' :
//             nodo.estado === 'Procesado' ? 'bg-green-100 text-green-700' :
//             'bg-yellow-50 text-yellow-700 border border-yellow-200'
//           }`}>
//             {nodo.estado}
//           </span>
//         </td>
//       </tr>

//       {/* Si hay una nota clínica que el bioanalista marcó para imprimir, la mostramos aquí */}
//       {nodo.notaClinica && nodo.imprimirObservacion && !isPadre && (
//         <tr className="bg-blue-50/50 border-b border-[#dce5ef]">
//           <td colSpan={5} className="py-2 pr-4 text-xs text-[#4e6681]" style={{ paddingLeft: `calc(${paddingIzquierdo} + 1.5rem)` }}>
//             <span className="font-bold text-[#2a79b7]">Observación: </span> 
//             {nodo.notaClinica}
//           </td>
//         </tr>
//       )}

//       {/* Recursividad para los hijos */}
//       {isPadre && nodo.subExamenes.map((subNodo) => (
//         <ResultDetailsRow 
//           key={subNodo.examenId} 
//           nodo={subNodo} 
//           nivel={nivel + 1} 
//         />
//       ))}
//     </>
//   );
// }









import { useState } from 'react';
import { Lock, AlertOctagon, X, Loader2, ChevronRight, AlertTriangle, FileText } from 'lucide-react';
import { useResultMutations } from '../hooks/useResultMutations';
import ResultStatusBadge from './ResultStatusBadge';
import { isOutOfRange } from '../../../utils/validation';

export interface NodoExamen {
  resultadoId?: string;
  examenId?: string;
  nombre: string;
  valorResultado?: string; // Corregido para mapear la API real
  unidad?: string;
  valoresReferencia?: string;
  estado: string;
  notaClinica?: string;
  imprimirObservacion?: boolean;
  subExamenes?: NodoExamen[];
}

interface ResultDetailsRowProps {
  nodo: NodoExamen;
  nivel?: number;
}

export default function ResultDetailsRow({ nodo, nivel = 0 }: ResultDetailsRowProps) {
  const [showModal, setShowModal] = useState(false);
  const [motivo, setMotivo] = useState('');
  const [errorMotivo, setErrorMotivo] = useState('');
  
  const { invalidarResultado } = useResultMutations();

  const isPadre = nodo.subExamenes && nodo.subExamenes.length > 0;
  const paddingIzquierdo = `${nivel * 2 + 1}rem`;
  
  // Evaluamos pánico y pendientes
  const hasPanicValue = isOutOfRange(nodo.valorResultado, nodo.valoresReferencia);
  const isPendiente = !nodo.valorResultado && !isPadre;

  const handleInvalidar = () => {
    if (motivo.trim().length < 10) {
      setErrorMotivo('El motivo debe tener al menos 10 caracteres por motivos de auditoría.');
      return;
    }
    
    invalidarResultado.mutate(
      { id: nodo.resultadoId!, motivo },
      {
        onSuccess: () => {
          setShowModal(false);
          setMotivo('');
          setErrorMotivo('');
          // Forzamos la recarga de la pantalla para ver el efecto dominó desde la base de datos
          window.location.reload(); 
        },
        onError: () => {
          setErrorMotivo('Ocurrió un error al intentar invalidar el resultado.');
        }
      }
    );
  };

  return (
    <>
      {/* Modal Legal de Invalidación */}
      {showModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#0f172a]/60 backdrop-blur-sm">
          <div className="bg-white rounded-xl shadow-2xl max-w-md w-full p-6 animate-in zoom-in-95 duration-200">
            
            {/* Cabecera del Modal alineada */}
            <div className="flex items-center justify-between mb-4">
              <div className="flex items-center gap-3">
                <div className="p-2.5 bg-red-50 text-red-600 rounded-full shrink-0">
                  <AlertOctagon className="w-6 h-6" />
                </div>
                <h3 className="text-xl font-black text-[#15243b] leading-none">Invalidar Resultado</h3>
              </div>
              <button onClick={() => setShowModal(false)} className="text-[#94a3b8] hover:text-[#15243b] -mt-2">
                <X className="w-5 h-5" />
              </button>
            </div>
            
            <p className="text-sm text-[#4e6681] mb-4">
              Estás a punto de abrir para corrección el parámetro <strong>{nodo.nombre}</strong> que ya estaba firmado. 
              Esta acción quedará registrada permanentemente en la auditoría del sistema.
            </p>
            
            <div className="mb-6">
              <label className="block text-xs font-bold text-[#64768b] uppercase tracking-wider mb-2">
                Motivo de la corrección (Obligatorio)
              </label>
              <textarea 
                rows={3}
                value={motivo}
                onChange={(e) => { setMotivo(e.target.value); setErrorMotivo(''); }}
                placeholder="Ej: Error tipográfico en la transcripción manual..."
                className="w-full px-4 py-3 bg-[#f8fafc] border border-[#dce5ef] rounded-lg text-sm text-[#15243b] focus:border-red-500 focus:ring-1 focus:ring-red-500 resize-none transition-all"
              />
              {errorMotivo && <p className="text-xs text-red-600 mt-2 font-medium">{errorMotivo}</p>}
            </div>

            <div className="flex items-center gap-3 justify-end">
              <button 
                onClick={() => setShowModal(false)} 
                className="px-4 py-2 text-sm font-bold text-[#64768b] hover:bg-[#f8fafc] rounded-lg transition-colors"
              >
                Cancelar
              </button>
              <button 
                onClick={handleInvalidar} 
                disabled={invalidarResultado.isPending}
                className="flex items-center gap-2 px-6 py-2 bg-red-600 text-white text-sm font-bold rounded-lg hover:bg-red-700 transition-colors disabled:opacity-50 shadow-sm"
              >
                {invalidarResultado.isPending ? <Loader2 className="w-4 h-4 animate-spin" /> : <Lock className="w-4 h-4" />}
                Confirmar Invalidez
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Fila Principal de la Tabla */}
      <tr className={`border-b border-[#dce5ef] ${isPadre ? 'bg-[#f8fafc]' : 'hover:bg-[#f3f7fb]'} transition-colors`}>
        
        <td className="py-3 pr-4" style={{ paddingLeft: paddingIzquierdo }}>
          <div className="flex items-center gap-2">
            {isPadre && <ChevronRight className="w-4 h-4 text-[#94a3b8]" />}
            <span className={`${isPadre ? 'font-bold text-[#15243b]' : 'font-medium text-[#4e6681]'} text-sm`}>
              {nodo.nombre}
            </span>
            {nodo.notaClinica && (
              <span title="Contiene nota clínica">
                <FileText className="w-3 h-3 text-[#2a79b7]" />
              </span>
            )}
          </div>
        </td>

        <td className="px-4 py-3 font-medium">
          {!isPadre ? (
            isPendiente ? (
              <span className="text-xs text-[#94a3b8] italic">Sin procesar</span>
            ) : (
              <div className={`flex items-center gap-1.5 ${hasPanicValue ? 'text-red-600 font-bold' : 'text-[#15243b]'}`}>
                {hasPanicValue && <AlertTriangle className="w-3 h-3" />}
                {nodo.valorResultado}
                {hasPanicValue && '*'}
              </div>
            )
          ) : null}
        </td>

        <td className="px-4 py-3 text-sm text-[#64768b]">
          {!isPadre && nodo.unidad}
        </td>

        <td className="px-4 py-3 text-sm text-[#64768b] whitespace-pre-line">
          {!isPadre && nodo.valoresReferencia}
        </td>

        <td className="px-4 py-3 text-center">
          <div className="flex items-center justify-center gap-2">
            <ResultStatusBadge estado={nodo.estado} />
            
            {/* El botón rojo con Candado CERRADO */}
            {nodo.estado === 'Validado' && nodo.resultadoId && !isPadre && (
              <button 
                onClick={() => setShowModal(true)}
                title="Desbloquear y Corregir"
                className="p-1.5 text-[#94a3b8] hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors"
              >
                <Lock className="w-4 h-4" />
              </button>
            )}
          </div>
        </td>
      </tr>

      {/* Fila de Observación Clínica */}
      {nodo.notaClinica && nodo.imprimirObservacion && !isPadre && (
        <tr className="bg-blue-50/50 border-b border-[#dce5ef]">
          <td colSpan={5} className="py-2 pr-4 text-xs text-[#4e6681]" style={{ paddingLeft: `calc(${paddingIzquierdo} + 1.5rem)` }}>
            <span className="font-bold text-[#2a79b7]">Observación: </span> 
            {nodo.notaClinica}
          </td>
        </tr>
      )}

      {/* Renderizado Recursivo para SubExámenes */}
      {nodo.subExamenes?.map((sub: NodoExamen, index: number) => (
        <ResultDetailsRow key={sub.examenId || index} nodo={sub} nivel={nivel + 1} />
      ))}
    </>
  );
}