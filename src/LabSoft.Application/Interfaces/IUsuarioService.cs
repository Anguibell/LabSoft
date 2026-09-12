using LabSoft.Application.DTOs;
namespace LabSoft.Application.Interfaces;

public interface IUsuarioService
{
    Task<bool> ActualizarMiPerfilAsync(Guid usuarioId, UpdateUsuarioDto dto);
    Task<UsuarioPerfilDto?> ObtenerPerfilAsync(Guid usuarioId);
    Task<IEnumerable<UsuarioListDto>> ObtenerTodosAsync();
    Task<PagedResultDto<UsuarioListDto>> ObtenerPaginadosAsync(int pageNumber, int pageSize, string? busqueda);
    Task<bool> CambiarRolAsync(Guid usuarioId, string nuevoRol);
}