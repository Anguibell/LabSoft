export const ROUTES = {
  // Públicas
  LOGIN: '/login',
  
  // Privadas
  DASHBOARD: '/',

  // Perfil
  PROFILE: '/perfil',

  // Configuracion
  SETTINGS: '/configuracion',
  
  // Pacientes
  PATIENTS: '/pacientes',
  PATIENT_NEW: '/pacientes/nuevo',
  PATIENT_DETAILS: (id: string) => `/pacientes/${id}`,
  PATIENT_EDIT: (id: string) => `/pacientes/${id}/editar`,
  
  // Catálogo (Exámenes)
  CATALOG: '/catalogo',
  EXAM_NEW: '/catalogo/nuevo',
  EXAM_EDIT: (id: string) => `/catalogo/${id}/editar`,

  // Paneles o perfiles
  PANELS: '/paneles',
  PANEL_NEW: '/paneles/nuevo',
  PANEL_EDIT: (id: string) => `/paneles/${id}/editar`,
  
  // Órdenes
  ORDERS: '/ordenes',
  ORDER_NEW: '/ordenes/nueva',
  ORDER_DETAILS: (id: string) => `/ordenes/${id}`,
  ORDER_EDIT: (id: string) => `/ordenes/${id}/editar`,
  
  // Resultados
  RESULTS: '/resultados',
  RESULT_PROCESS: (id: string) => `/resultados/${id}/procesar`,
  RESULT_DETAILS: (id: string) => `/resultados/${id}`,

} as const;