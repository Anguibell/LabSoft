namespace LabSoft.Application.DTOs;

public class HistorialPacienteDto
{
    public Guid OrdenId { get; set; }
    public string NumeroOrden { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public List<HistorialEspecialidadDto> Especialidades { get; set; } = new();
}
public class HistorialEspecialidadDto
{
    public string NombreEspecialidad { get; set; } = string.Empty;
    public List<HistorialItemDto> Examenes { get; set; } = new();
}
public class HistorialItemDto
{
    public string ExamenNombre { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;

    // Contexto médico
    public string ValoresReferencia { get; set; } = string.Empty; 
    public string? NotaClinica { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime? FechaProcesamiento { get; set; }
    
    // Lista recursiva para soportar grupos como "Fórmula Leucocitaria"
    public List<HistorialItemDto> SubExamenes { get; set; } = new();
}