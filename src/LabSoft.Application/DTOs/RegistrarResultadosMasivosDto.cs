namespace LabSoft.Application.DTOs;

public class RegistrarResultadosMasivosDto
{
    public Guid OrdenId { get; set; }
    public List<DetalleResultadoInputDto> DetallesResultados { get; set; } = new();
}

public class DetalleResultadoInputDto
{
    public Guid OrdenDetalleId { get; set; }
    public List<ResultadoParametroInputDto> ResultadosParametros { get; set; } = new();
}