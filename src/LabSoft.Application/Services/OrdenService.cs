using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;
using LabSoft.Domain.Entities;

namespace LabSoft.Application.Services;

public class OrdenService : IOrdenService
{
    private readonly IOrdenRepository _ordenRepository;

    public OrdenService(IOrdenRepository ordenRepository)
    {
        _ordenRepository = ordenRepository;
    }

    public async Task<IEnumerable<OrdenDto>> GetAllAsync()
    {
        var ordenes = await _ordenRepository.GetAllAsync();
        return ordenes.Select(o => new OrdenDto
        {
            Id = o.Id,
            FechaCreacion = o.FechaCreacion,
            Estado = o.Estado,
            PacienteNombre = o.Paciente != null ? $"{o.Paciente.Nombre} {o.Paciente.Apellido}" : "Desconocido",
            Detalles = o.Detalles
                .OrderBy(d => d.Examen?.Codigo)
                .Select(d => new OrdenDetalleDto
            {
                Id = d.Id,
                ItemNombre = d.Examen?.Nombre ?? "Examen",
                Tipo = d.PanelId.HasValue ? "Panel" : "Individual",
                Estado = d.Estado
            }).ToList()
        });
    }

    // public async Task<object?> GetByIdAsync(Guid id)
    // {
    //     var o = await _ordenRepository.GetByIdAsync(id);
    //     if (o == null) return null;

    //     return new 
    //     {
    //         Id = o.Id,
    //         FechaCreacion = o.FechaCreacion,
    //         Estado = o.Estado,
    //         PacienteNombre = o.Paciente != null ? $"{o.Paciente.Nombre} {o.Paciente.Apellido}" : "Desconocido",
            
    //         // Lista plana original (Útil para imprimir el ticket de cobro en recepción)
    //         DetallesPlanos = o.Detalles
    //             .OrderBy(d => d.Examen?.Codigo)
    //             .Select(d => new OrdenDetalleDto
    //         {
    //             Id = d.Id,
    //             ItemNombre = d.Examen?.Nombre ?? "Examen",
    //             Tipo = d.PanelId.HasValue ? "Panel" : "Individual",
    //             Estado = d.Estado
    //         }).ToList(),

    //         // Nueva estructura jerárquica médica usando nuestro Helper
    //         Especialidades = Common.EstructuraClinicaHelper.AgruparYConstruirEstructura(o.Detalles, o.Paciente?.Sexo)
    //     };
    // }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var o = await _ordenRepository.GetByIdAsync(id);
        if (o == null) return null;

