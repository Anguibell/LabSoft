import type { ReactNode } from 'react';
import { FlaskConical } from 'lucide-react';

interface AuthLayoutProps {
  children: ReactNode;
}

export default function AuthLayout({ children }: AuthLayoutProps) {
  return (
    <div className="min-h-screen bg-[#f3f7fb] flex flex-col justify-center items-center py-12 px-4 sm:px-6 lg:px-8 text-[#15243b]">
      <div className="w-full max-w-md">
        
        {/* Logo y Branding para Login */}
        <div className="flex flex-col items-center mb-8">
          <div className="w-14 h-14 rounded-2xl bg-[#0f2d52] flex items-center justify-center shadow-lg mb-4">
            <FlaskConical className="w-7 h-7 text-white" />
          </div>
          <h2 className="text-center text-3xl font-bold tracking-tight">
            LabSoft LIMS
          </h2>
          <p className="mt-2 text-center text-sm text-[#61748a]">
            Inicia sesión para acceder al sistema
          </p>
        </div>

        {/* Contenedor dinámico donde irá el LoginForm */}
        <div className="bg-white py-8 px-6 shadow-xl shadow-[#113152]/5 rounded-2xl sm:px-10 border border-[#dce5ef]">
          {children}
        </div>
        
      </div>
    </div>
  );
}