import type { ReactNode, ThHTMLAttributes } from 'react';

// Contenedor principal estandarizado (Tarjeta blanca con sombra)
export function TableContainer({ children }: { children: ReactNode }) {
  return (
    <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden">
      <div className="overflow-x-auto">
        <table className="w-full text-left text-sm whitespace-nowrap">
          {children}
        </table>
      </div>
    </div>
  );
}

// Cabecera estandarizada (<thead y <tr>)
export function TableHeader({ children }: { children: ReactNode }) {
  return (
    <thead className="bg-[#f8fafc] border-b border-[#dce5ef]">
      <tr>{children}</tr>
    </thead>
  );
}

// Celda de cabecera estandarizada (<th>) - Aquí resolvemos los márgenes distintos
export function Th({ children, className = '', ...props }: ThHTMLAttributes<HTMLTableCellElement>) {
  return (
    <th 
      className={`px-6 py-4 text-xs font-bold text-[#64768b] uppercase tracking-wider ${className}`} 
      {...props}
    >
      {children}
    </th>
  );
}