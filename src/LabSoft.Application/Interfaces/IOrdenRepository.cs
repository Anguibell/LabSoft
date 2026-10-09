using LabSoft.Domain.Entities;

namespace LabSoft.Application.Interfaces;

public interface IOrdenRepository
{
    Task<IEnumerable<Orden>> GetAllAsync();
    Task<(IEnumerable<Orden> Ordenes, int TotalRecords)> GetOrdenesPaginadasAsync(OrdenQueryFilter filter);
    Task<Orden?> GetByIdAsync(Guid id);
    Task AddAsync(Orden orden);
    Task UpdateConDetallesAsync(Orden orden, List<OrdenDetalle> nuevosDetalles);

    // Métodos auxiliares para la creación inteligente
    Task<Paciente?> GetPacienteByIdAsync(Guid pacienteId);
    Task<Panel?> GetPanelConExamenesAsync(Guid panelId);
}