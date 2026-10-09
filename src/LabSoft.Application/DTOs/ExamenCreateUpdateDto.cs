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

    public List<CreateSubExamenDto>? SubExamenes { get; set; }
}

public class CreateSubExamenDto
{
    public int? Orden { get; set; }
    
    [Required(ErrorMessage = "El nombre del parámetro es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public string? ValoresReferencia { get; set; }
    public string? ValoresReferenciaM { get; set; }
    public string? ValoresReferenciaF { get; set; }
    public bool EsOpcionalDinamico { get; set; } = false;
    public int? DecimalesRequeridos { get; set; }
    
    // Lista recursiva: permite que un hijo (ej. Fórmula Leucocitaria) tenga sus propios hijos (Neutrófilos)
    public List<CreateSubExamenDto>? SubExamenes { get; set; } 
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

    public List<UpdateSubExamenDto>? SubExamenes { get; set; }
}

public class UpdateSubExamenDto
{
    public Guid? Id { get; set; } // Si viene nulo, significa que es un parámetro NUEVO
    public int? Orden { get; set; }
    
    [Required(ErrorMessage = "El nombre del parámetro es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public string? ValoresReferencia { get; set; }
    public string? ValoresReferenciaM { get; set; }
    public string? ValoresReferenciaF { get; set; }
    public bool EsOpcionalDinamico { get; set; }
    public int? DecimalesRequeridos { get; set; }
    
    public List<UpdateSubExamenDto>? SubExamenes { get; set; }
}