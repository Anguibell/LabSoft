import { Edit, FileText } from 'lucide-react';
import { Link } from 'react-router-dom';
import { ROUTES } from '../../../routes/routeConfig';
import type { Paciente } from '../types/patient.types';
import { TableContainer, TableHeader, Th } from '../../../components/ui/Table';

interface PatientTableProps {
  patients: Paciente[];
  isLoading: boolean;
}

export default function PatientTable({ patients, isLoading }: PatientTableProps) {
  // Estado de Carga
  if (isLoading) {
    return (
      <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden p-12 flex justify-center">
        <div className="text-center text-[#64768b] font-medium flex items-center gap-2">
          <div className="w-4 h-4 border-2 border-[#2a79b7] border-t-transparent rounded-full animate-spin"></div>
          Cargando pacientes...
        </div>
      </div>
    );
  }

  // Estado Vacío
  if (patients.length === 0) {
    return (
      <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden p-12 text-center">
        <p className="text-[#64768b] font-medium">No se encontraron pacientes con los criterios actuales.</p>
      </div>
    );
  }

  // Tabla Principal
  return (
    <TableContainer>
      <TableHeader>
        <Th>Paciente</Th>
        <Th>Cédula</Th>
        <Th>Edad / Sexo</Th>
        <Th>Contacto</Th>
        <Th className="text-center">Órdenes</Th>
        <Th className="text-right">Acciones</Th>
      </TableHeader>

      <tbody className="divide-y divide-[#dce5ef]">
            {patients.map((patient) => (
              <tr key={patient.id} className="hover:bg-[#f3f7fb] transition-colors">
                <td className="px-6 py-4">
                  <p className="font-bold text-[#15243b]">
                    {patient.nombre} {patient.apellido}
                  </p>
                  
                  {/* Lógica Dinámica de Edad y Etiqueta */}
                  {/* {patient.esRecienNacido && (
                    <div className="mt-1">
                      {(() => {
                        const ageDate = new Date(Date.now() - new Date(patient.fechaNacimiento).getTime());
                        const age = Math.abs(ageDate.getUTCFullYear() - 1970);
                        const isBaby = age < 2;

                        return (
                          <span className={`inline-block text-[10px] uppercase tracking-wider font-bold px-2 py-0.5 rounded ${
                            isBaby ? 'bg-purple-100 text-purple-700' : 'bg-orange-100 text-orange-700'
                          }`}>
                            {isBaby ? 'Recién Nacido' : 'Menor sin ID'}
                          </span>
                        );
                      })()}
                    </div>
                  )} */}

                  {patient.esRecienNacido && (
                    <div className="mt-1">
                      <span className={`inline-block text-[10px] uppercase tracking-wider font-bold px-2 py-0.5 rounded ${
                        patient.edadFormateada?.includes('meses') || patient.edadFormateada?.includes('días')  
                          ? 'bg-purple-100 text-purple-700' 
                          : 'bg-orange-100 text-orange-700'
                      }`}>
                        {patient.edadFormateada?.includes('meses') || patient.edadFormateada?.includes('días') 
                          ? 'Recién Nacido' 
                          : 'Menor sin ID'}
                      </span>
                    </div>
                  )}

                  {/* Mostrar el Nombre del Titular */}
                  {patient.esRecienNacido && patient.titularNombre && (
                    <p className="text-xs text-[#64768b] mt-1.5 flex items-center gap-1">
                      <span className="font-bold text-[#4e6681]">Rep:</span> {patient.titularNombre}
                    </p>
                  )}

                  {/* Mostrar la cedula del Titular/Representante */}
                  {patient.esRecienNacido && patient.titularCedula && (
                    <p className="text-xs text-[#64768b] mt-1.5 flex items-center gap-1">
                      <span className="font-bold text-[#4e6681]">C.I.:</span> {patient.titularCedula}
                    </p>
                  )}

                </td>

                {/* <td className="px-6 py-4">
                  <p className="font-semibold text-[#15243b]">
                    {patient.nombre} {patient.apellido}
                  </p>
                  {patient.esRecienNacido && (
                    <span className="inline-block mt-1 text-[10px] uppercase tracking-wider font-bold bg-purple-100 text-purple-700 px-2 py-0.5 rounded">
                      Recién Nacido
                    </span>
                  )}
                </td> */}
                <td className="px-6 py-4 text-[#4e6681]">{patient.cedula || 'N/A'}</td>
                <td className="px-6 py-4 text-[#4e6681]">
                  <span className="font-medium text-[#15243b]">{patient.edadFormateada || 'N/A'}</span> <br/>
                  <span className="text-xs text-[#64768b] uppercase">{patient.sexo}</span>
                </td>
                <td className="px-6 py-4 text-[#4e6681]">
                  {patient.telefono || 'Sin teléfono'}
                </td>
                <td className="px-6 py-4 text-center">
                  <span className="inline-flex items-center justify-center px-2.5 py-1 rounded-full text-xs font-bold bg-[#eef5fa] text-[#2a79b7]">
                    {patient.totalOrdenes}
                  </span>
                </td>
                <td className="px-6 py-4 text-right space-x-2">
                  <Link
                    to={ROUTES.PATIENT_EDIT(patient.id)}
                    className="inline-flex p-2 text-[#64768b] hover:text-[#2a79b7] hover:bg-[#eef5fa] rounded transition-colors"
                    title="Editar paciente"
                  >
                    <Edit className="w-4 h-4" />
                  </Link>
                  <Link
                    to={ROUTES.PATIENT_DETAILS(patient.id)}
                    className="inline-flex p-2 text-[#64768b] hover:text-[#0f2d52] hover:bg-[#eef5fa] rounded transition-colors"
                    title="Ver historial"
                  >
                    <FileText className="w-4 h-4" />
                  </Link>
                </td>
              </tr>
            ))}
          </tbody>

    </TableContainer>
    // <div className="bg-white rounded-xl border border-[#dce5ef] shadow-sm overflow-hidden">
    //   <div className="overflow-x-auto">
    //     <table className="w-full text-left text-sm whitespace-nowrap">
    //       <thead className="bg-[#f8fafc] text-[#4e6681] border-b border-[#dce5ef]">
    //         <tr>
    //           <th className="px-6 py-3 font-semibold">Paciente</th>
    //           <th className="px-6 py-3 font-semibold">Cédula</th>
    //           <th className="px-6 py-3 font-semibold">Edad / Sexo</th>
    //           <th className="px-6 py-3 font-semibold">Contacto</th>
    //           <th className="px-6 py-3 font-semibold text-center">Órdenes</th>
    //           <th className="px-6 py-3 font-semibold text-right">Acciones</th>
    //         </tr>
    //       </thead>
          // <tbody className="divide-y divide-[#dce5ef]">
          //   {patients.map((patient) => (
          //     <tr key={patient.id} className="hover:bg-[#f3f7fb] transition-colors">
          //       <td className="px-6 py-4">
          //         <p className="font-semibold text-[#15243b]">
          //           {patient.nombre} {patient.apellido}
          //         </p>
          //         {patient.esRecienNacido && (
          //           <span className="inline-block mt-1 text-[10px] uppercase tracking-wider font-bold bg-purple-100 text-purple-700 px-2 py-0.5 rounded">
          //             Recién Nacido
          //           </span>
          //         )}
          //       </td>
          //       <td className="px-6 py-4 text-[#4e6681]">{patient.cedula || 'N/A'}</td>
          //       <td className="px-6 py-4 text-[#4e6681]">
          //         {/* Nota: En el DTO de GET deberíamos recibir la EdadFormateada, por ahora mostramos fecha */}
          //         {new Date(patient.fechaNacimiento).toLocaleDateString('es-VE', { timeZone: 'UTC' })} <br/>
          //         <span className="text-xs text-[#64768b] uppercase">{patient.sexo}</span>
          //       </td>
          //       <td className="px-6 py-4 text-[#4e6681]">
          //         {patient.telefono || 'Sin teléfono'}
          //       </td>
          //       <td className="px-6 py-4 text-center">
          //         <span className="inline-flex items-center justify-center px-2.5 py-1 rounded-full text-xs font-bold bg-[#eef5fa] text-[#2a79b7]">
          //           {patient.totalOrdenes}
          //         </span>
          //       </td>
          //       <td className="px-6 py-4 text-right space-x-2">
          //         <Link
          //           to={ROUTES.PATIENT_EDIT(patient.id)}
          //           className="inline-flex p-2 text-[#64768b] hover:text-[#2a79b7] hover:bg-[#eef5fa] rounded transition-colors"
          //           title="Editar paciente"
          //         >
          //           <Edit className="w-4 h-4" />
          //         </Link>
          //         <Link
          //           to={ROUTES.PATIENT_DETAILS(patient.id)}
          //           className="inline-flex p-2 text-[#64768b] hover:text-[#0f2d52] hover:bg-[#eef5fa] rounded transition-colors"
          //           title="Ver historial"
          //         >
          //           <FileText className="w-4 h-4" />
          //         </Link>
          //       </td>
          //     </tr>
          //   ))}
          // </tbody>
    //     </table>
    //   </div>
    // </div>
  );
}