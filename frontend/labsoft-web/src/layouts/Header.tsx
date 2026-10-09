// import { Bell, LogOut } from 'lucide-react';
// import { useAuthStore } from '../modules/auth/store/authStore';

// export default function Header() {
//   const { user, setLogout } = useAuthStore();

//   // Función para obtener iniciales reales (ej: "Juan Pérez" -> "JP")
//   const getInitials = (name?: string) => {
//     if (!name) return 'US';
//     const parts = name.trim().split(' ');
//     if (parts.length >= 2) return `${parts[0][0]}${parts[1][0]}`.toUpperCase();
//     return name.substring(0, 2).toUpperCase();
//   };

//   return (
//     <header className="flex justify-between items-center bg-white border-b border-[#dce5ef] px-5 py-4 md:px-8">
//       <div>
//         <p className="text-sm text-[#64768b]">Sistema / Entorno</p>
//         <p className="inline-flex mt-2 rounded-full px-2.5 py-1 text-xs font-semibold bg-[#edf5fb] text-[#36709c]">
//           Entorno de desarrollo
//         </p>
//       </div>
//       <div className="flex items-center gap-4">
//         <button className="relative rounded-lg p-2 text-[#4e6681] hover:bg-[#eef5fa] transition-colors">
//           <Bell className="w-5 h-5" />
//           <span className="absolute top-2 right-2 w-2 h-2 rounded-full bg-[#ea9a35] border border-white"></span>
//         </button>
        
//         {/* Perfil de Usuario */}
//         <div className="flex items-center gap-3 border-l border-[#dce5ef] pl-4 ml-2">
//           <div className="w-9 h-9 rounded-full bg-[#d9edf9] text-[#1769aa] flex items-center justify-center font-bold uppercase tracking-wider">
//             {getInitials(user?.nombre)}
//           </div>
//           <div className="hidden sm:block">
//             <p className="text-sm font-semibold leading-tight text-[#15243b]">
//               {user?.nombre || 'Usuario'}
//             </p>
//             <p className="text-xs text-[#6e8094] mt-0.5 capitalize">
//               {user?.rol || 'Cargando...'}
//             </p>
//           </div>
          
//           {/* Botón de Cerrar Sesión */}
//           <button 
//             onClick={setLogout}
//             title="Cerrar sesión"
//             className="ml-2 p-2 text-[#64768b] hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors"
//           >
//             <LogOut className="w-5 h-5" />
//           </button>
//         </div>
//       </div>
//     </header>
//   );
// }



































import { useState, useRef, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Bell, LogOut, ChevronDown, User, Settings, AlertTriangle, CheckCircle2 } from 'lucide-react';
import { useAuthStore } from '../modules/auth/store/authStore';

