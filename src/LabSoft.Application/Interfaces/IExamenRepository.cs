using LabSoft.Domain.Entities;

namespace LabSoft.Application.Interfaces;

public interface IExamenRepository
{
    Task<IEnumerable<Examen>> GetAllAsync();
    Task<IEnumerable<Examen>> GetByEspecialidadIdAsync(Guid especialidadId);
    Task<IEnumerable<Examen>> GetByEspecialidadAsync(string busqueda);
    Task<Examen?> GetByIdAsync(Guid id);
    Task<Examen?> GetByCodigoAsync(int codigo);
    Task<int> GetNextCodigoByEspecialidadIdAsync(Guid especialidadId);
    Task<int> GetNextOrdenAsync(Guid especialidadId, Guid? examenPadreId);
    Task AddAsync(Examen examen);
    Task UpdateAsync(Examen examen);

    Task<bool> ExistsCodigoAsync(int codigo, Guid excludeId);
    Task<bool> ExistsNombreAsync(string nombre, Guid excludeId);
}