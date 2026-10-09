// import { NavLink } from 'react-router-dom';
// import { 
//   FlaskConical, 
//   LayoutDashboard, 
//   UsersRound, 
//   ClipboardList, 
//   FileCheck2, 
//   ShieldCheck 
// } from 'lucide-react';

// export default function Sidebar() {
//   const navItemClass = ({ isActive }: { isActive: boolean }) =>
//     `w-full flex items-center justify-center lg:justify-start gap-3 rounded-lg p-3 text-left transition-all ${
//       isActive 
//         ? 'text-white bg-white/10 shadow-[inset_3px_0_0_#63b4ec]' 
//         : 'text-[#c9deee] hover:bg-white/10 hover:translate-x-1'
//     }`;

//   return (
//     <aside className="w-19 lg:w-66 bg-[#0f2d52] text-white shrink-0 sticky top-0 h-screen overflow-y-auto transition-all duration-300">
//       <div className="flex flex-col min-h-full p-4">
        
//         {/* Logo y Marca */}
//         <div className="flex items-center justify-center lg:justify-start gap-3 lg:px-3 py-4 mb-6">
//           <div className="w-10 h-10 shrink-0 rounded-xl bg-[#2a79b7] flex items-center justify-center shadow-lg shadow-[#061c36]/30">
//             <FlaskConical className="w-5 h-5" />
//           </div>
//           <div className="hidden lg:block">
//             <p className="font-bold tracking-tight leading-tight text-white text-base">LabSoft LIMS</p>
//             <p className="text-xs text-[#b7d3eb] mt-0.5">Gestión clínica</p>
//           </div>
//         </div>

//         {/* Menú de Navegación */}
//         <p className="hidden lg:block px-3 mb-3 text-[11px] uppercase tracking-[0.14em] text-[#8bb9da] font-bold">
//           Navegación
//         </p>
//         <nav className="space-y-2">
//           <NavLink to="/" className={navItemClass}>
//             <LayoutDashboard className="w-5 h-5 shrink-0" />
//             <span className="hidden lg:inline font-medium text-[16px]">Dashboard</span>
//           </NavLink>
//           <NavLink to="/pacientes" className={navItemClass}>
//             <UsersRound className="w-5 h-5 shrink-0" />
//             <span className="hidden lg:inline font-medium text-[16px]">Pacientes</span>
//           </NavLink>
//           <NavLink to="/ordenes" className={navItemClass}>
//             <ClipboardList className="w-5 h-5 shrink-0" />
//             <span className="hidden lg:inline font-medium text-[16px]">Órdenes</span>
//           </NavLink>
//           <NavLink to="/resultados" className={navItemClass}>
//             <FileCheck2 className="w-5 h-5 shrink-0" />
//             <span className="hidden lg:inline font-semibold text-[16px]">Resultados</span>
//           </NavLink>
//         </nav>

//         {/* Footer de Seguridad */}
//         <div className="hidden lg:block mt-auto mx-2 pt-6 border-t border-[#315677]">
//           <div className="flex items-center gap-2 text-[#a9cce3]">
//             <ShieldCheck className="w-4 h-4 shrink-0" />
//             <p className="text-xs">Flujo clínico seguro</p>
//           </div>
//         </div>
//       </div>
//     </aside>
//   );
// }























import { useState } from 'react';
import { NavLink } from 'react-router-dom';
import { 
  FlaskConical, 
  LayoutDashboard, 
  UsersRound, 
  ClipboardList, 
  FileCheck2, 
  ShieldCheck,
  ChevronLeft,
  ChevronRight
} from 'lucide-react';

