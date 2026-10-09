// import { useEffect, useState } from 'react';
// import { useForm, useWatch } from 'react-hook-form';
// import { zodResolver } from '@hookform/resolvers/zod';
// import { X, Save, UserPlus, Baby, Search } from 'lucide-react';
// import { usePatientMutations } from '../../patients/hooks/usePatientMutations';
// import { usePatients } from '../../patients/hooks/usePatients';
// import { patientSchema, type PatientFormInputs } from '../../patients/schemas/patientSchema';

// interface QuickPatientModalProps {
//   isOpen: boolean;
//   onClose: () => void;
//   onSuccess: (newPatientId: string) => void;
//   searchTerm: string;
// }

// export default function QuickPatientModal({ isOpen, onClose, onSuccess, searchTerm }: QuickPatientModalProps) {
//   const { createPatient } = usePatientMutations();
  
//   const { data: pagedPatients } = usePatients({ pageNumber: 1, pageSize: 1000 });
//   const existingPatients = pagedPatients?.items || [];

//   // Estados para el nuevo buscador de representantes
//   const [titularSearch, setTitularSearch] = useState('');
//   const [isTitularDropdownOpen, setIsTitularDropdownOpen] = useState(false);

//   const {
//     register,
//     handleSubmit,
//     reset,
//     control,
//     setError,
//     setValue,
//     formState: { errors },
//   } = useForm<PatientFormInputs>({
//     resolver: zodResolver(patientSchema),
//     defaultValues: {
//       esRecienNacido: false,
//       sexo: 'Masculino',
//       cedula: '',
//       nombre: '',
//       apellido: '',
//       fechaNacimiento: '',
//       telefono: '',
//       titularId: '',
//       parentesco: 'Hijo',
//     }
//   });

//   const esRecienNacido = useWatch({ control, name: 'esRecienNacido' });
//   const sexoActual = useWatch({ control, name: 'sexo' });
//   const titularIdActual = useWatch({ control, name: 'titularId' });

//   // Encontrar al titular seleccionado para mostrarlo como "tarjeta"
//   const selectedTitular = existingPatients.find(p => p.id === titularIdActual);

//   // Filtrar titulares en tiempo real
//   const filteredTitulares = existingPatients.filter(p => 
//     (p.cedula && p.cedula.includes(titularSearch)) ||
//     p.nombre.toLowerCase().includes(titularSearch.toLowerCase()) ||
//     p.apellido.toLowerCase().includes(titularSearch.toLowerCase())
//   );

//   useEffect(() => {
//     if (esRecienNacido) {
//       setValue('cedula', '');
//       setValue('parentesco', sexoActual === 'Femenino' ? 'Hija' : 'Hijo');
//     } else {
//       setValue('titularId', '');
//     }
//   }, [esRecienNacido, sexoActual, setValue]);

//   useEffect(() => {
//     if (isOpen) {
//       const isNumeric = /^\d+$/.test(searchTerm.trim());
//       const terms = searchTerm.trim().split(' ');
//       reset({
//         esRecienNacido: false,
//         cedula: isNumeric ? searchTerm.trim() : '',
//         nombre: !isNumeric ? terms[0] || '' : '',
//         apellido: !isNumeric ? terms.slice(1).join(' ') || '' : '',
//         sexo: 'Masculino',
//         fechaNacimiento: '',
//         telefono: '',
//         titularId: '',
//       });
//     }
//   }, [isOpen, searchTerm, reset]);

//   // Limpiamos los estados locales al cerrar para evitar el error de "cascading renders"
//   const handleCloseModal = () => {
//     setTitularSearch('');
//     setIsTitularDropdownOpen(false);
//     onClose();
//   };

//   if (!isOpen) return null;

//   const onSubmit = (data: PatientFormInputs) => {
//     if (!data.esRecienNacido && data.cedula) {
//       const cedulaRepetida = existingPatients.some(p => p.cedula === data.cedula);
//       if (cedulaRepetida) {
//         setError('cedula', { type: 'manual', message: 'Esta cédula ya está registrada.' });
//         return;
//       }
//     }

