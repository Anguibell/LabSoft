using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;
using LabSoft.Domain.Constants;

namespace LabSoft.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<bool> ActualizarMiPerfilAsync(Guid usuarioId, UpdateUsuarioDto dto)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(usuarioId);
        if (usuario == null) return false;

        // 1. Si envían un email y es diferente al actual, validamos duplicidad y lo actualizamos
        if (!string.IsNullOrEmpty(dto.Email) && dto.Email != usuario.Email)
        {
            if (await _usuarioRepository.ExisteEmailAsync(dto.Email, usuarioId))
                throw new InvalidOperationException("El correo electrónico ya está registrado por otro usuario.");
            
            usuario.Email = dto.Email;
        }

        // 2. Si envían un nombre de usuario y es diferente, validamos y actualizamos
        if (!string.IsNullOrEmpty(dto.NombreUsuario) && dto.NombreUsuario != usuario.NombreUsuario)
        {
            if (await _usuarioRepository.ExisteNombreUsuarioAsync(dto.NombreUsuario, usuarioId))
                throw new InvalidOperationException("El nombre de usuario ya está en uso por otra persona.");
            
            usuario.NombreUsuario = dto.NombreUsuario;
        }

        // 3. Actualizamos teléfono solo si el usuario mandó un valor nuevo
        if (!string.IsNullOrEmpty(dto.Telefono))
        {
            usuario.Telefono = dto.Telefono;
        }

        // 4. Actualizamos domicilio solo si el usuario mandó un valor nuevo
        if (!string.IsNullOrEmpty(dto.Domicilio))
        {
            usuario.Domicilio = dto.Domicilio;
        }

        // 5. Actualizamos contraseña solo si se envía una nueva
        if (!string.IsNullOrEmpty(dto.Password))
        {
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        await _usuarioRepository.ActualizarAsync(usuario);
        await _usuarioRepository.GuardarCambiosAsync();
        return true;
    }

    public async Task<UsuarioPerfilDto?> ObtenerPerfilAsync(Guid usuarioId)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(usuarioId);
        if (usuario == null) return null;

        return new UsuarioPerfilDto
        {
            Id = usuario.Id,
            NombreUsuario = usuario.NombreUsuario,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Telefono = usuario.Telefono,
            Domicilio = usuario.Domicilio
        };
    }

    public async Task<IEnumerable<UsuarioListDto>> ObtenerTodosAsync()
    {
        var usuarios = await _usuarioRepository.ObtenerTodosAsync();
        
        return usuarios.Select(u => new UsuarioListDto
        {
            Id = u.Id,
            NombreUsuario = u.NombreUsuario,
            Email = u.Email,
            Rol = u.Rol
        });
    }

    public async Task<bool> CambiarRolAsync(Guid usuarioId, string nuevoRol)
    {
        string rolEstandarizado = nuevoRol?.Trim().ToUpper() ?? string.Empty;
        
        if (!RolesConstantes.EsValido(rolEstandarizado))
        {
            throw new InvalidOperationException($"El rol especificado no es válido.");
        }

        var usuario = await _usuarioRepository.ObtenerPorIdAsync(usuarioId);
        if (usuario == null) return false;

        usuario.Rol = rolEstandarizado;
        await _usuarioRepository.ActualizarAsync(usuario);
        await _usuarioRepository.GuardarCambiosAsync();
        return true;
    }

    public async Task<PagedResultDto<UsuarioListDto>> ObtenerPaginadosAsync(int pageNumber, int pageSize, string? busqueda)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;

        // 1. Traemos todos los usuarios desde el repositorio
        var usuariosEnDb = await _usuarioRepository.ObtenerTodosAsync();
        var query = usuariosEnDb.AsQueryable();

        // 2. Aplicamos el filtro si el usuario escribió algo en la búsqueda
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            busqueda = busqueda.Trim().ToLower();
            query = query.Where(u => (!string.IsNullOrEmpty(u.NombreUsuario) && u.NombreUsuario.ToLower().Contains(busqueda)) || 
                                    (!string.IsNullOrEmpty(u.Email) && u.Email.ToLower().Contains(busqueda)));
        }

        // 3. Calculamos el total de registros filtrados
        var totalRecords = query.Count();

        // 4. Aplicamos paginación con Skip y Take, y mapeamos al DTO
        var usuariosPaginados = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UsuarioListDto 
            {
                Id = u.Id,
                NombreUsuario = u.NombreUsuario,
                Email = u.Email,
                Rol = u.Rol
            })
            .ToList();

        return new PagedResultDto<UsuarioListDto>
        {
            Items = usuariosPaginados,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords
        };
    }
}