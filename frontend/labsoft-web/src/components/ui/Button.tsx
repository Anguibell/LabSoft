import type { ButtonHTMLAttributes, ReactNode } from 'react';
import type { LucideIcon } from 'lucide-react';

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  children: ReactNode;
  variant?: 'primary' | 'secondary' | 'danger';
  icon?: LucideIcon;
}

export default function Button({ 
  children, 
  variant = 'primary', 
  icon: Icon, 
  className = '', 
  ...props 
}: ButtonProps) {
  // Aquí estandarizamos la altura, el texto y el padding para TODOS los botones
  const baseStyles = "inline-flex items-center justify-center gap-2 px-5 h-[42px] text-sm font-bold rounded-lg transition-colors shadow-sm disabled:opacity-50 disabled:cursor-not-allowed";
  
  const variants = {
    primary: "bg-[#2a79b7] text-white hover:bg-[#206094]",
    secondary: "bg-white border border-[#dce5ef] text-[#4e6681] hover:bg-[#f8fafc] hover:text-[#2a79b7]",
    danger: "bg-red-600 text-white hover:bg-red-700"
  };

  return (
    <button className={`${baseStyles} ${variants[variant]} ${className}`} {...props}>
      {Icon && <Icon className="w-5 h-5" />}
      {children}
    </button>
  );
}