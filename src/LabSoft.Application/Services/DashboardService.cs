using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;

namespace LabSoft.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<DashboardDto> GetResumenAsync()
    {
        // Calculamos las fechas aquí para que la capa de aplicación mantenga la lógica de negocio temporal
        var hoy = DateTime.UtcNow.Date;
        var inicioMes = new DateTime(hoy.Year, hoy.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        return await _dashboardRepository.GetResumenAsync(hoy, inicioMes);
    }
}