namespace LabSoft.Application.DTOs;

public class ResultadoDto
{
    public Guid Id { get; set; }
    public Guid OrdenDetalleId { get; set; }
    public string ValorResultado { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public DateTime FechaProcesamiento { get; set; }
    public string Estado { get; set; } = string.Empty;
}