import { useNavigate } from 'react-router-dom';
import { FilePlus2 } from 'lucide-react';
import OrderForm from '../components/OrderForm';
import { useOrderMutations } from '../hooks/useOrderMutations';
import type { OrderFormData } from '../schemas/orderSchema';

export default function CreateOrderPage() {
  const navigate = useNavigate();
  const { createOrder } = useOrderMutations();

  const handleSubmit = (data: OrderFormData) => {
    createOrder.mutate(data, {
      // Cuando la orden se guarde exitosamente, devolvemos al usuario a la lista
      onSuccess: () => navigate('/ordenes'), 
    });
  };

  return (
    <div className="max-w-7xl mx-auto space-y-6">
      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm flex items-center gap-3">
        <div className="w-12 h-12 bg-[#eef5fa] rounded-lg flex items-center justify-center shrink-0">
          <FilePlus2 className="w-6 h-6 text-[#2a79b7]" />
        </div>
        <div>
          <h1 className="text-2xl font-bold text-[#15243b]">Nueva Orden Médica</h1>
          <p className="text-[#64768b]">Selecciona el paciente y añade exámenes o perfiles al carrito.</p>
        </div>
      </div>

      <OrderForm 
        onSubmit={handleSubmit} 
        onCancel={() => navigate('/ordenes')} 
        isLoading={createOrder.isPending} 
      />
    </div>
  );
}