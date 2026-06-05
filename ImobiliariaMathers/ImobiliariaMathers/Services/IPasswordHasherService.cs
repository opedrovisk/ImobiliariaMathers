namespace ImobiliariaMathers.Services
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string persistedHash);
        bool IsLegacyHash(string persistedHash);
    }
}
