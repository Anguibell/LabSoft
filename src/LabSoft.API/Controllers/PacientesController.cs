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
            EdadFormateada = p.EdadFormateada,
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
            TitularNombre = p.Titular != null ? $"{p.Titular.Nombre} {p.Titular.Apellido}" : null,
            TitularCedula = p.Titular?.Cedula,

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
    public async Task<ActionResult<PacienteDto>> GetPacienteById(Guid id)
    {
        var p = await _pacienteRepository.GetByIdAsync(id);

        if (p == null) return NotFound();

        var dto = new PacienteDto
        {
            Id = p.Id,
            Cedula = p.Cedula,
            EsRecienNacido = p.EsRecienNacido,
            Nombre = p.Nombre,
            Apellido = p.Apellido,
            FechaNacimiento = p.FechaNacimiento,
            EdadFormateada = p.EdadFormateada,
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
            TitularNombre = p.Titular != null ? $"{p.Titular.Nombre} {p.Titular.Apellido}" : null,
            TitularCedula = p.Titular?.Cedula
        };

        return Ok(dto);
    }

    // POST: api/pacientes
    [HttpPost]
    public async Task<ActionResult> CreatePaciente([FromBody] PacienteDto dto)
    {
        // 1. VALIDACIÓN: Cédula única
        if (!string.IsNullOrWhiteSpace(dto.Cedula))
        {
            bool existe = await _pacienteRepository.ExisteCedulaAsync(dto.Cedula);
            if (existe) 
            {
                return BadRequest(new { message = "La cédula ingresada ya se encuentra registrada en otro paciente." });
            }
        }

        // 2. VALIDACIÓN: El representante debe existir y NO puede ser un menor sin ID
        if (dto.TitularId.HasValue)
        {
            var titular = await _pacienteRepository.GetByIdAsync(dto.TitularId.Value);
            if (titular == null)
            {
                return BadRequest(new { message = "El representante legal seleccionado no existe en el sistema." });
            }
            // --> NUEVA REGLA DE NEGOCIO <--
            if (titular.EsRecienNacido || string.IsNullOrWhiteSpace(titular.Cedula)) 
            {
                return BadRequest(new { message = "El paciente seleccionado no posee un documento de identidad válido para ser representante legal." });
            }
        }

        // 3. Mapeo del DTO a la Entidad (Solo datos propios de la tabla)
        var paciente = new Paciente
        {
            Id = Guid.NewGuid(),
            Cedula = dto.Cedula,
            EsRecienNacido = dto.EsRecienNacido,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            FechaNacimiento = DateTime.SpecifyKind(dto.FechaNacimiento, DateTimeKind.Utc),
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
    public async Task<IActionResult> UpdatePaciente(Guid id, [FromBody] PacienteDto dto) // AHORA USA DTO
    {
        if (id != dto.Id)
        {
            return BadRequest(new { message = "El ID de la URL no coincide con el ID del objeto." });
        }

        // 1. VALIDACIÓN: Cédula única (IGNORANDO AL PACIENTE ACTUAL PARA PERMITIR EDICIÓN)
        if (!string.IsNullOrWhiteSpace(dto.Cedula))
        {
            bool existe = await _pacienteRepository.ExisteCedulaAsync(dto.Cedula, id); // <-- CORREGIDO: Se pasa el ID
            if (existe) 
            {
                return BadRequest(new { message = "La cédula ingresada ya pertenece a otro paciente registrado." });
            }
        }

        // 2. VALIDACIÓN: El representante debe existir y NO puede ser un menor sin ID
        if (dto.TitularId.HasValue)
        {
            var titular = await _pacienteRepository.GetByIdAsync(dto.TitularId.Value);
            if (titular == null)
            {
                return BadRequest(new { message = "El representante legal seleccionado no existe en el sistema." });
            }
            // --> NUEVA REGLA DE NEGOCIO <--
            if (titular.EsRecienNacido || string.IsNullOrWhiteSpace(titular.Cedula)) 
            {
                return BadRequest(new { message = "El paciente seleccionado no posee un documento de identidad válido para ser representante legal." });
            }
        }

        var pacienteExistente = await _pacienteRepository.GetByIdAsync(id);
        if (pacienteExistente == null)
        {
            return NotFound(new { message = "El paciente que intenta editar no fue encontrado." });
        }

        // 3. Mapeo del DTO a la Entidad existente
        pacienteExistente.Cedula = dto.Cedula;
        pacienteExistente.EsRecienNacido = dto.EsRecienNacido;
        pacienteExistente.Nombre = dto.Nombre;
        pacienteExistente.Apellido = dto.Apellido;
        pacienteExistente.FechaNacimiento = DateTime.SpecifyKind(dto.FechaNacimiento, DateTimeKind.Utc);
        pacienteExistente.Sexo = dto.Sexo;
        pacienteExistente.Telefono = dto.Telefono;
        pacienteExistente.TelefonoAlternativo = dto.TelefonoAlternativo;
        pacienteExistente.Correo = dto.Correo;
        pacienteExistente.CorreoAlternativo = dto.CorreoAlternativo;
        pacienteExistente.Domicilio = dto.Domicilio;
        pacienteExistente.NotaMedica = dto.NotaMedica;
        pacienteExistente.NumeroAfiliado = dto.NumeroAfiliado;
        pacienteExistente.Parentesco = dto.Parentesco;
        pacienteExistente.TitularId = dto.TitularId;

        await _pacienteRepository.UpdateAsync(pacienteExistente);

        return NoContent(); 
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