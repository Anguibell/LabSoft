using LabSoft.Application.DTOs;

namespace LabSoft.Application.Interfaces;

public interface IResultadoService
{
    Task<IEnumerable<object>> GetOrdenesPendientesParaProcesarAsync();
    Task<IEnumerable<object>> GetPendientesPorCedulaAsync(string cedula);
    Task<bool> RegistrarResultadoAsync(CrearResultadoDto dto);
    Task RegistrarResultadosMasivosAsync(RegistrarResultadosMasivosDto dto);
    Task<bool> ActualizarResultadoAsync(Guid id, ActualizarResultadoDto dto);
    Task<object?> GetResultadosPorOrdenIdAsync(Guid ordenId);
    Task<IEnumerable<object>> GetResultadosPorCedulaAsync(string cedula);
    Task<bool> ValidarOrdenCompletaAsync(Guid ordenId);
    //HISTORICO
    Task<IEnumerable<object>> GetHistoricoOrdenesAsync(string? cedula, DateTime? fechaInicio, DateTime? fechaFin);
    Task<IEnumerable<HistorialPacienteDto>> GetHistorialPorPacienteIdAsync(Guid pacienteId);
    Task<PagedResultDto<ResultadoGlobalDto>> GetResultadosPaginadosAsync(ResultadoQueryFilter filter);
}