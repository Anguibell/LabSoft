public class OrdenQueryFilter
{
    public string? NumeroOrden { get; set; } // Búsqueda exacta rápida
    public string? TerminoPaciente { get; set; } // Busca "Anguibell" o "31031823"
    public string? Estado { get; set; } // "PENDIENTE", "COMPLETADA", etc.
    
    // Obligatorios para evitar colapsos. Si el frontend no los manda, el backend los setea al mes actual.
    public DateTime FechaInicio { get; set; } = DateTime.UtcNow.AddDays(-30);
    public DateTime FechaFin { get; set; } = DateTime.UtcNow;

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20; // Las órdenes requieren vistas más amplias
}