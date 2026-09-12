using LabSoft.Domain.Entities;
using System.Security.Claims;

namespace LabSoft.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(Usuario usuario);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}