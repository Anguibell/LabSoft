namespace LabSoft.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
public enum SexoEnum
{
    Masculino,
    Femenino
}

public enum ParentescoEnum
{
    Titular,
    Hijo,
    Hija,
    Conyuge,
    Otro
}

public class Paciente
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Cedula { get; set; } // Opcional para recién nacidos / menores
    public bool EsRecienNacido { get; set; } = false; // La casilla de control
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public DateTime FechaNacimiento { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Propiedad de la edad calculada automáticamente
    public string EdadFormateada
    {
        get
        {
            var today = DateTime.Today;
            var diff = today - FechaNacimiento.Date;

            if (diff.TotalDays < 30)
            {
                return $"{(int)diff.TotalDays} días";
            }
            else if (diff.TotalDays < 365)
            {
                int meses = (today.Year - FechaNacimiento.Year) * 12 + today.Month - FechaNacimiento.Month;
                if (today.Day < FechaNacimiento.Day) meses--;
                return $"{Math.Max(1, meses)} meses";
            }
            else
            {
                int age = today.Year - FechaNacimiento.Year;
                if (FechaNacimiento.Date > today.AddYears(-age)) age--;
                return $"{age} años";
            }
        }
    }

    public SexoEnum Sexo { get; set; } // Estrictamente Masculino o Femenino

    // --- BLOQUE DE CONTACTO ---
    public string? Telefono { get; set; }
    public string? TelefonoAlternativo { get; set; }
    public string? Correo { get; set; }
    public string? CorreoAlternativo { get; set; }
    public string? Domicilio { get; set; }
    public string? NotaMedica { get; set; } // Alergias, observaciones, etc.

    // --- BLOQUE DE AFILIACIÓN ---
    public string? NumeroAfiliado { get; set; }
    public ParentescoEnum? Parentesco { get; set; } // Opciones fijas controladas
    
    // Relación de Autoreferencia: Si es beneficiario, aquí guardamos el ID y la navegación al Titular
    public Guid? TitularId { get; set; } 
    public Paciente? Titular { get; set; }
    
    // Lista de beneficiarios por si este paciente es el titular
    public ICollection<Paciente> Beneficiarios { get; set; } = new List<Paciente>();

    //

    [NotMapped]
    public int TotalOrdenes { get; set; }

    [NotMapped]
    public DateTime? UltimaVisita { get; set; }
}