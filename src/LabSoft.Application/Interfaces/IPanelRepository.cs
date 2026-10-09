using LabSoft.Domain.Entities;

namespace LabSoft.Application.Interfaces;

public interface IPanelRepository
{
    Task<IEnumerable<Panel>> GetAllAsync();
    Task<Panel?> GetByIdAsync(Guid id);
    Task AddAsync(Panel panel);
    Task UpdateAsync(Panel panel);
    Task DeleteAsync(Guid id);
    Task<int> GetNextCodigoAsync();
}