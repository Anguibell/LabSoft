namespace LabSoft.Domain.Entities;

public class Panel
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    
    // Relación muchos a muchos
    public ICollection<PanelExamen> PanelExamenes { get; set; } = new List<PanelExamen>();
}