import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, Layers, AlertCircle } from 'lucide-react';
import { ROUTES } from '../../../routes/routeConfig';
import PanelForm from '../components/PanelForm';
import { usePanel } from '../hooks/usePanels';
import { usePanelMutations } from '../hooks/usePanelMutations';
import type { PanelFormData } from '../schemas/panelSchema';
import { AxiosError } from 'axios';

export default function EditPanelPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: panel, isLoading, isError } = usePanel(id);
  const { updatePanel } = usePanelMutations();

  const onSubmit = (formData: PanelFormData) => {
    if (!id) return;
    
    updatePanel.mutate(
      { id, payload: formData },
      {
        onSuccess: () => navigate(ROUTES.PANELS),
        onError: (error: unknown) => {
          let errorMessage = 'Ocurrió un error al actualizar el panel.';
          if (error instanceof AxiosError) {
            errorMessage = error.response?.data?.message || error.message;
          } else if (error instanceof Error) {
            errorMessage = error.message;
          }
          alert(`Error: ${errorMessage}`);
        },
      }
    );
  };

  if (isLoading) return <div className="p-8 text-center text-[#64768b]">Cargando información del panel...</div>;
  if (isError || !panel) return (
    <div className="p-8 flex flex-col items-center justify-center text-red-500">
      <AlertCircle className="w-8 h-8 mb-2" />
      <p>Error al cargar el panel o no fue encontrado.</p>
    </div>
  );

  // Mapeamos los datos del backend para que el formulario los entienda
  const initialValues: Partial<PanelFormData> = {
    codigo: panel.codigo,
    nombre: panel.nombre,
    examenIds: panel.examenes.map(e => e.id), // Extraemos solo los IDs de los exámenes
  };

  return (
    <div className="max-w-5xl mx-auto space-y-6">
      <div className="flex items-center gap-4">
        <button
          onClick={() => navigate(ROUTES.PANELS)}
          className="p-2 text-[#64768b] hover:text-[#15243b] hover:bg-[#eef5fa] rounded-lg transition-colors"
        >
          <ArrowLeft className="w-5 h-5" />
        </button>
        <div>
          <h1 className="text-2xl font-bold text-[#15243b] flex items-center gap-2">
            <Layers className="w-6 h-6 text-[#2a79b7]" />
            Editar Panel
          </h1>
          <p className="text-[#64768b] mt-1">
            Modificando: <span className="font-bold text-[#2a79b7]">{panel.nombre}</span>
          </p>
        </div>
      </div>

      <PanelForm 
        initialValues={initialValues}
        onSubmit={onSubmit} 
        onCancel={() => navigate(ROUTES.PANELS)}
        isLoading={updatePanel.isPending}
      />
    </div>
  );
}