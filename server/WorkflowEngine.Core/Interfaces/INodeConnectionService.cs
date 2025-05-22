using WorkflowEngine.Core.Models;

namespace WorkflowEngine.Core.Interfaces
{
    /// <summary>
    /// Defines methods for managing node connections within a project.
    /// </summary>
    /// <remarks>This service provides functionality to list, retrieve, create, update, and delete node
    /// connections associated with a specific project. Access to these operations is scoped to the provided Windows
    /// user.</remarks>
    public interface INodeConnectionService
    {
        Task<IEnumerable<NodeConnectionDto>> ListAsync(Guid projectId, string windowsUser);
        Task<NodeConnectionDto?> GetAsync(Guid projectId, Guid connectionId, string windowsUser);
        Task<NodeConnectionDto> CreateAsync(Guid projectId, string windowsUser, CreateNodeConnectionRequest req);
        Task<bool> UpdateAsync(Guid projectId, string windowsUser, UpdateNodeConnectionRequest req);
        Task<bool> DeleteAsync(Guid projectId, Guid connectionId, string windowsUser);
    }
}
