namespace LabSoft.Domain.Entities;

public class Examen
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public int Orden { get; set; } = 0; // Campo para secuencia clínica de impresión
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public string? ValoresReferencia { get; set; } // Valor general o por defecto
    public string? ValoresReferenciaM { get; set; } // Valor exclusivo para Hombres
    public string? ValoresReferenciaF { get; set; } // Valor exclusivo para Mujeres
    public bool Activo { get; set; } = true;
    public int? DecimalesRequeridos { get; set; }
    
    // Nueva propiedad clave: Indica si este subcomponente viene por defecto en la plantilla
    public bool EsOpcionalDinamico { get; set; } = false; 

    // Llave foránea hacia Especialidad
    public Guid EspecialidadId { get; set; }
    public Especialidad? Especialidad { get; set; }

    // Relación recursiva para subcomponentes (Niveles anidados: Principal -> Grupo -> Parámetro)
    public Guid? ExamenPadreId { get; set; }
    public Examen? ExamenPadre { get; set; }
    public ICollection<Examen> SubExamenes { get; set; } = new List<Examen>();
}