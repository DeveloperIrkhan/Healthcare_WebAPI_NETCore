using Microsoft.AspNetCore.Mvc;

namespace Healthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("health-response")]
        public IActionResult GetHealthStatus()
        {
            return Ok(new
            {
                status = "Healthy",
                service = "Healthcare API",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
