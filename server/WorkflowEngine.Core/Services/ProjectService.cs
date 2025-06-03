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
    /// Provides business logic for project management (CRUD / ownership checks);
    /// functionality for managing projects, including listing, retrieving, creating, and deleting projects
    /// associated with a specific user.
    /// </summary>
    /// <remarks>This service ensures that all operations are scoped to the user identified by their Windows
    /// username. If the user does not already exist in the system, they will be automatically created during the
    /// operation.</remarks>
    public class ProjectService : IProjectService
    {
        private readonly WorkflowEngineDbContext _db;
        public ProjectService(WorkflowEngineDbContext db) => _db = db;

        /// <summary>
        /// Retrieves an existing user by their Windows username or creates a new user if none exists.
        /// </summary>
        /// <remarks>If a user with the specified Windows username does not exist in the database, a new
        /// user is created, added to the database, and saved. The method ensures that a valid user is always
        /// returned.</remarks>
        /// <param name="windowsName">The Windows username of the user to retrieve or create. Cannot be null or empty.</param>
        /// <returns>A <see cref="User"/> object representing the existing or newly created user.</returns>
        private async Task<User> GetOrCreateUser(string windowsName)
        {
            windowsName = windowsName.ToLower().Trim();
            var existing = await _db.Users
                .Where(u => u.Username.ToLower() == windowsName)
                .FirstOrDefaultAsync();
            if (existing != null) return existing;
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = windowsName,
            };

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            return newUser;

        }

        /// <summary>
        /// Asynchronously retrieves a list of projects owned by the specified user.
        /// </summary>
        /// <remarks>This method ensures that the user exists in the system by creating a user record if
        /// one does not already exist.</remarks>
        /// <param name="windowsUserName">The Windows username of the user whose projects are to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
        /// cref="ProjectDto"/> objects,  each representing a project owned by the specified user. The collection will
        /// be empty if the user owns no projects.</returns>
        public async Task<IEnumerable<ProjectDto>> ListAsync(string windowsUserName)
        {
            var u = await GetOrCreateUser(windowsUserName);
            return await _db.Projects
                .Where(p => p.OwnerId == u.Id)
                .Select(p => new ProjectDto(p.Id, p.Name))
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a project associated with the specified ID and the given Windows user.
        /// </summary>
        /// <remarks>This method ensures that the project belongs to the user associated with the
        /// specified Windows username. If the user does not exist, it will be created automatically.</remarks>
        /// <param name="id">The unique identifier of the project to retrieve.</param>
        /// <param name="windowsUserName">The Windows username of the user requesting the project.</param>
        /// <returns>A <see cref="ProjectDto"/> representing the project if found and owned by the user;  otherwise, <see
        /// langword="null"/>.</returns>
        public async Task<ProjectDto?> GetAsync(Guid id, string windowsUserName)
        {
            var u = await GetOrCreateUser(windowsUserName);
            var p = await _db.Projects
                         .Where(x => x.Id == id && x.OwnerId == u.Id)
                         .SingleOrDefaultAsync();
            return p is null ? null : new ProjectDto(p.Id, p.Name);
        }

        /// <summary>
        /// Creates a new project and associates it with the specified user.
        /// </summary>
        /// <remarks>This method ensures that the specified user exists in the system before creating the
        /// project.  If the user does not exist, it will be created automatically. The project is then persisted in the
        /// database.</remarks>
        /// <param name="windowsUserName">The Windows username of the user who will own the project. This cannot be null or empty.</param>
        /// <param name="req">The request containing the details of the project to be created. This cannot be null.</param>
        /// <returns>A <see cref="ProjectDto"/> representing the newly created project, including its ID and name.</returns>
        public async Task<ProjectDto> CreateAsync(string windowsUserName, CreateProjectRequest req)
        {
            var u = await GetOrCreateUser(windowsUserName);
            var p = new Project { Id = Guid.NewGuid(), Name = req.Name, OwnerId = u.Id };
            _db.Projects.Add(p);
            await _db.SaveChangesAsync();
            return new ProjectDto(p.Id, p.Name);
        }

        /// <summary>
        /// Deletes the project with the specified identifier if it exists and is owned by the specified user.
        /// </summary>
        /// <remarks>The method ensures that the project is owned by the specified user before attempting
        /// to delete it. If no matching project is found, the method returns <see langword="false"/> without making any
        /// changes.</remarks>
        /// <param name="id">The unique identifier of the project to delete.</param>
        /// <param name="windowsUserName">The Windows username of the user attempting to delete the project.</param>
        /// <returns><see langword="true"/> if the project was successfully deleted; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> DeleteAsync(Guid id, string windowsUserName)
        {
            var u = await GetOrCreateUser(windowsUserName);
            var p = await _db.Projects.SingleOrDefaultAsync(x => x.Id == id && x.OwnerId == u.Id);
            if (p == null) return false;

            _db.Projects.Remove(p);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
