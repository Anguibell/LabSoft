import { useParams, useNavigate } from 'react-router-dom';
import { useForm, FormProvider } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Save, ArrowLeft, Activity } from 'lucide-react';

import PageHeader from '../../../components/common/PageHeader';
import ResultExamRow from '../components/ResultExamRow';
import { useResultadosPorOrden } from '../hooks/useResult';
import { useResultMutations } from '../hooks/useResultMutations';
import { processResultSchema, type ProcessResultFormValues } from '../schemas/resultSchema';
import type { RegistrarResultadosMasivosPayload } from '../types/result.types';

export default function ProcessResultPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  
  // 1. Obtenemos la orden y su árbol de exámenes desde el backend
  const { data: orden, isLoading, isError } = useResultadosPorOrden(id);
  const { registrarMasivos } = useResultMutations();

  // 2. Inicializamos el Súper Formulario con Zod
  const methods = useForm<ProcessResultFormValues>({
    resolver: zodResolver(processResultSchema),
    defaultValues: { resultados: {} },
  });

  if (isLoading) {
    return <div className="p-8 text-center text-[#64768b]">Cargando estructura de exámenes...</div>;
  }

  if (isError || !orden) {
    return (
      <div className="p-8 text-center text-red-500 bg-red-50 border border-red-200 rounded-lg">
        Error al cargar la orden para procesamiento.
      </div>
    );
  }

  // 3. Función que recolecta los datos y los envía a C#
  const onSubmit = (data: ProcessResultFormValues) => {
    // Transformamos el diccionario de React Hook Form al arreglo que espera C# (RegistrarResultadosMasivosDto)
    const payload: RegistrarResultadosMasivosPayload = {
      ordenId: id!,
      detallesResultados: Object.entries(data.resultados).map(([ordenDetalleId, examenes]) => {
        
        // Filtramos para enviar SOLO los exámenes en los que el usuario escribió un resultado
        // Usamos [, valores] dejando el espacio vacío antes de la coma para calmar a ESLint
        const parametros = Object.entries(examenes)
          .filter(([, valores]) => valores.valorResultado && valores.valorResultado.trim() !== '')
          .map(([examenId, valores]) => ({
            examenId,
            valorResultado: valores.valorResultado!.trim(),
            notaInterna: valores.notaInterna || null,
            notaClinica: valores.notaClinica || null,
            imprimirObservacion: valores.imprimirObservacion || false, // El valor por defecto lo aplicamos aquí
          }));

        return {
          ordenDetalleId,
          resultadosParametros: parametros
        };
      }).filter(d => d.resultadosParametros.length > 0) // No enviamos detalles vacíos
    };

    if (payload.detallesResultados.length === 0) {
      alert("Debes ingresar al menos un valor de resultado antes de guardar.");
      return;
    }

    // Mutación hacia el backend
    registrarMasivos.mutate(payload, {
      onSuccess: () => {
        navigate('/resultados');
      }
    });
  };

  return (
    <div className="space-y-6 animate-in fade-in duration-300 max-w-6xl mx-auto">
      
      {/* Botón de retroceso */}
      <div className="flex items-center justify-between mb-2">
        <button 
          onClick={() => navigate('/resultados')}
          className="flex items-center gap-2 text-sm font-medium text-[#64768b] hover:text-[#2a79b7] transition-colors"
        >
          <ArrowLeft className="w-4 h-4" />
          Volver a Pendientes
        </button>
      </div>

      <PageHeader 
        title="Procesar Resultados" 
        description={`Ingresando valores para el paciente ${orden.paciente}`}
      />

      {/* Cabecera de información rápida */}
      <div className="bg-white p-6 rounded-xl border border-[#dce5ef] shadow-sm flex items-center gap-4">
        <div className="p-3 bg-[#eef5fa] rounded-lg border border-[#dce5ef]">
          <Activity className="w-6 h-6 text-[#2a79b7]" />
        </div>
        <div>
          <h2 className="text-lg font-bold text-[#15243b]">Orden de Laboratorio</h2>
          <p className="text-sm text-[#64768b]">C.I: {orden.cedula || 'N/A'} • Fecha: {new Date(orden.fechaCreacion).toLocaleDateString('es-ES')}</p>
        </div>
      </div>

      {/* 4. EL CONTEXTO DEL FORMULARIO */}
      <FormProvider {...methods}>
        <form onSubmit={methods.handleSubmit(onSubmit)} className="space-y-8">
          
          {/* Iteramos sobre cada Especialidad (Hematología, Química, etc.) */}
          {orden.especialidades?.map((grupo, idx) => (
            <div key={idx} className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden">
              <div className="px-6 py-4 border-b border-[#dce5ef] bg-[#f8fafc]">
                <h3 className="text-sm font-bold text-[#2a79b7] uppercase tracking-wider">
                  {/* Cambio 1: nombreEspecialidad */}
                  {grupo.nombreEspecialidad} 
                </h3>
              </div>
              
              <div className="overflow-x-auto">
                <table className="w-full text-left">
                  <thead className="text-xs text-[#64768b] uppercase border-b border-[#dce5ef] bg-white">
                    <tr>
                      <th className="px-6 py-3 font-bold w-[35%]">Examen / Parámetro</th>
                      <th className="px-4 py-3 font-bold w-[20%]">Resultado</th>
                      <th className="px-4 py-3 font-bold w-[15%]">Unidad</th>
                      <th className="px-4 py-3 font-bold w-[20%]">Rango de Referencia</th>
                      <th className="px-4 py-3 font-bold w-[10%] text-center">Estado</th>
                    </tr>
                  </thead>
                  <tbody>
                    {/* Cambio 2: grupo.examenes */}
                    {grupo.examenes?.map((item) => (
                      <ResultExamRow 
                        key={item.ordenDetalleId}
                        detalleId={item.ordenDetalleId}
                        nodo={item.arbol}
                      />
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          ))}

          {/* Botones de acción del formulario */}
          <div className="flex items-center justify-end gap-3 pt-4 border-t border-[#dce5ef]">
            <button
              type="button"
              onClick={() => navigate('/resultados')}
              className="px-6 py-2.5 text-sm font-bold text-[#64768b] bg-white border border-[#dce5ef] rounded-lg hover:bg-[#f8fafc] transition-colors"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={registrarMasivos.isPending}
              className="flex items-center gap-2 px-8 py-2.5 bg-[#2a79b7] text-white rounded-lg hover:bg-[#206094] transition-colors font-medium disabled:opacity-50 shadow-sm"
            >
              <Save className="w-4 h-4" />
              {registrarMasivos.isPending ? 'Guardando...' : 'Guardar Resultados'}
            </button>
          </div>

        </form>
      </FormProvider>

    </div>
  );
}