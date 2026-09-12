namespace LabSoft.Application.DTOs;

public class ActualizarResultadoDto
{
    public string ValorResultado { get; set; } = string.Empty;
    public string? NotaInterna { get; set; }
    public string? NotaClinica { get; set; }
    public bool ImprimirObservacion { get; set; }
}