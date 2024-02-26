using Microsoft.AspNetCore.Mvc;

namespace MiddlewareExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<WeatherForecast> GetSomething()
        {
            throw new Exception($"Intentional exception");

        }
    }
}
