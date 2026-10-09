using LabSoft.Domain.Entities;

namespace LabSoft.Application.Interfaces;

public interface IPacienteRepository
{
    Task<IEnumerable<Paciente>> GetAllAsync();
    Task<IEnumerable<Paciente>> SearchAsync(string filtro); 
    Task<Paciente?> GetByIdAsync(Guid id);
    Task UpdateAsync(Paciente paciente);
    Task AddAsync(Paciente paciente);
    Task<bool> ExisteCedulaAsync(string cedula, Guid? pacienteIdAExcluir = null);
    Task<(IEnumerable<Paciente> Pacientes, int TotalRecords)> GetPacientesPaginadosAsync(PacienteQueryFilter filter);
}