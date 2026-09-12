namespace LabSoft.Domain.Entities;

public class Resultado
{
    public Guid Id { get; set; }
    
    // Relación 1 a 1 (o 1 a muchos si un detalle se repite) con OrdenDetalle
    public Guid OrdenDetalleId { get; set; }
    public OrdenDetalle OrdenDetalle { get; set; } = null!;

    // Identifica a qué examen exacto (sea simple o subcomponente hoja) pertenece este valor
    public Guid ExamenId { get; set; }
    public Examen Examen { get; set; } = null!;

    public string ValorResultado { get; set; } = string.Empty; // Ej: "95.5", "Positivo"

    // Notas del bioanalista
    public string? NotaInterna { get; set; }
    public string? NotaClinica { get; set; }
    public bool ImprimirObservacion { get; set; } = false;            
    public DateTime FechaProcesamiento { get; set; } = DateTime.UtcNow;
    
    public string Estado { get; set; } = "Procesado";           // Procesado, Validado, Repetido

    // --- CAMPOS DE AUDITORÍA ---
    public Guid? ProcesadoPorId { get; set; }
    public Guid? ValidadoPorId { get; set; }
    public bool EsModificado { get; set; } = false;
}