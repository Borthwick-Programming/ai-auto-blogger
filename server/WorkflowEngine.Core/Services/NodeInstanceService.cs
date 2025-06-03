using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Core.Interfaces;
using WorkflowEngine.Core.Models;
using WorkflowEngine.Infrastructure.Data;
using WorkflowEngine.Infrastructure.Entities;

namespace WorkflowEngine.Core.Services
{
    /// <summary>
    /// Provides functionality for managing node instances within a project, including creating, retrieving, updating,
    /// and deleting node instances. Access to these operations is restricted to users who own the project.
    /// </summary>
    /// <remarks>This service ensures that only authorized users can perform operations on node instances by
    /// verifying project ownership. It interacts with the database to persist changes and retrieve data, and it relies
    /// on the <see cref="IProjectService"/> to validate user access to projects.</remarks>
    public class NodeInstanceService : INodeInstanceService
    {
        private readonly WorkflowEngineDbContext _db;
        private readonly IProjectService _projectService;

        /// <summary>
        /// Determines whether the specified user owns the project with the given identifier.
        /// </summary>
        /// <remarks>This method checks if the project exists and is associated with the specified
        /// user.</remarks>
        /// <param name="projectId">The unique identifier of the project to check.</param>
        /// <param name="user">The user identifier to verify ownership for.</param>
        /// <returns><see langword="true"/> if the user owns the project; otherwise, <see langword="false"/>.</returns>
        private async Task<bool> UserOwnsProject(Guid projectId, string user)
        {
            var proj = await _projectService.GetAsync(projectId, user);
            return proj is not null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeInstanceService"/> class.
        /// </summary>
        /// <param name="db">The database context used to interact with the workflow engine's data store. Cannot be null.</param>
        /// <param name="projectService">The service used to manage project-related operations. Cannot be null.</param>
        public NodeInstanceService(WorkflowEngineDbContext db, IProjectService projectService)
        {
            _db = db;
            _projectService = projectService;
        }

        /// <summary>
        /// Creates a new node instance within the specified project.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project to which the node instance will be added.</param>
        /// <param name="windowsUser">The Windows username of the user attempting to create the node instance. The user must own the project.</param>
        /// <param name="req">The request object containing the details of the node instance to be created, including its type,
        /// configuration, and position.</param>
        /// <returns>A <see cref="NodeInstanceDto"/> representing the newly created node instance, including its unique
        /// identifier and associated properties.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the specified <paramref name="windowsUser"/> does not have ownership of the project identified by
        /// <paramref name="projectId"/>.</exception>
        public async Task<NodeInstanceDto> CreateAsync(Guid projectId, string windowsUser, CreateNodeInstanceRequest req)
        {
            if (!await UserOwnsProject(projectId, windowsUser))
                throw new UnauthorizedAccessException();

            var n = new NodeInstance
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                NodeTypeId = req.NodeTypeId,
                ConfigurationJson = req.ConfigurationJson,
                PositionX = req.PositionX,
                PositionY = req.PositionY
            };
            _db.NodeInstances.Add(n);
            await _db.SaveChangesAsync();
            return new NodeInstanceDto(n.Id, n.ProjectId, n.NodeTypeId, n.ConfigurationJson, n.PositionX, n.PositionY);
        }

