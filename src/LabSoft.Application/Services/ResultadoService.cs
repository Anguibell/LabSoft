using LabSoft.Application.Common;
using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;
using LabSoft.Domain.Entities;

namespace LabSoft.Application.Services;

public class ResultadoService : IResultadoService
{
    private readonly IResultadoRepository _resultadoRepository;
    private readonly ICurrentUserService _currentUserService;

    public ResultadoService(IResultadoRepository resultadoRepository, ICurrentUserService currentUserService)
    {
        _resultadoRepository = resultadoRepository;
        _currentUserService = currentUserService;
    }

    private dynamic MapearSubExamenesRecursivo(Examen examen, IEnumerable<Resultado> resultadosOrden)
    {
        var hijos = new List<dynamic>();

        if (examen.SubExamenes != null && examen.SubExamenes.Any())
        {
            foreach (var sub in examen.SubExamenes.OrderBy(s => s.Orden))
            {
                hijos.Add(MapearSubExamenesRecursivo(sub, resultadosOrden));
            }
        }

        // Buscamos si este nodo en específico tiene un resultado directo
        var resultado = resultadosOrden.FirstOrDefault(r => r.ExamenId == examen.Id);
        string estado = resultado?.Estado ?? "Pendiente";
        DateTime? fecha = resultado?.FechaProcesamiento;

        // --- INFERENCIA BOTTOM-UP ---
        if (hijos.Any())
        {
            // Si TODOS los hijos están procesados/validados, el padre lo está
            bool todosProcesados = hijos.All(h => h.Estado == "Procesado" || h.Estado == "Validado");
            estado = todosProcesados ? "Procesado" : "Pendiente";
            
            // La fecha del padre es la fecha máxima de sus hijos
            var fechasHijos = hijos.Where(h => h.FechaProcesamiento != null)
                                .Select(h => (DateTime)h.FechaProcesamiento)
                                .ToList();
            if (fechasHijos.Any())
            {
                fecha = fechasHijos.Max();
            }
        }

        return new
        {
            ExamenId = examen.Id,
            Nombre = examen.Nombre,
            Unidad = examen.Unidad ?? "",
            ValoresReferencia = examen.ValoresReferencia ?? "",
            Estado = estado,
            FechaProcesamiento = fecha,
            SubExamenes = hijos
        };
    }

    public async Task<IEnumerable<object>> GetOrdenesPendientesParaProcesarAsync()
    {
        var ordenes = await _resultadoRepository.GetOrdenesPendientesAsync();

        return ordenes.Select(o => new
        {
            OrdenId = o.Id,
            FechaCreacion = o.FechaCreacion,
            EstadoOrden = o.Estado,
            Paciente = o.Paciente != null ? $"{o.Paciente.Nombre} {o.Paciente.Apellido}" : "Desconocido",
            Cedula = o.Paciente?.Cedula,
            
            // MAGIA: Una sola llamada al helper hace todo el trabajo
            Especialidades = EstructuraClinicaHelper.AgruparYConstruirEstructura(o.Detalles, o.Paciente?.Sexo)
        });
    }

    public async Task<bool> RegistrarResultadoAsync(CrearResultadoDto dto)
    {
        var userId = _currentUserService.GetUserId();
        var detalle = await _resultadoRepository.GetDetalleByIdAsync(dto.OrdenDetalleId);
        if (detalle == null)
            throw new Exception("El detalle de la orden especificado no existe.");

        foreach (var item in dto.ResultadosParametros)
        {
            string valorNormalizado = FormattingHelper.NormalizarValorResultado(item.ValorResultado);
            string? notaInternaNormalizada = !string.IsNullOrEmpty(item.NotaInterna) ? FormattingHelper.NormalizarTexto(item.NotaInterna) : null;
            string? notaClinicaNormalizada = !string.IsNullOrEmpty(item.NotaClinica) ? FormattingHelper.NormalizarTexto(item.NotaClinica) : null;

            var resultadoExistente = detalle.Resultados?.FirstOrDefault(r => r.ExamenId == item.ExamenId);

            if (resultadoExistente != null)
            {
                resultadoExistente.ValorResultado = valorNormalizado;
                resultadoExistente.NotaInterna = notaInternaNormalizada;
                resultadoExistente.NotaClinica = notaClinicaNormalizada;
                resultadoExistente.ImprimirObservacion = item.ImprimirObservacion;
                resultadoExistente.FechaProcesamiento = DateTime.UtcNow;

                // --- NUEVAS LÍNEAS DE AUDITORÍA ---
                resultadoExistente.ProcesadoPorId = userId; 
                resultadoExistente.EsModificado = true;

                await _resultadoRepository.UpdateResultadoAsync(resultadoExistente);
            }
            else
            {
                var nuevoResultado = new Resultado
                {
                    Id = Guid.NewGuid(),
                    OrdenDetalleId = detalle.Id,
                    ExamenId = item.ExamenId,
                    ValorResultado = valorNormalizado,
                    NotaInterna = notaInternaNormalizada,
                    NotaClinica = notaClinicaNormalizada,
                    ImprimirObservacion = item.ImprimirObservacion,
                    FechaProcesamiento = DateTime.UtcNow,
                    Estado = "Procesado",

                    // --- NUEVAS LÍNEAS DE AUDITORÍA ---
                    ProcesadoPorId = userId,
                    EsModificado = false
                };
                await _resultadoRepository.AddResultadoAsync(nuevoResultado);
            }
        }

        detalle.Estado = "Procesado";
        await _resultadoRepository.UpdateDetalleAsync(detalle);

        var orden = detalle.Orden;
        if (orden != null && orden.Detalles.All(d => d.Estado.Equals("Procesado", StringComparison.OrdinalIgnoreCase)))
        {
            orden.Estado = "Procesado";
            await _resultadoRepository.UpdateOrdenEstadoAsync(orden);
        }

        return true;
    }

