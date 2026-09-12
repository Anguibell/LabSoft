using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LabSoft.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdenesController : ControllerBase
{
    private readonly IOrdenService _ordenService;

    public OrdenesController(IOrdenService ordenService)
    {
        _ordenService = ordenService;
    }

    // GET: api/Ordenes
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<OrdenDto>>> GetOrdenes([FromQuery] OrdenQueryFilter filter)
    {
        var result = await _ordenService.GetOrdenesPaginadasAsync(filter);
        return Ok(result);
    }

    // GET: api/Ordenes/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<object>> GetById(Guid id)
    {
        var orden = await _ordenService.GetByIdAsync(id);
        if (orden == null)
        {
            return NotFound(new { mensaje = "La orden no fue encontrada." });
        }
        return Ok(orden);
    }

    // POST: api/Ordenes
    [HttpPost]
    public async Task<ActionResult<OrdenDto>> Crear([FromBody] CreateOrdenDto dto)
    {
        try
        {
            var nuevaOrden = await _ordenService.CrearOrdenAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = nuevaOrden.Id }, nuevaOrden);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // PUT: api/Ordenes/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] UpdateOrdenDto dto)
    {
        try
        {
            var actualizado = await _ordenService.ActualizarOrdenAsync(id, dto);
            if (!actualizado)
            {
                return NotFound(new { mensaje = "La orden no fue encontrada." });
            }
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}