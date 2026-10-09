import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import axios from 'axios';
import { authApi } from '../services/authApi';
import { useAuthStore } from '../store/authStore';
import { useNavigate } from 'react-router-dom';
import { ROUTES } from '../../../routes/routeConfig';

// Esquema de validación con Zod
const loginSchema = z.object({
  email: z.string().email('Ingresa un correo válido'),
  password: z.string().min(6, 'La contraseña debe tener al menos 6 caracteres'),
});

type LoginFormInputs = z.infer<typeof loginSchema>;

export default function LoginForm() {
  const [apiError, setApiError] = useState<string | null>(null);
  const setLogin = useAuthStore((state) => state.setLogin);
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginFormInputs>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: LoginFormInputs) => {
    try {
      setApiError(null);
      // Llama a la API de ASP.NET
      const response = await authApi.login(data);

      // Ahora armamos el objeto User cumpliendo el contrato exacto
      const userData = {
        id: response.id,
        email: response.email,
        nombre: response.nombre || response.nombreUsuario || 'Usuario',
        rol: response.rol
      };

      // Lo guardamos en Zustand 
      // Guarda en Zustand (y localStorage)
      setLogin(userData, response.token);
      
      // Redirige al dashboard
      navigate(ROUTES.DASHBOARD, { replace: true });
    } catch (error) {
      // 3. Verificamos explícitamente si el error viene de nuestra petición HTTP
      if (axios.isAxiosError(error)) {
        setApiError(error.response?.data?.message || 'Error al iniciar sesión. Verifica tus credenciales.');
      } else {
        setApiError('Ocurrió un error inesperado de red.');
      }
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
      {apiError && (
        <div className="bg-red-50 text-red-600 p-3 rounded-lg text-sm border border-red-100">
          {apiError}
        </div>
      )}

      <div>
        <label className="block text-sm font-medium text-[#15243b] mb-1">
          Correo electrónico
        </label>
        <input
          {...register('email')}
          type="email"
          className={`w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-[#2a79b7] focus:outline-none transition-colors ${
            errors.email ? 'border-red-500 bg-red-50' : 'border-[#dce5ef]'
          }`}
          placeholder="admin@labsoft.com"
        />
        {errors.email && <p className="text-red-500 text-xs mt-1">{errors.email.message}</p>}
      </div>

      <div>
        <label className="block text-sm font-medium text-[#15243b] mb-1">
          Contraseña
        </label>
        <input
          {...register('password')}
          type="password"
          className={`w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-[#2a79b7] focus:outline-none transition-colors ${
            errors.password ? 'border-red-500 bg-red-50' : 'border-[#dce5ef]'
          }`}
          placeholder="••••••••"
        />
        {errors.password && <p className="text-red-500 text-xs mt-1">{errors.password.message}</p>}
      </div>

      <button
        type="submit"
        disabled={isSubmitting}
        className="w-full bg-[#0f2d52] hover:bg-[#1a4273] text-white font-medium py-2.5 rounded-lg transition-colors disabled:opacity-70 disabled:cursor-not-allowed mt-2"
      >
        {isSubmitting ? 'Iniciando sesión...' : 'Ingresar al sistema'}
      </button>
    </form>
  );
}