    public async Task RegistrarResultadosMasivosAsync(RegistrarResultadosMasivosDto dto)
    {
        var userId = _currentUserService.GetUserId();

        foreach (var detalleDto in dto.DetallesResultados)
        {
            var detalleExistente = await _resultadoRepository.GetDetalleByIdAsync(detalleDto.OrdenDetalleId);
            if (detalleExistente == null)
                throw new KeyNotFoundException($"El detalle con ID {detalleDto.OrdenDetalleId} no existe.");

            foreach (var paramDto in detalleDto.ResultadosParametros)
            {
                var valorLimpio = FormattingHelper.NormalizarValorResultado(paramDto.ValorResultado);
                var notaInternaLimpia = !string.IsNullOrEmpty(paramDto.NotaInterna) ? FormattingHelper.NormalizarTexto(paramDto.NotaInterna) : null;
                var notaClinicaLimpia = !string.IsNullOrEmpty(paramDto.NotaClinica) ? FormattingHelper.NormalizarTexto(paramDto.NotaClinica) : null;

                var resultadoExistente = detalleExistente.Resultados?.FirstOrDefault(r => r.ExamenId == paramDto.ExamenId);

                if (resultadoExistente != null)
                {
                    resultadoExistente.ValorResultado = valorLimpio;
                    resultadoExistente.NotaInterna = notaInternaLimpia;
                    resultadoExistente.NotaClinica = notaClinicaLimpia;
                    resultadoExistente.ImprimirObservacion = paramDto.ImprimirObservacion;
                    resultadoExistente.FechaProcesamiento = DateTime.UtcNow;
                    resultadoExistente.Estado = "Procesado";

                    // --- NUEVAS LÍNEAS DE AUDITORÍA ---
                    resultadoExistente.ProcesadoPorId = userId; 
                    resultadoExistente.EsModificado = true;

                    await _resultadoRepository.UpdateResultadoAsync(resultadoExistente);
                }
                else
                {
                    var nuevoResultado = new Resultado
                    {
                        Id = Guid.NewGuid(),
                        OrdenDetalleId = detalleDto.OrdenDetalleId,
                        ExamenId = paramDto.ExamenId,
                        ValorResultado = valorLimpio,
                        NotaInterna = notaInternaLimpia,
                        NotaClinica = notaClinicaLimpia,
                        ImprimirObservacion = paramDto.ImprimirObservacion,
                        FechaProcesamiento = DateTime.UtcNow,
                        Estado = "Procesado",

                        // --- NUEVAS LÍNEAS DE AUDITORÍA ---
                        ProcesadoPorId = userId,
                        EsModificado = false
                    };
                    await _resultadoRepository.AddResultadoAsync(nuevoResultado);
                }
            }

            detalleExistente.Estado = "Procesado";
            await _resultadoRepository.UpdateDetalleAsync(detalleExistente);
        }
    }

    public async Task<IEnumerable<object>> GetPendientesPorCedulaAsync(string cedula)
    {
        var ordenes = await _resultadoRepository.GetOrdenesPendientesAsync();
        var ordenesPaciente = ordenes.Where(o => o.Paciente != null && o.Paciente.Cedula != null && o.Paciente.Cedula.Trim() == cedula.Trim());

        return ordenesPaciente.Select(o => new
        {
            OrdenId = o.Id,
            FechaCreacion = o.FechaCreacion,
            EstadoOrden = o.Estado,
            Paciente = o.Paciente != null ? $"{o.Paciente.Nombre} {o.Paciente.Apellido}" : "Desconocido",
            Cedula = o.Paciente?.Cedula,
            Especialidades = EstructuraClinicaHelper.AgruparYConstruirEstructura(o.Detalles, o.Paciente?.Sexo)
        });
    }

