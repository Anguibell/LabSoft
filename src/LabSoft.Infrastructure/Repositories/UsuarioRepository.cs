using LabSoft.Application.Interfaces;
using LabSoft.Domain.Entities;
using LabSoft.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabSoft.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly LabSoftDbContext _context;

    public UsuarioRepository(LabSoftDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<bool> ExisteEmailAsync(string email, Guid excludeUsuarioId)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Email == email && u.Id != excludeUsuarioId);
    }

    public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, Guid excludeUsuarioId)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.NombreUsuario == nombreUsuario && u.Id != excludeUsuarioId);
    }

    public Task ActualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}