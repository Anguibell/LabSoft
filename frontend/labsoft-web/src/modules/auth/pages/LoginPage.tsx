import AuthLayout from '../../../layouts/AuthLayout';
import LoginForm from '../components/LoginForm';
import { useAuthStore } from '../store/authStore';
import { Navigate } from 'react-router-dom';
import { ROUTES } from '../../../routes/routeConfig';

export default function LoginPage() {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

  // Si el usuario ya tiene sesión, no mostrar el login
  if (isAuthenticated) {
    return <Navigate to={ROUTES.DASHBOARD} replace />;
  }

  return (
    <AuthLayout>
      <LoginForm />
    </AuthLayout>
  );
}