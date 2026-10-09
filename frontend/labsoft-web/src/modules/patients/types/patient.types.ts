export type SexoEnum = 'Masculino' | 'Femenino';
export type ParentescoEnum = 'Titular' | 'Hijo' | 'Hija' | 'Conyuge' | 'Otro';

export interface Paciente {
  id: string;
  cedula?: string;
  esRecienNacido: boolean;
  nombre: string;
  apellido: string;
  fechaNacimiento: string; // ISO String (Date)
  edadFormateada?: string;
  sexo: SexoEnum;
  
  // Datos calculados
  totalOrdenes: number;
  ultimaVisita?: string; // ISO String (Date)

  // Bloque de Contacto
  telefono?: string;
  telefonoAlternativo?: string;
  correo?: string;
  correoAlternativo?: string;
  domicilio?: string;
  notaMedica?: string;

  // Bloque de Afiliación
  numeroAfiliado?: string;
  parentesco?: ParentescoEnum;
  titularId?: string;
  titularNombre?: string;
  titularCedula?: string;
}

// Filtros para la búsqueda y paginación
export interface PacienteQueryFilter {
  terminoBusqueda?: string;
  sexo?: string; // "MASCULINO" o "FEMENINO"
  fechaInicio?: string;
  fechaFin?: string;
  pageNumber: number;
  pageSize: number;
}

// Estructura de respuesta paginada devuelta por el backend
export interface PagedResultDto<T> {
  items: T[];
  totalRecords: number;
  pageNumber: number;
  pageSize: number;
}

// DTO para la creación y actualización (sin los campos calculados)
export type PacienteFormData = Omit<Paciente, 'id' | 'totalOrdenes' | 'ultimaVisita'>;

export interface HistorialItem {
  examenNombre: string;
  valor: string;
  unidad: string;
  valoresReferencia: string;
  notaClinica?: string;
  estado: string;
  fechaProcesamiento?: string;
  subExamenes: HistorialItem[]; // recursivo para grupos como Fórmula Leucocitaria
}

export interface HistorialEspecialidad {
  nombreEspecialidad: string;
  examenes: HistorialItem[];
}

export interface HistorialPaciente {
  ordenId: string;
  numeroOrden: string;
  fechaCreacion: string;
  especialidades: HistorialEspecialidad[];
}

export interface HistorialOrdenResumen {
  ordenId: string;
  numeroOrden: string;
  fechaCreacion: string;
  especialidades: string[]; // Ej: ["Hematología", "Bioquímica", "Orina Completa"]
}