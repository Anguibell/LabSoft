using System.Security.Claims;
using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LabSoft.Domain.Constants;

[Route("api/usuarios")]
[ApiController]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("perfil")]
    public async Task<IActionResult> ObtenerMiPerfil()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

        var perfil = await _usuarioService.ObtenerPerfilAsync(Guid.Parse(userIdClaim));
        if (perfil == null) return NotFound("Usuario no encontrado.");

        return Ok(perfil);
    }

    [HttpPut("mi-perfil")]
    public async Task<IActionResult> ActualizarMiPerfil([FromBody] UpdateUsuarioDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

        try
        {
            var actualizado = await _usuarioService.ActualizarMiPerfilAsync(Guid.Parse(userIdClaim), dto);
            if (!actualizado) return NotFound("Usuario no encontrado.");

            return Ok(new { message = "Tus datos han sido actualizados exitosamente." });
        } 
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message }); 
        }
    }

    [HttpGet]
    [Authorize(Roles = RolesConstantes.ADMINISTRADOR + "," + RolesConstantes.BIOANALISTA)]
    public async Task<IActionResult> ObtenerTodos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? busqueda = null)
    {
        var resultadoPaginado = await _usuarioService.ObtenerPaginadosAsync(pageNumber, pageSize, busqueda);
        return Ok(resultadoPaginado);
    }

    [HttpPut("{id}/cambiar-rol")]
    [Authorize(Roles = RolesConstantes.ADMINISTRADOR)]
    public async Task<IActionResult> CambiarRol(Guid id, [FromBody] string nuevoRol)
    {
        try
        {
            var resultado = await _usuarioService.CambiarRolAsync(id, nuevoRol);
            if (!resultado) 
                return NotFound("Usuario no encontrado.");

            return Ok(new { message = "Rol actualizado exitosamente." });
        }
        catch (InvalidOperationException ex)
        {
            // Si el servicio detecta un rol inválido (o una regla de negocio), 
            // atrapamos la excepción aquí y devolvemos un 400 Bad Request limpio.
            return BadRequest(new { message = ex.Message });
        }
    }
}