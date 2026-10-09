import type { Exam } from '../../catalog/types/exam.types';

export interface Panel {
  id: string;
  codigo: number; 
  nombre: string;
  activo: boolean;
  examenes: Exam[]; 
}

export interface CreateUpdatePanelPayload {
  codigo: number; 
  nombre: string;
  examenIds: string[];
}