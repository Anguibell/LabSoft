import { useState } from 'react';
import { Trash2, Activity, Layers, ChevronDown, ChevronRight, X, RotateCcw } from 'lucide-react';
import { useExams } from '../../catalog/hooks/useExams';
import { usePanels } from '../../panels/hooks/usePanels';
import type { OrderFormData } from '../schemas/orderSchema';

interface OrderExamListProps {
  formData: OrderFormData;
  onRemoveExam: (id: string) => void;
  onRemovePanel: (id: string) => void;
  onToggleExclusion: (panelId: string, examId: string) => void;
}

export default function OrderExamList({ formData, onRemoveExam, onRemovePanel, onToggleExclusion }: OrderExamListProps) {
  const { data: exams } = useExams();
  const { data: panels } = usePanels();
  const [expandedPanels, setExpandedPanels] = useState<string[]>([]);

  const flatExams = exams?.flatMap(g => g.examenes || []) || [];
  const totalItems = (formData.examenIds?.length || 0) + (formData.panelesSolicitados?.length || 0);

  const toggleExpand = (panelId: string) => {
    setExpandedPanels(prev => 
      prev.includes(panelId) ? prev.filter(id => id !== panelId) : [...prev, panelId]
    );
  };

  if (totalItems === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-12 text-[#64768b]">
        <Activity className="w-12 h-12 mb-3 text-[#dce5ef]" />
        <p className="text-sm">El carrito está vacío.</p>
        <p className="text-xs mt-1">Selecciona perfiles o exámenes del catálogo.</p>
      </div>
    );
  }

  return (
    <div className="space-y-4">
      {/* 1. PERFILES SELECCIONADOS */}
      {formData.panelesSolicitados.length > 0 && (
        <div>
          <h4 className="text-xs font-bold text-[#64768b] uppercase mb-2">Perfiles Seleccionados</h4>
          <div className="space-y-2">
            {formData.panelesSolicitados.map(solicitud => {
              const panelInfo = panels?.find(p => p.id === solicitud.panelId);
              if (!panelInfo) return null;

              const isExpanded = expandedPanels.includes(solicitud.panelId);
              const excluidosCount = solicitud.examenesExcluidosIds.length;
              const activosCount = panelInfo.examenes.length - excluidosCount;

              return (
                <div key={solicitud.panelId} className="flex flex-col bg-white border border-[#2a79b7]/30 rounded-lg overflow-hidden transition-all shadow-sm">
                  
                  {/* Cabecera del Panel */}
                  <div className="flex items-center justify-between p-3 bg-[#f3f7fb] hover:bg-[#eef5fa] transition-colors">
                    <button 
                      type="button"
                      onClick={() => toggleExpand(solicitud.panelId)} 
                      className="flex flex-1 items-center gap-2 text-left"
                    >
                      {isExpanded ? <ChevronDown className="w-4 h-4 text-[#2a79b7]" /> : <ChevronRight className="w-4 h-4 text-[#64768b]" />}
                      <Layers className="w-4 h-4 text-[#2a79b7]" />
                      <div>
                        <span className="text-sm font-bold text-[#15243b]">{panelInfo.nombre}</span>
                        <p className="text-[11px] text-[#64768b] mt-0.5">
                          {activosCount} exámenes activos {excluidosCount > 0 && <span className="text-red-500 font-medium">({excluidosCount} excluidos)</span>}
                        </p>
                      </div>
                    </button>
                    <button type="button" onClick={() => onRemovePanel(solicitud.panelId)} className="p-2 text-red-400 hover:text-red-600 transition-colors">
                      <Trash2 className="w-4 h-4" />
                    </button>
                  </div>

                  {/* Lista de Exámenes Internos (Desplegable) */}
                  {isExpanded && (
                    <div className="border-t border-[#dce5ef] bg-white p-2 space-y-1 animate-in fade-in slide-in-from-top-2 duration-200">
                      {[...panelInfo.examenes].sort((a, b) => a.codigo - b.codigo).map(examen => {
                        const isExcluded = solicitud.examenesExcluidosIds.includes(examen.id);
                        return (
                          <div key={examen.id} className={`flex items-center justify-between p-2 rounded text-sm ${isExcluded ? 'bg-gray-50/50' : 'hover:bg-gray-50'}`}>
                            <div className={`flex items-center gap-2 ${isExcluded ? 'opacity-50 line-through' : ''}`}>
                              <span className="text-xs font-bold text-[#64768b] w-8">{examen.codigo}</span>
                              <span className="font-medium text-[#15243b]">{examen.nombre}</span>
                            </div>
                            <button
                              type="button"
                              onClick={() => onToggleExclusion(solicitud.panelId, examen.id)}
                              className={`p-1.5 rounded-md text-xs font-medium transition-colors ${
                                isExcluded 
                                ? 'text-[#2a79b7] bg-[#eef5fa] hover:bg-[#dce5ef]' 
                                : 'text-[#64768b] hover:text-red-500 hover:bg-red-50'
                              }`}
                              title={isExcluded ? 'Volver a incluir' : 'Excluir de la orden'}
                            >
                              {isExcluded ? <RotateCcw className="w-3.5 h-3.5" /> : <X className="w-3.5 h-3.5" />}
                            </button>
                          </div>
                        );
                      })}
                    </div>
                  )}
                </div>
              );
            })}
          </div>
        </div>
      )}

      {/* 2. EXÁMENES SUELTOS */}
      {formData.examenIds.length > 0 && (
        <div>
          <h4 className="text-xs font-bold text-[#64768b] uppercase mb-2 mt-4">Exámenes Sueltos</h4>
          <div className="space-y-2">
            {formData.examenIds.map(id => {
              const examInfo = flatExams.find(e => e.id === id);
              if (!examInfo) return null;

              return (
                <div key={id} className="flex items-center justify-between bg-white border border-[#dce5ef] rounded-lg p-3 hover:border-[#2a79b7]/50 transition-colors shadow-sm">
                  <span className="text-sm font-medium text-[#15243b]">
                    <span className="text-xs font-bold text-[#64768b] w-8 inline-block">{examInfo.codigo}</span>
                    {examInfo.nombre}
                  </span>
                  <button type="button" onClick={() => onRemoveExam(id)} className="p-1.5 text-red-400 hover:text-red-600 hover:bg-red-50 rounded-md transition-colors">
                    <Trash2 className="w-4 h-4" />
                  </button>
                </div>
              );
            })}
          </div>
        </div>
      )}
    </div>
  );
}