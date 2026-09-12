using LabSoft.Domain.Entities;
using LabSoft.Application.DTOs;

namespace LabSoft.Application.Interfaces;

public interface IResultadoRepository
{
    Task<IEnumerable<Orden>> GetOrdenesPendientesAsync(); // Órdenes que tienen detalles pendientes
    Task<OrdenDetalle?> GetDetalleByIdAsync(Guid ordenDetalleId);
    
    // Métodos para manejar la colección de resultados individualmente
    Task AddResultadoAsync(Resultado resultado);
    Task UpdateResultadoAsync(Resultado resultado);
    
    Task UpdateDetalleAsync(OrdenDetalle detalle);
    Task<Resultado?> GetResultadoByIdAsync(Guid id);
    Task UpdateOrdenEstadoAsync(Orden orden);

    Task<Orden?> GetOrdenConResultadosAsync(Guid ordenId);
    Task<IEnumerable<Orden>> GetOrdenesPorCedulaAsync(string cedula);

    // HISTORICO: IHistoricoRepository.cs
    Task<IEnumerable<Orden>> GetHistoricoOrdenesAsync(string? cedula, DateTime? fechaInicio, DateTime? fechaFin);
    Task<IEnumerable<Orden>> GetHistorialPorPacienteIdAsync(Guid pacienteId);
    Task<(IEnumerable<Resultado> Resultados, int TotalRecords)> GetResultadosPaginadosAsync(ResultadoQueryFilter filter);
    
}