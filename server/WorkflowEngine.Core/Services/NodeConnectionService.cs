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
    /// Provides methods for managing node connections within a project, including creating, updating,  retrieving,
    /// listing, and deleting connections between nodes.
    /// </summary>
    /// <remarks>This service ensures that only authorized users with ownership of the project can perform
    /// operations  on node connections. It interacts with the underlying database to persist and retrieve connection
    /// data.</remarks>
    public class NodeConnectionService : INodeConnectionService
    {
        private readonly WorkflowEngineDbContext _db;
        private readonly IProjectService _projects;
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConnectionService"/> class, providing access to the
        /// workflow engine database and project services.
        /// </summary>
        /// <param name="db">The database context used to interact with the workflow engine's data store. Cannot be null.</param>
        /// <param name="projects">The project service used to manage project-related operations. Cannot be null.</param>
        public NodeConnectionService(WorkflowEngineDbContext db, IProjectService projects)
        {
            _db = db;
            _projects = projects;
        }

        /// <summary>
        /// Ensures that the specified user has ownership of the project with the given identifier.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project to check ownership for.</param>
        /// <param name="user">The user whose ownership is being verified.</param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the specified user does not have ownership of the project.</exception>
        private async Task EnsureOwnership(Guid projectId, string user)
        {
            if (await _projects.GetAsync(projectId, user) == null)
                throw new UnauthorizedAccessException();
        }
        /// <summary>
        /// Ensures that the specified connection is owned by the given user within the specified project.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project to which the connection belongs.</param>
        /// <param name="connectionId">The unique identifier of the connection to verify ownership for.</param>
        /// <param name="user">The user whose ownership of the connection is being validated.</param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the specified connection does not exist or the user does not have ownership of the connection.</exception>
        private async Task EnsureConnectionOwnership(Guid projectId, Guid connectionId, string user)
        {
            var connection = await GetAsync(projectId, connectionId, user);
            if (connection == null)
                throw new UnauthorizedAccessException();
        }

        /// <summary>
        /// Creates a new node connection for the specified project.
        /// </summary>
        /// <remarks>This method ensures that the specified user has ownership of the project before
        /// creating the node connection. The created connection is persisted to the database and returned as a data
        /// transfer object.</remarks>
        /// <param name="projectId">The unique identifier of the project to which the node connection belongs.</param>
        /// <param name="windowsUser">The Windows user initiating the request. This is used to verify ownership of the project.</param>
        /// <param name="req">The request object containing details of the node connection to be created, including source and destination
        /// nodes and ports.</param>
        /// <returns>A <see cref="NodeConnectionDto"/> representing the newly created node connection, including its unique
        /// identifier and connection details.</returns>
        public async Task<NodeConnectionDto> CreateAsync(Guid projectId, string windowsUser, CreateNodeConnectionRequest req)
        {
            await EnsureOwnership(projectId, windowsUser);
            var conn = new NodeConnection
            {
                Id = Guid.NewGuid(),
                FromNodeInstanceId = req.FromNodeInstanceId,
                FromPortName = req.FromPortName,
                ToNodeInstanceId = req.ToNodeInstanceId,
                ToPortName = req.ToPortName
            };
            _db.NodeConnections.Add(conn);
            await _db.SaveChangesAsync();
            return new NodeConnectionDto(conn.Id, conn.FromNodeInstanceId, conn.FromPortName,
                                         conn.ToNodeInstanceId, conn.ToPortName);
        }

        /// <summary>
        /// Deletes a node connection associated with the specified connection ID if the user has ownership of the
        /// project.
        /// </summary>
        /// <remarks>This method ensures that the user has ownership of the project before attempting to
        /// delete the connection.  If the connection does not exist, the method returns <see langword="false"/> without
        /// making any changes.</remarks>
        /// <param name="projectId">The unique identifier of the project to which the connection belongs.</param>
        /// <param name="connectionId">The unique identifier of the connection to be deleted.</param>
        /// <param name="windowsUser">The Windows username of the user attempting the deletion. This is used to verify ownership of the project.</param>
        /// <returns><see langword="true"/> if the connection was successfully deleted; otherwise, <see langword="false"/> if the
        /// connection was not found.</returns>
        public async Task<bool> DeleteAsync(Guid projectId, Guid connectionId, string windowsUser)
        {
            await EnsureOwnership(projectId, windowsUser);
            var c = await _db.NodeConnections.FindAsync(connectionId);
            if (c == null) return false;
            _db.NodeConnections.Remove(c);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves a node connection by its identifier within a specified project.
        /// </summary>
        /// <remarks>This method ensures that the specified user has ownership of the project before
        /// attempting to retrieve the node connection. If the connection does not exist, the method returns <see
        /// langword="null"/>.</remarks>
        /// <param name="projectId">The unique identifier of the project to which the connection belongs.</param>
        /// <param name="connectionId">The unique identifier of the node connection to retrieve.</param>
        /// <param name="windowsUser">The Windows user requesting the operation. Used to verify ownership of the project.</param>
        /// <returns>A <see cref="NodeConnectionDto"/> representing the node connection if found; otherwise, <see langword="null"/>.
        /// </returns>
        public async Task<NodeConnectionDto?> GetAsync(Guid projectId, Guid connectionId, string windowsUser)
        {
            await EnsureOwnership(projectId, windowsUser);
            var c = await _db.NodeConnections.FindAsync(connectionId);
            return c is null ? null :
                new NodeConnectionDto(c.Id, c.FromNodeInstanceId, c.FromPortName,
                                      c.ToNodeInstanceId, c.ToPortName);
        }

        /// <summary>
        /// Retrieves a list of node connections associated with the specified project.
        /// </summary>
        /// <remarks>This method ensures that the specified user has ownership of the project before
        /// retrieving the node connections.</remarks>
        /// <param name="projectId">The unique identifier of the project for which to retrieve node connections.</param>
        /// <param name="windowsUser">The Windows user requesting the node connections. This is used to verify ownership of the project.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of 
        /// <see cref="NodeConnectionDto"/> objects representing the node connections for the specified project.</returns>
        public async Task<IEnumerable<NodeConnectionDto>> ListAsync(Guid projectId, string windowsUser)
        {
            await EnsureOwnership(projectId, windowsUser);
            return await _db.NodeConnections
                .Where(c => c.FromNode.ProjectId == projectId)
                .Select(c => new NodeConnectionDto(c.Id, c.FromNodeInstanceId, c.FromPortName,
                                                   c.ToNodeInstanceId, c.ToPortName))
                .ToListAsync();
        }

        /// <summary>
        /// Updates an existing node connection with the specified details.
        /// </summary>
        /// <remarks>This method ensures that the caller has ownership of the project before attempting to
        /// update the node connection.  If the specified connection does not exist, no changes are made, and the method
        /// returns <see langword="false"/>.</remarks>
        /// <param name="projectId">The unique identifier of the project to which the node connection belongs.</param>
        /// <param name="windowsUser">The Windows user performing the update operation. This is used to verify ownership of the project.</param>
        /// <param name="req">An object containing the updated details of the node connection, including its identifier and connection
        /// endpoints.</param>
        /// <returns><see langword="true"/> if the node connection was successfully updated; otherwise, <see langword="false"/>
        /// if the specified connection does not exist.</returns>
        public async Task<bool> UpdateAsync(Guid projectId, string windowsUser, UpdateNodeConnectionRequest req)
        {
            await EnsureOwnership(projectId, windowsUser);
            var c = await _db.NodeConnections.FindAsync(req.Id);
            if (c == null) return false;
            c.FromNodeInstanceId = req.FromNodeInstanceId;
            c.FromPortName = req.FromPortName;
            c.ToNodeInstanceId = req.ToNodeInstanceId;
            c.ToPortName = req.ToPortName;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
