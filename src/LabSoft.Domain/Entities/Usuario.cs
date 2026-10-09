namespace LabSoft.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Datos personales
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;

    // Credenciales y sistema
    public string NombreUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Rol { get; set; } = "Recepcionista"; // Roles: "Bioanalista" o "Recepcionista"

    // Auditoría y otros
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public string? Telefono { get; set; }
    public string? Domicilio { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}