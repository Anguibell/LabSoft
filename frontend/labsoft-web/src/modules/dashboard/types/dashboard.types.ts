export interface OrdenReciente {
  id: string;
  numeroOrden: number;
  paciente: string;
  fecha: string;
  estado: string;
}

export interface DashboardStats {
  totalPacientes: number;
  ordenesHoy: number;
  ordenesPendientes: number;
  resultadosValidadosMes: number;
  ordenesRecientes: OrdenReciente[];
}