        return new 
        {
            Id = o.Id,
            // 1. SOLUCIÓN AL NÚMERO DE ORDEN (#0000 y #ORD-B450)
            NumeroOrden = o.NumeroOrden, 
            FechaCreacion = o.FechaCreacion,
            Estado = o.Estado,
            
            // 2. SOLUCIÓN AL BLOQUEO DEL PACIENTE
            PacienteId = o.PacienteId,
            PacienteNombre = o.Paciente != null ? $"{o.Paciente.Nombre} {o.Paciente.Apellido}" : "Desconocido",
            
            // 3. SOLUCIÓN AL CARRITO VACÍO Y LAS EXCLUSIONES
            ExamenIds = o.Detalles
                .Where(d => d.PanelId == null)
                .Select(d => d.ExamenId)
                .ToList(),
                
            PanelesSolicitados = o.Detalles
                .Where(d => d.PanelId.HasValue && d.Panel != null)
                .GroupBy(d => d.Panel) // Agrupamos por el objeto Panel completo
                .Select(g => new PanelSolicitudDto
                {
                    PanelId = g.Key!.Id,
                    // Magia de C#: (Todos los IDs del panel original) EXCEPTO (Los IDs que sí están en la orden)
                    ExamenesExcluidosIds = g.Key!.PanelExamenes
                        .Select(pe => pe.ExamenId)
                        .Except(g.Select(d => d.ExamenId))
                        .ToList()
                }).ToList(),

            // Mantenemos lo que ya tenías para la vista de Detalles (Impresión)
            DetallesPlanos = o.Detalles
                .OrderBy(d => d.Examen?.Codigo)
                .Select(d => new OrdenDetalleDto
            {
                Id = d.Id,
                ItemNombre = d.Examen?.Nombre ?? "Examen",
                Tipo = d.PanelId.HasValue ? "Panel" : "Individual",
                Estado = d.Estado
            }).ToList(),

            Especialidades = Common.EstructuraClinicaHelper.AgruparYConstruirEstructura(o.Detalles, o.Paciente?.Sexo)
        };
    }

    public async Task<OrdenDto> CrearOrdenAsync(CreateOrdenDto dto)
    {
        // 1. Validar que el paciente exista usando el repositorio
        var paciente = await _ordenRepository.GetPacienteByIdAsync(dto.PacienteId);
        if (paciente == null)
        {
            throw new Exception("El paciente especificado no existe.");
        }

        var nuevaOrden = new Orden
        {
            Id = Guid.NewGuid(),
            PacienteId = dto.PacienteId,
            FechaCreacion = DateTime.UtcNow,
            Estado = "Pendiente",
            Detalles = new List<OrdenDetalle>()
        };

        // 2. Procesar Exámenes Sueltos
        foreach (var examenId in dto.ExamenIds)
        {
            nuevaOrden.Detalles.Add(new OrdenDetalle
            {
                Id = Guid.NewGuid(),
                ExamenId = examenId,
                PanelId = null,
                Estado = "Pendiente"
            });
        }

        // 3. Procesar Paneles Solicitados con la lógica de exclusiones
        foreach (var panelSolicitud in dto.PanelesSolicitados)
        {
            var panel = await _ordenRepository.GetPanelConExamenesAsync(panelSolicitud.PanelId);
            if (panel == null) continue;

            // Filtramos los exámenes excluidos (los que desmarcaron)
            var examenesAProcesar = panel.PanelExamenes
                .Where(pe => !panelSolicitud.ExamenesExcluidosIds.Contains(pe.ExamenId))
                .ToList();

            foreach (var pe in examenesAProcesar)
            {
                nuevaOrden.Detalles.Add(new OrdenDetalle
                {
                    Id = Guid.NewGuid(),
                    ExamenId = pe.ExamenId,
                    PanelId = panel.Id,
                    Estado = "Pendiente"
                });
            }
        }

        // 4. Guardar en la base de datos
        await _ordenRepository.AddAsync(nuevaOrden);

        // --- REEMPLAZA EL RETURN AWAIT GETBYID... POR ESTO ---
        var o = await _ordenRepository.GetByIdAsync(nuevaOrden.Id);
        if (o == null) throw new Exception("Error al recuperar la orden recién creada.");

        return new OrdenDto
        {
            Id = o.Id,
            NumeroOrden = o.NumeroOrden,
            FechaCreacion = o.FechaCreacion,
            Estado = o.Estado,
            PacienteNombre = o.Paciente != null ? $"{o.Paciente.Nombre} {o.Paciente.Apellido}" : "Desconocido",
            Detalles = o.Detalles
                .OrderBy(d => d.Examen?.Codigo)
                .Select(d => new OrdenDetalleDto
            {
                Id = d.Id,
                ItemNombre = d.Examen?.Nombre ?? "Examen",
                Tipo = d.PanelId.HasValue ? "Panel" : "Individual",
                Estado = d.Estado
            }).ToList()
        };
    }

    public async Task<bool> ActualizarOrdenAsync(Guid id, UpdateOrdenDto dto)
    {
        var orden = await _ordenRepository.GetByIdAsync(id);
        if (orden == null) return false;

        // Regla de negocio: Si ya no está pendiente, no se puede modificar
        if (!orden.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("No se puede modificar una orden que ya ha sido procesada o entregada.");
        }

        // Actualizamos el estado general si viene especificado
        if (!string.IsNullOrWhiteSpace(dto.Estado))
        {
            orden.Estado = dto.Estado;
        }

        // --- EL CAMBIO CLAVE: Creamos una lista NUEVA en lugar de limpiar la vieja ---
        var nuevosDetalles = new List<OrdenDetalle>();

        // 1. Procesar Exámenes Sueltos actualizados
        foreach (var examenId in dto.ExamenIds)
        {
            nuevosDetalles.Add(new OrdenDetalle
            {
                Id = Guid.NewGuid(),
                OrdenId = orden.Id,
                ExamenId = examenId,
                PanelId = null,
                Estado = "Pendiente"
            });
        }

        // 2. Procesar Paneles Solicitados actualizados (con exclusiones)
        foreach (var panelSolicitud in dto.PanelesSolicitados)
        {
            var panel = await _ordenRepository.GetPanelConExamenesAsync(panelSolicitud.PanelId);
            if (panel == null) continue;

            var examenesAProcesar = panel.PanelExamenes
                .Where(pe => !panelSolicitud.ExamenesExcluidosIds.Contains(pe.ExamenId))
                .ToList();

            foreach (var pe in examenesAProcesar)
            {
                nuevosDetalles.Add(new OrdenDetalle
                {
                    Id = Guid.NewGuid(),
                    OrdenId = orden.Id,
                    ExamenId = pe.ExamenId,
                    PanelId = panel.Id,
                    Estado = "Pendiente"
                });
            }
        }

        // Le pasamos la orden y la nueva lista al repositorio para que haga la transacción segura
        await _ordenRepository.UpdateConDetallesAsync(orden, nuevosDetalles);
        return true;
    }

    public async Task<PagedResultDto<OrdenDto>> GetOrdenesPaginadasAsync(OrdenQueryFilter filter)
    {
        var (ordenes, totalRecords) = await _ordenRepository.GetOrdenesPaginadasAsync(filter);

        var items = ordenes.Select(o => new OrdenDto
        {
            Id = o.Id,
            NumeroOrden = o.NumeroOrden, // Mapeamos el nuevo campo
            FechaCreacion = o.FechaCreacion,
            Estado = o.Estado,
            PacienteNombre = o.Paciente != null ? $"{o.Paciente.Nombre} {o.Paciente.Apellido}" : "Desconocido",
            Detalles = o.Detalles
                .OrderBy(d => d.Examen?.Codigo)
                .Select(d => new OrdenDetalleDto
            {
                Id = d.Id,
                ItemNombre = d.Examen?.Nombre ?? "Examen",
                Tipo = d.PanelId.HasValue ? "Panel" : "Individual",
                Estado = d.Estado
            }).ToList()
        }).ToList();

        return new PagedResultDto<OrdenDto>
        {
            Items = items,
            TotalRecords = totalRecords,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

}