import { useNavigate } from 'react-router-dom';
import { ArrowLeft, Layers } from 'lucide-react';
import { ROUTES } from '../../../routes/routeConfig';
import PanelForm from '../components/PanelForm';
import { usePanelMutations } from '../hooks/usePanelMutations';
import type { PanelFormData } from '../schemas/panelSchema';
import { AxiosError } from 'axios';

export default function CreatePanelPage() {
  const navigate = useNavigate();
  const { createPanel } = usePanelMutations();

  const onSubmit = (formData: PanelFormData) => {
    createPanel.mutate(formData, {
      onSuccess: () => navigate(ROUTES.PANELS),
      onError: (error: unknown) => {
        let errorMessage = 'Ocurrió un error al crear el panel.';
        if (error instanceof AxiosError) {
          errorMessage = error.response?.data?.message || error.message;
        } else if (error instanceof Error) {
          errorMessage = error.message;
        }
        alert(`Error: ${errorMessage}`);
      },
    });
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
            Nuevo Panel
          </h1>
          <p className="text-[#64768b] mt-1">Crea un grupo predeterminado de exámenes.</p>
        </div>
      </div>

      <PanelForm 
        onSubmit={onSubmit} 
        onCancel={() => navigate(ROUTES.PANELS)}
        isLoading={createPanel.isPending}
      />
    </div>
  );
}