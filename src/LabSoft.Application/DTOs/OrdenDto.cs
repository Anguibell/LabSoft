namespace LabSoft.Application.DTOs;

public class OrdenDto
{
    public Guid Id { get; set; }
    public long NumeroOrden { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public List<OrdenDetalleDto> Detalles { get; set; } = new();
}

public class OrdenDetalleDto
{
    public Guid Id { get; set; }
    public string ItemNombre { get; set; } = string.Empty; // Nombre del examen
    public string Tipo { get; set; } = string.Empty;       // Si vino de un panel o suelto
    public string Estado { get; set; } = string.Empty;
}