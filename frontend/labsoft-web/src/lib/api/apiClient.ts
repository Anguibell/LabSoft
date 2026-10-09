import axios from 'axios';
import { useAuthStore } from '../../modules/auth/store/authStore';

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5286/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor de peticiones (Request)
apiClient.interceptors.request.use(
  (config) => {
    // Leemos el token actual del estado global
    const token = useAuthStore.getState().token;
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Interceptor de respuestas (Response)
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    // Si el backend dice que el token expiró o es inválido (401)
    if (error.response?.status === 401) {
      useAuthStore.getState().setLogout();
      // Forzamos la redirección al login
      window.location.href = '/login'; 
    }
    return Promise.reject(error);
  }
);