    public async Task<object?> GetResultadosPorOrdenIdAsync(Guid ordenId)
    {
        var orden = await _resultadoRepository.GetOrdenConResultadosAsync(ordenId);
        if (orden == null) return null;

        return new
        {
            OrdenId = orden.Id,
            FechaCreacion = orden.FechaCreacion,
            EstadoOrden = orden.Estado,
            Paciente = orden.Paciente != null ? $"{orden.Paciente.Nombre} {orden.Paciente.Apellido}" : "Desconocido",
            Cedula = orden.Paciente?.Cedula,
            Especialidades = EstructuraClinicaHelper.AgruparYConstruirEstructura(orden.Detalles, orden.Paciente?.Sexo)
        };
    }

    public async Task<IEnumerable<object>> GetResultadosPorCedulaAsync(string cedula)
    {
        var ordenes = await _resultadoRepository.GetOrdenesPorCedulaAsync(cedula);

        return ordenes.Select(orden => new
        {
            OrdenId = orden.Id,
            FechaCreacion = orden.FechaCreacion,
            EstadoOrden = orden.Estado,
            Paciente = orden.Paciente != null ? $"{orden.Paciente.Nombre} {orden.Paciente.Apellido}" : "Desconocido",
            Cedula = orden.Paciente?.Cedula,
            Especialidades = EstructuraClinicaHelper.AgruparYConstruirEstructura(orden.Detalles, orden.Paciente?.Sexo)
        });
    }

    public async Task<bool> ValidarOrdenCompletaAsync(Guid ordenId)
    {
        var userId = _currentUserService.GetUserId();
        var orden = await _resultadoRepository.GetOrdenConResultadosAsync(ordenId);
        if (orden == null)
            throw new Exception("La orden especificada no existe.");

        var detallesAProcesar = orden.Detalles.Where(d => d.Estado.Equals("Procesado", StringComparison.OrdinalIgnoreCase)).ToList();

        if (!detallesAProcesar.Any())
            throw new Exception("La orden no tiene detalles en estado 'Procesado' listos para validar.");

        foreach (var detalle in detallesAProcesar)
        {
            detalle.Estado = "Validado";
            await _resultadoRepository.UpdateDetalleAsync(detalle);

            // --- NUEVO BLOQUE: Sellar todos los resultados hijos con la firma del validador ---
            if (detalle.Resultados != null)
            {
                foreach (var resultado in detalle.Resultados)
                {
                    resultado.Estado = "Validado";
                    resultado.ValidadoPorId = userId; // Firma de quien validó
                    await _resultadoRepository.UpdateResultadoAsync(resultado);
                }
            }
        }

        if (orden.Detalles.All(d => d.Estado.Equals("Validado", StringComparison.OrdinalIgnoreCase)))
        {
            orden.Estado = "Validado";
            await _resultadoRepository.UpdateOrdenEstadoAsync(orden);
        }

        return true;
    }

    public async Task<IEnumerable<object>> GetHistoricoOrdenesAsync(string? cedula, DateTime? fechaInicio, DateTime? fechaFin)
    {
        var ordenes = await _resultadoRepository.GetHistoricoOrdenesAsync(cedula, fechaInicio, fechaFin);

        return ordenes.Select(orden => new
        {
            OrdenId = orden.Id,
            FechaCreacion = orden.FechaCreacion,
            EstadoOrden = orden.Estado,
            Paciente = orden.Paciente != null ? $"{orden.Paciente.Nombre} {orden.Paciente.Apellido}" : "Desconocido",
            Cedula = orden.Paciente?.Cedula,
            Especialidades = EstructuraClinicaHelper.AgruparYConstruirEstructura(orden.Detalles, orden.Paciente?.Sexo)
        });
    }