//     if (data.esRecienNacido && !data.titularId) {
//       setError('titularId', { type: 'manual', message: 'Debe asignar un representante al recién nacido.' });
//       return;
//     }

//     type MutateVariables = Parameters<typeof createPatient.mutate>[0];
    
//     createPatient.mutate(data as unknown as MutateVariables, {
//       onSuccess: (response: unknown) => {
//         const safeResponse = response as { id?: string; data?: { id?: string } };
//         const newId = safeResponse?.id || safeResponse?.data?.id;
        
//         if (newId) onSuccess(newId);
//         handleCloseModal();
//       },
//     });
//   };

//   return (
//     <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-[#15243b]/40 backdrop-blur-sm animate-in fade-in duration-200">
//       <div className="bg-white rounded-xl shadow-xl w-full max-w-2xl overflow-hidden animate-in zoom-in-95 duration-200">
        
//         <div className="flex items-center justify-between px-6 py-4 border-b border-[#dce5ef] bg-[#f8fafc]">
//           <div className="flex items-center gap-2 text-[#15243b]">
//             <UserPlus className="w-5 h-5 text-[#2a79b7]" />
//             <h2 className="text-base font-bold uppercase tracking-wide">Registro Rápido de Paciente</h2>
//           </div>
//           <button type="button" onClick={handleCloseModal} className="text-[#64768b] hover:text-red-500 transition-colors">
//             <X className="w-5 h-5" />
//           </button>
//         </div>

//         <form onSubmit={handleSubmit(onSubmit)}>
//           <div className="p-6 grid grid-cols-1 md:grid-cols-2 gap-5 min-h-75">
            
//             <div className="md:col-span-2 flex items-center justify-between p-4 bg-[#f3f7fb] border border-[#dce5ef] rounded-lg">
//               <div className="flex items-center gap-3">
//                 <div className={`p-2 rounded-lg ${esRecienNacido ? 'bg-[#2a79b7] text-white' : 'bg-white text-[#64768b] border border-[#dce5ef]'}`}>
//                   <Baby className="w-5 h-5" />
//                 </div>
//                 <div>
//                   <p className="text-sm font-bold text-[#15243b]">Paciente Recién Nacido</p>
//                   <p className="text-xs text-[#64768b]">Se ocultará la cédula y se exigirá un representante.</p>
//                 </div>
//               </div>
//               <label className="relative inline-flex items-center cursor-pointer">
//                 <input type="checkbox" className="sr-only peer" {...register('esRecienNacido')} />
//                 <div className="w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-0.5 after:left-0.5 after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-[#2a79b7]"></div>
//               </label>
//             </div>

//             {esRecienNacido ? (
//               <>
//                 {/* BUSCADOR DE REPRESENTANTE */}
//                 <div className="relative">
//                   <label className="block text-xs font-bold text-[#2a79b7] mb-1 uppercase">Representante (Titular) *</label>
                  
//                   {selectedTitular ? (
//                     <div className="flex items-center justify-between px-3 py-2.5 border border-[#2a79b7] bg-[#eef5fa] rounded-lg">
//                       <span className="text-sm font-bold text-[#15243b] truncate">
//                         {selectedTitular.nombre} {selectedTitular.apellido}
//                         <span className="font-normal text-[#64768b] ml-1">(C.I: {selectedTitular.cedula})</span>
//                       </span>
//                       <button 
//                         type="button" 
//                         onClick={() => setValue('titularId', '', { shouldValidate: true })}
//                         className="text-red-400 hover:text-red-600 transition-colors"
//                       >
//                         <X className="w-4 h-4" />
//                       </button>
//                     </div>
//                   ) : (
//                     <div className="relative">
//                       <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-[#94a3b8]" />
//                       <input
//                         type="text"
//                         placeholder="Buscar por cédula o nombre..."
//                         value={titularSearch}
//                         onChange={e => {
//                           setTitularSearch(e.target.value);
//                           setIsTitularDropdownOpen(true);
//                         }}
//                         onFocus={() => setIsTitularDropdownOpen(true)}
//                         onBlur={() => setTimeout(() => setIsTitularDropdownOpen(false), 200)}
//                         className="w-full pl-9 pr-4 py-2.5 border border-[#2a79b7] rounded-lg text-sm focus:outline-none focus:ring-1 focus:ring-[#2a79b7]"
//                       />
                      
