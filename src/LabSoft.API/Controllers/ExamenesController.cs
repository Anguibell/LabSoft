using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;
using LabSoft.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LabSoft.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExamenesController : ControllerBase
{
    private readonly IExamenRepository _examenRepository;

    public ExamenesController(IExamenRepository examenRepository)
    {
        _examenRepository = examenRepository;
    }

    // GET: api/examenes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll()
    {
        var examenes = await _examenRepository.GetAllAsync();
        return Ok(AgruparPorEspecialidad(examenes));
    }

    // GET: api/examenes/especialidad/3 o api/examenes/especialidad/Hematologia
    [HttpGet("especialidad/{busqueda}")]
    public async Task<ActionResult<IEnumerable<object>>> GetByEspecialidad(string busqueda)
    {
        // El repositorio se encarga de buscar tanto por nombre como por código o ID
        var examenes = await _examenRepository.GetByEspecialidadAsync(busqueda);
        
        if (examenes == null || !examenes.Any())
        {
            return NotFound(new { success = false, message = "No se encontraron exámenes para esa especialidad." });
        }

        return Ok(AgruparPorEspecialidad(examenes));
    }

    // GET: api/examenes/codigo/101
    [HttpGet("codigo/{codigo:int}")]
    public async Task<ActionResult<ExamenDto>> GetByCodigo(int codigo)
    {
        var examen = await _examenRepository.GetByCodigoAsync(codigo);
        if (examen == null)
        {
            return NotFound(new { success = false, message = $"No se encontró el examen con el código {codigo}." });
        }
        
        return Ok(MapToDto(examen));
    }

    // GET: api/examenes/3fa85f64-5717-4562-b3fc-2c963f66afa6
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExamenDto>> GetById(Guid id)
    {
        var examen = await _examenRepository.GetByIdAsync(id);
        if (examen == null)
        {
            return NotFound(new { success = false, message = "Examen no encontrado." });
        }

        return Ok(MapToDto(examen));
    }

    // [HttpPost]
    // public async Task<ActionResult<ExamenDto>> Create([FromBody] CreateExamenDto createDto)
    // {
    //     // 1. Validar que el nombre no exista previamente
    //     if (await _examenRepository.ExistsNombreAsync(createDto.Nombre, Guid.Empty))
    //     {
    //         return BadRequest(new { success = false, message = $"Ya existe otro examen registrado con el nombre '{createDto.Nombre}'." });
    //     }

    //     // 2. Validar que la especialidad sea enviada
    //     if (createDto.EspecialidadId == Guid.Empty)
    //     {
    //         return BadRequest(new { success = false, message = "Debe indicar la especialidad a la cual pertenecerá el examen." });
    //     }

    //     // 3. Generar el código automático correlativo para esa especialidad
    //     int nuevoCodigo = await _examenRepository.GetNextCodigoByEspecialidadIdAsync(createDto.EspecialidadId);

    //     // 4. Determinar el Orden: Si no viene en el DTO, se calcula automáticamente
    //     int nuevoOrden = createDto.Orden.HasValue && createDto.Orden.Value > 0
    //         ? createDto.Orden.Value
    //         : await _examenRepository.GetNextOrdenAsync(createDto.EspecialidadId, createDto.ExamenPadreId);

    //     var examen = new Examen
    //     {
    //         Id = Guid.NewGuid(),
    //         Codigo = nuevoCodigo,
    //         Orden = nuevoOrden,
    //         Nombre = createDto.Nombre,
    //         Precio = createDto.Precio,
    //         Unidad = createDto.Unidad,
    //         ValoresReferencia = createDto.ValoresReferencia,
    //         Activo = true,
    //         EsOpcionalDinamico = createDto.EsOpcionalDinamico,
    //         EspecialidadId = createDto.EspecialidadId,
    //         ExamenPadreId = createDto.ExamenPadreId
    //     };

    //     await _examenRepository.AddAsync(examen);

    //     var createdExamen = await _examenRepository.GetByIdAsync(examen.Id);
    //     return CreatedAtAction(nameof(GetById), new { id = examen.Id }, MapToDto(createdExamen ?? examen));
    // }

    [HttpPost]
    public async Task<ActionResult<ExamenDto>> Create([FromBody] CreateExamenDto createDto)
    {
        // 1. Validar que el nombre no exista previamente
        if (await _examenRepository.ExistsNombreAsync(createDto.Nombre, Guid.Empty))
        {
            return BadRequest(new { success = false, message = $"Ya existe otro examen registrado con el nombre '{createDto.Nombre}'." });
        }

        // 2. Validar que la especialidad sea enviada
        if (createDto.EspecialidadId == Guid.Empty)
        {
            return BadRequest(new { success = false, message = "Debe indicar la especialidad a la cual pertenecerá el examen." });
        }

        // 3. Generar el código automático correlativo para esa especialidad
        int nuevoCodigo = await _examenRepository.GetNextCodigoByEspecialidadIdAsync(createDto.EspecialidadId);

        // 4. Determinar el Orden
        int nuevoOrden = createDto.Orden.HasValue && createDto.Orden.Value > 0
            ? createDto.Orden.Value
            : await _examenRepository.GetNextOrdenAsync(createDto.EspecialidadId, createDto.ExamenPadreId);

        var examen = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = nuevoCodigo,
            Orden = nuevoOrden,
            Nombre = createDto.Nombre,
            Precio = createDto.Precio,
            Unidad = createDto.Unidad,
            ValoresReferencia = createDto.ValoresReferencia,
            ValoresReferenciaM = createDto.ValoresReferenciaM,
            ValoresReferenciaF = createDto.ValoresReferenciaF,
            Activo = true,
            EsOpcionalDinamico = createDto.EsOpcionalDinamico,
            DecimalesRequeridos = createDto.DecimalesRequeridos,
            EspecialidadId = createDto.EspecialidadId,
            ExamenPadreId = createDto.ExamenPadreId,
            // Aquí enlazamos el árbol completo antes de guardarlo en base de datos
            SubExamenes = MapearSubExamenes(createDto.SubExamenes, createDto.EspecialidadId)
        };

        await _examenRepository.AddAsync(examen); // EF Core inserta padre e hijos en 1 sola transacción

        var createdExamen = await _examenRepository.GetByIdAsync(examen.Id);
        return CreatedAtAction(nameof(GetById), new { id = examen.Id }, MapToDto(createdExamen ?? examen));
    }

    // Método auxiliar recursivo para mapear toda la jerarquía de hijos
    private List<Examen> MapearSubExamenes(List<CreateSubExamenDto>? subExamenesDto, Guid especialidadId)
    {
        if (subExamenesDto == null || !subExamenesDto.Any())
            return new List<Examen>();

        var lista = new List<Examen>();
        int ordenSecuencial = 1;

        foreach (var dto in subExamenesDto)
        {
            var subExamen = new Examen
            {
                Id = Guid.NewGuid(),
                Codigo = 0, // Los hijos (parámetros) llevan código 0
                Orden = dto.Orden ?? ordenSecuencial++,
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Unidad = dto.Unidad,
                ValoresReferencia = dto.ValoresReferencia,
                ValoresReferenciaM = dto.ValoresReferenciaM,
                ValoresReferenciaF = dto.ValoresReferenciaF,
                Activo = true,
                EsOpcionalDinamico = dto.EsOpcionalDinamico,
                DecimalesRequeridos = dto.DecimalesRequeridos,
                EspecialidadId = especialidadId, // Heredan la especialidad de su padre obligatoriamente
                SubExamenes = MapearSubExamenes(dto.SubExamenes, especialidadId) // Recursividad por si el hijo tiene hijos
            };
            lista.Add(subExamen);
        }
        return lista;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExamenDto updateDto)
    {
        // IMPORTANTE: Asegúrate de que tu repositorio cargue los SubExamenes (Ej: .Include(e => e.SubExamenes))
        var examen = await _examenRepository.GetByIdAsync(id);
        if (examen == null)
        {
            return NotFound(new { success = false, message = "Examen no encontrado." });
        }

        if (await _examenRepository.ExistsCodigoAsync(updateDto.Codigo, examen.Id))
            return BadRequest(new { success = false, message = $"El código {updateDto.Codigo} ya está asignado a otro examen." });

        if (await _examenRepository.ExistsNombreAsync(updateDto.Nombre, examen.Id))
            return BadRequest(new { success = false, message = $"Ya existe otro examen con el nombre '{updateDto.Nombre}'." });

        // 1. Actualizar Propiedades del Padre
        examen.Nombre = updateDto.Nombre;
        examen.Precio = updateDto.Precio;
        examen.Unidad = updateDto.Unidad;
        examen.ValoresReferencia = updateDto.ValoresReferencia;
        examen.ValoresReferenciaM = updateDto.ValoresReferenciaM;
        examen.ValoresReferenciaF = updateDto.ValoresReferenciaF;
        examen.Activo = updateDto.Activo;
        examen.EsOpcionalDinamico = updateDto.EsOpcionalDinamico;
        examen.DecimalesRequeridos = updateDto.DecimalesRequeridos;

        // 2. Sincronizar el Árbol de Hijos
        SincronizarSubExamenes(examen, updateDto.SubExamenes, examen.EspecialidadId);

        await _examenRepository.UpdateAsync(examen);

        return NoContent();
    }

    // Método recursivo para comparar y actualizar el árbol
    private void SincronizarSubExamenes(Examen padreDb, List<UpdateSubExamenDto>? subExamenesDto, Guid especialidadId)
    {
        if (padreDb.SubExamenes == null) padreDb.SubExamenes = new List<Examen>();
        subExamenesDto ??= new List<UpdateSubExamenDto>();

        // A. Identificar cuáles hijos eliminar (Los que están en DB pero ya no vienen en el DTO)
        var idsEnDto = subExamenesDto.Where(d => d.Id.HasValue).Select(d => d.Id!.Value).ToList();
        var hijosAEliminar = padreDb.SubExamenes.Where(h => !idsEnDto.Contains(h.Id)).ToList();

        foreach (var hijo in hijosAEliminar)
        {
            padreDb.SubExamenes.Remove(hijo);
        }

        // B. Actualizar existentes o Agregar nuevos
        int ordenSecuencial = 1;
        foreach (var dto in subExamenesDto)
        {
            if (dto.Id.HasValue && dto.Id.Value != Guid.Empty)
            {
                // UPDATE: Ya existe, actualizamos sus datos
                var hijoExistente = padreDb.SubExamenes.FirstOrDefault(h => h.Id == dto.Id!.Value);
                if (hijoExistente != null)
                {
                    hijoExistente.Nombre = dto.Nombre;
                    hijoExistente.Precio = dto.Precio;
                    hijoExistente.Unidad = dto.Unidad;
                    hijoExistente.ValoresReferencia = dto.ValoresReferencia;
                    hijoExistente.ValoresReferenciaM = dto.ValoresReferenciaM;
                    hijoExistente.ValoresReferenciaF = dto.ValoresReferenciaF;
                    hijoExistente.EsOpcionalDinamico = dto.EsOpcionalDinamico;
                    hijoExistente.DecimalesRequeridos = dto.DecimalesRequeridos;
                    hijoExistente.Orden = dto.Orden ?? ordenSecuencial++;

                    // Llamada recursiva por si este hijo tiene a su vez otros hijos (nietos)
                    SincronizarSubExamenes(hijoExistente, dto.SubExamenes, especialidadId);
                }
            }
            else
            {
                // INSERT: Es un parámetro completamente nuevo
                var nuevoHijo = new Examen
                {
                    Id = Guid.NewGuid(),
                    Codigo = 0,
                    Orden = dto.Orden ?? ordenSecuencial++,
                    Nombre = dto.Nombre,
                    Precio = dto.Precio,
                    Unidad = dto.Unidad,
                    ValoresReferencia = dto.ValoresReferencia,
                    ValoresReferenciaM = dto.ValoresReferenciaM,
                    ValoresReferenciaF = dto.ValoresReferenciaF,
                    Activo = true,
                    EsOpcionalDinamico = dto.EsOpcionalDinamico,
                    DecimalesRequeridos = dto.DecimalesRequeridos,
                    EspecialidadId = especialidadId,
                    ExamenPadreId = padreDb.Id
                };
                
                // Mapear recursivamente si el nuevo hijo trae sus propios hijos
                // (Para esto podemos reutilizar tu método MapearSubExamenes que creamos para el POST)
                padreDb.SubExamenes.Add(nuevoHijo);
            }
        }
    }

    // [HttpPut("{id:guid}")]
    // public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExamenDto updateDto)
    // {
    //     var examen = await _examenRepository.GetByIdAsync(id);
    //     if (examen == null)
    //     {
    //         return NotFound(new { success = false, message = "Examen no encontrado." });
    //     }

    //     // 1. Validar si el nuevo código pertenece a OTRO examen
    //     if (await _examenRepository.ExistsCodigoAsync(updateDto.Codigo, examen.Id))
    //     {
    //         return BadRequest(new { success = false, message = $"El código {updateDto.Codigo} ya está asignado a otro examen." });
    //     }

    //     // 2. Validar si el nuevo nombre pertenece a OTRO examen
    //     if (await _examenRepository.ExistsNombreAsync(updateDto.Nombre, examen.Id))
    //     {
    //         return BadRequest(new { success = false, message = $"Ya existe otro examen registrado con el nombre '{updateDto.Nombre}'." });
    //     }

    //     // 3. Asignar nuevos valores
    //     examen.Nombre = updateDto.Nombre;
    //     examen.Precio = updateDto.Precio;
    //     examen.Unidad = updateDto.Unidad;
    //     examen.ValoresReferencia = updateDto.ValoresReferencia;
    //     examen.Activo = updateDto.Activo;
    //     examen.EsOpcionalDinamico = updateDto.EsOpcionalDinamico;

    //     await _examenRepository.UpdateAsync(examen);

    //     return NoContent();
    // }

    // PUT: api/examenes/codigo/304
    [HttpPut("codigo/{codigo:int}")]
    public async Task<IActionResult> UpdateByCodigo(int codigo, [FromBody] UpdateExamenDto updateDto)
    {
        var examen = await _examenRepository.GetByCodigoAsync(codigo);
        if (examen == null)
        {
            return NotFound(new { success = false, message = $"No se encontró el examen con el código {codigo}." });
        }

        // 1. Validar si el nuevo código ya lo tiene OTRO examen
        if (await _examenRepository.ExistsCodigoAsync(updateDto.Codigo, examen.Id))
        {
            return BadRequest(new { success = false, message = $"El código {updateDto.Codigo} ya está asignado a otro examen." });
        }

        // 2. Validar si el nuevo nombre ya lo tiene OTRO examen
        if (await _examenRepository.ExistsNombreAsync(updateDto.Nombre, examen.Id))
        {
            return BadRequest(new { success = false, message = $"Ya existe otro examen registrado con el nombre '{updateDto.Nombre}'." });
        }

        // 3. Asignar los nuevos valores
        examen.Nombre = updateDto.Nombre;
        examen.Precio = updateDto.Precio;
        examen.Unidad = updateDto.Unidad;
        examen.ValoresReferencia = updateDto.ValoresReferencia;
        examen.Activo = updateDto.Activo;
        examen.EsOpcionalDinamico = updateDto.EsOpcionalDinamico;

        await _examenRepository.UpdateAsync(examen);

        return NoContent();
    }

    private ExamenDto MapToDto(Examen examen)
    {
        return new ExamenDto
        {
            Id = examen.Id,
            Codigo = examen.Codigo,
            Orden = examen.Orden,
            Nombre = examen.Nombre,
            Precio = examen.Precio,
            Unidad = examen.Unidad,
            ValoresReferencia = examen.ValoresReferencia,
            Activo = examen.Activo,
            EsOpcionalDinamico = examen.EsOpcionalDinamico,
            EspecialidadId = examen.EspecialidadId,
            EspecialidadNombre = examen.Especialidad?.Nombre ?? string.Empty,
            ExamenPadreId = examen.ExamenPadreId,
            SubExamenes = examen.SubExamenes?
                .OrderBy(sub => sub.Orden)
                .Select(sub => MapToDto(sub))
                .ToList() ?? new List<ExamenDto>()
        };
    }

    private IEnumerable<object> AgruparPorEspecialidad(IEnumerable<Examen> examenes)
    {
        // El repositorio ya los trae ordenados, solo necesitamos agruparlos visualmente
        return examenes
            .GroupBy(e => e.Especialidad?.Nombre ?? "OTRAS PRUEBAS")
            .Select(grupo => new
            {
                NombreEspecialidad = grupo.Key,
                Examenes = grupo.Select(e => MapToDto(e)).ToList()
            });
    }
}