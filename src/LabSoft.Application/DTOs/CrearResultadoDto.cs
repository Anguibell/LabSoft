namespace LabSoft.Application.DTOs;

public class ResultadoParametroInputDto
{
    public Guid ExamenId { get; set; }
    public string ValorResultado { get; set; } = string.Empty;
    public string? NotaInterna { get; set; }
    public string? NotaClinica { get; set; }
    public bool ImprimirObservacion { get; set; }
}

public class CrearResultadoDto
{
    public Guid OrdenDetalleId { get; set; }
    public List<ResultadoParametroInputDto> ResultadosParametros { get; set; } = new();
}