//                       {isTitularDropdownOpen && (
//                         <div className="absolute z-50 w-full mt-1 bg-white border border-[#dce5ef] rounded-lg shadow-lg max-h-40 overflow-y-auto">
//                           {filteredTitulares.length === 0 ? (
//                             <p className="p-3 text-xs text-center text-[#64768b]">No se encontraron pacientes.</p>
//                           ) : (
//                             filteredTitulares.map(p => (
//                               <button
//                                 key={p.id}
//                                 type="button"
//                                 onClick={() => {
//                                   setValue('titularId', p.id, { shouldValidate: true });
//                                   setTitularSearch('');
//                                   setIsTitularDropdownOpen(false);
//                                 }}
//                                 className="w-full text-left px-3 py-2 hover:bg-[#f3f7fb] text-sm text-[#15243b] transition-colors border-b border-[#f1f5f9] last:border-0"
//                               >
//                                 <span className="font-bold">{p.cedula}</span> - {p.nombre} {p.apellido}
//                               </button>
//                             ))
//                           )}
//                         </div>
//                       )}
//                     </div>
//                   )}
//                   {errors.titularId && <p className="text-red-500 text-xs mt-1 font-medium">{errors.titularId.message}</p>}
//                 </div>
                
//                 <div>
//                   <label className="block text-xs font-bold text-[#2a79b7] mb-1 uppercase">Parentesco *</label>
//                   <select
//                     {...register('parentesco')}
//                     className="w-full px-4 py-2.5 border border-[#2a79b7] bg-[#eef5fa] rounded-lg text-sm focus:outline-none font-medium text-[#15243b]"
//                   >
//                     <option value="Hijo">Hijo</option>
//                     <option value="Hija">Hija</option>
//                     <option value="Otro">Otro</option>
//                   </select>
//                 </div>
//               </>
//             ) : (
//               <div>
//                 <label className="block text-xs font-bold text-[#15243b] mb-1 uppercase">Cédula *</label>
//                 <input
//                   {...register('cedula')}
//                   placeholder="Ej. 25123456"
//                   className="w-full px-4 py-2.5 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none font-medium"
//                 />
//                 {errors.cedula && <p className="text-red-500 text-xs mt-1 font-medium">{errors.cedula.message}</p>}
//               </div>
//             )}

//             <div>
//               <label className="block text-xs font-bold text-[#15243b] mb-1 uppercase">Teléfono de Emergencia</label>
//               <input
//                 {...register('telefono')}
//                 placeholder="Ej. 0414-1234567"
//                 className="w-full px-4 py-2.5 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none font-medium"
//               />
//               {errors.telefono && <p className="text-red-500 text-xs mt-1 font-medium">{errors.telefono.message}</p>}
//             </div>

//             <div>
//               <label className="block text-xs font-bold text-[#15243b] mb-1 uppercase">Nombres *</label>
//               <input
//                 {...register('nombre')}
//                 className="w-full px-4 py-2.5 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none font-medium"
//               />
//               {errors.nombre && <p className="text-red-500 text-xs mt-1 font-medium">{errors.nombre.message}</p>}
//             </div>

//             <div>
//               <label className="block text-xs font-bold text-[#15243b] mb-1 uppercase">Apellidos *</label>
//               <input
//                 {...register('apellido')}
//                 className="w-full px-4 py-2.5 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none font-medium"
//               />
//               {errors.apellido && <p className="text-red-500 text-xs mt-1 font-medium">{errors.apellido.message}</p>}
//             </div>

