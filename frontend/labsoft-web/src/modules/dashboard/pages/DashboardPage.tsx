import { Users, ClipboardList, Activity, CheckCircle, Clock, BookOpen, LayoutGrid, ArrowRight } from 'lucide-react';
import { useDashboardStats } from '../hooks/useDashboard';
import { useNavigate } from 'react-router-dom';
import { ROUTES } from '../../../routes/routeConfig';

export default function DashboardPage() {
  const { data: stats, isLoading, isError } = useDashboardStats();
  const navigate = useNavigate();

  if (isLoading) {
    return <div className="p-8 text-center text-[#64768b] animate-pulse">Cargando métricas del laboratorio...</div>;
  }

  if (isError || !stats) {
    return <div className="p-8 text-center text-red-500 bg-red-50 rounded-lg">Error al conectar con el servidor.</div>;
  }

  const statCards = [
    {
      title: 'Pacientes Registrados',
      value: stats.totalPacientes,
      icon: <Users className="w-6 h-6 text-blue-600" />,
      bg: 'bg-blue-50',
      border: 'border-blue-100',
    },
    {
      title: 'Órdenes Ingresadas Hoy',
      value: stats.ordenesHoy,
      icon: <ClipboardList className="w-6 h-6 text-indigo-600" />,
      bg: 'bg-indigo-50',
      border: 'border-indigo-100',
    },
    {
      title: 'Órdenes Pendientes',
      value: stats.ordenesPendientes,
      icon: <Activity className="w-6 h-6 text-yellow-600" />,
      bg: 'bg-yellow-50',
      border: 'border-yellow-100',
    },
    {
      title: 'Exámenes Validados (Mes)',
      value: stats.resultadosValidadosMes,
      icon: <CheckCircle className="w-6 h-6 text-green-600" />,
      bg: 'bg-green-50',
      border: 'border-green-100',
    },
  ];

  // Configuración de los accesos directos a módulos
  const quickModules = [
    {
      title: 'Catálogo de Exámenes',
      description: 'Gestión de pruebas de laboratorio, precios y valores de referencia.',
      icon: <BookOpen className="w-6 h-6 text-purple-600" />,
      bg: 'bg-purple-50',
      border: 'border-purple-200',
      path: ROUTES.CATALOG,
    },
    {
      title: 'Paneles (Perfiles)',
      description: 'Configuración de perfiles predefinidos (Ej. Perfil 20, Preoperatorio).',
      icon: <LayoutGrid className="w-6 h-6 text-teal-600" />,
      bg: 'bg-teal-50',
      border: 'border-teal-200',
      path: ROUTES.PANELS, 
    }
  ];

  return (
    <div className="space-y-6 animate-in fade-in duration-300">
      <div>
        <h1 className="text-2xl font-bold text-[#15243b]">Panel de Control</h1>
        <p className="text-[#64768b]">Resumen de actividad y estado operativo del laboratorio.</p>
      </div>

      {/* Tarjetas de Métricas */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        {statCards.map((card, idx) => (
          <div key={idx} className="bg-white p-5 rounded-xl border border-[#dce5ef] shadow-sm flex flex-col gap-4 hover:shadow-md transition-shadow">
            <div className="flex items-start justify-between">
              <div className={`p-3 rounded-lg ${card.bg} ${card.border} border`}>
                {card.icon}
              </div>
              <h3 className="text-3xl font-black text-[#15243b]">{card.value}</h3>
            </div>
            <p className="text-xs font-bold text-[#64768b] uppercase tracking-wider leading-snug">
              {card.title}
            </p>
          </div>
        ))}
      </div>

      {/* NUEVA SECCIÓN: Accesos Rápidos a Módulos */}
      {/* <div>
        <h2 className="text-lg font-bold text-[#15243b] mb-4 flex items-center gap-2">
           Accesos Rápidos
        </h2>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {quickModules.map((module, idx) => (
            <Link 
              key={idx} 
              to={module.path}
              className="bg-white p-5 rounded-xl border border-[#dce5ef] shadow-sm flex items-start gap-4 hover:border-blue-400 hover:shadow-md transition-all group"
            >
              <div className={`p-3 rounded-lg ${module.bg} ${module.border} border shrink-0`}>
                {module.icon}
              </div>
              <div className="flex-1">
                <h3 className="text-[#15243b] font-bold mb-1 group-hover:text-blue-600 transition-colors">
                  {module.title}
                </h3>
                <p className="text-sm text-[#64768b]">
                  {module.description}
                </p>
              </div>
              <div className="shrink-0 self-center opacity-0 group-hover:opacity-100 transition-opacity">
                 <ArrowRight className="w-5 h-5 text-blue-500" />
              </div>
            </Link>
          ))}
        </div>
      </div> */}

{/* Accesos Rápidos a Módulos */}
<div>
  <h2 className="mb-4 flex items-center gap-2 text-lg font-bold text-[#15243b]">
    Accesos Rápidos
  </h2>

  <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
    {quickModules.map((module) => (
      <button
        key={module.path}
        type="button"
        onClick={() => navigate(module.path)}
        className="group flex w-full items-start gap-4 rounded-xl border border-[#dce5ef] bg-white p-5 text-left shadow-sm transition-all hover:border-blue-400 hover:shadow-md focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-blue-500 focus-visible:ring-offset-2"
      >
        <div
          className={`shrink-0 rounded-lg border p-3 ${module.bg} ${module.border}`}
        >
          {module.icon}
        </div>

        <div className="min-w-0 flex-1">
          <h3 className="mb-1 font-bold text-[#15243b] transition-colors group-hover:text-blue-600">
            {module.title}
          </h3>

          <p className="text-sm text-[#64768b]">
            {module.description}
          </p>
        </div>

        <div className="shrink-0 self-center opacity-0 transition-opacity group-hover:opacity-100 group-focus-visible:opacity-100">
          <ArrowRight className="h-5 w-5 text-blue-500" />
        </div>
      </button>
    ))}
  </div>
</div>

      {/* Tabla de Órdenes Recientes */}
      <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden">
        <div className="px-6 py-4 border-b border-[#dce5ef] flex items-center gap-2 bg-[#f8fafc]">
          <Clock className="w-5 h-5 text-[#2a79b7]" />
          <h2 className="text-sm font-bold text-[#15243b] uppercase tracking-wider">Últimas 5 Órdenes Ingresadas</h2>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-left">
            <thead className="text-xs text-[#64768b] uppercase border-b border-[#dce5ef] bg-white">
              <tr>
                <th className="px-6 py-3 font-bold">Orden</th>
                <th className="px-6 py-3 font-bold">Paciente</th>
                <th className="px-6 py-3 font-bold">Fecha / Hora</th>
                <th className="px-6 py-3 font-bold text-center">Estado</th>
              </tr>
            </thead>
            <tbody>
              {stats.ordenesRecientes.length === 0 ? (
                <tr>
                  <td colSpan={4} className="px-6 py-8 text-center text-[#64768b]">
                    No hay órdenes recientes registradas.
                  </td>
                </tr>
              ) : (
                stats.ordenesRecientes.map((orden) => {
                  const numStr = orden.numeroOrden.toString().padStart(4, '0');
                  return (
                    <tr 
                      key={orden.id} 
                      onClick={() => navigate(`/ordenes/${orden.id}`)}
                      className="border-b border-[#dce5ef] hover:bg-[#f3f7fb] transition-colors cursor-pointer"
                    >
                      <td className="px-6 py-4 font-bold text-[#2a79b7]">#ORD-{numStr}</td>
                      <td className="px-6 py-4 font-bold text-[#15243b]">{orden.paciente}</td>
                      <td className="px-6 py-4 text-sm text-[#64768b]">
                        {new Date(orden.fecha).toLocaleString('es-ES', { 
                          day: '2-digit', month: 'short', hour: '2-digit', minute: '2-digit' 
                        })}
                      </td>
                      <td className="px-6 py-4 text-center">
                        <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-bold border ${
                          orden.estado === 'Validado' ? 'bg-blue-50 text-blue-700 border-blue-200' :
                          orden.estado === 'Completada' ? 'bg-green-50 text-green-700 border-green-200' :
                          orden.estado === 'Cancelada' ? 'bg-red-50 text-red-700 border-red-200' :
                          'bg-yellow-50 text-yellow-700 border-yellow-200'
                        }`}>
                          {orden.estado}
                        </span>
                      </td>
                    </tr>
                  );
                })
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}