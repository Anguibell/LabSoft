import { useState, useMemo } from 'react';
import { Search, Plus, Layers, FlaskConical, Check } from 'lucide-react';
// Importa tus hooks de los catálogos correspondientes
import { usePanels } from '../../panels/hooks/usePanels';
import { useExams } from '../../catalog/hooks/useExams';

interface ExamSelectorProps {
  onAddExam: (examenId: string) => void;
  onAddPanel: (panelId: string) => void;
  selectedExamIds: string[];
  selectedPanelIds: string[];
}

export default function ExamSelector({
  onAddExam,
  onAddPanel,
  selectedExamIds,
  selectedPanelIds,
}: ExamSelectorProps) {
  const [searchTerm, setSearchTerm] = useState('');
  const [activeTab, setActiveTab] = useState<'paneles' | 'examenes'>('paneles');

  const { data: panels, isLoading: isLoadingPanels } = usePanels();
  const { data: exams, isLoading: isLoadingExams } = useExams();

  const filteredPanels = useMemo(() => {
    if (!panels) return [];
    const lower = searchTerm.toLowerCase();
    return panels
      .filter((p) => p.activo && (p.nombre.toLowerCase().includes(lower) || p.codigo.toString().includes(lower)))
      .sort((a, b) => a.codigo - b.codigo);
  }, [panels, searchTerm]);

  // 1. Aplanamos la lista de grupos para extraer todos los exámenes sueltos
  const flatExams = useMemo(() => {
    if (!exams) return [];
    // Extraemos la propiedad 'examenes' de cada grupo y los unimos en un solo array
    return exams.flatMap((group) => group.examenes || []);
  }, [exams]);

  // 2. Filtramos la lista aplanada
  const filteredExams = useMemo(() => {
    const lower = searchTerm.toLowerCase();
    return flatExams
      .filter((e) => e.activo && (e.nombre.toLowerCase().includes(lower) || e.codigo.toString().includes(lower)))
      .sort((a, b) => a.codigo - b.codigo);
  }, [flatExams, searchTerm]);

  return (
    <div className="flex flex-col h-full w-full bg-white">
      {/* Pestañas de Navegación */}
      <div className="flex p-1 bg-[#f8fafc] border border-[#dce5ef] rounded-lg mb-4">
        <button
          type="button"
          onClick={() => setActiveTab('paneles')}
          className={`flex-1 flex items-center justify-center gap-2 py-2 text-sm font-bold rounded-md transition-all ${
            activeTab === 'paneles'
              ? 'bg-white text-[#2a79b7] shadow-sm ring-1 ring-[#dce5ef]'
              : 'text-[#64768b] hover:text-[#15243b]'
          }`}
        >
          <Layers className="w-4 h-4" /> Perfiles
        </button>
        <button
          type="button"
          onClick={() => setActiveTab('examenes')}
          className={`flex-1 flex items-center justify-center gap-2 py-2 text-sm font-bold rounded-md transition-all ${
            activeTab === 'examenes'
              ? 'bg-white text-[#2a79b7] shadow-sm ring-1 ring-[#dce5ef]'
              : 'text-[#64768b] hover:text-[#15243b]'
          }`}
        >
          <FlaskConical className="w-4 h-4" /> Exámenes Sueltos
        </button>
      </div>

      {/* Buscador */}
      <div className="relative mb-4">
        <Search className="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-[#94a3b8]" />
        <input
          type="text"
          placeholder={`Buscar ${activeTab === 'paneles' ? 'perfil' : 'examen'} por nombre o código...`}
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="w-full pl-10 pr-4 py-2 border border-[#dce5ef] rounded-lg text-sm focus:outline-none focus:border-[#2a79b7] focus:ring-1 focus:ring-[#2a79b7]"
        />
      </div>

      {/* Lista de Resultados (Scrollable) */}
      <div className="flex-1 overflow-y-auto pr-1 space-y-2 min-h-75 max-h-100">
        {activeTab === 'paneles' ? (
          isLoadingPanels ? (
            <p className="text-center text-[#64768b] text-sm py-4">Cargando perfiles...</p>
          ) : filteredPanels.length === 0 ? (
            <p className="text-center text-[#64768b] text-sm py-4">No se encontraron perfiles.</p>
          ) : (
            filteredPanels.map((panel) => {
              const isSelected = selectedPanelIds.includes(panel.id);
              return (
                <div
                  key={panel.id}
                  className={`flex items-center justify-between p-3 rounded-lg border ${
                    isSelected ? 'bg-[#f3f7fb] border-[#2a79b7]' : 'bg-white border-[#dce5ef] hover:border-[#2a79b7]'
                  } transition-colors`}
                >
                  <div>
                    <p className="text-sm font-bold text-[#15243b]">
                      <span className="text-[#64768b] mr-2">{panel.codigo}</span>
                      {panel.nombre}
                    </p>
                    <p className="text-xs text-[#64768b] mt-0.5">{panel.examenes?.length || 0} exámenes incluidos</p>
                  </div>
                  <button
                    type="button"
                    onClick={() => onAddPanel(panel.id)}
                    disabled={isSelected}
                    className={`p-1.5 rounded-md transition-colors ${
                      isSelected
                        ? 'bg-[#2a79b7] text-white cursor-default'
                        : 'bg-[#f8fafc] text-[#2a79b7] hover:bg-[#eef5fa] border border-[#dce5ef]'
                    }`}
                  >
                    {isSelected ? <Check className="w-4 h-4" /> : <Plus className="w-4 h-4" />}
                  </button>
                </div>
              );
            })
          )
        ) : isLoadingExams ? (
          <p className="text-center text-[#64768b] text-sm py-4">Cargando exámenes...</p>
        ) : filteredExams.length === 0 ? (
          <p className="text-center text-[#64768b] text-sm py-4">No se encontraron exámenes.</p>
        ) : (
          filteredExams.map((examen) => {
            const isSelected = selectedExamIds.includes(examen.id);
            return (
              <div
                key={examen.id}
                className={`flex items-center justify-between p-3 rounded-lg border ${
                  isSelected ? 'bg-[#f3f7fb] border-[#2a79b7]' : 'bg-white border-[#dce5ef] hover:border-[#2a79b7]'
                } transition-colors`}
              >
                <div>
                  <p className="text-sm font-bold text-[#15243b]">
                    <span className="text-[#64768b] mr-2">{examen.codigo}</span>
                    {examen.nombre}
                  </p>
                </div>
                <button
                  type="button"
                  onClick={() => onAddExam(examen.id)}
                  disabled={isSelected}
                  className={`p-1.5 rounded-md transition-colors ${
                    isSelected
                      ? 'bg-[#2a79b7] text-white cursor-default'
                      : 'bg-[#f8fafc] text-[#2a79b7] hover:bg-[#eef5fa] border border-[#dce5ef]'
                  }`}
                >
                  {isSelected ? <Check className="w-4 h-4" /> : <Plus className="w-4 h-4" />}
                </button>
              </div>
            );
          })
        )}
      </div>
    </div>
  );
}