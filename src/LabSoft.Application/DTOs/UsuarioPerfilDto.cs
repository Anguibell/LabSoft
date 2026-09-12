namespace LabSoft.Application.DTOs;

public class UsuarioPerfilDto
{
    public Guid Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Domicilio { get; set; }
}