export default function Header() {
  const { user, setLogout } = useAuthStore();
  const navigate = useNavigate();
  
  const [isProfileOpen, setIsProfileOpen] = useState(false);
  const [isNotifOpen, setIsNotifOpen] = useState(false);
  const [showLogoutModal, setShowLogoutModal] = useState(false);
  
  const profileRef = useRef<HTMLDivElement>(null);
  const notifRef = useRef<HTMLDivElement>(null);

  // Fecha actual formateada
  const today = new Date().toLocaleDateString('es-ES', { 
    weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' 
  });

  // Cierra los menús si se hace clic afuera
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (profileRef.current && !profileRef.current.contains(event.target as Node)) setIsProfileOpen(false);
      if (notifRef.current && !notifRef.current.contains(event.target as Node)) setIsNotifOpen(false);
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const getInitials = (name?: string) => {
    if (!name) return 'US';
    const parts = name.trim().split(' ');
    if (parts.length >= 2) return `${parts[0][0]}${parts[1][0]}`.toUpperCase();
    return name.substring(0, 2).toUpperCase();
  };

  const handleNavigate = (path: string) => {
    navigate(path);
    setIsProfileOpen(false);
  };

  const handleLogout = () => {
    setIsProfileOpen(false); // Cierra el menú desplegable
    setShowLogoutModal(true); // Abre nuestro nuevo Modal
  };

  return (
    <>
    <header className="flex justify-between items-center bg-white border-b border-[#dce5ef] px-5 py-4 md:px-8 z-30">

      {/* Nuevo Modal de Cerrar Sesión */}
      {showLogoutModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#0f172a]/40 backdrop-blur-sm">
          <div className="bg-white rounded-xl shadow-2xl max-w-sm w-full p-6 animate-in zoom-in-95 duration-200">
            <div className="flex items-center gap-4 mb-4">
              <div className="p-3 bg-red-50 text-red-600 rounded-full shrink-0">
                <LogOut className="w-6 h-6" />
              </div>
              <h3 className="text-xl font-black text-[#15243b]">Cerrar Sesión</h3>
            </div>
            <p className="text-sm text-[#4e6681] mb-6">
              ¿Estás seguro de que deseas salir del sistema? Tendrás que volver a ingresar tus credenciales para acceder a LabSoft.
            </p>
            <div className="flex items-center gap-3 justify-end">
              <button 
                onClick={() => setShowLogoutModal(false)} 
                className="px-4 py-2 text-sm font-bold text-[#64768b] hover:bg-[#f8fafc] rounded-lg transition-colors"
              >
                Cancelar
              </button>
              <button 
                onClick={setLogout} 
                className="px-6 py-2 bg-red-600 text-white text-sm font-bold rounded-lg hover:bg-red-700 transition-colors"
              >
                Cerrar Sesión
              </button>
            </div>
          </div>
        </div>
      )}
      
      {/* Lado Izquierdo: Fecha */}
      <div className="hidden sm:block text-sm font-medium text-[#64768b] capitalize">
        {today}
      </div>
      
      {/* Lado Derecho: Interacciones */}
      <div className="flex items-center gap-4 ml-auto">
        
        {/* Campana de Notificaciones */}
        <div className="relative" ref={notifRef}>
          <button 
            onClick={() => { setIsNotifOpen(!isNotifOpen); setIsProfileOpen(false); }}
            className={`relative rounded-lg p-2 transition-colors ${isNotifOpen ? 'bg-[#eef5fa] text-[#2a79b7]' : 'text-[#4e6681] hover:bg-[#eef5fa]'}`}
          >
            <Bell className="w-5 h-5" />
            <span className="absolute top-2 right-2 w-2 h-2 rounded-full bg-[#ea9a35] border border-white"></span>
          </button>

          {/* Menú Flotante de Notificaciones */}
          {isNotifOpen && (
            <div className="absolute right-0 mt-2 w-80 bg-white rounded-xl shadow-lg border border-[#dce5ef] overflow-hidden animate-in fade-in slide-in-from-top-2 duration-200">
              <div className="p-3 border-b border-[#dce5ef] bg-[#f8fafc] flex justify-between items-center">
                <p className="text-xs font-bold text-[#15243b] uppercase tracking-wider">Notificaciones</p>
                <button className="text-xs text-[#2a79b7] hover:underline font-medium">Marcar leídas</button>
              </div>
              <div className="max-h-80 overflow-y-auto">
                <div className="p-3 border-b border-[#f3f7fb] hover:bg-[#f8fafc] cursor-pointer flex gap-3">
                  <div className="mt-0.5 text-red-500"><AlertTriangle className="w-4 h-4" /></div>
                  <div>
                    <p className="text-sm font-medium text-[#15243b]">Valor de Pánico Detectado</p>
                    <p className="text-xs text-[#64768b] mt-0.5">Orden #ORD-0003 (Ana Penoth) - Hematocrito alto.</p>
                    <p className="text-[10px] text-[#94a3b8] mt-1 font-semibold">Hace 10 min</p>
                  </div>
                </div>
                <div className="p-3 border-b border-[#f3f7fb] hover:bg-[#f8fafc] cursor-pointer flex gap-3">
                  <div className="mt-0.5 text-green-500"><CheckCircle2 className="w-4 h-4" /></div>
                  <div>
                    <p className="text-sm font-medium text-[#15243b]">Resultados Listos</p>
                    <p className="text-xs text-[#64768b] mt-0.5">La orden #ORD-0001 está lista para su validación.</p>
                    <p className="text-[10px] text-[#94a3b8] mt-1 font-semibold">Hace 1 hora</p>
                  </div>
                </div>
              </div>
              <div className="p-2 border-t border-[#dce5ef] text-center">
                <button className="text-xs font-bold text-[#64768b] hover:text-[#2a79b7] transition-colors">Ver todo el historial</button>
              </div>
            </div>
          )}
        </div>
        
        {/* Perfil Interactivo */}
        <div className="relative border-l border-[#dce5ef] pl-4 ml-2" ref={profileRef}>
          <button 
            onClick={() => { setIsProfileOpen(!isProfileOpen); setIsNotifOpen(false); }}
            className="flex items-center gap-3 p-1.5 rounded-lg hover:bg-[#f8fafc] transition-colors text-left"
          >
            <div className="w-9 h-9 rounded-full bg-[#d9edf9] text-[#1769aa] flex items-center justify-center font-bold uppercase tracking-wider">
              {getInitials(user?.nombre)}
            </div>
            <div className="hidden sm:block">
              <p className="text-sm font-semibold leading-tight text-[#15243b]">
                {user?.nombre || 'Usuario'}
              </p>
              <p className="text-xs text-[#6e8094] mt-0.5 capitalize">
                {user?.rol || 'Cargando...'}
              </p>
            </div>
            <ChevronDown className={`w-4 h-4 text-[#94a3b8] transition-transform duration-200 ${isProfileOpen ? 'rotate-180' : ''}`} />
          </button>

          {/* Menú Flotante del Perfil */}
          {isProfileOpen && (
            <div className="absolute right-0 mt-2 w-56 bg-white rounded-xl shadow-lg border border-[#dce5ef] overflow-hidden animate-in fade-in slide-in-from-top-2 duration-200">
              <div className="p-3 border-b border-[#dce5ef] bg-[#f8fafc]">
                <p className="text-xs font-bold text-[#64768b] uppercase tracking-wider">Cuenta Activa</p>
                <p className="text-sm font-medium text-[#15243b] mt-1 truncate">{user?.email || 'usuario@labgenesis.com'}</p>
              </div>
              
              <div className="p-1.5">
                <button 
                  onClick={() => handleNavigate('/perfil')}
                  className="w-full flex items-center gap-3 px-3 py-2 text-sm font-medium text-[#4e6681] rounded-lg hover:bg-[#f3f7fb] hover:text-[#2a79b7] transition-colors"
                >
                  <User className="w-4 h-4" /> Mi Perfil
                </button>
                <button 
                  onClick={() => handleNavigate('/configuracion')}
                  className="w-full flex items-center gap-3 px-3 py-2 text-sm font-medium text-[#4e6681] rounded-lg hover:bg-[#f3f7fb] hover:text-[#2a79b7] transition-colors"
                >
                  <Settings className="w-4 h-4" /> Configuración
                </button>
              </div>
              
              <div className="p-1.5 border-t border-[#dce5ef]">
                <button 
                  onClick={handleLogout}
                  className="w-full flex items-center gap-3 px-3 py-2 text-sm font-bold text-red-600 rounded-lg hover:bg-red-50 transition-colors"
                >
                  <LogOut className="w-4 h-4" /> Cerrar Sesión
                </button>
              </div>
            </div>
          )}
        </div>

      </div>
    </header>
  </>
  );
}