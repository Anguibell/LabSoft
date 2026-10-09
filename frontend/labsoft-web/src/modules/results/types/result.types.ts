// DTOs de Entrada (Para enviar a C#)
export interface ResultadoParametroInput {
  examenId: string;
  valorResultado: string;
  notaInterna?: string | null;
  notaClinica?: string | null;
  imprimirObservacion: boolean;
}

export interface DetalleResultadoInput {
  ordenDetalleId: string;
  resultadosParametros: ResultadoParametroInput[];
}

export interface RegistrarResultadosMasivosPayload {
  ordenId: string;
  detallesResultados: DetalleResultadoInput[];
}

// Estructura del Árbol Clínico (Devuelto por el Helper de C#)
export interface NodoExamen {
  examenId: string;
  nombre: string;
  unidad: string;
  valoresReferencia: string;
  estado: string;
  fechaProcesamiento?: string | null;
  valorResultado?: string;
  notaInterna?: string;
  notaClinica?: string;
  imprimirObservacion?: boolean;
  subExamenes: NodoExamen[];
}

export interface EspecialidadGrupo {
  nombreEspecialidad: string;
  examenes: {
    ordenDetalleId: string;
    arbol: NodoExamen;
  }[];
}

// DTO de Salida (Lo que devuelve el GET de Pendientes y Por Orden)
export interface OrdenPendienteResultado {
  ordenId: string;
  numeroOrden?: number;
  fechaCreacion: string;
  estadoOrden: string;
  paciente: string;
  cedula: string;
  especialidades: EspecialidadGrupo[]; 
}