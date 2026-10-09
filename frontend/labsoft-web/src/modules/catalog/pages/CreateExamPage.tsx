import { useNavigate } from 'react-router-dom';
import { ArrowLeft, FlaskConical } from 'lucide-react';
import { AxiosError } from 'axios';
import { ROUTES } from '../../../routes/routeConfig';
import ExamForm from '../components/ExamForm';
import { useExamMutations } from '../hooks/useExamMutations';
import type { ExamFormData } from '../schemas/examSchema';

export default function CreateExamPage() {
  const navigate = useNavigate();
  const { createExam } = useExamMutations();

  const onSubmit = (data: ExamFormData) => {
    createExam.mutate(data, {
      onSuccess: () => {
        navigate(ROUTES.CATALOG);
      },
      onError: (error: unknown) => {
        let errorMessage = 'Ocurrió un error al guardar el examen.';
        
        if (error instanceof AxiosError) {
          errorMessage = error.response?.data?.message || error.message;
        } else if (error instanceof Error) {
          errorMessage = error.message;
        }

        console.error('Error al crear el examen:', errorMessage);
        alert(errorMessage);
      }
    });
  };

  return (
    <div className="max-w-4xl mx-auto space-y-6">
      {/* Cabecera */}
      <div className="flex items-center gap-4">
        <button
          onClick={() => navigate(ROUTES.CATALOG)}
          className="p-2 text-[#64768b] hover:text-[#15243b] hover:bg-[#eef5fa] rounded-lg transition-colors"
        >
          <ArrowLeft className="w-5 h-5" />
        </button>
        <div>
          <h1 className="text-2xl font-bold text-[#15243b] flex items-center gap-2">
            <FlaskConical className="w-6 h-6 text-[#2a79b7]" />
            Nuevo Examen
          </h1>
          <p className="text-[#64768b] mt-1">Registra una nueva prueba en el catálogo del laboratorio.</p>
        </div>
      </div>

      {/* Formulario */}
      <ExamForm 
        onSubmit={onSubmit} 
        onCancel={() => navigate(ROUTES.CATALOG)}
        isLoading={createExam.isPending}
      />
    </div>
  );
}