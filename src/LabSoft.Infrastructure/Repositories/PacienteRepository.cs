using Microsoft.EntityFrameworkCore;
using LabSoft.Domain.Entities;
using LabSoft.Application.Interfaces;
using LabSoft.Infrastructure.Persistence;

namespace LabSoft.Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly LabSoftDbContext _context;

    public PacienteRepository(LabSoftDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Paciente>> GetAllAsync()
    {
        return await _context.Pacientes.ToListAsync();
    }

    public async Task<IEnumerable<Paciente>> SearchAsync(string filtro)
    {
        return await _context.Pacientes
            .Where(p => (p.Cedula != null && p.Cedula.Contains(filtro)) || 
                         p.Nombre.ToLower().Contains(filtro.ToLower()) || 
                         p.Apellido.ToLower().Contains(filtro.ToLower()))
            .ToListAsync();
    }

    public async Task<Paciente?> GetByIdAsync(Guid id)
    {
        return await _context.Pacientes.FindAsync(id);
    }


    public async Task UpdateAsync(Paciente paciente)
    {
        _context.Pacientes.Update(paciente);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(Paciente paciente)
    {
        await _context.Pacientes.AddAsync(paciente);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Paciente> Pacientes, int TotalRecords)> GetPacientesPaginadosAsync(PacienteQueryFilter filter)
    {
        var query = _context.Pacientes.AsQueryable();

        // Filtro de Texto Libre
        if (!string.IsNullOrWhiteSpace(filter.TerminoBusqueda))
        {
            var termino = filter.TerminoBusqueda.ToLower().Trim();
            var pacientesIdsConEsaOrden = new List<Guid>();

            // 1. Si el usuario escribe con ceros (ej. "000001"), lo limpiamos y convertimos a número real (1)
            if (int.TryParse(termino, out int numeroOrdenBuscado))
            {
                // Buscamos directamente a qué pacientes les pertenece esa orden exacta
                pacientesIdsConEsaOrden = await _context.Ordenes
                    .Where(o => o.NumeroOrden == numeroOrdenBuscado)
                    .Select(o => o.PacienteId)
                    .Distinct()
                    .ToListAsync();
            }

            // 2. Comparamos todo en la consulta principal
            query = query.Where(p => 
                p.Nombre.ToLower().Contains(termino) || 
                p.Apellido.ToLower().Contains(termino) || 
                (p.Cedula != null && p.Cedula.Contains(termino)) ||
                pacientesIdsConEsaOrden.Contains(p.Id) // Cruzamos los resultados aquí
            );
        }

        // Filtro por Sexo
        if (!string.IsNullOrWhiteSpace(filter.Sexo))
        {
            if (Enum.TryParse<SexoEnum>(filter.Sexo, true, out var sexoEnum))
            {
                query = query.Where(p => p.Sexo == sexoEnum);
            }
        }

        // Filtro por Rango de Fechas
        if (filter.FechaInicio.HasValue)
        {
            query = query.Where(p => p.FechaRegistro >= filter.FechaInicio.Value.Date);
        }

        if (filter.FechaFin.HasValue)
        {
            query = query.Where(p => p.FechaRegistro < filter.FechaFin.Value.Date.AddDays(1));
        }

        // Conteo antes de paginar
        var totalRecords = await query.CountAsync();

        // Paginación y ejecución
        var pacientes = await query
            .OrderByDescending(p => p.FechaRegistro)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        // Consultamos las estadísticas solo para los pacientes de esta página
        var pacienteIds = pacientes.Select(p => p.Id).ToList();

        var stats = await _context.Ordenes
            .Where(o => pacienteIds.Contains(o.PacienteId))
            .GroupBy(o => o.PacienteId)
            .Select(g => new { 
                PacienteId = g.Key, 
                Total = g.Count(), 
                Ultima = g.Max(o => o.FechaCreacion) 
            })
            .ToListAsync();

        // Asignamos los cálculos a los objetos en memoria
        foreach (var p in pacientes)
        {
            var stat = stats.FirstOrDefault(s => s.PacienteId == p.Id);
            p.TotalOrdenes = stat?.Total ?? 0;
            p.UltimaVisita = stat?.Ultima;
        }

        return (pacientes, totalRecords);
    }
}