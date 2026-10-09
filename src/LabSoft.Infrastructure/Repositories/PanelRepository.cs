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
                    .ThenInclude(e => e.SubExamenes) 
                        .ThenInclude(se => se.SubExamenes)
            .Include(p => p.PanelExamenes)
                .ThenInclude(pe => pe.Examen)
                    .ThenInclude(e => e.Especialidad)
            .ToListAsync();
    }

    public async Task<Panel?> GetByIdAsync(Guid id)
    {
        return await _context.Paneles
            .Include(p => p.PanelExamenes)
                .ThenInclude(pe => pe.Examen)
                    .ThenInclude(e => e.SubExamenes)
                        .ThenInclude(se => se.SubExamenes)
            .Include(p => p.PanelExamenes)
                .ThenInclude(pe => pe.Examen)
                    .ThenInclude(e => e.Especialidad)
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

    public async Task DeleteAsync(Guid id)
    {
        // Incluimos la tabla intermedia para poder borrarla
        var panel = await _context.Paneles
            .Include(p => p.PanelExamenes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (panel != null)
        {
            // 1. Borramos las relaciones explícitamente
            _context.RemoveRange(panel.PanelExamenes);
            
            // 2. Borramos el panel
            _context.Paneles.Remove(panel);
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> GetNextCodigoAsync()
    {
        var maxCodigo = await _context.Paneles.MaxAsync(p => (int?)p.Codigo) ?? 2000;
        return maxCodigo + 1;
    }
}