    public async Task<IEnumerable<HistorialPacienteDto>> GetHistorialPorPacienteIdAsync(Guid pacienteId)
    {
        var ordenes = await _resultadoRepository.GetHistorialPorPacienteIdAsync(pacienteId);
        var historial = new List<HistorialPacienteDto>();

        foreach (var orden in ordenes)
        {
            var dto = new HistorialPacienteDto
            {
                OrdenId = orden.Id,
                NumeroOrden = orden.NumeroOrden.ToString("D6"),
                FechaCreacion = orden.FechaCreacion,
                Especialidades = new List<HistorialEspecialidadDto>()
            };

            // 1. Filtramos los detalles válidos y los ordenamos por Código[cite: 18]
            var detallesValidos = orden.Detalles
                .Where(d => d.Examen != null)
                .OrderBy(d => d.Examen!.Codigo);

            // 2. Agrupamos los exámenes por el nombre de su Especialidad. 
            // Si por algún error un examen no tiene especialidad asignada, lo mandamos a "OTRAS PRUEBAS"
            var detallesAgrupados = detallesValidos
                .GroupBy(d => d.Examen!.Especialidad?.Nombre ?? "OTRAS PRUEBAS");

            // 3. Iteramos sobre cada grupo (Ej: "HEMATOLOGIA", "QUIMICA")
            foreach (var grupo in detallesAgrupados)
            {
                var especialidadDto = new HistorialEspecialidadDto
                {
                    NombreEspecialidad = grupo.Key,
                    Examenes = new List<HistorialItemDto>()
                };

                // Procesamos cada examen dentro de esta especialidad
                foreach (var detalle in grupo)
                {
                    var nodoRaiz = ConstruirArbolResultados(detalle.Examen!, detalle.Resultados.ToList(), orden.Paciente?.Sexo);
                    
                    if (nodoRaiz != null)
                    {
                        especialidadDto.Examenes.Add(nodoRaiz);
                    }
                }

                // Solo agregamos la especialidad si realmente tuvo algún examen procesado adentro
                if (especialidadDto.Examenes.Any())
                {
                    dto.Especialidades.Add(especialidadDto);
                }
            }

            // Solo agregamos la orden si tiene alguna especialidad procesada
            if (dto.Especialidades.Any())
            {
                historial.Add(dto);
            }
        }

        return historial;
    }

    private HistorialItemDto? ConstruirArbolResultados(Examen examen, List<Resultado> resultadosOrden, SexoEnum? sexoPaciente)
    {
        var resultado = resultadosOrden.FirstOrDefault(r => r.ExamenId == examen.Id);
        
        // LÓGICA DE VALORES DE REFERENCIA
        string refValue = examen.ValoresReferencia ?? "";
        if (sexoPaciente == SexoEnum.Masculino && !string.IsNullOrWhiteSpace(examen.ValoresReferenciaM))
        {
            refValue = examen.ValoresReferenciaM;
        }
        else if (sexoPaciente == SexoEnum.Femenino && !string.IsNullOrWhiteSpace(examen.ValoresReferenciaF))
        {
            refValue = examen.ValoresReferenciaF;
        }

        var nodo = new HistorialItemDto
        {
            ExamenNombre = examen.Nombre,
            Valor = resultado?.ValorResultado ?? "",
            Unidad = examen.Unidad ?? "",
            ValoresReferencia = refValue, 
            NotaClinica = (resultado != null && resultado.ImprimirObservacion && !string.IsNullOrWhiteSpace(resultado.NotaClinica)) 
                          ? resultado.NotaClinica 
                          : null,
            Estado = resultado?.Estado ?? "",
            FechaProcesamiento = resultado?.FechaProcesamiento
        };

        // Procesamos los hijos primero (Bottom-Up)
        if (examen.SubExamenes != null && examen.SubExamenes.Any())
        {
            foreach (var subExamen in examen.SubExamenes.OrderBy(s => s.Orden))
            {
                var subNodo = ConstruirArbolResultados(subExamen, resultadosOrden, sexoPaciente);
                if (subNodo != null)
                {
                    nodo.SubExamenes.Add(subNodo);
                }
            }

            // --- NUEVA LÓGICA: INFERENCIA PARA NODOS PADRE ---
            // Si el nodo logró recolectar hijos con datos, hereda sus propiedades
            if (nodo.SubExamenes.Any())
            {
                // Si el padre no tiene estado propio, revisamos a los hijos
                if (string.IsNullOrEmpty(nodo.Estado))
                {
                    // Si TODOS los hijos visibles están procesados (o validados), el padre también lo está
                    bool todosProcesados = nodo.SubExamenes.All(h => h.Estado == "Procesado" || h.Estado == "Validado");
                    nodo.Estado = todosProcesados ? "Procesado" : "Pendiente";
                }

                // La fecha del padre será la fecha más alta (reciente) de cualquiera de sus hijos
                if (!nodo.FechaProcesamiento.HasValue)
                {
                    nodo.FechaProcesamiento = nodo.SubExamenes.Max(h => h.FechaProcesamiento);
                }
            }
        }

        // Si está vacío y no tiene hijos, no lo mostramos
        if (string.IsNullOrWhiteSpace(nodo.Valor) && !nodo.SubExamenes.Any())
        {
            return null;
        }

        return nodo;
    }

