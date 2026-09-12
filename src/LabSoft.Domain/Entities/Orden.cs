namespace LabSoft.Domain.Entities;

public class Orden
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long NumeroOrden { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public string Estado { get; set; } = "Pendiente"; // Pendiente, Procesado, Entregado
    
    // Relación con Paciente
    public Guid PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;

    // Detalle de los exámenes o perfiles solicitados en esta orden
    public ICollection<OrdenDetalle> Detalles { get; set; } = new List<OrdenDetalle>();
}