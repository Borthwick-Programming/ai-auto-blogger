using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowEngine.Core.Interfaces;
using WorkflowEngine.Core.Models;

namespace WorkflowEngine.Api.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing node connections within a project.
    /// </summary>
    /// <remarks>This controller allows clients to perform CRUD operations on node connections associated with
    /// a specific project. Each endpoint requires the project ID as part of the route, and operations are scoped to the
    /// current authenticated user.</remarks>
    [Route("api/projects/{projectId:guid}/nodeconnections")]
    [ApiController]
    public class NodeConnectionsController : ControllerBase
    {
        private readonly INodeConnectionService _svc;
        private string CurrentUser => User.Identity!.Name!;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConnectionsController"/> class with the specified service.
        /// </summary>
        /// <param name="svc">The service used to manage node connections. Cannot be null.</param>
        public NodeConnectionsController(INodeConnectionService svc) => _svc = svc;

        /// <summary>
        /// Retrieves all items associated with the specified project for the current user.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project whose items are to be retrieved.</param>
        /// <returns>An <see cref="IActionResult"/> containing an HTTP 200 response with the list of items  associated with the
        /// specified project for the current user.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(Guid projectId)
            => Ok(await _svc.ListAsync(projectId, CurrentUser));

        /// <summary>
        /// Retrieves a specific resource by its unique identifier within the context of a project.
        /// </summary>
        /// <remarks>This method returns an HTTP 200 response with the resource data if it exists, or an
        /// HTTP 404 response if the resource is not found.</remarks>
        /// <param name="projectId">The unique identifier of the project to which the resource belongs.</param>
        /// <param name="id">The unique identifier of the resource to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the resource if found, or a <see cref="NotFoundResult"/> if the
        /// resource does not exist.</returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid projectId, Guid id)
        {
            var dto = await _svc.GetAsync(projectId, id, CurrentUser);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Creates a new node connection for the specified project.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project to which the node connection belongs.</param>
        /// <param name="req">The request object containing the details of the node connection to create.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the operation.  Returns a <see
        /// cref="CreatedAtActionResult"/> containing the created node connection if successful.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(Guid projectId, CreateNodeConnectionRequest req)
        {
            var dto = await _svc.CreateAsync(projectId, CurrentUser, req);
            return CreatedAtAction(nameof(Get), new { projectId, id = dto.Id }, dto);
        }

        /// <summary>
        /// Updates an existing node connection within the specified project.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project containing the node connection to update.</param>
        /// <param name="id">The unique identifier of the node connection to update.</param>
        /// <param name="req">The request object containing the updated details of the node connection.</param>
        /// <returns>Returns <see cref="NoContentResult"/> if the update is successful,  <see cref="BadRequestResult"/> if the
        /// <paramref name="id"/> does not match the <see cref="UpdateNodeConnectionRequest.Id"/>,  or <see
        /// cref="NotFoundResult"/> if the node connection does not exist.</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid projectId, Guid id, UpdateNodeConnectionRequest req)
        {
            if (id != req.Id) return BadRequest();
            var ok = await _svc.UpdateAsync(projectId, CurrentUser, req);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a resource identified by the specified ID within the given project.
        /// </summary>
        /// <remarks>The operation requires the caller to have appropriate permissions for the specified
        /// project.</remarks>
        /// <param name="projectId">The unique identifier of the project containing the resource to delete.</param>
        /// <param name="id">The unique identifier of the resource to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see
        /// cref="NoContentResult"/> if the resource was successfully deleted,  or <see cref="NotFoundResult"/> if the
        /// resource does not exist.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid projectId, Guid id)
        {
            var ok = await _svc.DeleteAsync(projectId, id, CurrentUser);
            return ok ? NoContent() : NotFound();
        }
    }
}
