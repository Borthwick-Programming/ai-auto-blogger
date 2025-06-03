using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Core.Models;

namespace WorkflowEngine.Core.Interfaces
{
    /// <summary>
    /// Defines CRUD operations for node instances within a workflow project;
    /// methods for managing node instances within a project.
    /// </summary>
    /// <remarks>This service allows for listing, retrieving, creating, updating, and deleting node instances
    /// associated with a specific project. Access to these operations is typically scoped to a specific Windows
    /// user.</remarks>
    public interface INodeInstanceService
    {
        /// <summary>
        /// Asynchronously retrieves a list of node instances associated with the specified project.
        /// </summary>
        /// <remarks>This method is intended to be used in scenarios where node instances need to be
        /// retrieved for a specific project  in the context of a given Windows user. Ensure that the <paramref
        /// name="projectId"/> corresponds to a valid project  and that the <paramref name="windowsUser"/> has the
        /// necessary permissions to access the data.</remarks>
        /// <param name="projectId">The unique identifier of the project for which to retrieve node instances.</param>
        /// <param name="windowsUser">The Windows user context under which the operation is performed. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of 
        /// <see cref="NodeInstanceDto"/> objects representing the node instances for the specified project.</returns>
        Task<IEnumerable<NodeInstanceDto>> ListAsync(Guid projectId, string windowsUser);

        /// <summary>
        /// Retrieves a node instance associated with the specified project and node IDs for the given Windows user.
        /// </summary>
        /// <remarks>This method performs a lookup for a node instance based on the provided project and
        /// node IDs. Ensure that the <paramref name="windowsUser"/> has the necessary permissions to access the
        /// node.</remarks>
        /// <param name="projectId">The unique identifier of the project to which the node belongs.</param>
        /// <param name="nodeId">The unique identifier of the node to retrieve.</param>
        /// <param name="windowsUser">The Windows username for which the node instance is being retrieved. This value cannot be null or empty.</param>
        /// <returns>A <see cref="NodeInstanceDto"/> representing the node instance if found; otherwise, <see langword="null"/>.</returns>
        Task<NodeInstanceDto?> GetAsync(Guid projectId, Guid nodeId, string windowsUser);

        /// <summary>
        /// Creates a new node instance within the specified project.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project in which the node instance will be created.</param>
        /// <param name="windowsUser">The Windows user associated with the creation of the node instance. Cannot be null or empty.</param>
        /// <param name="req">The request object containing the details of the node instance to be created. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="NodeInstanceDto"/>
        /// representing the created node instance.</returns>
        Task<NodeInstanceDto> CreateAsync(Guid projectId, string windowsUser, CreateNodeInstanceRequest req);

        /// <summary>
        /// Updates the node instance associated with the specified project.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project containing the node instance to update.</param>
        /// <param name="windowsUser">The Windows user performing the update. This value cannot be null or empty.</param>
        /// <param name="req">The request object containing the details of the update operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the update
        /// was successful; otherwise, <see langword="false"/>.</returns>
        Task<bool> UpdateAsync(Guid projectId, string windowsUser, UpdateNodeInstanceRequest req);

        /// <summary>
        /// Deletes a node associated with the specified project.
        /// </summary>
        /// <remarks>Ensure that the caller has the necessary permissions to delete the node. The
        /// operation may fail if the node is locked or if the user lacks sufficient privileges.</remarks>
        /// <param name="projectId">The unique identifier of the project containing the node to delete.</param>
        /// <param name="nodeId">The unique identifier of the node to delete.</param>
        /// <param name="windowsUser">The Windows user performing the deletion. This value cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the node was
        /// successfully deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeleteAsync(Guid projectId, Guid nodeId, string windowsUser);
    }
}
