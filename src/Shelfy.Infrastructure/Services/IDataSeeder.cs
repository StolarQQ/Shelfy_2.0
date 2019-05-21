using System.Threading.Tasks;

namespace Shelfy.Infrastructure.Services
{
    public interface IDataSeeder
    {
        Task SeedAsync();
    }
}