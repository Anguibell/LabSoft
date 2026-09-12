using LabSoft.Domain.Entities; 

namespace LabSoft.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorIdAsync(Guid id);
    Task<IEnumerable<Usuario>> ObtenerTodosAsync();
    Task<bool> ExisteEmailAsync(string email, Guid excludeUsuarioId);
    Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, Guid excludeUsuarioId);
    Task ActualizarAsync(Usuario usuario);
    Task GuardarCambiosAsync();
}