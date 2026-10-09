import { useNavigate } from 'react-router-dom';
import { ArrowLeft } from 'lucide-react';
import { ROUTES } from '../../../routes/routeConfig';
import PatientForm from '../components/PatientForm';
import { usePatientMutations } from '../hooks/usePatientMutations';
import type { PatientFormInputs } from '../schemas/patientSchema';
import { AxiosError } from 'axios';
import { useState } from 'react';

export default function CreatePatientPage() {
  const navigate = useNavigate();
  const { createPatient } = usePatientMutations();

  // Nuevo estado para controlar el error
  const [serverError, setServerError] = useState<string | null>(null);

  const handleCreate = (data: PatientFormInputs) => {
    // La mutación enviará el POST a /api/pacientes
    createPatient.mutate(data, {
      onSuccess: () => {
        // Redirige a la tabla si el backend responde 201 Created
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
    });
  };

  return (
    <div className="max-w-5xl mx-auto space-y-6">
      {/* Cabecera */}
      <div className="flex items-center gap-4">
        <button
          onClick={() => navigate(ROUTES.PATIENTS)}
          className="p-2 text-[#64768b] hover:text-[#15243b] hover:bg-[#eef5fa] rounded-lg transition-colors"
        >
          <ArrowLeft className="w-5 h-5" />
        </button>
        <div>
          <h1 className="text-2xl font-bold text-[#15243b]">Nuevo Paciente</h1>
          <p className="text-[#64768b] mt-1">Registra un nuevo paciente en el sistema.</p>
        </div>
      </div>

      {/* Formulario */}
      <PatientForm 
        onSubmit={handleCreate} 
        isLoading={createPatient.isPending}
        serverError={serverError}
       />
    </div>
  );
}