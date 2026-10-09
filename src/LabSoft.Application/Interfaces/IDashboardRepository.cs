using LabSoft.Application.DTOs;

namespace LabSoft.Application.Interfaces;

public interface IDashboardRepository
{
    Task<DashboardDto> GetResumenAsync(DateTime hoy, DateTime inicioMes);
}