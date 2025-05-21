using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowEngine.Infrastructure.Data;

namespace WorkflowEngine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly WorkflowEngineDbContext _db;

        public ProjectsController(WorkflowEngineDbContext db)
        {
            _db = db;
        }
    }
}
