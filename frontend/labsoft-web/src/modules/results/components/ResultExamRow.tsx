// import { useFormContext } from 'react-hook-form';
// import type { NodoExamen } from '../types/result.types';
// import type { ProcessResultFormValues } from '../schemas/resultSchema';
// import { ChevronRight } from 'lucide-react';

// interface ResultExamRowProps {
//   nodo: NodoExamen;
//   detalleId: string;
//   nivel?: number; // Controla la indentación visual (Padre = 0, Hijo = 1, Nieto = 2)
// }

// export default function ResultExamRow({ nodo, detalleId, nivel = 0 }: ResultExamRowProps) {
//   // Usamos useFormContext para no tener que pasar 'register' de padre a hijo infinitamente
//   const { register } = useFormContext<ProcessResultFormValues>();

//   const isPadre = nodo.subExamenes && nodo.subExamenes.length > 0;
//   const paddingIzquierdo = `${nivel * 2 + 1}rem`; // Aumenta 2rem por cada nivel de profundidad

//   return (
//     <>
//       {/* 1. RENDERIZAMOS LA FILA ACTUAL */}
//       <tr className={`border-b border-[#dce5ef] ${isPadre ? 'bg-[#f8fafc]' : 'hover:bg-[#f3f7fb]'} transition-colors`}>
        
//         {/* Nombre del Examen con Indentación */}
//         <td className="py-3 pr-4" style={{ paddingLeft: paddingIzquierdo }}>
//           <div className="flex items-center gap-2">
//             {isPadre && <ChevronRight className="w-4 h-4 text-[#94a3b8]" />}
//             <span className={`${isPadre ? 'font-bold text-[#15243b]' : 'font-medium text-[#4e6681]'} text-sm`}>
//               {nodo.nombre}
//             </span>
//           </div>
//         </td>

//         {/* Input del Resultado (Solo si NO es un padre agrupador) */}
//         <td className="px-4 py-3">
//           {!isPadre ? (
//             <input
//               type="text"
//               autoComplete="off"
//               placeholder="Ingresar valor..."
//               defaultValue={nodo.valorResultado}
//               {...register(`resultados.${detalleId}.${nodo.examenId}.valorResultado` as const)}
//               className="w-full px-3 py-1.5 border border-[#dce5ef] rounded-md text-sm focus:border-[#2a79b7] focus:ring-1 focus:ring-[#2a79b7] focus:outline-none"
//             />
//           ) : (
//             <span className="text-xs text-[#94a3b8] italic">Múltiples parámetros...</span>
//           )}
//         </td>

//         {/* Unidad */}
//         <td className="px-4 py-3 text-sm text-[#64768b]">
//           {nodo.unidad || '-'}
//         </td>

//         {/* Valores de Referencia */}
//         <td className="px-4 py-3 text-sm text-[#64768b] whitespace-pre-line">
//           {nodo.valoresReferencia || '-'}
//         </td>

//         {/* Estado Visual */}
//         <td className="px-4 py-3 text-center">
//           <span className={`inline-flex items-center px-2 py-0.5 rounded text-[10px] font-bold uppercase tracking-wider ${
//             nodo.estado === 'Procesado' || nodo.estado === 'Validado' 
//               ? 'bg-green-100 text-green-700' 
//               : 'bg-yellow-50 text-yellow-700 border border-yellow-200'
//           }`}>
//             {nodo.estado}
//           </span>
//         </td>
//       </tr>

//       {/* 2. LA RECURSIVIDAD: Si tiene hijos, nos llamamos a nosotros mismos */}
//       {isPadre && nodo.subExamenes.map((subNodo) => (
//         <ResultExamRow 
//           key={subNodo.examenId} 
//           nodo={subNodo} 
//           detalleId={detalleId} 
//           nivel={nivel + 1} // Aumentamos el nivel para que el hijo salga más a la derecha
//         />
//       ))}
//     </>
//   );
// }

















import { useState } from 'react';
import { useFormContext } from 'react-hook-form';
import { ChevronRight, MessageSquareText } from 'lucide-react';
import type { NodoExamen } from '../types/result.types';
import type { ProcessResultFormValues } from '../schemas/resultSchema';

interface ResultExamRowProps {
  nodo: NodoExamen;
  detalleId: string;
  nivel?: number;
}

