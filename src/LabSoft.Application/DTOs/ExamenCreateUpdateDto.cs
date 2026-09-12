using System.ComponentModel.DataAnnotations;

namespace LabSoft.Application.DTOs;

public class CreateExamenDto
{
    public int Codigo { get; set; }

    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    public Guid EspecialidadId { get; set; }
    public int? Orden { get; set; } // Campo para secuencia clínica de impresión

    [Required(ErrorMessage = "El nombre del examen es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public string? ValoresReferencia { get; set; } // Valor general o por defecto
    public string? ValoresReferenciaM { get; set; } // Valor exclusivo para Hombres
    public string? ValoresReferenciaF { get; set; } // Valor exclusivo para Mujeres
    public bool EsOpcionalDinamico { get; set; } = false;
    public int? DecimalesRequeridos { get; set; }
    public Guid? ExamenPadreId { get; set; } // Opcional: si viene lleno, es subcomponente
}

public class UpdateExamenDto
{
    public int Codigo { get; set; }
    public int Orden { get; set; } = 0; // Campo para secuencia clínica de impresión
    
    [Required(ErrorMessage = "El nombre del examen es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public string? ValoresReferencia { get; set; } // Valor general o por defecto
    public string? ValoresReferenciaM { get; set; } // Valor exclusivo para Hombres
    public string? ValoresReferenciaF { get; set; } // Valor exclusivo para Mujeres
    public bool Activo { get; set; }
    public bool EsOpcionalDinamico { get; set; }
    public int? DecimalesRequeridos { get; set; }
}