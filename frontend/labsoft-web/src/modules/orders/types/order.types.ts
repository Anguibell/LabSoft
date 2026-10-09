// Tipos de salida (GET)
export interface OrderDetail {
  id: string;
  itemNombre: string;
  tipo: string;
  estado: string;
}

export interface Order {
  id: string;
  numeroOrden: number;
  fechaCreacion: string;
  estado: string;
  pacienteNombre: string;
  detalles: OrderDetail[];
}

export interface OrderDetailsExtended extends Order {
  detallesPlanos: OrderDetail[];
  especialidades: unknown; // Aquí mapearemos la estructura del EstructuraClinicaHelper más adelante
}

// Tipos de entrada (POST / PUT)
export interface PanelSolicitudPayload {
  panelId: string;
  examenesExcluidosIds: string[];
}

export interface CreateOrderPayload {
  pacienteId: string;
  examenIds: string[];
  panelesSolicitados: PanelSolicitudPayload[];
}

export interface UpdateOrderPayload {
  estado: string;
  examenIds: string[];
  panelesSolicitados: PanelSolicitudPayload[];
}

// Filtros
export interface OrderQueryFilter {
  numeroOrden?: string;
  terminoPaciente?: string;
  estado?: string;
  fechaInicio?: string;
  fechaFin?: string;
  pageNumber: number;
  pageSize: number;
}