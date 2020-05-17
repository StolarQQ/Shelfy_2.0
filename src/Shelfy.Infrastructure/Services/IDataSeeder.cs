using System.Threading.Tasks;

namespace Shelfy.Infrastructure.Services
{
    public interface IDataSeeder : IService
    {
        Task SeedAsync();
    }
}

                