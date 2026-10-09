using LabSoft.Domain.Entities;

namespace LabSoft.Application.DTOs;

public class PacienteDto
{
    public Guid Id { get; set; }
    public string? Cedula { get; set; }
    public bool EsRecienNacido { get; set; } = false;
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public DateTime FechaNacimiento { get; set; }
    public string EdadFormateada { get; set; } = string.Empty;
    public SexoEnum Sexo { get; set; }
    
    // --- DATOS CALCULADOS PARA EL HISTORIAL ---
    public int TotalOrdenes { get; set; }
    public DateTime? UltimaVisita { get; set; }

    // --- BLOQUE DE CONTACTO ---
    public string? Telefono { get; set; }
    public string? TelefonoAlternativo { get; set; }
    public string? Correo { get; set; }
    public string? CorreoAlternativo { get; set; }
    public string? Domicilio { get; set; }
    public string? NotaMedica { get; set; }

    // --- BLOQUE DE AFILIACIÓN ---
    public string? NumeroAfiliado { get; set; }
    public ParentescoEnum? Parentesco { get; set; }
    public Guid? TitularId { get; set; }
    public string? TitularNombre { get; set; }
    public string? TitularCedula { get; set; }
}