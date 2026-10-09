// import { useState } from 'react';
// import { Printer, Download, Loader2 } from 'lucide-react';
// import { apiClient } from '../../../lib/api/apiClient';

// interface PrintResultButtonProps {
//   ordenId: string;
//   nombreArchivo: string;
//   estadoOrden: string; // Para bloquear si no hay nada validado
// }

// export default function PrintResultButton({ ordenId, nombreArchivo }: PrintResultButtonProps) {
//   const [isPrinting, setIsPrinting] = useState(false);

//   const handleAction = async (accion: 'ver' | 'descargar') => {
//     try {
//       setIsPrinting(true);
//       const response = await apiClient.get(`/resultados/pdf/${ordenId}`, { responseType: 'blob' });
//       const file = new Blob([response.data], { type: 'application/pdf' });
//       const fileURL = URL.createObjectURL(file);
      
//       if (accion === 'descargar') {
//         // Fuerza la descarga con el nombre correcto (Ej: "RESULTADO 000003 ANA PENOTH.pdf")
//         const link = document.createElement('a');
//         link.href = fileURL;
//         link.download = `${nombreArchivo}.pdf`;
//         document.body.appendChild(link);
//         link.click();
//         document.body.removeChild(link);
//       } else {
//         // Opción de solo Ver
//         window.open(fileURL, '_blank');
//       }
//       setTimeout(() => URL.revokeObjectURL(fileURL), 10000);
//     } catch {
//       alert("Error al generar el documento PDF.");
//     } finally {
//       setIsPrinting(false);
//     }
//   };

//   return (
//     <div className="flex items-center gap-2">
//       <button
//         onClick={() => handleAction('ver')}
//         disabled={isPrinting}
//         className="flex items-center gap-2 px-4 py-2 bg-white border border-[#dce5ef] text-[#15243b] rounded-lg hover:bg-[#f8fafc] transition-colors font-bold shadow-sm"
//       >
//         {isPrinting ? <Loader2 className="w-4 h-4 animate-spin" /> : <Printer className="w-4 h-4 text-[#2a79b7]" />}
//         Ver Informe
//       </button>
//       <button
//         onClick={() => handleAction('descargar')}
//         disabled={isPrinting}
//         title="Descargar PDF"
//         className="flex items-center justify-center w-10 h-10 bg-white border border-[#dce5ef] text-[#64768b] rounded-lg hover:bg-[#f8fafc] hover:text-[#2a79b7] transition-colors shadow-sm"
//       >
//         <Download className="w-4 h-4" />
//       </button>
//     </div>
//   );
// }



















import { useState } from 'react';
import { Printer, Download, Loader2 } from 'lucide-react';
import { apiClient } from '../../../lib/api/apiClient';

interface PrintResultButtonProps {
  ordenId: string;
  nombreArchivo: string;
}

export default function PrintResultButton({ ordenId, nombreArchivo }: PrintResultButtonProps) {
  const [isPrinting, setIsPrinting] = useState(false);

  const handleAction = async (accion: 'ver' | 'descargar') => {
    try {
      setIsPrinting(true);
      const response = await apiClient.get(`/resultados/pdf/${ordenId}`, { responseType: 'blob' });
      const file = new Blob([response.data], { type: 'application/pdf' });
      const fileURL = URL.createObjectURL(file);
      
      if (accion === 'descargar') {
        const link = document.createElement('a');
        link.href = fileURL;
        link.download = `${nombreArchivo}.pdf`;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      } else {
        window.open(fileURL, '_blank');
      }
      setTimeout(() => URL.revokeObjectURL(fileURL), 10000);
    } catch {
      alert("Error al generar el documento PDF.");
    } finally {
      setIsPrinting(false);
    }
  };

  return (
    <div className="flex items-center gap-2">
      <button
        onClick={() => handleAction('ver')}
        disabled={isPrinting}
        className="flex items-center gap-2 px-4 py-2 bg-white border border-[#dce5ef] text-[#15243b] rounded-lg hover:bg-[#f8fafc] transition-colors font-bold shadow-sm"
      >
        {isPrinting ? <Loader2 className="w-4 h-4 animate-spin" /> : <Printer className="w-4 h-4 text-[#2a79b7]" />}
        Ver Informe
      </button>
      <button
        onClick={() => handleAction('descargar')}
        disabled={isPrinting}
        title="Descargar PDF"
        className="flex items-center justify-center w-10 h-10 bg-white border border-[#dce5ef] text-[#64768b] rounded-lg hover:bg-[#f8fafc] hover:text-[#2a79b7] transition-colors shadow-sm"
      >
        <Download className="w-4 h-4" />
      </button>
    </div>
  );
}