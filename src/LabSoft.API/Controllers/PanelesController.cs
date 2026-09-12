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
            Nombre = p.Nombre,
            Activo = p.Activo,
            Examenes = p.PanelExamenes.Select(pe => new ExamenSimpleDto { Id = pe.ExamenId, Nombre = pe.Examen.Nombre }).ToList()
        }));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateUpdatePanelDto dto)
    {
        var panel = new Panel
        {
            Id = Guid.NewGuid(),
            Nombre = dto.Nombre,
            PanelExamenes = dto.ExamenIds.Select(id => new PanelExamen { ExamenId = id }).ToList()
        };

        await _panelRepository.AddAsync(panel);
        return CreatedAtAction(nameof(GetAll), new { id = panel.Id }, null);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PanelDto>> GetById(Guid id)
    {
        var panel = await _panelRepository.GetByIdAsync(id);
        if (panel == null)
        {
            return NotFound(new { mensaje = "Panel no encontrado." });
        }

        return Ok(new PanelDto
        {
            Id = panel.Id,
            Nombre = panel.Nombre,
            Activo = panel.Activo,
            Examenes = panel.PanelExamenes.Select(pe => new ExamenSimpleDto 
            { 
                Id = pe.ExamenId, 
                Nombre = pe.Examen.Nombre 
            }).ToList()
        });
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

}