    public async Task<bool> ActualizarResultadoAsync(Guid id, ActualizarResultadoDto dto)
    {
        var userId = _currentUserService.GetUserId();
        var resultado = await _resultadoRepository.GetResultadoByIdAsync(id);
        if (resultado == null)
            throw new KeyNotFoundException("El resultado especificado no existe.");

        if (resultado.OrdenDetalle?.Orden?.Estado?.Equals("Validada", StringComparison.OrdinalIgnoreCase) == true)
            throw new InvalidOperationException("No se pueden modificar resultados de una orden que ya ha sido validada.");

        resultado.ValorResultado = FormattingHelper.NormalizarValorResultado(dto.ValorResultado);
        resultado.NotaInterna = !string.IsNullOrEmpty(dto.NotaInterna) ? FormattingHelper.NormalizarTexto(dto.NotaInterna) : null;
        resultado.NotaClinica = !string.IsNullOrEmpty(dto.NotaClinica) ? FormattingHelper.NormalizarTexto(dto.NotaClinica) : null;
        resultado.ImprimirObservacion = dto.ImprimirObservacion;
        resultado.FechaProcesamiento = DateTime.UtcNow;

        // --- NUEVAS LÍNEAS DE AUDITORÍA ---
        resultado.ProcesadoPorId = userId; 
        resultado.EsModificado = true; 

        await _resultadoRepository.UpdateResultadoAsync(resultado);
        
        return true;
    }

    public async Task<PagedResultDto<ResultadoGlobalDto>> GetResultadosPaginadosAsync(ResultadoQueryFilter filter)
    {
        // Llamamos al repositorio que creamos en el Paso 3
        var (resultados, totalRecords) = await _resultadoRepository.GetResultadosPaginadosAsync(filter);

        var items = resultados
            // 1. Mantenemos todos los resultados de una misma orden juntos
            .OrderByDescending(r => r.OrdenDetalle.Orden.FechaCreacion)
            
            // 2. Ordenamos por la Especialidad del Examen RAÍZ (Ej: Hematología -> Bioquímica)
            .ThenBy(r => r.OrdenDetalle.Examen?.Especialidad?.Codigo ?? int.MaxValue)
            
            // 3. Ordenamos por el Código del Examen RAÍZ (Ej: 101 Hematología, 301 Glicemia)
            .ThenBy(r => r.OrdenDetalle.Examen?.Codigo ?? int.MaxValue)
            
            // 4. LÓGICA NIVEL 1 (Hijos): Si es un examen directo (Glicemia) su orden es 0. Si es hijo directo (Hematocrito), usamos su propio orden. Si es Nieto (Neutrófilos), usamos el orden de su padre (Fórmula).
            .ThenBy(r => r.ExamenId == r.OrdenDetalle.ExamenId 
                ? 0 
                : (r.Examen?.ExamenPadreId == r.OrdenDetalle.ExamenId ? r.Examen?.Orden : (r.Examen?.ExamenPadre?.Orden ?? 0)))
            
            // 5. LÓGICA NIVEL 2 (Nietos): Si es nieto, desempatamos usando su propio orden. Si no, va primero.
            .ThenBy(r => r.ExamenId == r.OrdenDetalle.ExamenId 
                ? 0 
                : (r.Examen?.ExamenPadreId != r.OrdenDetalle.ExamenId ? r.Examen?.Orden : 0))
                
            .Select(r => new ResultadoGlobalDto
            {
                Id = r.Id,
                // Usamos la especialidad del examen Raíz
                Especialidad = r.OrdenDetalle.Examen?.Especialidad?.Nombre ?? "OTRAS PRUEBAS",
                PacienteNombre = r.OrdenDetalle?.Orden?.Paciente != null 
                    ? $"{r.OrdenDetalle.Orden.Paciente.Nombre} {r.OrdenDetalle.Orden.Paciente.Apellido}" 
                    : "Desconocido",
                PacienteCedula = r.OrdenDetalle?.Orden?.Paciente?.Cedula ?? "Sin Cédula",
                ExamenNombre = r.Examen?.Nombre ?? "Desconocido",
                ValorResultado = r.ValorResultado,
                Unidad = r.Examen?.Unidad ?? "",
                FechaProcesamiento = r.FechaProcesamiento,
                Estado = r.Estado
            }).ToList();

        // Envolvemos todo en el paginador estándar
        return new PagedResultDto<ResultadoGlobalDto>
        {
            Items = items,
            TotalRecords = totalRecords,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }
}