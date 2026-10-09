using LabSoft.Domain.Entities;

namespace LabSoft.Application.Common;

public static class EstructuraClinicaHelper
{
    public static IEnumerable<object> AgruparYConstruirEstructura(IEnumerable<OrdenDetalle> detalles, SexoEnum? sexoPaciente = null)
    {
        // 1. Filtramos y ORDENAMOS los exámenes principales por su Código
        var detallesValidos = detalles
            .Where(d => d.Examen != null)
            .OrderBy(d => d.Examen!.Codigo);

        // 2. AGRUPAMOS por el nombre de la Especialidad
        var agrupados = detallesValidos
            .GroupBy(d => d.Examen!.Especialidad?.Nombre ?? "OTRAS PRUEBAS");

        var especialidades = new List<object>();

        foreach (var grupo in agrupados)
        {
            var examenes = new List<object>();

            foreach (var detalle in grupo)
            {
                // Pasamos los resultados físicos asociados a esta orden
                var nodo = MapearRecursivo(detalle.Examen!, detalle.Resultados.ToList(), sexoPaciente);
                if (nodo != null)
                {
                    // --- CAMBIO CLAVE AQUÍ ---
                    // Envolvemos el árbol de exámenes con su respectivo OrdenDetalleId
                    examenes.Add(new
                    {
                        OrdenDetalleId = detalle.Id,
                        Arbol = nodo
                    });
                }
            }

            if (examenes.Any())
            {
                especialidades.Add(new
                {
                    NombreEspecialidad = grupo.Key,
                    Examenes = examenes
                });
            }
        }

        return especialidades;
    }

    private static dynamic MapearRecursivo(Examen examen, List<Resultado> resultados, SexoEnum? sexo)
    {
        var resultadoDirecto = resultados.FirstOrDefault(r => r.ExamenId == examen.Id);
        var hijos = new List<dynamic>();

        // 3. ORDENAMOS a los hijos por su propiedad "Orden"
        if (examen.SubExamenes != null && examen.SubExamenes.Any())
        {
            foreach (var sub in examen.SubExamenes.OrderBy(s => s.Orden))
            {
                var subNodo = MapearRecursivo(sub, resultados, sexo);
                if (subNodo != null)
                    hijos.Add(subNodo);
            }
        }

        string estado = resultadoDirecto?.Estado ?? "Pendiente";
        DateTime? fecha = resultadoDirecto?.FechaProcesamiento;

        // 4. INFERENCIA BOTTOM-UP (Los padres heredan el estado de los hijos)
        if (hijos.Any())
        {
            bool todosValidados = hijos.All(h => h.Estado == "Validado");
            bool todosProcesadosOValidados = hijos.All(h => h.Estado == "Procesado" || h.Estado == "Validado");

            if (todosValidados) 
                estado = "Validado";
            else if (todosProcesadosOValidados) 
                estado = "Procesado";
            else 
                estado = "Pendiente";
            
            var fechasHijos = hijos.Where(h => h.FechaProcesamiento != null).Select(h => (DateTime)h.FechaProcesamiento).ToList();
            if (fechasHijos.Any()) fecha = fechasHijos.Max();
        }

        // 5. VALORES DE REFERENCIA DINÁMICOS
        string refValue = examen.ValoresReferencia ?? "";
        if (sexo == SexoEnum.Masculino && !string.IsNullOrWhiteSpace(examen.ValoresReferenciaM))
            refValue = examen.ValoresReferenciaM;
        else if (sexo == SexoEnum.Femenino && !string.IsNullOrWhiteSpace(examen.ValoresReferenciaF))
            refValue = examen.ValoresReferenciaF;

        // Devolvemos el nodo con TODA la información que el frontend necesita para renderizar inputs o tablas
        return new
        {
            ExamenId = examen.Id,
            Nombre = examen.Nombre,
            Unidad = examen.Unidad ?? "",
            ValoresReferencia = refValue,
            Estado = estado,
            FechaProcesamiento = fecha,
            
            // Datos específicos del resultado (si existen)
            ResultadoId = resultadoDirecto?.Id,
            ValorResultado = resultadoDirecto?.ValorResultado ?? "",
            NotaInterna = resultadoDirecto?.NotaInterna,
            NotaClinica = resultadoDirecto?.NotaClinica,
            ImprimirObservacion = resultadoDirecto?.ImprimirObservacion ?? false,
            
            SubExamenes = hijos
        };
    }
}