using System.ComponentModel.DataAnnotations;

namespace LabSoft.Application.DTOs;

public class ExamenDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }

    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    public Guid EspecialidadId { get; set; }
    public int Orden { get; set; } // Campo para secuencia clínica de impresión

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
    public string EspecialidadNombre { get; set; } = string.Empty;
    public Guid? ExamenPadreId { get; set; }
    
    // Lista anidada limpia solo hacia abajo (hijos), sin apuntar hacia arriba al padre
    public List<ExamenDto> SubExamenes { get; set; } = new List<ExamenDto>();
}