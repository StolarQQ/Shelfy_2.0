namespace Shelfy.Infrastructure.Services
{
    public interface IEncrypterService : IService
    {
        string GetSalt();
        string GetHash(string password, string salt);
    }
}
