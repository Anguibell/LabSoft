import { useForm, useWatch, useFieldArray } from 'react-hook-form';
import type { SubmitHandler, Control, UseFormRegister } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Save, Plus, Trash2, CornerDownRight } from 'lucide-react';
import { examSchema, type ExamFormData } from '../schemas/examSchema';
import { useSpecialties } from '../hooks/useSpecialties';
import { useExams } from '../hooks/useExams';

// --- COMPONENTE RECURSIVO PARA LAS FILAS (Soporta N Niveles) ---
interface RecursiveRowProps {
  control: Control<ExamFormData>;
  register: UseFormRegister<ExamFormData>;
  path: string;
  index: number;
  remove: (index: number) => void;
  level: number;
}

const RecursiveParamRow = ({ control, register, path, index, remove, level }: RecursiveRowProps) => {
  const childrenPath = `${path}.subExamenes`;
  
  const { fields, append, remove: removeChild } = useFieldArray({
    control,
    name: childrenPath as never, // 'never' evita que TS intente validar strings infinitos
  });

  const hasChildren = fields.length > 0;

  return (
    <div className="flex flex-col gap-2">
      {/* Fila Principal */}
      <div 
        className="flex items-center gap-2 bg-white hover:bg-[#f8fafc] py-1.5 px-2 rounded border border-[#dce5ef] transition-all group"
        style={{ marginLeft: `${level * 24}px` }} 
      >
        {level > 0 && <CornerDownRight className="w-4 h-4 text-[#94a3b8] shrink-0" />}

        <input
          {...register(`${path}.nombre` as never)}
          className={`flex-1 px-2 py-1.5 text-xs border border-[#dce5ef] rounded focus:border-[#2a79b7] focus:outline-none ${hasChildren ? 'font-bold bg-[#eef5fa]' : ''}`}
          placeholder={hasChildren ? "Ej. Fórmula Leucocitaria (Agrupador)" : "Ej. Neutrófilos"}
        />
        <input
          {...register(`${path}.unidad` as never)}
          disabled={hasChildren}
          className="w-16 px-2 py-1.5 text-xs border border-[#dce5ef] rounded disabled:bg-gray-100 disabled:text-gray-400 focus:border-[#2a79b7] focus:outline-none"
          placeholder="Unidad"
        />
        <input
          {...register(`${path}.valoresReferencia` as never)}
          disabled={hasChildren}
          className="w-20 px-2 py-1.5 text-xs border border-[#dce5ef] rounded disabled:bg-gray-100 disabled:text-gray-400 focus:border-[#2a79b7] focus:outline-none"
          placeholder="V. Gen"
        />
        <input
          {...register(`${path}.valoresReferenciaM` as never)}
          disabled={hasChildren}
          className="w-20 px-2 py-1.5 text-xs border border-[#dce5ef] rounded disabled:bg-gray-100 disabled:text-gray-400 focus:border-[#2a79b7] focus:outline-none"
          placeholder="V. Hom"
        />
        <input
          {...register(`${path}.valoresReferenciaF` as never)}
          disabled={hasChildren}
          className="w-20 px-2 py-1.5 text-xs border border-[#dce5ef] rounded disabled:bg-gray-100 disabled:text-gray-400 focus:border-[#2a79b7] focus:outline-none"
          placeholder="V. Muj"
        />
        <div className="w-12 flex justify-center">
          <input type="checkbox" {...register(`${path}.esOpcionalDinamico` as never)} className="w-4 h-4 text-[#2a79b7] rounded border-gray-300" title="Es Opcional" />
        </div>

        {/* Botones de Acción */}
        <div className="flex items-center gap-1 w-16 justify-end">
          {level < 2 && ( 
            <button
              type="button"
              onClick={() => append({ nombre: '', unidad: '', valoresReferencia: '', valoresReferenciaM: '', valoresReferenciaF: '', esOpcionalDinamico: false, precio: 0, subExamenes: [] } as never)}
              className="text-[#2a79b7] hover:bg-[#eef5fa] p-1.5 rounded transition-colors opacity-0 group-hover:opacity-100"
              title="Agregar Hijo"
            >
              <Plus className="w-4 h-4" />
            </button>
          )}
          <button type="button" onClick={() => remove(index)} className="text-red-500 hover:bg-red-50 p-1.5 rounded transition-colors" title="Eliminar Fila">
            <Trash2 className="w-4 h-4" />
          </button>
        </div>
      </div>

      {/* Renderizado Recursivo de Hijos */}
      {fields.map((childField, childIndex) => (
        <RecursiveParamRow
          key={childField.id}
          control={control}
          register={register}
          path={`${childrenPath}.${childIndex}`}
          index={childIndex}
          remove={removeChild}
          level={level + 1}
        />
      ))}
    </div>
  );
};

