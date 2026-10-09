import { z } from 'zod';

// Esquema para un examen individual
const resultadoItemSchema = z.object({
  valorResultado: z.string().optional(), // Es opcional para permitir guardados parciales
  notaInterna: z.string().optional(),
  notaClinica: z.string().optional(),
  imprimirObservacion: z.boolean().optional(),
});

// Esquema principal del formulario: Un diccionario anidado
export const processResultSchema = z.object({
  // resultados -> ordenDetalleId -> examenId -> valores
  resultados: z.record(
    z.string(), 
    z.record(z.string(), resultadoItemSchema)
  )
});

// Inferimos el tipo de TypeScript a partir del esquema
export type ProcessResultFormValues = z.infer<typeof processResultSchema>;