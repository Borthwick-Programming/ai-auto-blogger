using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowEngine.Core.Interfaces;
using WorkflowEngine.Core.Models;


namespace WorkflowEngine.Api.Controllers
{
    /// <summary>
    /// Controller to manage user workflow projects.
    /// All endpoints require Windows-integrated authentication via [Authorize].
    /// //Future implementation will use oauth2.0 and JWT tokens.
    /// </summary>
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private string _currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectsController"/> class.
        /// </summary>
        /// <param name="projectService">The service used to manage project-related operations.</param>
        /// <param name="user">The service providing information about the current user.</param>
        public ProjectsController(IProjectService projectService, ICurrentUserService user)
        {
            _projectService = projectService;
            _currentUser = user.Username;
            Console.WriteLine($"(ProjectsController)Fetching projects for user: {_currentUser}");
        }

        /// <summary>
        /// GET: api/projects
        /// Retrieves all projects accessible (authenticated) to the current user.
        /// </summary>
        /// <remarks>This method returns a list of projects that the current user has permission to
        /// access. The result is wrapped in an HTTP 200 OK response.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing an HTTP 200 OK response with a list of projects.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.ListAsync(_currentUser);
            return Ok(projects);
        }

        /// <summary>
        /// GET: api/projects/{id}
        /// Retrieves a project by its unique identifier, if owned by the user.
        /// </summary>
        /// <remarks>This method returns an HTTP 200 response with the project data if the project is
        /// found,  or an HTTP 404 response if no project with the specified identifier exists.</remarks>
        /// <param name="id">The unique identifier of the project to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the project data if found;  otherwise, a <see
        /// cref="NotFoundResult"/> if the project does not exist.</returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var project = await _projectService.GetAsync(id, _currentUser);
            return project is null ? NotFound() : Ok(project);
        }

        /// <summary>
        /// POST: api/projects
        /// Creates a new project for the authenticated user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectRequest request)
        {
            var created = await _projectService.CreateAsync(_currentUser, request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// DELETE: api/projects/{id}
        /// Deletes the project with the specified identifier.
        /// </summary>
        /// <remarks>This action requires the caller to be authenticated and authorized to delete the
        /// specified project.</remarks>
        /// <param name="id">The unique identifier of the project to delete.</param>
        /// <returns>A <see cref="NoContentResult"/> if the project was successfully deleted;  otherwise, a <see
        /// cref="NotFoundResult"/> if the project does not exist.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _projectService.DeleteAsync(id, _currentUser);
            return success ? NoContent() : NotFound();
        }
    }
}
