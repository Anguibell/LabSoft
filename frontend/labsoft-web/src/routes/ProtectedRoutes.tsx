import { Navigate, Outlet } from 'react-router-dom';
import { ROUTES } from './routeConfig';
import { useAuthStore } from '../modules/auth/store/authStore';

export default function ProtectedRoutes() {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

  if (!isAuthenticated) {
    return <Navigate to={ROUTES.LOGIN} replace />;
  }

  // <Outlet /> renderiza las rutas hijas si el usuario está autenticado
  return <Outlet />;
}