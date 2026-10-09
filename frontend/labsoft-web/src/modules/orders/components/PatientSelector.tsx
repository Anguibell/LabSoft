import { useState, useMemo, useRef, useEffect } from 'react';
import { Search, UserCircle, X, CheckCircle2 } from 'lucide-react';
import { usePatients } from '../../patients/hooks/usePatients'; 

interface PatientSelectorProps {
  value: string;
  onChange: (patientId: string) => void;
  error?: string;
}

export default function PatientSelector({ value, onChange, error }: PatientSelectorProps) {
  const [searchTerm, setSearchTerm] = useState('');
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  const { data: pagedResult, isLoading } = usePatients({ pageNumber: 1, pageSize: 100 });

  // FIX 1: Envolvemos el arreglo en un useMemo para estabilizar su referencia en memoria
  const patientsList = useMemo(() => pagedResult?.items || [], [pagedResult?.items]);

  const filteredPatients = useMemo(() => {
    const lowerSearch = searchTerm.toLowerCase();
    return patientsList.filter((p) => 
      p.nombre.toLowerCase().includes(lowerSearch) ||
      p.apellido.toLowerCase().includes(lowerSearch) ||
      (p.cedula && p.cedula.toLowerCase().includes(lowerSearch))
    );
  }, [patientsList, searchTerm]);

  const selectedPatient = useMemo(() => {
    return patientsList.find((p) => p.id === value);
  }, [patientsList, value]);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  if (selectedPatient) {
    // FIX 2: Conversión segura a String para evitar el error de solapamiento del Enum
    const isMasculino = String(selectedPatient.sexo) === 'M' || String(selectedPatient.sexo) === '0' || String(selectedPatient.sexo).toLowerCase() === 'masculino';
    
    return (
      <div className="p-4 bg-[#f3f7fb] border border-[#2a79b7] rounded-lg flex items-center justify-between shadow-sm animate-in fade-in duration-200">
        <div className="flex items-center gap-4">
          <div className="w-10 h-10 bg-[#2a79b7] rounded-full flex items-center justify-center text-white font-bold shadow-sm">
            {selectedPatient.nombre.charAt(0)}{selectedPatient.apellido.charAt(0)}
          </div>
          <div>
            <h4 className="text-sm font-bold text-[#15243b]">
              {selectedPatient.nombre} {selectedPatient.apellido}
            </h4>
            <p className="text-xs text-[#64768b] font-medium mt-0.5">
              C.I: {selectedPatient.cedula || 'N/A'} • {isMasculino ? 'Masculino' : 'Femenino'}
            </p>
          </div>
        </div>
        <button
          type="button"
          onClick={() => {
            onChange(''); 
            setSearchTerm('');
          }}
          className="p-2 text-[#64768b] hover:text-red-500 hover:bg-red-50 rounded-lg transition-colors"
          title="Cambiar paciente"
        >
          <X className="w-5 h-5" />
        </button>
      </div>
    );
  }

  return (
    <div className="relative" ref={dropdownRef}>
      <div className="relative">
        <Search className="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-[#94a3b8]" />
        <input
          type="text"
          placeholder="Buscar paciente por nombre, apellido o cédula..."
          value={searchTerm}
          onChange={(e) => {
            setSearchTerm(e.target.value);
            setIsOpen(true);
          }}
          onFocus={() => setIsOpen(true)}
          className={`w-full pl-10 pr-4 py-3 border rounded-lg text-sm focus:outline-none focus:ring-1 focus:border-[#2a79b7] transition-colors ${
            error ? 'border-red-300 focus:ring-red-500' : 'border-[#dce5ef] focus:ring-[#2a79b7]'
          }`}
        />
      </div>

      {isOpen && (
        <div className="absolute z-10 w-full mt-1 bg-white border border-[#dce5ef] rounded-lg shadow-lg max-h-60 overflow-y-auto animate-in fade-in slide-in-from-top-2 duration-200">
          {isLoading ? (
            <div className="p-4 text-center text-sm text-[#64768b]">Buscando pacientes...</div>
          ) : filteredPatients.length === 0 ? (
            <div className="p-4 flex flex-col items-center justify-center text-[#64768b]">
              <UserCircle className="w-8 h-8 mb-2 text-[#dce5ef]" />
              <p className="text-sm">No se encontraron pacientes.</p>
            </div>
          ) : (
            <ul className="py-1">
              {filteredPatients.map((p) => (
                <li key={p.id}>
                  <button
                    type="button"
                    onClick={() => {
                      onChange(p.id);
                      setIsOpen(false);
                    }}
                    className="w-full text-left px-4 py-2 hover:bg-[#f3f7fb] flex items-center justify-between group transition-colors"
                  >
                    <div>
                      <p className="text-sm font-bold text-[#15243b] group-hover:text-[#2a79b7] transition-colors">
                        {p.nombre} {p.apellido}
                      </p>
                      <p className="text-xs text-[#64768b]">C.I: {p.cedula || 'N/A'}</p>
                    </div>
                    <CheckCircle2 className="w-4 h-4 text-transparent group-hover:text-[#2a79b7] transition-colors" />
                  </button>
                </li>
              ))}
            </ul>
          )}
        </div>
      )}
    </div>
  );
}