export default function ResultExamRow({ nodo, detalleId, nivel = 0 }: ResultExamRowProps) {
  const { register } = useFormContext<ProcessResultFormValues>();
  const [showNotes, setShowNotes] = useState(false);

  const isPadre = nodo.subExamenes && nodo.subExamenes.length > 0;
  const paddingIzquierdo = `${nivel * 2 + 1}rem`;

  return (
    <>
      <tr className={`border-b border-[#dce5ef] ${isPadre ? 'bg-[#f8fafc]' : 'hover:bg-[#f3f7fb]'} transition-colors`}>
        
        {/* Nombre del Examen */}
        <td className="py-3 pr-4" style={{ paddingLeft: paddingIzquierdo }}>
          <div className="flex items-center gap-2">
            {isPadre && <ChevronRight className="w-4 h-4 text-[#94a3b8]" />}
            <span className={`${isPadre ? 'font-bold text-[#15243b]' : 'font-medium text-[#4e6681]'} text-sm`}>
              {nodo.nombre}
            </span>
          </div>
        </td>

        {/* Input y Botón de Notas */}
        <td className="px-4 py-3">
          {!isPadre && (
            <div className="flex items-center gap-2">
              <input
                type="text"
                autoComplete="off"
                placeholder="Valor"
                defaultValue={nodo.valorResultado}
                {...register(`resultados.${detalleId}.${nodo.examenId}.valorResultado` as const)}
                className="w-24 px-3 py-1.5 border border-[#dce5ef] rounded-md text-sm focus:border-[#2a79b7] focus:ring-1 focus:ring-[#2a79b7] focus:outline-none"
              />
              <button
                type="button"
                onClick={() => setShowNotes(!showNotes)}
                className={`p-1.5 rounded-md transition-colors ${showNotes || nodo.notaClinica ? 'text-[#2a79b7] bg-[#eef5fa]' : 'text-[#94a3b8] hover:bg-[#f3f7fb]'}`}
                title="Añadir observación clínica"
              >
                <MessageSquareText className="w-4 h-4" />
              </button>
            </div>
          )}
        </td>

        {/* Unidad (vacío si es padre) */}
        <td className="px-4 py-3 text-sm text-[#64768b]">
          {!isPadre && nodo.unidad}
        </td>

        {/* Valores de Referencia (vacío si es padre) */}
        <td className="px-4 py-3 text-sm text-[#64768b] whitespace-pre-line">
          {!isPadre && nodo.valoresReferencia}
        </td>

        {/* Estado Visual */}
        <td className="px-4 py-3 text-center">
          <span className={`inline-flex items-center px-2 py-0.5 rounded text-[10px] font-bold uppercase tracking-wider ${
            nodo.estado === 'Procesado' || nodo.estado === 'Validado' 
              ? 'bg-green-100 text-green-700' 
              : 'bg-yellow-50 text-yellow-700 border border-yellow-200'
          }`}>
            {nodo.estado}
          </span>
        </td>
      </tr>

      {/* Fila desplegable para la Nota Clínica */}
      {showNotes && !isPadre && (
        <tr className="bg-[#f8fafc] border-b border-[#dce5ef]">
          <td colSpan={5} className="py-3 pr-4" style={{ paddingLeft: `calc(${paddingIzquierdo} + 1.5rem)` }}>
            <div className="flex flex-col gap-2 max-w-2xl">
              <textarea
                placeholder="Observación clínica (Ej: Muestra hemolizada, paciente pediátrico de difícil extracción...)"
                defaultValue={nodo.notaClinica}
                {...register(`resultados.${detalleId}.${nodo.examenId}.notaClinica` as const)}
                className="w-full px-3 py-2 border border-[#dce5ef] rounded-md text-sm focus:border-[#2a79b7] focus:ring-1 focus:ring-[#2a79b7] focus:outline-none resize-none"
                rows={2}
              />
              <label className="flex items-center gap-2 text-xs font-medium text-[#4e6681] cursor-pointer">
                <input
                  type="checkbox"
                  defaultChecked={nodo.imprimirObservacion ?? true}
                  {...register(`resultados.${detalleId}.${nodo.examenId}.imprimirObservacion` as const)}
                  className="rounded border-[#dce5ef] text-[#2a79b7] focus:ring-[#2a79b7]"
                />
                Imprimir esta observación en el reporte final del paciente
              </label>
            </div>
          </td>
        </tr>
      )}

      {/* Recursividad para los sub-exámenes */}
      {isPadre && nodo.subExamenes.map((subNodo) => (
        <ResultExamRow 
          key={subNodo.examenId} 
          nodo={subNodo} 
          detalleId={detalleId} 
          nivel={nivel + 1} 
        />
      ))}
    </>
  );
}