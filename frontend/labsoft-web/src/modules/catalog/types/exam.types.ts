export interface Exam {
  id: string;
  codigo: number;
  especialidadId: string;
  orden: number;
  nombre: string;
  precio: number;
  unidad: string;
  valoresReferencia?: string | null;
  valoresReferenciaM?: string | null;
  valoresReferenciaF?: string | null;
  activo: boolean;
  esOpcionalDinamico: boolean;
  decimalesRequeridos?: number | null;
  especialidadNombre: string;
  examenPadreId?: string | null;
  subExamenes: Exam[];
}

export interface ExamGroup {
  nombreEspecialidad: string;
  examenes: Exam[];
}

// Nueva interfaz para los hijos
export interface CreateSubExamPayload {
  orden?: number | null;
  nombre: string;
  precio: number;
  unidad: string;
  valoresReferencia?: string | null;
  valoresReferenciaM?: string | null;
  valoresReferenciaF?: string | null;
  esOpcionalDinamico: boolean;
  decimalesRequeridos?: number | null;
  subExamenes?: CreateSubExamPayload[]; // Soporte recursivo
}

export interface CreateExamPayload {
  codigo: number;
  especialidadId: string;
  orden?: number | null;
  nombre: string;
  precio: number;
  unidad: string;
  valoresReferencia?: string | null;
  valoresReferenciaM?: string | null;
  valoresReferenciaF?: string | null;
  esOpcionalDinamico: boolean;
  decimalesRequeridos?: number | null;
  examenPadreId?: string | null;
  subExamenes?: CreateSubExamPayload[]; // Array de hijos agregado
}

export interface UpdateExamPayload {
  codigo: number;
  orden: number;
  nombre: string;
  precio: number;
  unidad: string;
  valoresReferencia?: string | null;
  valoresReferenciaM?: string | null;
  valoresReferenciaF?: string | null;
  activo: boolean;
  esOpcionalDinamico: boolean;
  decimalesRequeridos?: number | null;
}