// --- FORMULARIO PRINCIPAL ---
interface ExamFormProps {
  initialValues?: Partial<ExamFormData>;
  onSubmit: SubmitHandler<ExamFormData>;
  onCancel: () => void;
  isLoading?: boolean;
}

export default function ExamForm({ initialValues, onSubmit, onCancel, isLoading }: ExamFormProps) {
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<ExamFormData>({
    resolver: zodResolver(examSchema),
    defaultValues: {
      codigo: 0, orden: 0, precio: 0, unidad: '', valoresReferencia: '', valoresReferenciaM: '', valoresReferenciaF: '',
      activo: true, esOpcionalDinamico: false, examenPadreId: '', subExamenes: [], ...initialValues,
    },
  });

  const { data: specialties } = useSpecialties();
  const { data: groupedExams } = useExams();

  const { fields, append, remove } = useFieldArray({ control, name: 'subExamenes' });
  const hasChildren = fields.length > 0;

  const selectedSpecialtyId = useWatch({ control, name: 'especialidadId' });
  const posiblesPadres = groupedExams?.flatMap((g) => g.examenes)?.filter((e) => e.especialidadId === selectedSpecialtyId) || [];

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      
      {/* 1. SECCIÓN DEL PADRE */}
      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm space-y-6">
        <h3 className="text-sm font-bold text-[#2a79b7] uppercase border-b border-[#dce5ef] pb-2">Información del Examen Principal</h3>
        
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">Nombre del Examen *</label>
            <input {...register('nombre')} className="w-full px-3 py-2 border border-[#dce5ef] rounded-lg focus:outline-none focus:border-[#2a79b7]" placeholder="Ej. HEMATOLOGIA COMPLETA" />
            {errors.nombre && <p className="text-red-500 text-xs mt-1">{errors.nombre.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">Especialidad *</label>
            <select {...register('especialidadId')} className="w-full px-3 py-2 border border-[#dce5ef] rounded-lg focus:outline-none focus:border-[#2a79b7]">
              <option value="">Seleccione especialidad...</option>
              {specialties?.map((esp) => <option key={esp.id} value={esp.id}>{esp.nombre}</option>)}
            </select>
            {errors.especialidadId && <p className="text-red-500 text-xs mt-1">{errors.especialidadId.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">Examen Padre (Opcional)</label>
            <select {...register('examenPadreId')} disabled={!selectedSpecialtyId || posiblesPadres.length === 0} className="w-full px-3 py-2 border border-[#dce5ef] rounded-lg disabled:bg-gray-100 disabled:text-gray-400 focus:outline-none focus:border-[#2a79b7]">
              <option value="">Ninguno (Es Principal)</option>
              {posiblesPadres.map((p) => <option key={p.id} value={p.id}>{p.nombre}</option>)}
            </select>
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">Precio</label>
            <input type="number" step="0.01" {...register('precio', { valueAsNumber: true })} className="w-full px-3 py-2 border border-[#dce5ef] rounded-lg focus:outline-none focus:border-[#2a79b7]" />
          </div>
          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">Unidad</label>
            <input {...register('unidad')} disabled={hasChildren} className="w-full px-3 py-2 border border-[#dce5ef] rounded-lg disabled:bg-gray-100 disabled:text-gray-400 focus:outline-none focus:border-[#2a79b7]" placeholder={hasChildren ? "Bloqueado (Agrupador)" : "Ej. mg/dL"} />
          </div>
          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">V. Ref General</label>
            <input {...register('valoresReferencia')} disabled={hasChildren} className="w-full px-3 py-2 border border-[#dce5ef] rounded-lg disabled:bg-gray-100 disabled:text-gray-400 focus:outline-none focus:border-[#2a79b7]" placeholder={hasChildren ? "Bloqueado" : "Ej. 70 - 100"} />
          </div>
          <div className="flex flex-col justify-end gap-2 pb-2">
            <label className="flex items-center gap-2 cursor-pointer">
              <input type="checkbox" {...register('activo')} className="w-4 h-4 text-[#2a79b7] rounded border-gray-300" />
              <span className="text-sm font-medium">Activo</span>
            </label>
          </div>
        </div>
      </div>

      {/* 2. SECCIÓN DINÁMICA DE PARÁMETROS RECURSIVOS */}
      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm space-y-4">
        <div className="flex justify-between items-center border-b border-[#dce5ef] pb-2">
          <h3 className="text-sm font-bold text-[#2a79b7] uppercase">Estructura del Examen (Árbol)</h3>
          <button
            type="button"
            onClick={() => append({ nombre: '', unidad: '', valoresReferencia: '', valoresReferenciaM: '', valoresReferenciaF: '', esOpcionalDinamico: false, precio: 0, subExamenes: [] })}
            className="flex items-center gap-1.5 px-3 py-1.5 bg-[#eef5fa] text-[#2a79b7] text-xs font-bold rounded-lg hover:bg-[#2a79b7] hover:text-white transition-colors"
          >
            <Plus className="w-4 h-4" /> Agregar Parámetro Raíz
          </button>
        </div>

        {fields.length === 0 ? (
          <p className="text-sm text-[#64768b] text-center py-6 bg-gray-50 rounded-lg border border-dashed border-gray-300">
            Este examen no tiene sub-parámetros. Utiliza el botón superior para agregar el primer elemento.
          </p>
        ) : (
          <div className="flex flex-col gap-2">
            {/* Cabecera Estática */}
            <div className="flex items-center gap-2 text-xs font-bold text-[#64768b] bg-[#f8fafc] px-2 py-2 rounded border border-[#dce5ef]">
              <div className="flex-1 px-2">Nombre del Parámetro *</div>
              <div className="w-16 px-2">Unidad</div>
              <div className="w-20 px-2">V. Gen</div>
              <div className="w-20 px-2">V. Hom</div>
              <div className="w-20 px-2">V. Muj</div>
              <div className="w-12 text-center">Opc.</div>
              <div className="w-16 text-right px-2">Acciones</div>
            </div>

            {/* Renderizado Raíces */}
            {fields.map((field, index) => (
              <RecursiveParamRow key={field.id} control={control} register={register} path={`subExamenes.${index}`} index={index} remove={remove} level={0} />
            ))}
          </div>
        )}
      </div>

      <div className="flex justify-end gap-3 pt-2">
        <button type="button" onClick={onCancel} className="px-6 py-2 text-[#4e6681] bg-white border border-[#dce5ef] hover:bg-[#f3f7fb] rounded-lg font-medium transition-colors">Cancelar</button>
        <button type="submit" disabled={isLoading} className="flex items-center gap-2 px-8 py-2 bg-[#2a79b7] text-white rounded-lg hover:bg-[#206094] transition-colors font-medium disabled:opacity-50 shadow-sm">
          <Save className="w-4 h-4" /> {isLoading ? 'Guardando...' : 'Guardar Árbol Completo'}
        </button>
      </div>
    </form>
  );
}