//             <div>
//               <label className="block text-xs font-bold text-[#15243b] mb-1 uppercase">Sexo *</label>
//               <select
//                 {...register('sexo')}
//                 className="w-full px-4 py-2.5 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none bg-white font-medium"
//               >
//                 <option value="Masculino">Masculino</option>
//                 <option value="Femenino">Femenino</option>
//               </select>
//               {errors.sexo && <p className="text-red-500 text-xs mt-1 font-medium">{errors.sexo.message}</p>}
//             </div>

//             <div>
//               <label className="block text-xs font-bold text-[#15243b] mb-1 uppercase">Fecha de Nacimiento *</label>
//               <input
//                 type="date"
//                 {...register('fechaNacimiento')}
//                 className="w-full px-4 py-2.5 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none font-medium"
//               />
//               {errors.fechaNacimiento && <p className="text-red-500 text-xs mt-1 font-medium">{errors.fechaNacimiento.message}</p>}
//             </div>
            
//           </div>

//           <div className="bg-[#f8fafc] px-6 py-4 flex justify-end gap-3 border-t border-[#dce5ef]">
//             <button
//               type="button"
//               onClick={handleCloseModal}
//               className="px-6 py-2.5 text-sm font-medium text-[#4e6681] bg-white border border-[#dce5ef] rounded-lg hover:bg-[#f3f7fb] transition-colors"
//             >
//               Cancelar
//             </button>
//             <button
//               type="submit"
//               disabled={createPatient.isPending}
//               className="flex items-center gap-2 px-6 py-2.5 text-sm font-medium text-white bg-[#2a79b7] rounded-lg hover:bg-[#206094] transition-colors disabled:opacity-50 shadow-sm"
//             >
//               <Save className="w-4 h-4" />
//               {createPatient.isPending ? 'Guardando...' : 'Guardar y Seleccionar'}
//             </button>
//           </div>
//         </form>

//       </div>
//     </div>
//   );
// }































import { useEffect, useState } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { X, Save, UserPlus, Baby, Search, AlertTriangle } from 'lucide-react';
import { usePatientMutations } from '../../patients/hooks/usePatientMutations';
import { usePatients } from '../../patients/hooks/usePatients';
import { patientSchema, type PatientFormInputs } from '../../patients/schemas/patientSchema';
import { AxiosError } from 'axios';

interface QuickPatientModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: (newPatientId: string) => void;
  searchTerm: string;
}

