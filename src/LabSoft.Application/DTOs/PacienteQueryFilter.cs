public class PacienteQueryFilter
{
    public string? TerminoBusqueda { get; set; } // Busca Nombre, Apellido, Cédula o Nro de Orden
    public string? Sexo { get; set; } // "MASCULINO" o "FEMENINO"
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    
    // Paginación por defecto
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}