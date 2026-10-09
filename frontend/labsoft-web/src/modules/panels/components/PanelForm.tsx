import { useState, useMemo } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import type { SubmitHandler } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Save, Search, Plus, X, ArrowRightLeft } from 'lucide-react';
import { panelSchema, type PanelFormData } from '../schemas/panelSchema';
import { useExams } from '../../catalog/hooks/useExams';

interface PanelFormProps {
  initialValues?: Partial<PanelFormData>;
  onSubmit: SubmitHandler<PanelFormData>;
  onCancel: () => void;
  isLoading?: boolean;
}

export default function PanelForm({ initialValues, onSubmit, onCancel, isLoading }: PanelFormProps) {
  const [searchTerm, setSearchTerm] = useState('');
  const { data: groupedExams, isLoading: isLoadingExams } = useExams();

  const {
    register,
    handleSubmit,
    control,
    setValue,
    formState: { errors },
  } = useForm<PanelFormData>({
    resolver: zodResolver(panelSchema),
    defaultValues: {
      codigo: 0,
      nombre: '',
      examenIds: [],
      ...initialValues,
    },
  });

  // Estabilizamos la referencia del arreglo para evitar re-renderizados innecesarios
  const watchedIds = useWatch({ control, name: 'examenIds' });
  const selectedIds = useMemo(() => watchedIds || [], [watchedIds]);

  const allParentExams = useMemo(() => {
    return groupedExams
      ?.flatMap((grupo) => grupo.examenes)
      ?.filter((examen) => !examen.examenPadreId) || [];
  }, [groupedExams]);

  const availableExams = useMemo(() => {
    const searchLower = searchTerm.toLowerCase();
    return allParentExams.filter(
      (examen) => 
        !selectedIds.includes(examen.id) &&
        (examen.nombre.toLowerCase().includes(searchLower) || 
         examen.codigo.toString().includes(searchLower)) 
    );
  }, [allParentExams, selectedIds, searchTerm]);

  const selectedExams = useMemo(() => {
    return allParentExams.filter((examen) => selectedIds.includes(examen.id));
  }, [allParentExams, selectedIds]);

  const addExam = (id: string) => {
    setValue('examenIds', [...selectedIds, id], { shouldValidate: true });
  };

  const removeExam = (id: string) => {
    setValue('examenIds', selectedIds.filter((selectedId) => selectedId !== id), { shouldValidate: true });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      
      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
        <div className="grid grid-cols-1 md:grid-cols-[150px_1fr] gap-4">
          <div>
            <label className="block text-sm font-bold text-[#15243b] mb-2 uppercase tracking-wide">
              Código
            </label>
            <div className="w-full px-4 py-3 bg-[#f8fafc] border border-[#dce5ef] rounded-lg text-[#64768b] font-bold text-center">
              {initialValues?.codigo ? initialValues.codigo : 'Automático'}
            </div>
          </div>
          <div>
            <label className="block text-sm font-bold text-[#15243b] mb-2 uppercase tracking-wide">
              Nombre del Perfil / Panel *
            </label>
            <input
              {...register('nombre')}
              className="w-full px-4 py-3 border border-[#dce5ef] rounded-lg focus:outline-none focus:border-[#2a79b7] font-medium text-[#15243b]"
              placeholder="Ej. PERFIL PREOPERATORIO"
            />
            {errors.nombre && <p className="text-red-500 text-xs mt-1 font-medium">{errors.nombre.message}</p>}
          </div>
        </div>
      </div>



      {/* <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
        <label className="block text-sm font-bold text-[#15243b] mb-2 uppercase tracking-wide">
          Nombre del Perfil / Panel *
        </label>
        <input
          {...register('nombre')}
          className="w-full px-4 py-3 border border-[#dce5ef] rounded-lg focus:outline-none focus:border-[#2a79b7] focus:ring-1 focus:ring-[#2a79b7] font-medium text-[#15243b]"
          placeholder="Ej. PERFIL PREOPERATORIO"
        />
        {errors.nombre && <p className="text-red-500 text-xs mt-1 font-medium">{errors.nombre.message}</p>}
      </div> */}

      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm space-y-4">
        <div className="flex items-center justify-between border-b border-[#dce5ef] pb-2">
          <h3 className="text-sm font-bold text-[#2a79b7] uppercase">Composición del Panel</h3>
          <span className="text-xs font-medium text-[#64768b] bg-[#f3f7fb] px-2 py-1 rounded">
            {selectedIds.length} Exámenes seleccionados
          </span>
        </div>

        {errors.examenIds && <p className="text-red-500 text-xs font-medium">{errors.examenIds.message}</p>}

        {/* Cambiado h-[400px] por h-100 */}
        <div className="grid grid-cols-1 lg:grid-cols-[1fr_auto_1fr] gap-4 items-stretch h-100">
          
          <div className="flex flex-col border border-[#dce5ef] rounded-lg overflow-hidden bg-[#f8fafc]">
            <div className="p-3 border-b border-[#dce5ef] bg-white">
              <div className="relative">
                <Search className="w-4 h-4 absolute left-3 top-1/2 -translate-y-1/2 text-[#94a3b8]" />
                <input
                  type="text"
                  placeholder="Buscar en el catálogo..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="w-full pl-9 pr-3 py-2 text-sm border border-[#dce5ef] rounded bg-gray-50 focus:outline-none focus:border-[#2a79b7]"
                />
              </div>
            </div>
            <div className="flex-1 overflow-y-auto p-2 space-y-1">
              {isLoadingExams ? (
                <p className="text-xs text-center text-[#64768b] mt-4">Cargando catálogo...</p>
              ) : availableExams.length === 0 ? (
                <p className="text-xs text-center text-[#94a3b8] mt-4">No hay exámenes disponibles.</p>
              ) : (
                availableExams.map((examen) => (
                  <div 
                    key={examen.id}
                    onClick={() => addExam(examen.id)}
                    className="flex items-center justify-between p-2 text-sm text-[#15243b] bg-white border border-[#dce5ef] rounded hover:border-[#2a79b7] hover:shadow-sm cursor-pointer transition-all group"
                  >
                    <div className="flex flex-col">
                      {/* <span className="font-medium">{examen.nombre}</span> */}
                      <span className="text-[#64768b] mr-2">{examen.codigo}</span> {examen.nombre}
                      <span className="text-[10px] text-[#64768b]">{examen.especialidadNombre}</span>
                    </div>
                    <Plus className="w-4 h-4 text-[#94a3b8] group-hover:text-[#2a79b7]" />
                  </div>
                ))
              )}
            </div>
          </div>

          <div className="hidden lg:flex flex-col justify-center items-center px-2">
            <div className="w-10 h-10 rounded-full bg-[#eef5fa] flex items-center justify-center text-[#2a79b7]">
              <ArrowRightLeft className="w-5 h-5" />
            </div>
          </div>

          <div className="flex flex-col border border-[#2a79b7] rounded-lg overflow-hidden bg-white shadow-sm ring-1 ring-[#2a79b7]/10">
            <div className="p-3 border-b border-[#2a79b7]/20 bg-[#f3f7fb]">
              <h4 className="text-sm font-bold text-[#15243b]">Exámenes Incluidos</h4>
            </div>
            <div className="flex-1 overflow-y-auto p-2 space-y-1 bg-[#f8fafc]">
              {selectedExams.length === 0 ? (
                <p className="text-xs text-center text-[#94a3b8] mt-4">
                  Haz clic en un examen de la izquierda para agregarlo.
                </p>
              ) : (
                selectedExams.map((examen) => (
                  <div 
                    key={examen.id}
                    className="flex items-center justify-between p-2 text-sm text-[#15243b] bg-white border border-[#dce5ef] rounded shadow-sm group"
                  >
                    <span className="font-bold">{examen.nombre}</span>
                    <button
                      type="button"
                      onClick={() => removeExam(examen.id)}
                      className="p-1 rounded text-[#94a3b8] hover:text-red-500 hover:bg-red-50 transition-colors"
                    >
                      <X className="w-4 h-4" />
                    </button>
                  </div>
                ))
              )}
            </div>
          </div>

        </div>
      </div>

      <div className="flex justify-end gap-3 pt-4">
        <button
          type="button"
          onClick={onCancel}
          className="px-6 py-2.5 text-[#4e6681] bg-white border border-[#dce5ef] hover:bg-[#f3f7fb] rounded-lg font-medium transition-colors"
        >
          Cancelar
        </button>
        <button
          type="submit"
          disabled={isLoading}
          className="flex items-center gap-2 px-8 py-2.5 bg-[#2a79b7] text-white rounded-lg hover:bg-[#206094] transition-colors font-medium disabled:opacity-50 shadow-sm"
        >
          <Save className="w-4 h-4" />
          {isLoading ? 'Guardando...' : 'Guardar Panel'}
        </button>
      </div>
    </form>
  );
}