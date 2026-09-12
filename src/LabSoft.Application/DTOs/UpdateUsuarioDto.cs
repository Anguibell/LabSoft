namespace LabSoft.Application.DTOs;

public class UpdateUsuarioDto
{
    public string? NombreUsuario { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Telefono { get; set; }
    public string? Domicilio { get; set; }
    // Nota: ¡No incluimos el campo 'Rol' aquí para evitar que el usuario común intente modificarlo!
}