export default function QuickPatientModal({ isOpen, onClose, onSuccess, searchTerm }: QuickPatientModalProps) {
  const { createPatient } = usePatientMutations();
  const { data: pagedPatients } = usePatients({ pageNumber: 1, pageSize: 1000 });
  const existingPatients = pagedPatients?.items || [];

  const [titularSearch, setTitularSearch] = useState('');
  const [isTitularDropdownOpen, setIsTitularDropdownOpen] = useState(false);

  const [serverError, setServerError] = useState<string | null>(null);

  const {
    register, handleSubmit, reset, control, setError, setValue, formState: { errors },
  } = useForm<PatientFormInputs>({
    resolver: zodResolver(patientSchema),
    defaultValues: {
      esRecienNacido: false, sexo: 'Masculino', cedula: '', nombre: '', apellido: '', fechaNacimiento: '', telefono: '', titularId: '', parentesco: 'Hijo',
    }
  });

  const esRecienNacido = useWatch({ control, name: 'esRecienNacido' });
  const sexoActual = useWatch({ control, name: 'sexo' });
  const titularIdActual = useWatch({ control, name: 'titularId' });

  const selectedTitular = existingPatients.find(p => p.id === titularIdActual);
  const filteredTitulares = existingPatients.filter(p => 
    // REGLA ESTRICTA: Para ser titular, TIENE que tener una cédula válida
    p.cedula && p.cedula.trim() !== '' &&
    (
      p.cedula.includes(titularSearch) ||
      p.nombre.toLowerCase().includes(titularSearch.toLowerCase()) ||
      p.apellido.toLowerCase().includes(titularSearch.toLowerCase())
    )
  );

  useEffect(() => {
    if (esRecienNacido) {
      setValue('cedula', '', { shouldValidate: true });
      setValue('parentesco', sexoActual === 'Femenino' ? 'Hija' : 'Hijo');
    } else {
      setValue('titularId', undefined);
    }
  }, [esRecienNacido, sexoActual, setValue]);

  useEffect(() => {
    if (isOpen) {
      const isNumeric = /^\d+$/.test(searchTerm.trim());
      const terms = searchTerm.trim().split(' ');
      reset({
        esRecienNacido: false,
        cedula: isNumeric ? searchTerm.trim() : '',
        nombre: !isNumeric ? terms[0] || '' : '',
        apellido: !isNumeric ? terms.slice(1).join(' ') || '' : '',
        sexo: 'Masculino', fechaNacimiento: '', telefono: '', titularId: '',
      });
    }
  }, [isOpen, searchTerm, reset]);

  const handleCloseModal = () => {
    setTitularSearch('');
    setIsTitularDropdownOpen(false);
    setServerError(null);
    onClose();
  };

  if (!isOpen) return null;

  const onSubmit = (data: PatientFormInputs) => {
    if (!data.esRecienNacido && data.cedula) {
      const cedulaRepetida = existingPatients.some(p => p.cedula === data.cedula);
      if (cedulaRepetida) {
        setError('cedula', { type: 'manual', message: 'Esta cédula ya está registrada.' });
        return;
      }
    }
    if (data.esRecienNacido && !data.titularId) {
      setError('titularId', { type: 'manual', message: 'Debe asignar un representante legal.' });
      return;
    }

    const payloadToSubmit = {
      ...data,
      titularId: data.titularId === '' ? undefined : data.titularId,
      parentesco: data.esRecienNacido ? data.parentesco : 'Titular',
    };

    setServerError(null);
    createPatient.mutate(payloadToSubmit as unknown as Parameters<typeof createPatient.mutate>[0], {
      onSuccess: (response: unknown) => {
        // Le decimos a TypeScript la forma exacta que esperamos recibir
        const res = response as { data?: { id?: string }; id?: string };
        const newId = res?.data?.id || res?.id;
        
        if (newId) onSuccess(newId);
        handleCloseModal();
      },
      onError: (error: unknown) => {
        let errorMessage = 'Ocurrió un error inesperado al guardar el paciente.';
        
        // Verificamos si es un error que viene del backend (Axios)
        if (error instanceof AxiosError) {
          // Extraemos el mensaje específico que enviamos desde el Controlador de C#
          errorMessage = error.response?.data?.message || error.message;
        } 
        // Verificamos si es un error general de Javascript
        else if (error instanceof Error) {
          errorMessage = error.message;
        }
        
        // SIN ALERT. Actualizamos el estado.
        setServerError(errorMessage);
      }
    });
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLDivElement>) => {
    if (e.key === 'Enter') { e.preventDefault(); handleSubmit(onSubmit)(); }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-[#15243b]/60 backdrop-blur-sm animate-in fade-in duration-200">
      <div className="bg-white rounded-xl shadow-2xl w-full max-w-2xl max-h-[90vh] flex flex-col overflow-hidden animate-in zoom-in-95 duration-200">
        
        {/* Header del Modal */}
        <div className="flex items-center justify-between px-6 py-4 border-b border-[#dce5ef] bg-[#f8fafc] shrink-0">
          <div className="flex items-center gap-3">
            <div className="p-2 bg-[#2a79b7]/10 text-[#2a79b7] rounded-lg">
              <UserPlus className="w-5 h-5" />
            </div>
            <h2 className="text-base font-bold text-[#15243b] tracking-wide">Registro Rápido de Paciente</h2>
          </div>
          <button type="button" onClick={handleCloseModal} className="text-[#94a3b8] hover:text-red-500 transition-colors">
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* Cuerpo con Scroll */}
        <div className="overflow-y-auto flex-1 p-6" onKeyDown={handleKeyDown}>

          {/* BANNER DE ERROR */}
          {serverError && (
            <div className="mb-6 bg-red-50 border-l-4 border-red-500 p-4 rounded-r-lg shadow-sm animate-in fade-in">
              <div className="flex items-start gap-3">
                <AlertTriangle className="w-5 h-5 text-red-500 shrink-0 mt-0.5" />
                <div>
                  <h3 className="text-sm font-bold text-red-800">No se pudo guardar</h3>
                  <p className="text-sm text-red-700 mt-1">{serverError}</p>
                </div>
              </div>
            </div>
          )}
          
          {/* Tarjeta del Switch (Menor sin ID) */}
          <div className="flex items-center justify-between p-4 mb-6 bg-[#f8fafc] border border-[#dce5ef] rounded-xl transition-colors">
            <div className="flex items-center gap-3">
              <div className={`p-2.5 rounded-lg transition-colors ${esRecienNacido ? 'bg-[#2a79b7] text-white' : 'bg-white text-[#64768b] border border-[#dce5ef] shadow-sm'}`}>
                <Baby className="w-5 h-5" />
              </div>
              <div>
                <p className="text-sm font-bold text-[#15243b]">Menor sin Identificación</p>
                <p className="text-xs text-[#64768b] mt-0.5">Oculta la cédula y exige vincular a un representante legal.</p>
              </div>
            </div>
            <label className="relative inline-flex items-center cursor-pointer">
              <input type="checkbox" className="sr-only peer" {...register('esRecienNacido')} />
              <div className="w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-0.5 after:left-0.5 after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-[#2a79b7]"></div>
            </label>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
            {esRecienNacido ? (
              <>
                <div className="relative md:col-span-1">
                  <label className="block text-[11px] font-bold text-[#2a79b7] uppercase tracking-wider mb-1.5">Representante (Titular) *</label>
                  {selectedTitular ? (
                    <div className="flex items-center justify-between px-3 py-2.5 border border-[#2a79b7] bg-[#eef5fa] rounded-lg">
                      <span className="text-sm font-bold text-[#15243b] truncate">
                        {selectedTitular.nombre} {selectedTitular.apellido}
                      </span>
                      <button type="button" onClick={() => setValue('titularId', undefined, { shouldValidate: true })} className="text-red-400 hover:text-red-600 transition-colors">
                        <X className="w-4 h-4" />
                      </button>
                    </div>
                  ) : (
                    <div className="relative">
                      <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-[#94a3b8]" />
                      <input
                        type="text"
                        placeholder="Buscar titular..."
                        value={titularSearch}
                        onChange={e => { setTitularSearch(e.target.value); setIsTitularDropdownOpen(true); }}
                        onFocus={() => setIsTitularDropdownOpen(true)}
                        onBlur={() => setTimeout(() => setIsTitularDropdownOpen(false), 200)}
                        className="w-full pl-9 pr-4 py-2 border border-[#2a79b7] rounded-lg text-sm focus:outline-none focus:ring-1 focus:ring-[#2a79b7]"
                      />
                      {isTitularDropdownOpen && (
                        <div className="absolute z-50 w-full mt-1 bg-white border border-[#dce5ef] rounded-lg shadow-lg max-h-40 overflow-y-auto">
                          {filteredTitulares.length === 0 ? (
                            <p className="p-3 text-xs text-center text-[#64768b]">No encontrado.</p>
                          ) : (
                            filteredTitulares.map(p => (
                              <button
                                key={p.id} type="button"
                                onMouseDown={(e) => {
                                  e.preventDefault();
                                  setValue('titularId', p.id, { shouldValidate: true });
                                  setTitularSearch(''); setIsTitularDropdownOpen(false);
                                }}
                                className="w-full text-left px-3 py-2 hover:bg-[#f3f7fb] text-sm text-[#15243b] border-b border-[#f1f5f9] last:border-0"
                              >
                                <span className="font-bold">{p.cedula}</span> - {p.nombre} {p.apellido}
                              </button>
                            ))
                          )}
                        </div>
                      )}
                    </div>
                  )}
                  {errors.titularId && <p className="text-red-500 text-[10px] font-bold uppercase mt-1">{errors.titularId.message}</p>}
                </div>

                <div>
                  <label className="block text-[11px] font-bold text-[#2a79b7] uppercase tracking-wider mb-1.5">Parentesco *</label>
                  <select {...register('parentesco')} className="w-full px-4 py-2 border border-[#2a79b7] bg-[#eef5fa] rounded-lg text-sm focus:outline-none font-medium text-[#15243b]">
                    <option value="Hijo">Hijo</option>
                    <option value="Hija">Hija</option>
                    <option value="Otro">Otro</option>
                  </select>
                </div>
              </>
            ) : (
              <div>
                <label className="block text-[11px] font-bold text-[#64768b] uppercase tracking-wider mb-1.5">Cédula *</label>
                <input {...register('cedula')} placeholder="Ej. 25123456" className="w-full px-4 py-2 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none font-medium" />
                {errors.cedula && <p className="text-red-500 text-[10px] font-bold uppercase mt-1">{errors.cedula.message}</p>}
              </div>
            )}

            {!esRecienNacido && (
              <div>
                <label className="block text-[11px] font-bold text-[#64768b] uppercase tracking-wider mb-1.5">Tel. Emergencia</label>
                <input {...register('telefono')} placeholder="Ej. 0414-1234567" className="w-full px-4 py-2 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none font-medium" />
              </div>
            )}

            <div>
              <label className="block text-[11px] font-bold text-[#64768b] uppercase tracking-wider mb-1.5">Nombres *</label>
              <input {...register('nombre')} className="w-full px-4 py-2 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none font-medium" />
              {errors.nombre && <p className="text-red-500 text-[10px] font-bold uppercase mt-1">{errors.nombre.message}</p>}
            </div>

            <div>
              <label className="block text-[11px] font-bold text-[#64768b] uppercase tracking-wider mb-1.5">Apellidos *</label>
              <input {...register('apellido')} className="w-full px-4 py-2 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none font-medium" />
              {errors.apellido && <p className="text-red-500 text-[10px] font-bold uppercase mt-1">{errors.apellido.message}</p>}
            </div>

            <div>
              <label className="block text-[11px] font-bold text-[#64768b] uppercase tracking-wider mb-1.5">Sexo *</label>
              <select {...register('sexo')} className="w-full px-4 py-2 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none bg-white font-medium">
                <option value="Masculino">Masculino</option>
                <option value="Femenino">Femenino</option>
              </select>
            </div>

            <div>
              <label className="block text-[11px] font-bold text-[#64768b] uppercase tracking-wider mb-1.5">Fecha de Nacimiento *</label>
              <input type="date" {...register('fechaNacimiento')} className="w-full px-4 py-2 border border-[#dce5ef] rounded-lg text-sm focus:border-[#2a79b7] focus:outline-none font-medium" />
              {errors.fechaNacimiento && <p className="text-red-500 text-[10px] font-bold uppercase mt-1">{errors.fechaNacimiento.message}</p>}
            </div>
          </div>
        </div>

        {/* Botones Fijos Abajo */}
        <div className="bg-[#f8fafc] px-6 py-4 flex justify-end gap-3 border-t border-[#dce5ef] shrink-0">
          <button type="button" onClick={handleCloseModal} className="px-6 py-2.5 text-sm font-bold text-[#4e6681] bg-white border border-[#dce5ef] rounded-lg hover:bg-[#f3f7fb] transition-colors">
            Cancelar
          </button>
          <button type="button" onClick={handleSubmit(onSubmit)} disabled={createPatient.isPending} className="flex items-center gap-2 px-6 py-2.5 text-sm font-bold text-white bg-[#2a79b7] rounded-lg hover:bg-[#206094] transition-colors disabled:opacity-50 shadow-sm">
            <Save className="w-4 h-4" />
            {createPatient.isPending ? 'Guardando...' : 'Guardar y Seleccionar'}
          </button>
        </div>

      </div>
    </div>
  );
}