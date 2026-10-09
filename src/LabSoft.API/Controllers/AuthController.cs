using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabSoft.Application.DTOs;
using LabSoft.Application.Interfaces;
using LabSoft.Infrastructure.Persistence; 
using LabSoft.Domain.Entities;
using LabSoft.Domain.Constants;

namespace LabSoft.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly LabSoftDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(LabSoftDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    // [HttpPost("register")]
    // public async Task<IActionResult> Register([FromBody] RegistrarUsuarioDto dto)
    // {
    //     // 1. Verificar duplicidad de Email O Nombre de Usuario
    //     if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
    //         return BadRequest("El correo ya está registrado.");

    //     if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == dto.NombreUsuario))
    //         return BadRequest("El nombre de usuario ya está en uso.");

    //     // 2. Validar que el rol sea uno de los permitidos
    //     string rolEstandarizado = dto.Rol?.Trim().ToUpper() ?? string.Empty;
    //     if (!RolesConstantes.EsValido(rolEstandarizado))
    //     {
    //         return BadRequest($"El rol '{dto.Rol}' no es válido. Roles permitidos: {string.Join(", ", RolesConstantes.Todos)}");
    //     }

    //     // 3. Hashear la contraseña con BCrypt
    //     var usuario = new Usuario
    //     {
    //         Nombre = dto.Nombre,           
    //         Apellido = dto.Apellido,
    //         NombreUsuario = dto.NombreUsuario,
    //         Email = dto.Email,
    //         PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
    //         Rol = rolEstandarizado,
    //         Telefono = dto.Telefono,
    //         Domicilio = dto.Domicilio
    //     };

    //     _context.Usuarios.Add(usuario);
    //     await _context.SaveChangesAsync();

    //     return Ok(new { message = "Usuario registrado exitosamente." });
    // }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegistrarUsuarioDto dto)
    {
        // BLOQUEO DE SEGURIDAD PARA MVP: 
        // El sistema es privado y cerrado. Los usuarios solo se crean por Seeder o por un futuro panel SuperAdmin.
        return StatusCode(StatusCodes.Status403Forbidden, new { message = "El registro público de usuarios se encuentra deshabilitado." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            return Unauthorized("Credenciales inválidas.");

        var token = _tokenService.CreateToken(usuario);
        var refreshToken = _tokenService.GenerateRefreshToken();

        usuario.RefreshToken = refreshToken;
        usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Expira en 7 días
        await _context.SaveChangesAsync();

        return Ok(new 
        { 
            token,
            refreshToken,
            id = usuario.Id,              
            email = usuario.Email,
            rol = usuario.Rol,
            nombre = string.IsNullOrWhiteSpace($"{usuario.Nombre} {usuario.Apellido}") 
                ? usuario.NombreUsuario 
                : $"{usuario.Nombre} {usuario.Apellido}".Trim(),
            nombreUsuario = usuario.NombreUsuario
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        if (dto is null || string.IsNullOrEmpty(dto.Token) || string.IsNullOrEmpty(dto.RefreshToken))
            return BadRequest("Solicitud de renovación inválida.");

        // Opcional: Validar claims del token expirado si requieres extraer la identidad
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.RefreshToken == dto.RefreshToken);

        if (usuario == null || usuario.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return Unauthorized("Refresh Token inválido o expirado.");

        // Generar nuevos tokens
        var nuevoToken = _tokenService.CreateToken(usuario);
        var nuevoRefreshToken = _tokenService.GenerateRefreshToken();

        usuario.RefreshToken = nuevoRefreshToken;
        usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            token = nuevoToken,
            refreshToken = nuevoRefreshToken
        });
    }

    [HttpGet("roles")]
    public IActionResult ObtenerRolesDisponibles()
    {
        // Devuelve la lista estandarizada para que el Front pinte un selector (<select>)
        return Ok(RolesConstantes.Todos);
    }

}