export default function Sidebar() {
  // Estado para controlar si la barra está abierta o cerrada
  const [isCollapsed, setIsCollapsed] = useState(false);

  // Clases dinámicas para los botones de navegación
  const navItemClass = ({ isActive }: { isActive: boolean }) =>
    `w-full flex items-center ${isCollapsed ? 'justify-center' : 'justify-start'} gap-3 rounded-lg p-3 text-left transition-all duration-200 ${
      isActive 
        ? 'text-white bg-white/10 shadow-[inset_3px_0_0_#63b4ec]' 
        : 'text-[#c9deee] hover:bg-white/10 hover:translate-x-1'
    }`;

  return (
    <aside 
      className={`${isCollapsed ? 'w-20' : 'w-64'} sticky bg-[#0f2d52] text-white shrink-0 top-0 h-screen transition-all duration-300 ease-in-out z-40 shadow-xl`}
    >
      {/* Botón flotante para colapsar/expandir */}
      <button 
        onClick={() => setIsCollapsed(!isCollapsed)}
        className="absolute -right-3 top-8 bg-[#2a79b7] text-white rounded-full p-1 shadow-md hover:bg-[#206094] transition-colors border-2 border-[#f3f7fb] z-50 hidden md:block"
        title={isCollapsed ? "Expandir menú" : "Colapsar menú"}
      >
        {isCollapsed ? <ChevronRight className="w-4 h-4" /> : <ChevronLeft className="w-4 h-4" />}
      </button>

      <div className="flex flex-col min-h-full p-4 overflow-hidden">
        
        {/* Logo y Marca */}
        <div className={`flex items-center ${isCollapsed ? 'justify-center' : 'justify-start'} gap-3 py-4 mb-6 transition-all duration-300`}>
          <div className="w-10 h-10 shrink-0 rounded-xl bg-[#2a79b7] flex items-center justify-center shadow-lg shadow-[#061c36]/30">
            <FlaskConical className="w-5 h-5" />
          </div>
          
          <div className={`transition-all duration-300 ${isCollapsed ? 'opacity-0 w-0 hidden' : 'opacity-100 w-auto'}`}>
            <p className="font-bold tracking-tight leading-tight text-white text-base whitespace-nowrap">LabSoft LIMS</p>
            <p className="text-xs text-[#b7d3eb] mt-0.5 whitespace-nowrap">Gestión clínica</p>
          </div>
        </div>

        {/* Menú de Navegación */}
        <div className={`transition-all duration-300 ${isCollapsed ? 'opacity-0 h-0 hidden' : 'opacity-100 h-auto'}`}>
          <p className="px-3 mb-3 text-[11px] uppercase tracking-[0.14em] text-[#8bb9da] font-bold whitespace-nowrap">
            Navegación
          </p>
        </div>

        <nav className="space-y-2">
          <NavLink to="/" className={navItemClass}>
            <LayoutDashboard className="w-5 h-5 shrink-0" />
            {!isCollapsed && <span className="font-medium text-[16px] whitespace-nowrap">Dashboard</span>}
          </NavLink>
          
          <NavLink to="/pacientes" className={navItemClass}>
            <UsersRound className="w-5 h-5 shrink-0" />
            {!isCollapsed && <span className="font-medium text-[16px] whitespace-nowrap">Pacientes</span>}
          </NavLink>
          
          <NavLink to="/ordenes" className={navItemClass}>
            <ClipboardList className="w-5 h-5 shrink-0" />
            {!isCollapsed && <span className="font-medium text-[16px] whitespace-nowrap">Órdenes</span>}
          </NavLink>
          
          <NavLink to="/resultados" className={navItemClass}>
            <FileCheck2 className="w-5 h-5 shrink-0" />
            {!isCollapsed && <span className="font-semibold text-[16px] whitespace-nowrap">Resultados</span>}
          </NavLink>
        </nav>

        {/* Footer de Seguridad */}
        <div className={`mt-auto pt-6 border-t border-[#315677] transition-all duration-300 ${isCollapsed ? 'mx-0 flex justify-center' : 'mx-2'}`}>
          <div 
            title={isCollapsed ? "Flujo clínico seguro" : undefined}
            className={`flex items-center text-[#a9cce3] ${isCollapsed ? 'justify-center' : 'gap-2'}`}
          >
            <ShieldCheck className="w-5 h-5 shrink-0" />
            {!isCollapsed && <p className="text-xs whitespace-nowrap">Flujo clínico seguro</p>}
          </div>
        </div>
        
      </div>
    </aside>
  );
}