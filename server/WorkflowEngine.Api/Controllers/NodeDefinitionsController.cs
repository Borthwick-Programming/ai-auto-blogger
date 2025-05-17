using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowEngine.Runtime.Interfaces;

namespace WorkflowEngine.Api.Controllers
{
    /// <summary>
    /// API Controller for exposing available node definitions.
    /// Provides a GET endpoint at /api/nodes to retrieve all registered nodes.
    /// </summary>
    [Route("api/nodes")]
    [ApiController]
    public class NodeDefinitionsController : ControllerBase
    {
        private readonly INodeRegistry _registry;

        /// <summary>
        /// Injects the node registry (in-memory or future extensible implementation).
        /// </summary>
        /// <param name="registry">The node registry instance that holds all node definitions.</param>
        public NodeDefinitionsController(INodeRegistry registry)
        {
            _registry = registry;
        }

        /// <summary>
        /// Returns all registered node definitions.
        /// </summary>
        /// <returns>HTTP 200 OK with a JSON array of node definitions.</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var nodes = _registry.GetAll();
            return Ok(nodes);
        }
    }
}
