import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, FlaskConical, AlertCircle } from 'lucide-react';
import { AxiosError } from 'axios';
import { ROUTES } from '../../../routes/routeConfig';
import ExamForm from '../components/ExamForm';
import { useExam } from '../hooks/useExams';
import { useExamMutations } from '../hooks/useExamMutations';
import type { ExamFormData } from '../schemas/examSchema';

export default function EditExamPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: exam, isLoading, isError } = useExam(id);
  const { updateExam } = useExamMutations();

  const onSubmit = (formData: ExamFormData) => {
    if (!id) return;
    
    updateExam.mutate(
      { id, payload: formData },
      {
        onSuccess: () => navigate(ROUTES.CATALOG),
        onError: (error: unknown) => {
          let errorMessage = 'Ocurrió un error al actualizar el examen.';
          if (error instanceof AxiosError) {
            errorMessage = error.response?.data?.message || error.message;
          } else if (error instanceof Error) {
            errorMessage = error.message;
          }
          alert(`Error al actualizar: ${errorMessage}`);
        },
      }
    );
  };

  if (isLoading) return <div className="p-8 text-center text-[#64768b]">Cargando información del examen...</div>;
  if (isError || !exam) return (
    <div className="p-8 flex flex-col items-center justify-center text-red-500">
      <AlertCircle className="w-8 h-8 mb-2" />
      <p>Error al cargar el examen o no fue encontrado.</p>
    </div>
  );

  // Mapeamos el modelo del backend (Exam) al formato del formulario (ExamFormData)
  // Como tu ExamenDto ya devuelve la lista de SubExamenes, el formulario pintará el árbol automáticamente
  const initialValues: Partial<ExamFormData> = {
    ...exam,
    examenPadreId: exam.examenPadreId || null,
    valoresReferencia: exam.valoresReferencia || null,
    valoresReferenciaM: exam.valoresReferenciaM || null,
    valoresReferenciaF: exam.valoresReferenciaF || null,
    decimalesRequeridos: exam.decimalesRequeridos ?? null,
  };

  return (
    <div className="max-w-4xl mx-auto space-y-6">
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
            Editar Examen
          </h1>
          <p className="text-[#64768b] mt-1">
            Modificando: <span className="font-bold text-[#2a79b7]">{exam.nombre}</span> ({exam.codigo})
          </p>
        </div>
      </div>

      <ExamForm 
        initialValues={initialValues}
        onSubmit={onSubmit} 
        onCancel={() => navigate(ROUTES.CATALOG)}
        isLoading={updateExam.isPending}
      />
    </div>
  );
}