import { Routes, Route, Navigate } from 'react-router-dom';
import { ROUTES } from './routeConfig';

// Layouts
import MainLayout from '../layouts/MainLayout';

// Páginas (Módulos)
import LoginPage from '../modules/auth/pages/LoginPage';

import ProfilePage from '../modules/profile/pages/ProfilePage';

import SettingsPage from '../modules/settings/pages/SettingsPage';

import DashboardPage from '../modules/dashboard/pages/DashboardPage';

import PatientsPage from '../modules/patients/pages/PatientsPage';
import CreatePatientPage from '../modules/patients/pages/CreatePatientPage';
import EditPatientPage from '../modules/patients/pages/EditPatientPage';
import PatientDetailsPage from '../modules/patients/pages/PatientDetailsPage';

import ExamsPage from '../modules/catalog/pages/ExamsPage';
import CreateExamPage from '../modules/catalog/pages/CreateExamPage';
import EditExamPage from '../modules/catalog/pages/EditExamPage';

import PanelsPage from '../modules/panels/pages/PanelsPage';
import CreatePanelPage from '../modules/panels/pages/CreatePanelPage';
import EditPanelPage from '../modules/panels/pages/EditPanelPage';

import OrdersPage from '../modules/orders/pages/OrdersPage';
import CreateOrderPage from '../modules/orders/pages/CreateOrderPage';
import OrderDetailsPage from '../modules/orders/pages/OrderDetailsPage';
import EditOrderPage from '../modules/orders/pages/EditOrderPage';

import ResultsPage from '../modules/results/pages/ResultsPage';
import ProcessResultPage from '../modules/results/pages/ProcessResultPage';
import ResultDetailsPage from '../modules/results/pages/ResultDetailsPage';

import ProtectedRoutes from './ProtectedRoutes';

export default function AppRoutes() {
  return (
    <Routes>
      {/* RUTAS PÚBLICAS */}
      <Route path={ROUTES.LOGIN} element={<LoginPage />} />

      {/* RUTAS PRIVADAS (Requieren sesión y Layout principal) */}
      <Route element={<ProtectedRoutes />}>
        <Route element={<MainLayout />}>
          
          {/* Dashboard */}
          <Route path={ROUTES.DASHBOARD} element={<DashboardPage />} />

          {/* Perfil del Usuario */}
          <Route path={ROUTES.PROFILE} element={<ProfilePage />} />

          {/* Configuracion */}
          <Route path={ROUTES.SETTINGS} element={<SettingsPage />} />

          {/* Pacientes */}
          <Route path={ROUTES.PATIENTS} element={<PatientsPage />} />
          <Route path={ROUTES.PATIENT_NEW} element={<CreatePatientPage />} /> 
          <Route path={ROUTES.PATIENT_EDIT(':id')} element={<EditPatientPage />} />
          <Route path={ROUTES.PATIENT_DETAILS(':id')} element={<PatientDetailsPage />} />

          {/* Catalogo */}
          <Route path={ROUTES.CATALOG} element={<ExamsPage />} />
          <Route path={ROUTES.EXAM_NEW} element={<CreateExamPage />} />
          <Route path={ROUTES.EXAM_EDIT(':id')} element={<EditExamPage />} />

          {/* Panel */}
          <Route path={ROUTES.PANELS} element={<PanelsPage />} />
          <Route path={ROUTES.PANEL_NEW} element={<CreatePanelPage />} />
          <Route path={ROUTES.PANEL_EDIT(':id')} element={<EditPanelPage />} />
          
          {/* Órdenes */}
          <Route path={ROUTES.ORDERS} element={<OrdersPage />} />
          <Route path={ROUTES.ORDER_NEW} element={<CreateOrderPage />} />
          <Route path={ROUTES.ORDER_DETAILS(':id')} element={<OrderDetailsPage />} />
          <Route path={ROUTES.ORDER_EDIT(':id')} element={<EditOrderPage />} />
          
          {/* Resultados */}
          <Route path={ROUTES.RESULTS} element={<ResultsPage />} />
          <Route path={ROUTES.RESULT_PROCESS(':id')} element={<ProcessResultPage />} />
          <Route path={ROUTES.RESULT_DETAILS(':id')} element={<ResultDetailsPage />} /> 
          
        </Route>
      </Route>

      {/* FALLBACK: Cualquier ruta no encontrada va al dashboard */}
      <Route path="*" element={<Navigate to={ROUTES.DASHBOARD} replace />} />
    </Routes>
  );
}