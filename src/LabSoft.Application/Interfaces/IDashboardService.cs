using LabSoft.Application.DTOs;

namespace LabSoft.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetResumenAsync();
}