import { z } from 'zod';

export const panelSchema = z.object({
  codigo: z.coerce.number().min(1, 'El código es obligatorio'),
  nombre: z.string().min(1, 'El nombre del perfil o panel es obligatorio'),
  examenIds: z.array(z.string()).min(1, 'Debe seleccionar al menos un examen para crear el panel'),
});

export type PanelFormData = z.infer<typeof panelSchema>;