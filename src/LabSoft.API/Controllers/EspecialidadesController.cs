using LabSoft.Infrastructure.Persistence; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LabSoft.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EspecialidadesController : ControllerBase
{
    private readonly LabSoftDbContext _context;

    public EspecialidadesController(LabSoftDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var especialidades = await _context.Especialidades
            .Where(e => e.Activa)
            .OrderBy(e => e.Codigo)
            .Select(e => new 
            { 
                e.Id, 
                e.Nombre 
            })
            .ToListAsync();

        return Ok(especialidades);
    }
}