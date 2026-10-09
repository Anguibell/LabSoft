export interface User {
  id: string;
  email: string;
  nombre: string;
  rol: string;
}

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  id: string;          
  email: string;
  usuario: User;       
  rol: string;
  nombre: string;
  nombreUsuario?: string;
}