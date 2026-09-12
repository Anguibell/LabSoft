namespace LabSoft.Application.DTOs;

public class CreateOrdenDto
{
    public Guid PacienteId { get; set; }
    public List<Guid> ExamenIds { get; set; } = new(); // Exámenes sueltos
    public List<PanelSolicitudDto> PanelesSolicitados { get; set; } = new(); // Perfiles con o sin exclusiones
}

public class PanelSolicitudDto
{
    public Guid PanelId { get; set; }
    public List<Guid> ExamenesExcluidosIds { get; set; } = new(); // IDs de exámenes que el bioanalista desmarcó
}