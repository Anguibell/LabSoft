import { User, Mail, Shield } from 'lucide-react';
import PageHeader from '../../../components/common/PageHeader';
import { useAuthStore } from '../../auth/store/authStore';

export default function ProfilePage() {
  const { user } = useAuthStore();

  return (
    <div className="space-y-6 animate-in fade-in duration-300">
      <PageHeader title="Mi Perfil" description="Gestiona tu información personal y credenciales de acceso." />
      
      <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm p-6 md:p-8 flex flex-col md:flex-row gap-8">
        <div className="flex flex-col items-center gap-4 border-r border-[#dce5ef] pr-8">
          <div className="w-24 h-24 rounded-full bg-[#d9edf9] text-[#1769aa] flex items-center justify-center font-black text-3xl uppercase tracking-wider">
            {user?.nombre?.substring(0, 2) || 'US'}
          </div>
          <button className="text-sm font-bold text-[#2a79b7] hover:underline">Cambiar foto</button>
        </div>
        
        <div className="flex-1 space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="flex items-center gap-2 text-xs font-bold text-[#64768b] uppercase mb-2"><User className="w-4 h-4"/> Nombre Completo</label>
              <input type="text" disabled value={user?.nombre || ''} className="w-full px-4 py-2 bg-[#f8fafc] border border-[#dce5ef] rounded-lg text-[#15243b] cursor-not-allowed" />
            </div>
            <div>
              <label className="flex items-center gap-2 text-xs font-bold text-[#64768b] uppercase mb-2"><Mail className="w-4 h-4"/> Correo Electrónico</label>
              <input type="email" disabled value={user?.email || ''} className="w-full px-4 py-2 bg-[#f8fafc] border border-[#dce5ef] rounded-lg text-[#15243b] cursor-not-allowed" />
            </div>
            <div>
              <label className="flex items-center gap-2 text-xs font-bold text-[#64768b] uppercase mb-2"><Shield className="w-4 h-4"/> Nivel de Acceso</label>
              <input type="text" disabled value={user?.rol || ''} className="w-full px-4 py-2 bg-[#f8fafc] border border-[#dce5ef] rounded-lg text-[#15243b] cursor-not-allowed uppercase" />
            </div>
          </div>
          <div className="pt-4 border-t border-[#dce5ef]">
            <button className="px-4 py-2 bg-white border border-[#dce5ef] text-[#15243b] font-bold rounded-lg hover:bg-[#f8fafc] transition-colors">
              Cambiar Contraseña
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}