        /// <summary>
        /// Deletes a node instance associated with the specified project and user.
        /// </summary>
        /// <remarks>This method ensures that the user has ownership of the project before attempting to
        /// delete the node instance. If the node instance does not exist, the method returns <see langword="false"/>
        /// without performing any deletion.</remarks>
        /// <param name="projectId">The unique identifier of the project to which the node belongs.</param>
        /// <param name="nodeId">The unique identifier of the node instance to delete.</param>
        /// <param name="windowsUser">The Windows username of the user attempting the deletion.</param>
        /// <returns><see langword="true"/> if the node instance was successfully deleted;  otherwise, <see langword="false"/> if
        /// the node instance does not exist.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the specified user does not have ownership of the project.</exception>
        public async Task<bool> DeleteAsync(Guid projectId, Guid nodeId, string windowsUser)
        {
            if (!await UserOwnsProject(projectId, windowsUser))
                throw new UnauthorizedAccessException();

            var n = await _db.NodeInstances
                .SingleOrDefaultAsync(x => x.Id == nodeId && x.ProjectId == projectId);
            if (n is null) return false;

            _db.NodeInstances.Remove(n);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves a node instance associated with the specified project and node identifiers.
        /// </summary>
        /// <remarks>This method checks whether the specified user owns the project before attempting to
        /// retrieve the node instance. If the user does not own the project, an <see
        /// cref="UnauthorizedAccessException"/> is thrown.</remarks>
        /// <param name="projectId">The unique identifier of the project to which the node belongs.</param>
        /// <param name="nodeId">The unique identifier of the node instance to retrieve.</param>
        /// <param name="windowsUser">The Windows username of the user making the request. This is used to verify project ownership.</param>
        /// <returns>A <see cref="NodeInstanceDto"/> representing the node instance if found; otherwise, <see langword="null"/>
        /// if no matching node instance exists.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the specified <paramref name="windowsUser"/> does not have ownership of the project identified by
        /// <paramref name="projectId"/>.</exception>
        public async Task<NodeInstanceDto?> GetAsync(Guid projectId, Guid nodeId, string windowsUser)
        {
            if (!await UserOwnsProject(projectId, windowsUser))
                throw new UnauthorizedAccessException();

            var n = await _db.NodeInstances
                .SingleOrDefaultAsync(x => x.Id == nodeId && x.ProjectId == projectId);
            return n is null ? null
               : new NodeInstanceDto(n.Id, n.ProjectId, n.NodeTypeId, n.ConfigurationJson, n.PositionX, n.PositionY);
        }

        /// <summary>
        /// Retrieves a collection of node instances associated with the specified project.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project whose node instances are to be retrieved.</param>
        /// <param name="windowsUser">The Windows username of the user making the request. This is used to verify project ownership.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/>
        /// of  <see cref="NodeInstanceDto"/> objects representing the node instances associated with the specified
        /// project.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the specified <paramref name="windowsUser"/> does not have ownership of the project identified by
        /// <paramref name="projectId"/>.</exception>
        public async Task<IEnumerable<NodeInstanceDto>> ListAsync(Guid projectId, string windowsUser)
        {
            if (!await UserOwnsProject(projectId, windowsUser))
                throw new UnauthorizedAccessException();

            return await _db.NodeInstances
                .Where(n => n.ProjectId == projectId)
                .Select(n => new NodeInstanceDto(
                    n.Id, n.ProjectId, n.NodeTypeId, n.ConfigurationJson, n.PositionX, n.PositionY))
                .ToListAsync();
        }

        /// <summary>
        /// Updates the specified node instance within a project with new configuration and position data.
        /// </summary>
        /// <remarks>This method ensures that only the owner of the project can update its node instances.
        /// If the node instance does not exist or does not belong to the specified project, the method returns <see
        /// langword="false"/>.</remarks>
        /// <param name="projectId">The unique identifier of the project containing the node instance to update.</param>
        /// <param name="windowsUser">The Windows username of the user attempting the update. The user must own the project.</param>
        /// <param name="req">An object containing the updated data for the node instance, including its type, configuration, and
        /// position.</param>
        /// <returns><see langword="true"/> if the node instance was successfully updated;  otherwise, <see langword="false"/> if
        /// the node instance does not exist in the specified project.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the specified <paramref name="windowsUser"/> does not own the project identified by <paramref
        /// name="projectId"/>.</exception>
        public async Task<bool> UpdateAsync(Guid projectId, string windowsUser, UpdateNodeInstanceRequest req)
        {
            if (!await UserOwnsProject(projectId, windowsUser))
                throw new UnauthorizedAccessException();

            var n = await _db.NodeInstances
                .SingleOrDefaultAsync(x => x.Id == req.Id && x.ProjectId == projectId);
            if (n is null) return false;

            n.NodeTypeId = req.NodeTypeId;
            n.ConfigurationJson = req.ConfigurationJson;
            n.PositionX = req.PositionX;
            n.PositionY = req.PositionY;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
