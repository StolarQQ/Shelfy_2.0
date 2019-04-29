namespace Shelfy.Infrastructure.Services
{
    public interface IEncrypterService
    {
        string GetSalt();
        string GetHash(string password, string salt);
    }
}
