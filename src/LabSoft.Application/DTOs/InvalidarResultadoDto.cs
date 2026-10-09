using System.ComponentModel.DataAnnotations;

namespace LabSoft.Application.DTOs;

public class InvalidarResultadoDto
{
    [Required(ErrorMessage = "El motivo de invalidación es obligatorio por motivos legales.")]
    [MinLength(10, ErrorMessage = "El motivo debe ser detallado (mínimo 10 caracteres).")]
    public string Motivo { get; set; } = string.Empty;
}