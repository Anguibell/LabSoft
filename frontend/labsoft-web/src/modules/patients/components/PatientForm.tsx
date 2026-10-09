import { useState, useEffect } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useNavigate } from 'react-router-dom';
import { Search, X, AlertTriangle } from 'lucide-react';
import { patientSchema, type PatientFormInputs } from '../schemas/patientSchema';
import { usePatients } from '../hooks/usePatients';

interface PatientFormProps {
  initialValues?: Partial<PatientFormInputs>;
  onSubmit: (data: PatientFormInputs) => void;
  isLoading: boolean;
  serverError?: string | null;
}

export default function PatientForm({ initialValues, onSubmit, isLoading, serverError }: PatientFormProps) {
  const navigate = useNavigate();
  
  const {
    register,
    handleSubmit,
    control,
    setValue, // EXTRACCIÓN CLAVE PARA PODER MODIFICAR VALORES
    formState: { errors },
  } = useForm<PatientFormInputs>({
    resolver: zodResolver(patientSchema),
    defaultValues: {
      esRecienNacido: false,
      sexo: 'Masculino', // Default útil
      ...initialValues,
    },
  });

  const isRecienNacido = useWatch({ control, name: 'esRecienNacido' });
  const sexoActual = useWatch({ control, name: 'sexo' });
  const titularIdActual = useWatch({ control, name: 'titularId' });

  // Lógica para el buscador de representantes
  const { data: pagedPatients } = usePatients({ pageNumber: 1, pageSize: 1000 });
  const existingPatients = pagedPatients?.items || [];
  
  const [titularSearch, setTitularSearch] = useState('');
  const [isTitularDropdownOpen, setIsTitularDropdownOpen] = useState(false);

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
  
  // Limpiar cédula y establecer parentesco por defecto si es recién nacido
  useEffect(() => {
    if (isRecienNacido) {
      setValue('cedula', '', { shouldValidate: true });
      setValue('parentesco', sexoActual === 'Femenino' ? 'Hija' : 'Hijo');
    } else {
      setValue('titularId', undefined); // Limpia el titular si desmarca la casilla
    }
  }, [isRecienNacido, sexoActual, setValue]);

  const handleFormSubmit = (data: PatientFormInputs) => {
    // Si es recién nacido, asegurar que el payload lleve parentesco y titular
    const payloadToSubmit = {
      ...data,
      titularId: data.titularId === '' ? undefined : data.titularId,
      parentesco: data.esRecienNacido ? data.parentesco : 'Titular',
    };
    onSubmit(payloadToSubmit as PatientFormInputs);
  };

  return (
    <form onSubmit={handleSubmit(handleFormSubmit)} className="space-y-8">

      {/* --- BANNER DE ERROR PROFESIONAL --- */}
      {serverError && (
        <div className="bg-red-50 border-l-4 border-red-500 p-4 rounded-r-lg shadow-sm animate-in fade-in slide-in-from-top-2">
          <div className="flex items-start gap-3">
            <AlertTriangle className="w-5 h-5 text-red-500 shrink-0 mt-0.5" />
            <div>
              <h3 className="text-sm font-bold text-red-800">No se pudo guardar la información</h3>
              <p className="text-sm text-red-700 mt-1">{serverError}</p>
            </div>
          </div>
        </div>
      )}
      {/* ---------------------------------- */}

      {/* Sección: Datos Personales */}
      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
        <h2 className="text-lg font-semibold text-[#15243b] mb-4 border-b border-[#dce5ef] pb-2">
          Datos Personales
        </h2>
        
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          
          <div className="col-span-full md:col-span-2 lg:col-span-3 flex items-center gap-2 mb-2">
            <input
              type="checkbox"
              id="esRecienNacido"
              {...register('esRecienNacido')}
              className="w-4 h-4 text-[#2a79b7] rounded border-[#dce5ef] focus:ring-[#2a79b7]"
            />
            <label htmlFor="esRecienNacido" className="text-sm font-medium text-[#4e6681]">
              Es recién nacido o paciente sin identificación
            </label>
          </div>

          {/* Renderizado Condicional: Titular o Cédula */}
          {isRecienNacido ? (
            <div className="col-span-full grid grid-cols-1 md:grid-cols-2 gap-6 bg-[#f8fafc] p-5 rounded-xl border border-[#dce5ef]">
              <div className="relative">
                <label className="block text-sm font-bold text-[#2a79b7] mb-1">Representante (Titular) *</label>

                {selectedTitular ? (
                  <div className="flex items-center justify-between px-4 py-2 border border-[#2a79b7] bg-[#eef5fa] rounded-lg">
                    <span className="text-sm font-bold text-[#15243b] truncate">
                      {selectedTitular.nombre} {selectedTitular.apellido}
                      <span className="font-normal text-[#64768b] ml-1">(C.I: {selectedTitular.cedula})</span>
                    </span>
                    <button 
                      type="button" 
                      onClick={() => setValue('titularId', undefined, { shouldValidate: true })}
                      className="text-red-400 hover:text-red-600 transition-colors"
                      title="Quitar titular"
                    >
                      <X className="w-4 h-4" />
                    </button>
                  </div>
                ) : (
                  <div className="relative">
                    <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-[#94a3b8]" />
                    <input
                      type="text"
                      placeholder="Buscar por cédula o nombre..."
                      value={titularSearch}
                      onChange={e => {
                        setTitularSearch(e.target.value);
                        setIsTitularDropdownOpen(true);
                      }}
                      onFocus={() => setIsTitularDropdownOpen(true)}
                      onBlur={() => setTimeout(() => setIsTitularDropdownOpen(false), 200)}
                      className="w-full pl-9 pr-4 py-2 border border-[#2a79b7] rounded-lg text-sm focus:outline-none focus:ring-1 focus:ring-[#2a79b7]"
                    />

                    {isTitularDropdownOpen && (
                      <div className="absolute z-50 w-full mt-1 bg-white border border-[#dce5ef] rounded-lg shadow-lg max-h-40 overflow-y-auto">
                        {filteredTitulares.length === 0 ? (
                          <p className="p-3 text-sm text-center text-[#64768b]">No se encontraron pacientes.</p>
                        ) : (
                          filteredTitulares.map(p => (
                            <button
                              key={p.id}
                              type="button"
                              onMouseDown={(e) => {
                                e.preventDefault(); // Evita que se dispare el onBlur antes del click
                                setValue('titularId', p.id, { shouldValidate: true });
                                setTitularSearch('');
                                setIsTitularDropdownOpen(false);
                              }}
                              className="w-full text-left px-4 py-2 hover:bg-[#f3f7fb] text-sm text-[#15243b] transition-colors border-b border-[#f1f5f9] last:border-0"
                            >
                              <span className="font-bold mr-2">{p.cedula}</span>
                              {p.nombre} {p.apellido}
                            </button>
                          ))
                        )}
                      </div>
                    )}
                  </div>
                )}
                {errors.titularId && <p className="text-red-500 text-xs mt-1 font-medium">{errors.titularId.message}</p>}
              </div>

              <div>
                <label className="block text-sm font-bold text-[#2a79b7] mb-1">Parentesco *</label>
                <select
                  {...register('parentesco')}
                  className="w-full px-4 py-2 border border-[#2a79b7] bg-[#eef5fa] rounded-lg text-sm focus:outline-none font-medium text-[#15243b]"
                >
                  <option value="Hijo">Hijo</option>
                  <option value="Hija">Hija</option>
                  <option value="Otro">Otro</option>
                </select>
              </div>
            </div>
          ) : (
            <div>
              <label className="block text-sm font-medium text-[#15243b] mb-1">Cédula / ID *</label>
              <input
                {...register('cedula')}
                placeholder="Ej. 25123456"
                className={`w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-[#2a79b7] ${
                  errors.cedula ? 'border-red-500' : 'border-[#dce5ef]'
                }`}
              />
               {errors.cedula && <p className="text-red-500 text-xs mt-1">{errors.cedula.message}</p>}
            </div>
          )}

          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">Nombre *</label>
            <input
              {...register('nombre')}
              className={`w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-[#2a79b7] ${
                errors.nombre ? 'border-red-500' : 'border-[#dce5ef]'
              }`}
            />
            {errors.nombre && <p className="text-red-500 text-xs mt-1">{errors.nombre.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">Apellido *</label>
            <input
              {...register('apellido')}
              className={`w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-[#2a79b7] ${
                errors.apellido ? 'border-red-500' : 'border-[#dce5ef]'
              }`}
            />
            {errors.apellido && <p className="text-red-500 text-xs mt-1">{errors.apellido.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">Sexo *</label>
            <select
              {...register('sexo')}
              className={`w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-[#2a79b7] bg-white ${
                errors.sexo ? 'border-red-500' : 'border-[#dce5ef]'
              }`}
            >
              <option value="Masculino">Masculino</option>
              <option value="Femenino">Femenino</option>
            </select>
            {errors.sexo && <p className="text-red-500 text-xs mt-1">{errors.sexo.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">Fecha de Nacimiento *</label>
            <input
              type="date"
              {...register('fechaNacimiento')}
              className={`w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-[#2a79b7] ${
                errors.fechaNacimiento ? 'border-red-500' : 'border-[#dce5ef]'
              }`}
            />
            {errors.fechaNacimiento && <p className="text-red-500 text-xs mt-1">{errors.fechaNacimiento.message}</p>}
          </div>

        </div>
      </div>

      {/* Sección: Contacto */}
      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm">
        <h2 className="text-lg font-semibold text-[#15243b] mb-4 border-b border-[#dce5ef] pb-2">
          Información de Contacto
        </h2>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">Teléfono Principal</label>
            <input {...register('telefono')} className="w-full px-4 py-2 border border-[#dce5ef] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#2a79b7]" />
          </div>
          <div>
            <label className="block text-sm font-medium text-[#15243b] mb-1">Correo Electrónico</label>
            <input {...register('correo')} type="email" className="w-full px-4 py-2 border border-[#dce5ef] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#2a79b7]" />
            {errors.correo && <p className="text-red-500 text-xs mt-1">{errors.correo.message}</p>}
          </div>
          <div className="md:col-span-2">
            <label className="block text-sm font-medium text-[#15243b] mb-1">Domicilio</label>
            <input {...register('domicilio')} className="w-full px-4 py-2 border border-[#dce5ef] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#2a79b7]" />
          </div>
        </div>
      </div>

      {/* Botones de acción */}
      <div className="flex justify-end gap-4">
        <button
          type="button"
          onClick={() => navigate(-1)}
          className="px-6 py-2.5 border border-[#dce5ef] text-[#4e6681] rounded-lg font-medium hover:bg-[#f8fafc] transition-colors"
        >
          Cancelar
        </button>
        <button
          type="submit"
          disabled={isLoading}
          className="px-6 py-2.5 bg-[#2a79b7] text-white rounded-lg font-medium hover:bg-[#206094] transition-colors disabled:opacity-70 shadow-sm"
        >
          {isLoading ? 'Guardando...' : 'Guardar Paciente'}
        </button>
      </div>
    </form>
  );
}