import { z } from 'zod';

export const patientSchema = z.object({
  cedula: z.string().optional(),
  esRecienNacido: z.boolean(),
  nombre: z.string().min(2, 'El nombre es obligatorio y debe tener al menos 2 caracteres'),
  apellido: z.string().min(2, 'El apellido es obligatorio y debe tener al menos 2 caracteres'),
  // Transformamos el input de fecha a un string ISO para C#
  fechaNacimiento: z.string().min(1, 'La fecha de nacimiento es obligatoria'),
  sexo: z.enum(['Masculino', 'Femenino'], {
    required_error: 'Debe seleccionar un sexo',
  }),

  // Bloque de Contacto
  telefono: z.string().optional(),
  telefonoAlternativo: z.string().optional(),
  correo: z.union([z.literal(''), z.string().email('Formato de correo inválido')]).optional(),
  correoAlternativo: z.union([z.literal(''), z.string().email('Formato de correo inválido')]).optional(),
  domicilio: z.string().optional(),
  notaMedica: z.string().optional(),

  // Bloque de Afiliación
  numeroAfiliado: z.string().optional(),
  parentesco: z.enum(['Titular', 'Hijo', 'Hija', 'Conyuge', 'Otro']).optional(),
  // Transforma automáticamente el string vacío a undefined
  titularId: z.string().uuid().optional().or(z.literal('')).transform(val => val === '' ? undefined : val),
  //titularId: z.string().uuid().optional().or(z.literal('')),
});

export type PatientFormInputs = z.infer<typeof patientSchema>;