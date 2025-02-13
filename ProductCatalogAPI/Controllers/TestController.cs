using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.Models;
using ProductCatalogAPI.Service;

namespace ProductCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetConnectionString()
        {
            var mongoHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
            var mongoPort = Environment.GetEnvironmentVariable("DATABASE_PORT");
            var mongoDatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
            var mongoUsername = Environment.GetEnvironmentVariable("DATABASE_USERNAME");
            var mongoPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

            var mongoConnectionString = $"mongodb://{mongoUsername}:{mongoPassword}@{mongoHost}:{mongoPort}";

            return Ok(mongoConnectionString);
        }

        [HttpGet("mongo-url")]
        public async Task<ActionResult> GetURL()
        {
            var mongoURL = Environment.GetEnvironmentVariable("MONGO_URL");
            
            return Ok(mongoURL);
        }
    }
}
