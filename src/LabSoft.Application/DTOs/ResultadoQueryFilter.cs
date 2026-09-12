namespace LabSoft.Application.DTOs;

public class ResultadoQueryFilter
{
    public string? TerminoBusqueda { get; set; } // Buscará por Nombre de Paciente, Cédula o Nombre del Examen
    public string? Estado { get; set; } // Ej: "Procesado", "Validado"
    
    // Por defecto, cargamos solo los últimos 7 días para no colapsar la vista inicial
    public DateTime FechaInicio { get; set; } = DateTime.UtcNow.AddDays(-7);
    public DateTime FechaFin { get; set; } = DateTime.UtcNow;

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}