import { z } from 'zod';

const emptyToNull = z.string().transform(val => val === '' ? null : val).nullable().optional();

// 1. Declaramos la interfaz puramente en TypeScript para evitar confusiones de nulabilidad
export type SubExamFormData = {
  id?: string;
  nombre: string;
  unidad: string;
  valoresReferencia?: string | null;
  valoresReferenciaM?: string | null;
  valoresReferenciaF?: string | null;
  esOpcionalDinamico: boolean;
  decimalesRequeridos?: number | null;
  precio: number;
  orden?: number | null;
  subExamenes: SubExamFormData[]; // Estrictamente un arreglo
};

// 2. Obligamos a Zod a cumplir la interfaz usando z.ZodType
const subExamSchema: z.ZodType<SubExamFormData> = z.lazy(() =>
  z.object({
    id: z.string().optional(),
    nombre: z.string().min(1, 'Obligatorio'),
    unidad: z.string(),
    valoresReferencia: emptyToNull,
    valoresReferenciaM: emptyToNull,
    valoresReferenciaF: emptyToNull,
    esOpcionalDinamico: z.boolean(),
    decimalesRequeridos: z.coerce.number().nullable().optional(),
    precio: z.coerce.number(),
    orden: z.coerce.number().nullable().optional(),
    subExamenes: z.array(subExamSchema),
  })
);

// 3. Esquema Principal
export const examSchema = z.object({
  codigo: z.coerce.number(),
  especialidadId: z.string().min(1, 'La especialidad es obligatoria'),
  orden: z.coerce.number(),
  nombre: z.string().min(1, 'El nombre del examen es obligatorio'),
  precio: z.coerce.number(),
  unidad: z.string(),
  valoresReferencia: emptyToNull,
  valoresReferenciaM: emptyToNull,
  valoresReferenciaF: emptyToNull,
  esOpcionalDinamico: z.boolean(),
  activo: z.boolean(),
  decimalesRequeridos: z.coerce.number().nullable().optional(),
  examenPadreId: emptyToNull,
  subExamenes: z.array(subExamSchema), // Siempre un arreglo
});

export type ExamFormData = z.infer<typeof examSchema>;