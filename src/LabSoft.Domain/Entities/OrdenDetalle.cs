namespace LabSoft.Domain.Entities;

public class OrdenDetalle
{
    public Guid Id { get; set; } 
    
    public Guid OrdenId { get; set; }
    public Orden Orden { get; set; } = null!;

    // Cada detalle apunta a un Examen específico
    public Guid ExamenId { get; set; }
    public Examen Examen { get; set; } = null!;
    
    // Opcionalmente podemos registrar si vino de un panel en específico
    public Guid? PanelId { get; set; }
    public Panel? Panel { get; set; }

    public string Estado { get; set; } = "Pendiente"; // Pendiente, Completado

    // Propiedad de navegación al resultado técnico
    // Una colección de resultados (soporta exámenes simples o múltiples subcomponentes)
    public ICollection<Resultado> Resultados { get; set; } = new List<Resultado>();
}