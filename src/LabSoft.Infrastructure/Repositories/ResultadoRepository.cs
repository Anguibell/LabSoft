using LabSoft.Infrastructure.Persistence;
using LabSoft.Application.Interfaces;
using LabSoft.Domain.Entities;
using LabSoft.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LabSoft.Infrastructure.Repositories;

public class ResultadoRepository : IResultadoRepository
{
    private readonly LabSoftDbContext _context;

    public ResultadoRepository(LabSoftDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Orden>> GetOrdenesPendientesAsync()
    {
        return await _context.Ordenes
            .Include(o => o.Paciente)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.Especialidad)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.SubExamenes) // Nivel 1 de hijos
                        .ThenInclude(sub1 => sub1.SubExamenes) // Nivel 2 de hijos (nietos)
                            .ThenInclude(sub2 => sub2.SubExamenes) // Nivel 3 (por si acaso)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Resultados)
            .Where(o => o.Detalles.Any(d => d.Estado == "Pendiente"))
            .ToListAsync();
    }

    public async Task<OrdenDetalle?> GetDetalleByIdAsync(Guid ordenDetalleId)
    {
        return await _context.OrdenDetalles
            .Include(od => od.Resultados)
            .Include(od => od.Examen)
                .ThenInclude(e => e.SubExamenes) // Nivel 1 de hijos
                    .ThenInclude(sub1 => sub1.SubExamenes) // Nivel 2 de hijos (nietos)
                        .ThenInclude(sub2 => sub2.SubExamenes) // Nivel 3 (por si acaso)
            .Include(od => od.Orden) // Importante: incluimos Orden para poder actualizar su estado
                .ThenInclude(o => o.Detalles)
            .FirstOrDefaultAsync(od => od.Id == ordenDetalleId);
    }

    public async Task<Orden?> GetOrdenConResultadosAsync(Guid ordenId)
    {
        return await _context.Ordenes
            .Include(o => o.Paciente)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.Especialidad)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.SubExamenes) // Nivel 1 de hijos
                        .ThenInclude(sub1 => sub1.SubExamenes) // Nivel 2 de hijos (nietos)
                            .ThenInclude(sub2 => sub2.SubExamenes) // Nivel 3 (por si acaso)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Resultados)
            .FirstOrDefaultAsync(o => o.Id == ordenId);
    }

    public async Task<IEnumerable<Orden>> GetOrdenesPorCedulaAsync(string cedula)
    {
        return await _context.Ordenes
            .Include(o => o.Paciente)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.Especialidad)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.SubExamenes) // Nivel 1 de hijos
                        .ThenInclude(sub1 => sub1.SubExamenes) // Nivel 2 de hijos (nietos)
                            .ThenInclude(sub2 => sub2.SubExamenes) // Nivel 3 (por si acaso)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Resultados)
            .Where(o => o.Paciente != null && o.Paciente.Cedula != null && o.Paciente.Cedula.Trim() == cedula.Trim())
            .ToListAsync();
    }

    //HISTORICO
    public async Task<IEnumerable<Orden>> GetHistoricoOrdenesAsync(string? cedula, DateTime? fechaInicio, DateTime? fechaFin)
    {
        var query = _context.Ordenes
            .Include(o => o.Paciente)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.Especialidad)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.SubExamenes) // Nivel 1 de hijos
                        .ThenInclude(sub1 => sub1.SubExamenes) // Nivel 2 de hijos (nietos)
                            .ThenInclude(sub2 => sub2.SubExamenes) // Nivel 3 (por si acaso)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Resultados)
            .AsQueryable();

        // Filtro por cédula si viene especificada
        if (!string.IsNullOrWhiteSpace(cedula))
        {
            query = query.Where(o => o.Paciente != null && o.Paciente.Cedula != null && o.Paciente.Cedula.Trim() == cedula.Trim());
        }

        // Filtro por fecha de inicio (desde) -> Blindaje de Fecha Inicio: Forzamos el Kind a UTC
        if (fechaInicio.HasValue)
        {
            var inicioUtc = DateTime.SpecifyKind(fechaInicio.Value.Date, DateTimeKind.Utc);
            query = query.Where(o => o.FechaCreacion >= inicioUtc);
        }

        // Filtro por fecha fin (hasta) - Le sumamos un día o ajustamos para abarcar todo el día seleccionado
        // Blindaje de Fecha Fin: Forzamos el Kind a UTC y abarcamos todo el día
        if (fechaFin.HasValue)
        {
            var finUtc = DateTime.SpecifyKind(fechaFin.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc); // Final del día (23:59:59)
            query = query.Where(o => o.FechaCreacion <= finUtc);
        }

        return await query.OrderByDescending(o => o.FechaCreacion).ToListAsync();
    }

    public async Task<Resultado?> GetResultadoByIdAsync(Guid id)
    {
        return await _context.Resultados
            .Include(r => r.OrdenDetalle)
                .ThenInclude(od => od.Orden)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Orden>> GetHistorialPorPacienteIdAsync(Guid pacienteId)
    {
        return await _context.Ordenes
            .Include(o => o.Paciente)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.Especialidad)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Examen)
                    .ThenInclude(e => e.SubExamenes) // Nivel 1 (Ej: Fórmula Leucocitaria)
                        .ThenInclude(sub1 => sub1.SubExamenes) // Nivel 2 (Ej: Linfocitos)
                            .ThenInclude(sub2 => sub2.SubExamenes) // Nivel 3 (por si acaso)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Resultados) 
                    .ThenInclude(r => r.Examen) 
            .Where(o => o.PacienteId == pacienteId)
            .OrderByDescending(o => o.FechaCreacion)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Resultado> Resultados, int TotalRecords)> GetResultadosPaginadosAsync(ResultadoQueryFilter filter)
    {

        var query = _context.Resultados
            .Include(r => r.Examen)
                .ThenInclude(e => e.ExamenPadre) // NECESARIO: Para saber el orden del padre (Ej: Fórmula Leuco)
            .Include(r => r.OrdenDetalle)
                .ThenInclude(od => od.Examen)    // NECESARIO: El examen raíz facturado (Ej: Hematología Completa)
                    .ThenInclude(e => e.Especialidad)
            .Include(r => r.OrdenDetalle)
                .ThenInclude(od => od.Orden)
                    .ThenInclude(o => o.Paciente)
            .AsQueryable();

        // Filtro de texto libre (Examen o Paciente o Nro. Orden)
        if (!string.IsNullOrWhiteSpace(filter.TerminoBusqueda))
        {
            var termino = filter.TerminoBusqueda.ToLower().Trim();
            var terminoNumero = termino.Replace("#ord-", "").TrimStart('0'); // Limpia el formato visual

            query = query.Where(r => 
                (r.Examen != null && r.Examen.Nombre.ToLower().Contains(termino)) ||
                (r.OrdenDetalle.Orden.Paciente != null && (
                    r.OrdenDetalle.Orden.Paciente.Nombre.ToLower().Contains(termino) ||
                    r.OrdenDetalle.Orden.Paciente.Apellido.ToLower().Contains(termino) ||
                    (r.OrdenDetalle.Orden.Paciente.Cedula != null && r.OrdenDetalle.Orden.Paciente.Cedula.Contains(termino))
                )) ||
                (r.OrdenDetalle.Orden.NumeroOrden.ToString().Contains(terminoNumero)) // <-- AHORA SÍ TIENE LA VARIABLE
            );
        }

        // Filtro por Estado
        if (!string.IsNullOrWhiteSpace(filter.Estado))
        {
            var estado = filter.Estado.ToUpper().Trim();
            query = query.Where(r => r.Estado.ToUpper() == estado);
        }

        // Filtro por Fechas (Blindaje UTC)
        var inicioUtc = DateTime.SpecifyKind(filter.FechaInicio.Date, DateTimeKind.Utc);
        var finUtc = DateTime.SpecifyKind(filter.FechaFin.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);
        
        query = query.Where(r => r.FechaProcesamiento >= inicioUtc && r.FechaProcesamiento <= finUtc);

        var totalRecords = await query.CountAsync();

        var resultados = await query
            .OrderByDescending(r => r.OrdenDetalle.Orden.FechaCreacion)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return (resultados, totalRecords);
    }

    public async Task AddResultadoAsync(Resultado resultado)
    {
        await _context.Resultados.AddAsync(resultado);
        await _context.SaveChangesAsync();
    }

    // Cambiado de UpdateAsync a UpdateResultadoAsync
    public async Task UpdateResultadoAsync(Resultado resultado)
    {
        _context.Resultados.Update(resultado);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDetalleAsync(OrdenDetalle detalle)
    {
        _context.OrdenDetalles.Update(detalle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrdenEstadoAsync(Orden orden)
    {
        _context.Ordenes.Update(orden);
        await _context.SaveChangesAsync();
    }
}