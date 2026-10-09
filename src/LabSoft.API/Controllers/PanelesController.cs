using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;
using LabSoft.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LabSoft.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PanelesController : ControllerBase
{
    private readonly IPanelRepository _panelRepository;

    public PanelesController(IPanelRepository panelRepository)
    {
        _panelRepository = panelRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PanelDto>>> GetAll()
    {
        var paneles = await _panelRepository.GetAllAsync();
        return Ok(paneles.Select(p => new PanelDto
        {
            Id = p.Id,
            Codigo = p.Codigo,
            Nombre = p.Nombre,
            Activo = p.Activo,
            // Reutilizamos tu lógica de mapeo de exámenes o mapeamos directamente:
            Examenes = p.PanelExamenes
                .OrderBy(pe => pe.Examen.Orden) // Respetamos el orden general
                .Select(pe => MapearExamenADto(pe.Examen)).ToList()
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PanelDto>> GetById(Guid id)
    {
        var panel = await _panelRepository.GetByIdAsync(id);
        if (panel == null) return NotFound(new { mensaje = "Panel no encontrado." });

        return Ok(new PanelDto
        {
            Id = panel.Id,
            Codigo = panel.Codigo,
            Nombre = panel.Nombre,
            Activo = panel.Activo,
            Examenes = panel.PanelExamenes
                .OrderBy(pe => pe.Examen.Orden)
                .Select(pe => MapearExamenADto(pe.Examen)).ToList()
        });
    }

    // Método auxiliar 
    private ExamenDto MapearExamenADto(Examen examen)
    {
        return new ExamenDto
        {
            Id = examen.Id,
            Codigo = examen.Codigo,
            Nombre = examen.Nombre,
            EspecialidadId = examen.EspecialidadId,
            EspecialidadNombre = examen.Especialidad?.Nombre ?? string.Empty,
            Orden = examen.Orden,
            Precio = examen.Precio,
            Unidad = examen.Unidad,
            ValoresReferencia = examen.ValoresReferencia,
            ValoresReferenciaM = examen.ValoresReferenciaM,
            ValoresReferenciaF = examen.ValoresReferenciaF,
            Activo = examen.Activo,
            EsOpcionalDinamico = examen.EsOpcionalDinamico,
            DecimalesRequeridos = examen.DecimalesRequeridos,
            // Recursividad para mantener el árbol ordenado
            SubExamenes = examen.SubExamenes?
                .OrderBy(s => s.Orden)
                .Select(MapearExamenADto).ToList() ?? new List<ExamenDto>()
        };
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateUpdatePanelDto dto)
    {
        var panel = new Panel
        {
            Id = Guid.NewGuid(),
            Codigo = await _panelRepository.GetNextCodigoAsync(),
            Nombre = dto.Nombre,
            PanelExamenes = dto.ExamenIds.Select(id => new PanelExamen { ExamenId = id }).ToList()
        };

        await _panelRepository.AddAsync(panel);
        return CreatedAtAction(nameof(GetAll), new { id = panel.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateUpdatePanelDto dto)
    {
        var panel = await _panelRepository.GetByIdAsync(id);
        if (panel == null)
        {
            return NotFound(new { mensaje = "Panel no encontrado." });
        }

        panel.Nombre = dto.Nombre;

        // Actualizamos la lista de exámenes limpiando los anteriores y asignando los nuevos IDs
        panel.PanelExamenes = dto.ExamenIds.Select(examenId => new PanelExamen 
        { 
            PanelId = panel.Id, 
            ExamenId = examenId 
        }).ToList();

        await _panelRepository.UpdateAsync(panel);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var panel = await _panelRepository.GetByIdAsync(id);
        if (panel == null)
        {
            return NotFound(new { mensaje = "Panel no encontrado." });
        }

        await _panelRepository.DeleteAsync(id);
        return NoContent();
    }

}