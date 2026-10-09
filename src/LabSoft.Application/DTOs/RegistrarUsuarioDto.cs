using LabSoft.Domain.Constants;

namespace LabSoft.Application.DTOs;

public class RegistrarUsuarioDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Rol { get; set; } = RolesConstantes.RECEPCIONISTA; // Valor por defecto
    public string? Telefono { get; set; }
    public string? Domicilio { get; set; }
}