using Microsoft.AspNetCore.Mvc;
using LabSoft.Domain.Entities;
using LabSoft.Application.Interfaces;
using LabSoft.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace LabSoft.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PacientesController : ControllerBase
{
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IResultadoService _resultadoService;

    public PacientesController(IPacienteRepository pacienteRepository, IResultadoService resultadoService)
    {
        _pacienteRepository = pacienteRepository;
        _resultadoService = resultadoService;
    }

    // GET: api/pacientes
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<PacienteDto>>> GetPacientes([FromQuery] PacienteQueryFilter filter)
    {
        var (pacientes, totalRecords) = await _pacienteRepository.GetPacientesPaginadosAsync(filter);

        // Mapeo manual de Entidad a DTO
        var itemsDto = pacientes.Select(p => new PacienteDto
        {
            Id = p.Id,
            Cedula = p.Cedula,
            EsRecienNacido = p.EsRecienNacido,
            Nombre = p.Nombre,
            Apellido = p.Apellido,
            FechaNacimiento = p.FechaNacimiento,
            Sexo = p.Sexo,
            Telefono = p.Telefono,
            TelefonoAlternativo = p.TelefonoAlternativo,
            Correo = p.Correo,
            CorreoAlternativo = p.CorreoAlternativo,
            Domicilio = p.Domicilio,
            NotaMedica = p.NotaMedica,
            NumeroAfiliado = p.NumeroAfiliado,
            Parentesco = p.Parentesco,
            TitularId = p.TitularId,

            // ASIGNACIONES NUEVAS: Calculamos la data al vuelo
            TotalOrdenes = p.TotalOrdenes,
            UltimaVisita = p.UltimaVisita

        }).ToList();

        var result = new PagedResultDto<PacienteDto>
        {
            Items = itemsDto,
            TotalRecords = totalRecords, // Corregido: tu clase dice TotalRecords
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
            // TotalPages no se asigna porque tu clase lo calcula automáticamente con Math.Ceiling
        };

        return Ok(result);
    }

    // GET: api/pacientes/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Paciente>> GetPacienteById(Guid id)
    {
        var paciente = await _pacienteRepository.GetByIdAsync(id);

        if (paciente == null)
        {
            return NotFound();
        }

        return Ok(paciente);
    }

    // POST: api/pacientes
    [HttpPost]
    public async Task<ActionResult> CreatePaciente([FromBody] PacienteDto dto)
    {
        // Mapeamos manualmente el DTO a la entidad
        var paciente = new Paciente
        {
            Id = Guid.NewGuid(),
            Cedula = dto.Cedula,
            EsRecienNacido = dto.EsRecienNacido,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            FechaNacimiento = dto.FechaNacimiento,
            Sexo = dto.Sexo,
            Telefono = dto.Telefono,
            TelefonoAlternativo = dto.TelefonoAlternativo,
            Correo = dto.Correo,
            CorreoAlternativo = dto.CorreoAlternativo,
            Domicilio = dto.Domicilio,
            NotaMedica = dto.NotaMedica,
            NumeroAfiliado = dto.NumeroAfiliado,
            Parentesco = dto.Parentesco,
            TitularId = dto.TitularId
        };

        await _pacienteRepository.AddAsync(paciente);
        
        return CreatedAtAction(nameof(GetPacienteById), new { id = paciente.Id }, paciente);
    }

    // PUT: api/pacientes/5
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePaciente(Guid id, [FromBody] Paciente paciente)
    {
        if (id != paciente.Id)
        {
            return BadRequest("El ID de la URL no coincide con el ID del objeto.");
        }

        var pacienteExistente = await _pacienteRepository.GetByIdAsync(id);
        if (pacienteExistente == null)
        {
            return NotFound();
        }

        // Actualizamos los campos
        pacienteExistente.Cedula = paciente.Cedula;
        pacienteExistente.Nombre = paciente.Nombre;
        pacienteExistente.Apellido = paciente.Apellido;
        pacienteExistente.FechaNacimiento = paciente.FechaNacimiento;
        pacienteExistente.Sexo = paciente.Sexo;
        pacienteExistente.Telefono = paciente.Telefono;
        pacienteExistente.TelefonoAlternativo = paciente.TelefonoAlternativo;
        pacienteExistente.Correo = paciente.Correo;
        pacienteExistente.CorreoAlternativo = paciente.CorreoAlternativo;
        pacienteExistente.Domicilio = paciente.Domicilio;
        pacienteExistente.NotaMedica = paciente.NotaMedica;
        pacienteExistente.NumeroAfiliado = paciente.NumeroAfiliado;
        pacienteExistente.Parentesco = paciente.Parentesco;
        pacienteExistente.TitularId = paciente.TitularId;
        pacienteExistente.EsRecienNacido = paciente.EsRecienNacido;

        await _pacienteRepository.UpdateAsync(pacienteExistente);

        return NoContent(); // 204 No Content indica actualización exitosa
    }

    // GET: api/pacientes/{id}/historial
    [HttpGet("{id:guid}/historial")]
    public async Task<ActionResult<IEnumerable<HistorialPacienteDto>>> GetHistorialClinico(Guid id)
    {
        // 1. Verificamos que el paciente exista
        var paciente = await _pacienteRepository.GetByIdAsync(id);
        if (paciente == null)
        {
            return NotFound(new { mensaje = "El paciente no existe." });
        }

        // 2. Buscamos y devolvemos su historial
        var historial = await _resultadoService.GetHistorialPorPacienteIdAsync(id);
        return Ok(historial);
    }

}