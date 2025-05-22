using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowEngine.Core.Interfaces;
using WorkflowEngine.Core.Models;

namespace WorkflowEngine.Api.Controllers
{
    [Route("api/projects/{projectId:guid}/nodeinstances")]
    [ApiController]
    public class NodeInstancesController : ControllerBase
    {
        private readonly INodeInstanceService _nodes;
        private string CurrentUser => User.Identity!.Name!;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeInstancesController"/> class.
        /// </summary>
        /// <param name="nodes">The service used to manage node instances.</param>
        public NodeInstancesController(INodeInstanceService nodes)
        {
            _nodes = nodes;
        }

        /// <summary>
        /// GET api/projects/{projectId}/nodeinstances
        /// Retrieves all nodes associated with the specified project for the current user.
        /// </summary>
        /// <remarks>This method requires the caller to be authenticated as the current user is used to
        /// filter the results.</remarks>
        /// <param name="projectId">The unique identifier of the project whose nodes are to be retrieved.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of nodes associated with the specified project. The
        /// response is returned with an HTTP 200 status code if successful.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(Guid projectId)
            => Ok(await _nodes.ListAsync(projectId, CurrentUser));

        /// <summary>
        /// GET api/projects/{projectId}/nodeinstances/{nodeId}
        /// Retrieves the details of a specific node within a project.
        /// </summary>
        /// <remarks>This method returns an HTTP 200 response with the node details if the node is found,
        /// or an HTTP 404 response if the node is not found.</remarks>
        /// <param name="projectId">The unique identifier of the project containing the node.</param>
        /// <param name="nodeId">The unique identifier of the node to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the node details if found, or a <see cref="NotFoundResult"/> if
        /// the node does not exist.</returns>
        [HttpGet("{nodeId:guid}")]
        public async Task<IActionResult> Get(Guid projectId, Guid nodeId)
        {
            var dto = await _nodes.GetAsync(projectId, nodeId, CurrentUser);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// POST api/projects/{projectId}/nodeinstances
        /// Creates a new node instance within the specified project.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project where the node instance will be created.</param>
        /// <param name="req">The request object containing the details of the node instance to be created.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the operation.  Returns a <see
        /// cref="CreatedAtActionResult"/> containing the URI of the created node instance and its details.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(Guid projectId, CreateNodeInstanceRequest req)
        {
            var dto = await _nodes.CreateAsync(projectId, CurrentUser, req);
            return CreatedAtAction(nameof(Get), new { projectId, nodeId = dto.Id }, dto);
        }

        /// <summary>
        /// PUT api/projects/{projectId}/nodeinstances/{nodeId}
        /// Updates an existing node instance within the specified project.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project containing the node instance to update.</param>
        /// <param name="nodeId">The unique identifier of the node instance to update. Must match the <paramref name="req"/> ID.</param>
        /// <param name="req">The request object containing the updated details of the node instance.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.  Returns <see
        /// cref="NoContentResult"/> if the update is successful,  <see cref="BadRequestResult"/> if the <paramref
        /// name="nodeId"/> does not match the ID in <paramref name="req"/>,  or <see cref="NotFoundResult"/> if the
        /// node instance does not exist.</returns>
        [HttpPut("{nodeId:guid}")]
        public async Task<IActionResult> Update(Guid projectId, Guid nodeId, UpdateNodeInstanceRequest req)
        {
            if (nodeId != req.Id) return BadRequest();
            var ok = await _nodes.UpdateAsync(projectId, CurrentUser, req);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>
        /// DELETE api/projects/{projectId}/nodeinstances/{nodeId}
        /// Deletes a node associated with the specified project.
        /// </summary>
        /// <remarks>This operation requires the current user to have appropriate permissions to delete
        /// the node.</remarks>
        /// <param name="projectId">The unique identifier of the project to which the node belongs.</param>
        /// <param name="nodeId">The unique identifier of the node to delete.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.  Returns <see
        /// cref="NoContentResult"/> if the node was successfully deleted;  otherwise, returns <see
        /// cref="NotFoundResult"/> if the node does not exist.</returns>
        [HttpDelete("{nodeId:guid}")]
        public async Task<IActionResult> Delete(Guid projectId, Guid nodeId)
        {
            var ok = await _nodes.DeleteAsync(projectId, nodeId, CurrentUser);
            return ok ? NoContent() : NotFound();
        }
    }
}
