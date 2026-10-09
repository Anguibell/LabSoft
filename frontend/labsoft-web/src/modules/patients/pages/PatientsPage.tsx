import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Plus } from 'lucide-react';
import { ROUTES } from '../../../routes/routeConfig';
import { usePatients } from '../hooks/usePatients';
import PatientTable from '../components/PatientTable';
import PageHeader from '../../../components/common/PageHeader';
import SearchInput from '../../../components/common/SearchInput';
import Button from '../../../components/ui/Button';

export default function PatientsPage() {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState('');
  
  const { data, isLoading, isError } = usePatients({
    terminoBusqueda: searchTerm,
    pageNumber: 1,
    pageSize: 10,
  });

  return (
    <div className="space-y-6 animate-in fade-in duration-300">

      <PageHeader 
        title="Pacientes" 
        description="Gestión del directorio de pacientes y su historial clínico."
        action={
          <Button icon={Plus} onClick={() => navigate(ROUTES.PATIENT_NEW)}>
            Nuevo Paciente
          </Button>
        }
      />

      <SearchInput 
        value={searchTerm} 
        onChange={setSearchTerm} 
        placeholder="Buscar por cédula, nombre o apellido..." 
      />

      {isError ? (
        <div className="p-8 text-center text-red-500 bg-red-50 rounded-xl border border-red-100">
          Error al cargar los pacientes. Verifica la conexión.
        </div>
      ) : (
        <PatientTable patients={data?.items || []} isLoading={isLoading} />
      )}

    </div>
  );
}