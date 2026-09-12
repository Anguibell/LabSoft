using LabSoft.Application.Interfaces;
using LabSoft.Domain.Entities;
using LabSoft.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabSoft.Infrastructure.Repositories;

public class PanelRepository : IPanelRepository
{
    private readonly LabSoftDbContext _context;

    public PanelRepository(LabSoftDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Panel>> GetAllAsync()
    {
        return await _context.Paneles
            .Include(p => p.PanelExamenes)
                .ThenInclude(pe => pe.Examen)
            .ToListAsync();
    }

    public async Task<Panel?> GetByIdAsync(Guid id)
    {
        return await _context.Paneles
            .Include(p => p.PanelExamenes)
                .ThenInclude(pe => pe.Examen)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Panel panel)
    {
        await _context.Paneles.AddAsync(panel);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Panel panel)
    {
        _context.Paneles.Update(panel);
        await _context.SaveChangesAsync();
    }
}