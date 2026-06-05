using ImobiliariaMathers.Models;

namespace ImobiliariaMathers.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(Usuario usuario);
    }
}
