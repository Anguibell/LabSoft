using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LabSoft.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using LabSoft.Domain.Constants;

namespace LabSoft.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize (Roles = RolesConstantes.BIOANALISTA)]
public class ResultadosController : ControllerBase
{
    private readonly IResultadoService _resultadoService;
    private readonly IPdfService _pdfService;

    public ResultadosController(IResultadoService resultadoService, IPdfService pdfService)
    {
        _resultadoService = resultadoService;
        _pdfService = pdfService;
    }

    // GET: api/Resultados
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ResultadoGlobalDto>>> GetResultadosPaginados([FromQuery] ResultadoQueryFilter filter)
    {
        var result = await _resultadoService.GetResultadosPaginadosAsync(filter);
        return Ok(result);
    }

    // GET: api/Resultados/pendientes
    [HttpGet("pendientes")]
    public async Task<ActionResult<IEnumerable<object>>> GetPendientes()
    {
        var pendientes = await _resultadoService.GetOrdenesPendientesParaProcesarAsync();
        return Ok(pendientes);
    }

    // POST: api/Resultados
    [HttpPost]
    public async Task<ActionResult<ResultadoDto>> Registrar([FromBody] CrearResultadoDto dto)
    {
        try
        {
            var resultado = await _resultadoService.RegistrarResultadoAsync(dto);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("masivos")]
    public async Task<IActionResult> RegistrarResultadosMasivos([FromBody] RegistrarResultadosMasivosDto dto)
    {
        try
        {
            await _resultadoService.RegistrarResultadosMasivosAsync(dto);
            return Ok(new { mensaje = "Resultados registrados masivamente con éxito." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // GET: api/Resultados/pendientes/por-cedula/{cedula}
    [HttpGet("pendientes/por-cedula/{cedula}")]
    public async Task<ActionResult<IEnumerable<object>>> GetPendientesPorCedula(string cedula)
    {
        var pendientes = await _resultadoService.GetPendientesPorCedulaAsync(cedula);
        return Ok(pendientes);
    }

    // GET: api/Resultados/orden/{ordenId}
    [HttpGet("orden/{ordenId}")]
    public async Task<ActionResult<object>> GetResultadosPorOrden(Guid ordenId)
    {
        var resultado = await _resultadoService.GetResultadosPorOrdenIdAsync(ordenId);
        if (resultado == null)
        {
            return NotFound(new { error = "La orden especificada no existe." });
        }
        return Ok(resultado);
    }

    // GET: api/Resultados/cedula/{cedula}
    [HttpGet("cedula/{cedula}")]
    public async Task<ActionResult<IEnumerable<object>>> GetResultadosPorCedula(string cedula)
    {
        var resultados = await _resultadoService.GetResultadosPorCedulaAsync(cedula);
        return Ok(resultados);
    }

    // PUT: api/Resultados/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarResultado(Guid id, [FromBody] ActualizarResultadoDto dto)
    {
        try
        {
            await _resultadoService.ActualizarResultadoAsync(id, dto);
            return Ok(new { mensaje = "Resultado actualizado exitosamente." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
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

    // PUT: api/Resultados/masivos
    // Permite usar el verbo PUT para actualizaciones masivas manteniendo la misma lógica del POST
    [HttpPut("masivos")]
    public async Task<IActionResult> ActualizarResultadosMasivos([FromBody] RegistrarResultadosMasivosDto dto)
    {
        try
        {
            await _resultadoService.RegistrarResultadosMasivosAsync(dto);
            return Ok(new { mensaje = "Resultados actualizados masivamente con éxito." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // PUT: api/Resultados/validar-orden/{ordenId}
    [HttpPut("validar-orden/{ordenId}")]
    public async Task<ActionResult<bool>> ValidarOrden(Guid ordenId)
    {
        try
        {
            var resultado = await _resultadoService.ValidarOrdenCompletaAsync(ordenId);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    //HISTORICO
    // GET: api/Resultados/historico
    [HttpGet("historico")]
    public async Task<ActionResult<IEnumerable<object>>> GetHistorico(
        [FromQuery] string? cedula, 
        [FromQuery] DateTime? fechaInicio, 
        [FromQuery] DateTime? fechaFin)
    {
        var historico = await _resultadoService.GetHistoricoOrdenesAsync(cedula, fechaInicio, fechaFin);
        return Ok(historico);
    }

    //REPORTES DE RESULTADOS EN PDF
    // GET: api/Resultados/pdf/{ordenId}
    [HttpGet("pdf/{ordenId}")]
    public IActionResult DescargarPdfResultados(Guid ordenId)
    {
        try
        {
            // Desestructuramos la tupla que viene del servicio
            var (pdfBytes, fileName) = _pdfService.GenerarReporteResultadosPdf(ordenId); 

            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("{id}/invalidar")]
    [Authorize] // Más adelante lo restringiremos con Roles
    public async Task<IActionResult> InvalidarResultado(Guid id, [FromBody] InvalidarResultadoDto dto)
    {
        try
        {
            await _resultadoService.InvalidarResultadoAsync(id, dto);
            return Ok(new { message = "Resultado invalidado y abierto para corrección exitosamente." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno al invalidar el resultado.", error = ex.Message });
        }
    }

    
}