using LabSoft.Application.Interfaces;
using LabSoft.Domain.Entities;
using LabSoft.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabSoft.Infrastructure.Repositories;

public class OrdenRepository : IOrdenRepository
{
    private readonly LabSoftDbContext _context;

    public OrdenRepository(LabSoftDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Orden>> GetAllAsync()
    {
        return await _context.Ordenes
            .Include(o => o.Paciente)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Panel)
            .ToListAsync();
    }

    public async Task<Orden?> GetByIdAsync(Guid id)
    {
        return await _context.Ordenes
            .Include(o => o.Paciente)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.Especialidad)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.SubExamenes)
                        .ThenInclude(sub1 => sub1.SubExamenes)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Resultados) // Clave para el Helper
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Panel)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<(IEnumerable<Orden> Ordenes, int TotalRecords)> GetOrdenesPaginadasAsync(OrdenQueryFilter filter)
    {
        var query = _context.Ordenes
            .Include(o => o.Paciente)
            .AsQueryable();

        // Filtro por Número de Orden Exacto
        if (!string.IsNullOrWhiteSpace(filter.NumeroOrden) && long.TryParse(filter.NumeroOrden, out long numeroOrdenBuscado))
        {
            query = query.Where(o => o.NumeroOrden == numeroOrdenBuscado);
        }

        // Filtro por Paciente (Nombre, Apellido, Cédula)
        if (!string.IsNullOrWhiteSpace(filter.TerminoPaciente))
        {
            var termino = filter.TerminoPaciente.ToLower().Trim();
            query = query.Where(o => 
                o.Paciente.Nombre.ToLower().Contains(termino) ||
                o.Paciente.Apellido.ToLower().Contains(termino) ||
                (o.Paciente.Cedula != null && o.Paciente.Cedula.Contains(termino))
            );
        }

        // Filtro por Estado (Pendiente, Completado, etc.)
        if (!string.IsNullOrWhiteSpace(filter.Estado))
        {
            var estadoUpper = filter.Estado.ToUpper().Trim();
            query = query.Where(o => o.Estado.ToUpper() == estadoUpper);
        }

        // Filtro por Fechas (el filtro de órdenes exige fechas para no colapsar)
        query = query.Where(o => o.FechaCreacion >= filter.FechaInicio.Date);
        query = query.Where(o => o.FechaCreacion < filter.FechaFin.Date.AddDays(1));

        var totalRecords = await query.CountAsync();

        var ordenes = await query
            .OrderByDescending(o => o.FechaCreacion)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Include(o => o.Detalles).ThenInclude(d => d.Examen)
            .Include(o => o.Detalles).ThenInclude(d => d.Panel)
            .ToListAsync();

        return (ordenes, totalRecords);
    }

    public async Task AddAsync(Orden orden)
    {
        await _context.Ordenes.AddAsync(orden);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Orden orden)
    {
        _context.Ordenes.Update(orden);
        await _context.SaveChangesAsync();
    }

    public async Task<Paciente?> GetPacienteByIdAsync(Guid pacienteId)
    {
        return await _context.Pacientes.FindAsync(pacienteId);
    }

    public async Task<Panel?> GetPanelConExamenesAsync(Guid panelId)
    {
        return await _context.Paneles
            .Include(p => p.PanelExamenes)
            .FirstOrDefaultAsync(p => p.Id == panelId);
    }
}