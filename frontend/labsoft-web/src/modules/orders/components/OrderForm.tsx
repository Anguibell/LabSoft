// import { useState } from 'react';
// import { useForm, useWatch } from 'react-hook-form';
// import type { SubmitHandler } from 'react-hook-form';
// import { zodResolver } from '@hookform/resolvers/zod';
// import { Save, User, FlaskConical, ClipboardList, UserPlus } from 'lucide-react';
// import { orderSchema, type OrderFormData } from '../schemas/orderSchema';
// import { usePanels } from '../../panels/hooks/usePanels';
// import PatientSelector from './PatientSelector';
// import ExamSelector from './ExamSelector';
// import OrderExamList from './OrderExamList';
// import QuickPatientModal from './QuickPatientModal';

// interface OrderFormProps {
//   initialValues?: Partial<OrderFormData>;
//   onSubmit: SubmitHandler<OrderFormData>;
//   onCancel: () => void;
//   isLoading?: boolean;
// }

// export default function OrderForm({ initialValues, onSubmit, onCancel, isLoading }: OrderFormProps) {
//   const {
//     control,
//     handleSubmit,
//     setValue,
//     getValues,
//     trigger,
//     formState: { errors },
//   } = useForm<OrderFormData>({
//     resolver: zodResolver(orderSchema),
//     defaultValues: {
//       pacienteId: '',
//       examenIds: [],
//       panelesSolicitados: [],
//       ...initialValues,
//     },
//   });

//   const [isNewPatientModalOpen, setIsNewPatientModalOpen] = useState(false);

//   // Usamos useWatch para monitorear el estado completo sin forzar re-renders excesivos
//   const formData = useWatch({ control });

//   // Observamos los arreglos para pasarlos al selector (así sabe qué pintar de azul)
//   const currentExamIds = useWatch({ control, name: 'examenIds' }) || [];
//   const currentPanels = useWatch({ control, name: 'panelesSolicitados' }) || [];
//   const currentPanelIds = currentPanels.map((p) => p.panelId);

// // Traemos los paneles para poder inspeccionar sus exámenes internos
//   const { data: panels } = usePanels();

//   const handleAddExam = (examenId: string) => {
//     const currentExams = getValues('examenIds');
//     const currentPanels = getValues('panelesSolicitados');

//     // Validación extra: No dejar agregar un examen suelto si ya está en un panel seleccionado (y no está excluido)
//     let isAlreadyInPanel = false;
//     for (const p of currentPanels) {
//       const panelInfo = panels?.find(panel => panel.id === p.panelId);
//       if (panelInfo?.examenes.some(e => e.id === examenId) && !p.examenesExcluidosIds.includes(examenId)) {
//         isAlreadyInPanel = true;
//         break;
//       }
//     }

//     if (isAlreadyInPanel) {
//       alert('Este examen ya está activo dentro de uno de los perfiles seleccionados.');
//       return;
//     }

//     if (!currentExams.includes(examenId)) {
//       setValue('examenIds', [...currentExams, examenId]);
//       trigger();
//     }
//   };

//   const handleAddPanel = (panelId: string) => {
//     const currentPanels = getValues('panelesSolicitados');
//     const currentExams = getValues('examenIds');

//     if (!currentPanels.some((p) => p.panelId === panelId)) {
//       // 1. Buscamos qué exámenes trae este panel
//       const panelInfo = panels?.find(p => p.id === panelId);
//       const examenesDelPanel = panelInfo?.examenes.map(e => e.id) || [];

//       // 2. Limpiamos de la lista de "sueltos" cualquier examen que venga en este panel
//       const examenesFiltrados = currentExams.filter(id => !examenesDelPanel.includes(id));
      
//       setValue('examenIds', examenesFiltrados);
//       setValue('panelesSolicitados', [...currentPanels, { panelId, examenesExcluidosIds: [] }]);
//       trigger();
//     }
//   };

//   const handleToggleExclusion = (panelId: string, examId: string) => {
//     const currentPanels = getValues('panelesSolicitados');
//     const updatedPanels = currentPanels.map(p => {
//       if (p.panelId === panelId) {
//         const isExcluded = p.examenesExcluidosIds.includes(examId);
//         return {
//           ...p,
//           // Si está excluido, lo quitamos de la lista (lo volvemos a incluir). Si no, lo agregamos a los excluidos.
//           examenesExcluidosIds: isExcluded 
//             ? p.examenesExcluidosIds.filter(id => id !== examId) 
//             : [...p.examenesExcluidosIds, examId]
//         };
//       }
//       return p;
//     });
//     setValue('panelesSolicitados', updatedPanels, { shouldValidate: true });
//   };

//   // Funciones para eliminar del carrito
//   const handleRemoveExam = (id: string) => {
//     setValue('examenIds', currentExamIds.filter(eId => eId !== id));
//     trigger();
//   };

