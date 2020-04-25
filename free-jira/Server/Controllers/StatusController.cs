using Microsoft.AspNetCore.Mvc;

namespace FreeJira.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public object Get() {
            return new {
                running = true,
            };
        }
    }
}