interface OrderStatusBadgeProps {
  status: string;
}

export default function OrderStatusBadge({ status }: OrderStatusBadgeProps) {
  const normalizedStatus = status.toLowerCase();

  // Definimos colores según el estado
  let bg = 'bg-gray-100';
  let text = 'text-gray-700';
  let border = 'border-gray-200';

  if (normalizedStatus === 'pendiente') {
    bg = 'bg-yellow-50';
    text = 'text-yellow-700';
    border = 'border-yellow-200';
  } else if (normalizedStatus === 'en proceso' || normalizedStatus === 'en_proceso') {
    bg = 'bg-blue-50';
    text = 'text-blue-700';
    border = 'border-blue-200';
  } else if (normalizedStatus === 'completada' || normalizedStatus === 'lista') {
    bg = 'bg-green-50';
    text = 'text-green-700';
    border = 'border-green-200';
  } else if (normalizedStatus === 'cancelada') {
    bg = 'bg-red-50';
    text = 'text-red-700';
    border = 'border-red-200';
  }

  return (
    <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium border ${bg} ${text} ${border}`}>
      {status.toUpperCase()}
    </span>
  );
}