//   const handleRemovePanel = (id: string) => {
//     setValue('panelesSolicitados', currentPanels.filter(p => p.panelId !== id));
//     trigger();
//   };

//   return (
//     <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      
//       {/* SECCIÓN 1: Selección del Paciente */}
//       {/* <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
//         <h3 className="text-sm font-bold text-[#2a79b7] uppercase mb-4 flex items-center gap-2">
//           <User className="w-4 h-4" /> 1. Datos del Paciente
//         </h3>
//         <PatientSelector
//           value={control._formValues.pacienteId} // Leemos el valor actual del form
//           onChange={(id) => setValue('pacienteId', id, { shouldValidate: true })}
//           error={errors.pacienteId?.message}
//         />
//         {errors.pacienteId && <p className="text-red-500 text-xs mt-2 font-medium">{errors.pacienteId.message}</p>}
//       </div> */}


//       {/* SECCIÓN 1: Selección del Paciente */}
//       <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
//         <div className="flex items-center justify-between mb-4">
//           <h3 className="text-sm font-bold text-[#2a79b7] uppercase flex items-center gap-2">
//             <User className="w-4 h-4" /> 1. Datos del Paciente
//           </h3>
//           <button
//             type="button"
//             onClick={() => setIsNewPatientModalOpen(true)}
//             className="flex items-center gap-2 px-3 py-1.5 text-xs font-bold text-[#2a79b7] bg-[#eef5fa] hover:bg-[#dce5ef] rounded-md transition-colors"
//           >
//             <UserPlus className="w-4 h-4" />
//             NUEVO PACIENTE
//           </button>
//         </div>
        
//         <PatientSelector
//           value={control._formValues.pacienteId}
//           onChange={(id) => {
//             setValue('pacienteId', id);
//             trigger('pacienteId'); // Limpia los errores al instante
//           }}
//           error={errors.pacienteId?.message}
//         />
//         {errors.pacienteId && <p className="text-red-500 text-xs mt-2 font-medium">{errors.pacienteId.message}</p>}
//       </div>

//       {/* SECCIÓN 2: Catálogo Disponible */}
//       <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
//         <h3 className="text-sm font-bold text-[#2a79b7] uppercase mb-4 flex items-center gap-2">
//           <FlaskConical className="w-4 h-4" /> 2. Catálogo Disponible
//         </h3>
//         {/* Le quitamos la altura fija gigante y dejamos que el ExamSelector maneje su propio scroll interno compactamente */}
//         <div>
//           <ExamSelector
//             onAddExam={handleAddExam}
//             onAddPanel={handleAddPanel}
//             selectedExamIds={currentExamIds}
//             selectedPanelIds={currentPanelIds}
//           />
//         </div>
//       </div>

//       {/* SECCIÓN 3: Resumen de la Orden */}
//       <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
//         <h3 className="text-sm font-bold text-[#2a79b7] uppercase mb-4 flex items-center gap-2">
//           <ClipboardList className="w-4 h-4" /> 3. Resumen de la Orden
//         </h3>
//         {/* Contenedor compacto que crece con el contenido pero tiene un límite visual */}
//         <div className="bg-[#f8fafc] border border-[#dce5ef] rounded p-4 max-h-100 overflow-y-auto">
//             <OrderExamList 
//               formData={formData as OrderFormData} 
//               onRemoveExam={handleRemoveExam} 
//               onRemovePanel={handleRemovePanel} 
//               onToggleExclusion={handleToggleExclusion}
//             />
//         </div>
//         {errors.examenIds && <p className="text-red-500 text-xs mt-2 font-medium">{errors.examenIds.message}</p>}
//       </div>

//       {/* Botones de Acción */}
//       <div className="flex justify-end gap-3 pt-4 border-t border-[#dce5ef]">
//         <button
//           type="button"
//           onClick={onCancel}
//           className="px-6 py-2.5 text-[#4e6681] bg-white border border-[#dce5ef] hover:bg-[#f3f7fb] rounded-lg font-medium transition-colors"
//         >
//           Cancelar
//         </button>
//         <button
//           type="submit"
//           disabled={isLoading}
//           className="flex items-center gap-2 px-8 py-2.5 bg-[#2a79b7] text-white rounded-lg hover:bg-[#206094] transition-colors font-medium disabled:opacity-50 shadow-sm"
//         >
//           <Save className="w-4 h-4" />
//           {isLoading ? 'Generando Orden...' : 'Crear Orden Médica'}
//         </button>
//       </div>

