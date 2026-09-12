using LabSoft.Application.Interfaces;
using LabSoft.Domain.Entities;
using LabSoft.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabSoft.Infrastructure.Repositories;

public class ExamenRepository : IExamenRepository
{
    private readonly LabSoftDbContext _context;

    public ExamenRepository(LabSoftDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Examen>> GetAllAsync()
    {
        // Traemos las especialidades y el árbol de subcomponentes (Padre -> Hijos -> Nietos)
        return await _context.Examenes
            .Where(e => e.ExamenPadreId == null) // Solo exámenes principales
            .OrderBy(e => e.Especialidad!.Codigo)  // 1. Ordena principales por Codigo de Especialidad
            .ThenBy(e => e.Codigo)                // 2. Luego por Codigo del Examen
            .Include(e => e.Especialidad)         // 3. Incluye las Especialidades
            .Include(e => e.SubExamenes           // 4. Incluye hijos de nivel 1 ordenados
                .OrderBy(sub => sub.Codigo))
                .ThenInclude(sub => sub.SubExamenes // 5. Incluye nietos de nivel 2 ordenados
                    .OrderBy(nieto => nieto.Codigo))
            .ToListAsync();
    }

    public async Task<IEnumerable<Examen>> GetByEspecialidadIdAsync(Guid especialidadId)
    {
        return await _context.Examenes
            .Where(e => e.ExamenPadreId == null && e.EspecialidadId == especialidadId)
            .OrderBy(e => e.Codigo)
            .Include(e => e.Especialidad)
            .Include(e => e.SubExamenes
                .OrderBy(sub => sub.Codigo))
                .ThenInclude(sub => sub.SubExamenes
                    .OrderBy(nieto => nieto.Codigo))
            .ToListAsync();
    }

    public async Task<IEnumerable<Examen>> GetByEspecialidadAsync(string busqueda)
    {
        return await _context.Examenes
            .Where(e => e.ExamenPadreId == null && // 1. Solo exámenes principales (evita duplicados)
                    (e.Especialidad!.Nombre.ToLower().Contains(busqueda.ToLower()) || 
                        e.Especialidad.Codigo.ToString() == busqueda))
            .OrderBy(e => e.Codigo) // 2. Ordena los principales por código
            .Include(e => e.Especialidad)
            .Include(e => e.SubExamenes
                .OrderBy(sub => sub.Codigo)) // 3. Ordena hijos
                .ThenInclude(sub => sub.SubExamenes
                    .OrderBy(nieto => nieto.Codigo)) // 4. Ordena nietos
            .ToListAsync();
    }

    public async Task<Examen?> GetByIdAsync(Guid id)
        {
            return await _context.Examenes
                .Include(e => e.Especialidad)
                .Include(e => e.SubExamenes
                    .OrderBy(sub => sub.Codigo))
                    .ThenInclude(sub => sub.SubExamenes
                        .OrderBy(nieto => nieto.Codigo))
                .FirstOrDefaultAsync(e => e.Id == id);
        }

    public async Task<Examen?> GetByCodigoAsync(int codigo)
    {
        return await _context.Examenes
            .Include(e => e.Especialidad)
            .Include(e => e.SubExamenes
                .OrderBy(sub => sub.Codigo))
                .ThenInclude(sub => sub.SubExamenes
                    .OrderBy(nieto => nieto.Codigo))
            .FirstOrDefaultAsync(e => e.Codigo == codigo);
    }

    public async Task<bool> ExistsCodigoAsync(int codigo, Guid excludeId)
    {
        return await _context.Examenes
            .AnyAsync(e => e.Codigo == codigo && e.Id != excludeId);
    }

    public async Task<bool> ExistsNombreAsync(string nombre, Guid excludeId)
    {
        return await _context.Examenes
            .AnyAsync(e => e.Nombre.ToLower() == nombre.ToLower() && e.Id != excludeId);
    }

    public async Task<int> GetNextCodigoByEspecialidadIdAsync(Guid especialidadId)
    {
        // Obtener los códigos de exámenes de esa especialidad
        var codigos = await _context.Examenes
            .Where(e => e.EspecialidadId == especialidadId)
            .Select(e => e.Codigo)
            .ToListAsync();

        if (codigos.Any())
        {
            return codigos.Max() + 1;
        }

        // Si la especialidad no tiene exámenes, obtener el código de la especialidad (ej. 1 -> 101)
        var especialidad = await _context.Especialidades
            .FirstOrDefaultAsync(e => e.Id == especialidadId);

        int codigoBase = especialidad?.Codigo ?? 1;
        return (codigoBase * 100) + 1;
    }

    public async Task<int> GetNextOrdenAsync(Guid especialidadId, Guid? examenPadreId)
    {
        // Si es un subexamen, buscamos el máximo Orden entre los hijos de ese padre
        if (examenPadreId.HasValue)
        {
            var maxOrdenHijos = await _context.Examenes
                .Where(e => e.ExamenPadreId == examenPadreId.Value)
                .Select(e => (int?)e.Orden)
                .MaxAsync();

            return (maxOrdenHijos ?? 0) + 1;
        }

        // Si es un examen principal, buscamos el máximo Orden de los exámenes principales de esa especialidad
        var maxOrdenPadres = await _context.Examenes
            .Where(e => e.EspecialidadId == especialidadId && e.ExamenPadreId == null)
            .Select(e => (int?)e.Orden)
            .MaxAsync();

        return (maxOrdenPadres ?? 0) + 1;
    }

    public async Task AddAsync(Examen examen)
    {
        await _context.Examenes.AddAsync(examen);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Examen examen)
    {
        _context.Examenes.Update(examen);
        await _context.SaveChangesAsync();
    }
}