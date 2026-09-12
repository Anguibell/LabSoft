namespace LabSoft.Application.DTOs;

public class ResultadoGlobalDto
{
    public Guid Id { get; set; }
    public string Especialidad { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteCedula { get; set; } = string.Empty;
    public string ExamenNombre { get; set; } = string.Empty;
    public string ValorResultado { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public DateTime FechaProcesamiento { get; set; }
    public string Estado { get; set; } = string.Empty;
}