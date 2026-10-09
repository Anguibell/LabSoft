import { z } from 'zod';

export const panelSolicitudSchema = z.object({
  panelId: z.string(),
  examenesExcluidosIds: z.array(z.string()),
});

export const orderSchema = z.object({
  pacienteId: z.string().min(1, 'Debe seleccionar un paciente para la orden'),
  examenIds: z.array(z.string()),
  panelesSolicitados: z.array(panelSolicitudSchema),
}).refine(
  (data) => data.examenIds.length > 0 || data.panelesSolicitados.length > 0,
  {
    message: 'La orden debe contener al menos un examen o panel.',
    path: ['examenIds'], 
  }
);

export type OrderFormData = z.infer<typeof orderSchema>;