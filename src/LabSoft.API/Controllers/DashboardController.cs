using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabSoft.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Protegido para cualquier usuario logueado
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("resumen")]
    public async Task<ActionResult<DashboardDto>> GetResumen()
    {
        var resumen = await _dashboardService.GetResumenAsync();
        return Ok(resumen);
    }
}