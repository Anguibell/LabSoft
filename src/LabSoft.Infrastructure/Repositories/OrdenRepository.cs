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
                .ThenInclude(d => d.Resultados) 
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Panel)
                    .ThenInclude(p => p!.PanelExamenes)
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

        // // Filtro por Paciente (Nombre, Apellido, Cédula)
        // if (!string.IsNullOrWhiteSpace(filter.TerminoPaciente))
        // {
        //     var termino = filter.TerminoPaciente.ToLower().Trim();
        //     query = query.Where(o => 
        //         o.Paciente.Nombre.ToLower().Contains(termino) ||
        //         o.Paciente.Apellido.ToLower().Contains(termino) ||
        //         (o.Paciente.Cedula != null && o.Paciente.Cedula.Contains(termino))
        //     );
        // }

        // Filtro por Paciente (Nombre, Apellido, Cédula o Número de Orden)
        if (!string.IsNullOrWhiteSpace(filter.TerminoPaciente))
        {
            var termino = filter.TerminoPaciente.ToLower().Trim();
            // Truco: Si el usuario escribe "#ORD-0003" o "0003", extraemos solo el número real
            var terminoNumero = termino.Replace("#ord-", "").TrimStart('0'); 

            query = query.Where(o =>
                o.Paciente.Nombre.ToLower().Contains(termino) ||
                o.Paciente.Apellido.ToLower().Contains(termino) ||
                (o.Paciente.Cedula != null && o.Paciente.Cedula.Contains(termino)) ||
                o.NumeroOrden.ToString().Contains(terminoNumero) // <-- Búsqueda por número
            );
        }

        // Filtro por Estado (Pendiente, Completado, etc.)
        if (!string.IsNullOrWhiteSpace(filter.Estado))
        {
            var estadoUpper = filter.Estado.ToUpper().Trim();
            query = query.Where(o => o.Estado.ToUpper() == estadoUpper);
        }

        // Filtro por Fechas (SOLO si no están buscando a alguien en específico)
        if (string.IsNullOrWhiteSpace(filter.TerminoPaciente))
        {
            var inicioUtc = DateTime.SpecifyKind(filter.FechaInicio.Date, DateTimeKind.Utc);
            var finUtc = DateTime.SpecifyKind(filter.FechaFin.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);
            query = query.Where(o => o.FechaCreacion >= inicioUtc && o.FechaCreacion <= finUtc);
        }

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

    // public async Task UpdateAsync(Orden orden)
    // {
    //     _context.Ordenes.Update(orden);
    //     await _context.SaveChangesAsync();
    // }

    public async Task UpdateConDetallesAsync(Orden orden, List<OrdenDetalle> nuevosDetalles)
    {
        // 1. Limpiamos absolutamente toda la memoria caché de EF Core
        // Esto evita que EF Core recuerde la orden gigante que cargamos al principio
        _context.ChangeTracker.Clear();

        // 2. Abrimos una Transacción Explícita en la Base de Datos
        // Si cualquiera de estos pasos falla, no se guardará nada y la orden quedará intacta
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // PASO A: ELIMINAR LOS DETALLES VIEJOS PRIMERO
            var detallesViejos = await _context.Set<OrdenDetalle>()
                .Where(d => d.OrdenId == orden.Id)
                .ToListAsync();

            if (detallesViejos.Any())
            {
                _context.Set<OrdenDetalle>().RemoveRange(detallesViejos);
                await _context.SaveChangesAsync(); // Forzamos el DELETE inmediato
            }

            // PASO B: ACTUALIZAR EL ESTADO DE LA ORDEN PRINCIPAL
            // Buscamos una versión fresca y limpia directamente de la BD
            var ordenDb = await _context.Ordenes.FindAsync(orden.Id);
            if (ordenDb != null)
            {
                ordenDb.Estado = orden.Estado;
                await _context.SaveChangesAsync(); // Forzamos el UPDATE inmediato
            }

            // PASO C: INSERTAR LOS NUEVOS DETALLES
            if (nuevosDetalles.Any())
            {
                // Aseguramos que apunten al ID correcto por si acaso
                nuevosDetalles.ForEach(d => d.OrdenId = orden.Id);
                
                await _context.Set<OrdenDetalle>().AddRangeAsync(nuevosDetalles);
                await _context.SaveChangesAsync(); // Forzamos el INSERT inmediato
            }

            // Si llegamos hasta aquí sin errores, confirmamos todos los cambios juntos
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            // Si algo explota (como el error que tenías), hacemos Rollback
            await transaction.RollbackAsync();
            throw; // Relanzamos el error para que el Frontend lo muestre
        }
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