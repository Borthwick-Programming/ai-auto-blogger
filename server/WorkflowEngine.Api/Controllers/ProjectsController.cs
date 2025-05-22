using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowEngine.Core.Interfaces;
using WorkflowEngine.Core.Models;


namespace WorkflowEngine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private string CurrentUser => User.Identity?.Name!;
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.ListAsync(CurrentUser);
            return Ok(projects);
        }

        // GET: api/projects/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var project = await _projectService.GetAsync(id, CurrentUser);
            return project is null ? NotFound() : Ok(project);
        }

        // POST: api/projects
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectRequest request)
        {
            var created = await _projectService.CreateAsync(CurrentUser, request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // DELETE: api/projects/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _projectService.DeleteAsync(id, CurrentUser);
            return success ? NoContent() : NotFound();
        }
    }
}