//       {/* EL MODAL SUPERPUESTO */}
//       <QuickPatientModal
//         isOpen={isNewPatientModalOpen}
//         onClose={() => setIsNewPatientModalOpen(false)}
//         searchTerm="" 
//         onSuccess={(newPatientId) => {
//           setValue('pacienteId', newPatientId);
//           trigger('pacienteId');
//         }}
//       />
//     </form>
//   );
// }



















import { useState } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import type { SubmitHandler } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Save, User, FlaskConical, ClipboardList, UserPlus } from 'lucide-react';
import { orderSchema, type OrderFormData } from '../schemas/orderSchema';
import { usePanels } from '../../panels/hooks/usePanels';
import PatientSelector from './PatientSelector';
import ExamSelector from './ExamSelector';
import OrderExamList from './OrderExamList';
import QuickPatientModal from './QuickPatientModal';

interface OrderFormProps {
  initialValues?: Partial<OrderFormData>;
  onSubmit: SubmitHandler<OrderFormData>;
  onCancel: () => void;
  isLoading?: boolean;
  isEditing?: boolean;       
  pacienteNombre?: string;   
}

export default function OrderForm({ initialValues, onSubmit, onCancel, isLoading, isEditing, pacienteNombre }: OrderFormProps) {
  const {
    register,
    control,
    handleSubmit,
    setValue,
    getValues,
    trigger,
    formState: { errors },
  } = useForm<OrderFormData>({
    resolver: zodResolver(orderSchema),
    defaultValues: {
      pacienteId: '',
      examenIds: [],
      panelesSolicitados: [],
      ...initialValues,
    },
  });

  const [isNewPatientModalOpen, setIsNewPatientModalOpen] = useState(false);

  // Usamos useWatch para monitorear el estado completo sin forzar re-renders excesivos
  const formData = useWatch({ control });

  // Observamos los arreglos para pasarlos al selector (así sabe qué pintar de azul)
  const currentExamIds = useWatch({ control, name: 'examenIds' }) || [];
  const currentPanels = useWatch({ control, name: 'panelesSolicitados' }) || [];
  const currentPanelIds = currentPanels.map((p) => p.panelId);

  // Traemos los paneles para poder inspeccionar sus exámenes internos
  const { data: panels } = usePanels();

  const handleAddExam = (examenId: string) => {
    const currentExams = getValues('examenIds');
    const currentPanels = getValues('panelesSolicitados');

    // Validación extra: No dejar agregar un examen suelto si ya está en un panel seleccionado (y no está excluido)
    let isAlreadyInPanel = false;
    for (const p of currentPanels) {
      const panelInfo = panels?.find(panel => panel.id === p.panelId);
      if (panelInfo?.examenes.some(e => e.id === examenId) && !p.examenesExcluidosIds.includes(examenId)) {
        isAlreadyInPanel = true;
        break;
      }
    }

    if (isAlreadyInPanel) {
      alert('Este examen ya está activo dentro de uno de los perfiles seleccionados.');
      return;
    }

    if (!currentExams.includes(examenId)) {
      setValue('examenIds', [...currentExams, examenId]);
      trigger();
    }
  };

  const handleAddPanel = (panelId: string) => {
    const currentPanels = getValues('panelesSolicitados');
    const currentExams = getValues('examenIds');

    if (!currentPanels.some((p) => p.panelId === panelId)) {
      // 1. Buscamos qué exámenes trae este panel
      const panelInfo = panels?.find(p => p.id === panelId);
      const examenesDelPanel = panelInfo?.examenes.map(e => e.id) || [];

      // 2. Limpiamos de la lista de "sueltos" cualquier examen que venga en este panel
      const examenesFiltrados = currentExams.filter(id => !examenesDelPanel.includes(id));

      setValue('examenIds', examenesFiltrados);
      setValue('panelesSolicitados', [...currentPanels, { panelId, examenesExcluidosIds: [] }]);
      trigger();
    }
  };

  const handleToggleExclusion = (panelId: string, examId: string) => {
    const currentPanels = getValues('panelesSolicitados');
    const updatedPanels = currentPanels.map(p => {
      if (p.panelId === panelId) {
        const isExcluded = p.examenesExcluidosIds.includes(examId);
        return {
          ...p,
          // Si está excluido, lo quitamos de la lista (lo volvemos a incluir). Si no, lo agregamos a los excluidos.
          examenesExcluidosIds: isExcluded 
            ? p.examenesExcluidosIds.filter(id => id !== examId) 
            : [...p.examenesExcluidosIds, examId]
        };
      }
      return p;
    });
    setValue('panelesSolicitados', updatedPanels, { shouldValidate: true });
  };

  // Funciones para eliminar del carrito
  const handleRemoveExam = (id: string) => {
    setValue('examenIds', currentExamIds.filter(eId => eId !== id));
    trigger();
  };

  const handleRemovePanel = (id: string) => {
    setValue('panelesSolicitados', currentPanels.filter(p => p.panelId !== id));
    trigger();
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">

      {/* SECCIÓN 1: Selección del Paciente */}
      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
        <div className="flex items-center justify-between mb-4">
          <h3 className="text-sm font-bold text-[#2a79b7] uppercase flex items-center gap-2">
            <User className="w-4 h-4" /> 1. Datos del Paciente
          </h3>
          
          {!isEditing && !formData?.pacienteId && (
            <button
              type="button"
              onClick={() => setIsNewPatientModalOpen(true)}
              className="flex items-center gap-2 px-3 py-1.5 text-xs font-bold text-[#2a79b7] bg-[#eef5fa] hover:bg-[#dce5ef] rounded-md transition-colors"
            >
              <UserPlus className="w-4 h-4" />
              NUEVO PACIENTE
            </button>
          )}
        </div>

        {isEditing ? (
          <div className="bg-[#f8fafc] p-4 rounded-lg border border-[#dce5ef] flex items-center gap-3">
            <input type="hidden" {...register('pacienteId')} />
            <div className="p-2 bg-white rounded-md border border-[#dce5ef]">
              <User className="w-5 h-5 text-[#64768b]" />
            </div>
            <div>
              <p className="text-sm font-bold text-[#15243b]">{pacienteNombre || 'Paciente seleccionado'}</p>
              <p className="text-xs text-[#64768b]">No es posible modificar el paciente de una orden existente.</p>
            </div>
          </div>
        ) : (
          <>
            <PatientSelector
              value={control._formValues.pacienteId}
              onChange={(id) => {
                setValue('pacienteId', id);
                trigger('pacienteId'); 
              }}
              error={errors.pacienteId?.message}
            />
            {errors.pacienteId && <p className="text-red-500 text-xs mt-2 font-medium">{errors.pacienteId.message}</p>}
          </>
        )}
      </div>

      {/* SECCIÓN 2: Catálogo Disponible */}
      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
        <h3 className="text-sm font-bold text-[#2a79b7] uppercase mb-4 flex items-center gap-2">
          <FlaskConical className="w-4 h-4" /> 2. Catálogo Disponible
        </h3>
        <div>
          <ExamSelector
            onAddExam={handleAddExam}
            onAddPanel={handleAddPanel}
            selectedExamIds={currentExamIds}
            selectedPanelIds={currentPanelIds}
          />
        </div>
      </div>

      {/* SECCIÓN 3: Resumen de la Orden */}
      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
        <h3 className="text-sm font-bold text-[#2a79b7] uppercase mb-4 flex items-center gap-2">
          <ClipboardList className="w-4 h-4" /> 3. Resumen de la Orden
        </h3>
        <div className="bg-[#f8fafc] border border-[#dce5ef] rounded p-4 max-h-100 overflow-y-auto">
            <OrderExamList 
              formData={formData as OrderFormData} 
              onRemoveExam={handleRemoveExam} 
              onRemovePanel={handleRemovePanel} 
              onToggleExclusion={handleToggleExclusion}
            />
        </div>
        {errors.examenIds && <p className="text-red-500 text-xs mt-2 font-medium">{errors.examenIds.message}</p>}
      </div>

      {/* Botones de Acción */}
      <div className="flex justify-end gap-3 pt-4 border-t border-[#dce5ef]">
        <button
          type="button"
          onClick={onCancel}
          className="px-6 py-2.5 text-[#4e6681] bg-white border border-[#dce5ef] hover:bg-[#f3f7fb] rounded-lg font-medium transition-colors"
        >
          Cancelar
        </button>
        {/* <button
          type="submit"
          disabled={isLoading}
          className="flex items-center gap-2 px-8 py-2.5 bg-[#2a79b7] text-white rounded-lg hover:bg-[#206094] transition-colors font-medium disabled:opacity-50 shadow-sm"
        >
          <Save className="w-4 h-4" />
          {isLoading ? 'Generando Orden...' : 'Crear Orden Médica'}
        </button> */}
        <button
          type="submit"
          disabled={isLoading}
          className="flex items-center gap-2 px-8 py-2.5 bg-[#2a79b7] text-white rounded-lg hover:bg-[#206094] transition-colors font-medium disabled:opacity-50 shadow-sm"
        >
          <Save className="w-4 h-4" />
          {isLoading ? 'Guardando...' : (isEditing ? 'Actualizar Orden' : 'Crear Orden Médica')}
        </button>
      </div>

      {/* EL MODAL SUPERPUESTO */}
      <QuickPatientModal
        isOpen={isNewPatientModalOpen}
        onClose={() => setIsNewPatientModalOpen(false)}
        searchTerm="" 
        onSuccess={(newPatientId) => {
          setValue('pacienteId', newPatientId);
          trigger('pacienteId');
        }}
      />
    </form>
  );
}
