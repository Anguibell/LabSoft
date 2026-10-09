import { useState, useEffect } from 'react';
import { X, Download, Loader2 } from 'lucide-react';
import type { AxiosResponse } from 'axios';
import { apiClient } from '../../../lib/api/apiClient';

interface PdfViewerModalProps {
  isOpen: boolean;
  onClose: () => void;
  ordenId: string;
  numeroOrden: string;
  nombrePaciente?: string;
  fechaOrden?: string;
}

export default function PdfViewerModal({ 
  isOpen, 
  onClose, 
  ordenId, 
  numeroOrden, 
  nombrePaciente = 'PACIENTE', 
  fechaOrden = '' 
}: PdfViewerModalProps) {
  const [pdfUrl, setPdfUrl] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!isOpen || !ordenId) return;

    let objectUrl: string | null = null;

    const fetchPdf = async () => {
      setIsLoading(true);
      setError(null);

      try {
        const response: AxiosResponse<Blob> = await apiClient.get(`/resultados/pdf/${ordenId}`, {
          responseType: 'blob',
        });
        const blob = new Blob([response.data], { type: 'application/pdf' });
        objectUrl = URL.createObjectURL(blob);
        setPdfUrl(objectUrl);
      } catch (err: unknown) {
        console.error('Error cargando el PDF:', err);
        setError('No se pudo cargar el documento PDF de los resultados.');
      } finally {
        setIsLoading(false);
      }
    };

    fetchPdf();

    return () => {
      if (objectUrl) {
        URL.revokeObjectURL(objectUrl);
      }
    };
  }, [isOpen, ordenId]);

  if (!isOpen) return null;

  // Formateamos la fecha para el nombre del archivo 
  // Formateamos la fecha actual o la recibida asegurando el cero inicial (DD-MM-YYYY)
  const formatDate = (dateString?: string) => {
    const d = dateString ? new Date(dateString) : new Date();
    // Validar fecha inválida
    if (isNaN(d.getTime())) return new Date().toLocaleDateString('es-VE');
    
    const day = String(d.getUTCDate()).padStart(2, '0');
    const month = String(d.getUTCMonth() + 1).padStart(2, '0');
    const year = d.getUTCFullYear();
    
    return `${day}-${month}-${year}`;
  };

  const fechaLimpia = formatDate(fechaOrden);
  const nombreArchivoDeseado = `RESULTADO ${numeroOrden} ${nombrePaciente.toUpperCase()} ${fechaLimpia} .pdf`;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/70 backdrop-blur-sm p-2 sm:p-4">
      {/* Ampliamos el ancho máximo a max-w-6xl y la altura a 95vh para mayor comodidad visual */}
      <div className="bg-white w-full max-w-6xl h-[95vh] rounded-2xl shadow-2xl flex flex-col overflow-hidden animate-in fade-in zoom-in-95 duration-200">
        
        {/* Cabecera del Modal */}
        <div className="flex justify-between items-center px-6 py-4 bg-[#f8fafc] border-b border-[#dce5ef]">
          <div>
            <h3 className="text-lg font-bold text-[#15243b]">Resultado de Orden #{numeroOrden}</h3>
            <p className="text-xs text-[#64768b]">Vista previa oficial del laboratorio</p>
          </div>
          
          <div className="flex items-center gap-3">
            {pdfUrl && (
              <a
                href={pdfUrl}
                download={nombreArchivoDeseado}
                className="flex items-center gap-2 px-4 py-2 bg-[#2a79b7] text-white text-sm font-medium rounded-lg hover:bg-[#206094] transition-colors shadow-sm"
              >
                <Download className="w-4 h-4" />
                Descargar PDF
              </a>
            )}
            <button
              onClick={onClose}
              className="p-1.5 text-[#64768b] hover:text-[#15243b] hover:bg-[#eef5fa] rounded-lg transition-colors"
            >
              <X className="w-5 h-5" />
            </button>
          </div>
        </div>

        {/* Cuerpo del Visor con Zoom Forzado al 125% (#view=FitH&zoom=125) */}
        <div className="flex-1 bg-gray-200 p-1 flex items-center justify-center relative">
          {isLoading && (
            <div className="flex flex-col items-center gap-2 text-[#64768b]">
              <Loader2 className="w-8 h-8 animate-spin text-[#2a79b7]" />
              <p className="text-sm font-medium">Generando vista previa del PDF...</p>
            </div>
          )}

          {error && (
            <div className="text-center text-red-500 p-4">
              <p>{error}</p>
            </div>
          )}

          {pdfUrl && !isLoading && !error && (
            <iframe
              src={`${pdfUrl}#view=FitH&zoom=125&toolbar=0`}
              className="w-full h-full rounded-lg border border-[#dce5ef] bg-white shadow-inner"
              title={`PDF Orden ${numeroOrden}`}
            />
          )}
        </div>
      </div>
    </div>
  );
}