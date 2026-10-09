using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;
using LabSoft.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabSoft.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly LabSoftDbContext _context;

    public DashboardRepository(LabSoftDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetResumenAsync(DateTime hoy, DateTime inicioMes)
    {
        var totalPacientes = await _context.Pacientes.CountAsync();
        
        var ordenesHoy = await _context.Ordenes
            .CountAsync(o => o.FechaCreacion >= hoy);
            
        var ordenesPendientes = await _context.Ordenes
            .CountAsync(o => o.Estado == "Pendiente" || o.Estado == "En Proceso");

        var resultadosValidadosMes = await _context.Resultados
            .CountAsync(r => r.Estado == "Validado" && r.FechaProcesamiento >= inicioMes);

        var ordenesRecientes = await _context.Ordenes
            .Include(o => o.Paciente)
            .OrderByDescending(o => o.FechaCreacion)
            .Take(5)
            .Select(o => new OrdenRecienteDto
            {
                Id = o.Id,
                NumeroOrden = o.NumeroOrden,
                Paciente = o.Paciente != null ? $"{o.Paciente.Nombre} {o.Paciente.Apellido}" : "Desconocido",
                Fecha = o.FechaCreacion,
                Estado = o.Estado
            })
            .ToListAsync();

        return new DashboardDto
        {
            TotalPacientes = totalPacientes,
            OrdenesHoy = ordenesHoy,
            OrdenesPendientes = ordenesPendientes,
            ResultadosValidadosMes = resultadosValidadosMes,
            OrdenesRecientes = ordenesRecientes
        };
    }
}