using ImobiliariaViviane.Data;
using ImobiliariaViviane.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ImobiliariaViviane.Services
{
    public class RecoveryCodeService
    {
        private const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        public List<string> GenerateCodes(int count = 8)
        {
            var codes = new List<string>(count);

            for (var i = 0; i < count; i++)
            {
                var first = GenerateChunk(4);
                var second = GenerateChunk(4);
                codes.Add($"{first}-{second}");
            }

            return codes;
        }

        public List<CodigoRecuperacao> BuildEntities(long usuarioId, IEnumerable<string> rawCodes)
        {
            return rawCodes.Select(code => new CodigoRecuperacao
            {
                UsuarioId = usuarioId,
                CodigoHash = HashCode(code),
                CriadoEm = DateTime.UtcNow
            }).ToList();
        }

        public async Task<bool> TryConsumeCodeAsync(AppDbContext context, long usuarioId, string rawCode)
        {
            var codeHash = HashCode(rawCode);

            var storedCode = await context.CodigosRecuperacao
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId
                    && c.CodigoHash == codeHash
                    && c.UsadoEm == null);

            if (storedCode is null)
            {
                return false;
            }

            storedCode.UsadoEm = DateTime.UtcNow;
            return true;
        }

        private static string GenerateChunk(int size)
        {
            var chars = new char[size];
            var bytes = RandomNumberGenerator.GetBytes(size);

            for (var i = 0; i < size; i++)
            {
                chars[i] = Alphabet[bytes[i] % Alphabet.Length];
            }

            return new string(chars);
        }

        private static string HashCode(string rawCode)
        {
            var normalized = rawCode.Trim().ToUpperInvariant();
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
            return Convert.ToHexString(bytes);
        }
    }
}
