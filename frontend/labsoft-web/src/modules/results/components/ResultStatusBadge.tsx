interface ResultStatusBadgeProps {
  estado: string;
}

export default function ResultStatusBadge({ estado }: ResultStatusBadgeProps) {
  if (!estado) return null;

  // Diccionario de estilos (mucho más limpio que un switch)
  const badgeStyles: Record<string, string> = {
    pendiente: 'bg-yellow-50 text-yellow-700 border-yellow-200',
    procesado: 'bg-green-50 text-green-700 border-green-200',
    validado: 'bg-blue-50 text-blue-700 border-blue-200',
    cancelada: 'bg-red-50 text-red-700 border-red-200',
    repetido: 'bg-red-50 text-red-700 border-red-200',
  };

  // Busca el estilo en el diccionario, si no existe usa el gris por defecto
  const style = badgeStyles[estado.toLowerCase()] || 'bg-gray-50 text-gray-700 border-gray-200';

  return (
    <span className={`inline-flex items-center px-2 py-0.5 rounded text-[11px] font-bold uppercase tracking-wider border ${style}`}>
      {estado}
    </span>
  );
}