using ImobiliariaMathers.Data;
using ImobiliariaMathers.Models;

namespace ImobiliariaMathers.Services
{
    public interface IRecoveryCodeService
    {
        List<string> GenerateCodes(int count = 8);
        List<CodigoRecuperacao> BuildEntities(long usuarioId, IEnumerable<string> rawCodes);
        Task<bool> TryConsumeCodeAsync(AppDbContext context, long usuarioId, string rawCode);
    }
}
