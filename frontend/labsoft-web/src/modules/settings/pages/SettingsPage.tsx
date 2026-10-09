import { Settings2 } from 'lucide-react';
import PageHeader from '../../../components/common/PageHeader';

export default function SettingsPage() {
  return (
    <div className="space-y-6 animate-in fade-in duration-300">
      <PageHeader title="Configuración del Sistema" description="Ajustes generales, firmas y preferencias de impresión." />
      
      <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm p-12 text-center flex flex-col items-center justify-center text-[#64768b]">
        <Settings2 className="w-12 h-12 mb-4 text-[#94a3b8]" />
        <h3 className="text-lg font-bold text-[#15243b] mb-2">Módulo en Desarrollo</h3>
        <p className="max-w-md">Aquí se gestionarán las firmas digitales, membretes, colores del sistema y conexión con equipos automatizados.</p>
      </div>
    </div>
  );
}