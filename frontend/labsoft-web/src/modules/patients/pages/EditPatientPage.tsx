import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft } from 'lucide-react';
import { ROUTES } from '../../../routes/routeConfig';
import PatientForm from '../components/PatientForm';
import { usePatient } from '../hooks/usePatient';
import { usePatientMutations } from '../hooks/usePatientMutations';
import type { PatientFormInputs } from '../schemas/patientSchema';
import { AxiosError } from 'axios';
import { useState } from 'react';

export default function EditPatientPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  
  // 1. Buscamos los datos actuales del paciente
  const { data: patient, isLoading: isLoadingPatient, isError } = usePatient(id);
  
  // 2. Preparamos la mutación para actualizar
  const { updatePatient } = usePatientMutations();

  // Nuevo estado para controlar el error
  const [serverError, setServerError] = useState<string | null>(null);

  const handleUpdate = (formData: PatientFormInputs) => {
    if (!id) return;

    // Limpiamos los strings vacíos que van hacia propiedades Guid? en C#
    const payload = {
      ...formData,
      titularId: formData.titularId === '' ? undefined : formData.titularId,
      // Si el backend se queja de un campo UUID opcional vacío, lo volvemos undefined aquí.
    };
    
    // Y enviamos el payload limpio:
    updatePatient.mutate(
      { id, data: payload},
      {
        onSuccess: () => {
          // Si todo sale bien, volvemos a la tabla
          navigate(ROUTES.PATIENTS);
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
        
        // EN LUGAR DEL ALERT, ACTUALIZAMOS EL ESTADO
        setServerError(errorMessage);
      },
      }
    );
  };

  if (isLoadingPatient) {
    return <div className="p-8 text-center text-[#64768b]">Cargando datos del paciente...</div>;
  }

  if (isError || !patient) {
    return (
      <div className="p-8 text-center">
        <p className="text-red-500 mb-4">No se pudo cargar la información del paciente.</p>
        <button onClick={() => navigate(ROUTES.PATIENTS)} className="text-[#2a79b7] underline">
          Volver a la lista
        </button>
      </div>
    );
  }

  // Preparamos los valores iniciales con la fecha formateada y los datos limpios asegurando que los nulos se conviertan en strings vacíos
  const initialValues = {
    ...patient,
    fechaNacimiento: patient?.fechaNacimiento ? patient.fechaNacimiento.split('T')[0] : '',
    
    // Limpieza explícita y Type-Safe
    cedula: patient?.cedula ?? '',
    telefono: patient?.telefono ?? '',
    telefonoAlternativo: patient?.telefonoAlternativo ?? '',
    correo: patient?.correo ?? '',
    correoAlternativo: patient?.correoAlternativo ?? '',
    domicilio: patient?.domicilio ?? '',
    notaMedica: patient?.notaMedica ?? '',
    numeroAfiliado: patient?.numeroAfiliado ?? '',
    titularId: patient?.titularId ?? '',
    parentesco: patient?.parentesco ?? undefined, // Zod prefiere undefined para enums opcionales
  };

  return (
    <div className="max-w-5xl mx-auto space-y-6">
      <div className="flex items-center gap-4">
        <button
          onClick={() => navigate(ROUTES.PATIENTS)}
          className="p-2 text-[#64768b] hover:text-[#15243b] hover:bg-[#eef5fa] rounded-lg transition-colors"
        >
          <ArrowLeft className="w-5 h-5" />
        </button>
        <div>
          <h1 className="text-2xl font-bold text-[#15243b]">Editar Paciente</h1>
          <p className="text-[#64768b] mt-1">Actualiza la información de {patient.nombre} {patient.apellido}.</p>
        </div>
      </div>

      <PatientForm 
        initialValues={initialValues} 
        onSubmit={handleUpdate} 
        isLoading={updatePatient.isPending} 
        serverError={serverError}
      />
    </div>
  );
}