namespace LabSoft.Application.DTOs;

public class UpdateOrdenDto
{
    public string Estado { get; set; } = string.Empty;
    public List<Guid> ExamenIds { get; set; } = new(); // Nueva lista completa de exámenes sueltos deseados
    public List<PanelSolicitudDto> PanelesSolicitados { get; set; } = new(); // Nueva lista de paneles deseados con sus exclusiones
}