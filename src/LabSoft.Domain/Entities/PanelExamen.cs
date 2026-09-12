namespace LabSoft.Domain.Entities;

public class PanelExamen
{
    public Guid PanelId { get; set; }
    public Panel Panel { get; set; } = null!;

    public Guid ExamenId { get; set; }
    public Examen Examen { get; set; } = null!;
}