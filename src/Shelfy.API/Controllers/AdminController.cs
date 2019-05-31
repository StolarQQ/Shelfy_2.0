using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shelfy.Infrastructure.Services;

namespace Shelfy.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ApiControllerBase
    {
        private readonly IDataSeeder _dataSeeder;

        public AdminController(IDataSeeder dataSeeder)
        {
            _dataSeeder = dataSeeder;
        }
        
        [HttpGet("seed")]
        //[Authorize(Policy = "HasAdminRole")]
        public async Task<IActionResult> Get()
        {
            await _dataSeeder.SeedAsync();

            return NoContent();
        }
    }
}