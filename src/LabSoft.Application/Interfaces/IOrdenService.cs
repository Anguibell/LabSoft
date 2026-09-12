using LabSoft.Application.DTOs;

namespace LabSoft.Application.Interfaces;

public interface IOrdenService
{
    Task<IEnumerable<OrdenDto>> GetAllAsync();
    Task<PagedResultDto<OrdenDto>> GetOrdenesPaginadasAsync(OrdenQueryFilter filter);
    Task<object?> GetByIdAsync(Guid id);
    Task<OrdenDto> CrearOrdenAsync(CreateOrdenDto dto);
    Task<bool> ActualizarOrdenAsync(Guid id, UpdateOrdenDto dto);
}