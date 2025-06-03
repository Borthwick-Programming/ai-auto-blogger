using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace WorkflowEngine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Returns a simple health check response.
        /// </summary>
        /// <returns>A 200 OK response with a message indicating the service is healthy.</returns>
        [HttpGet]
        [ResponseCache(NoStore = true, Duration = 0)]
        public IActionResult Get()
        {
            return Ok(new
            {
                ok = true,
                timeUtc = DateTime.UtcNow, // helpful when pinging
                version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() 
            });
        }
    }
}
