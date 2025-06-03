using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Core.Models;

namespace WorkflowEngine.Core.Interfaces
{
    /// <summary>
    /// Defines use-case operations for managing workflow projects;
    /// Provides methods for managing projects, including listing, retrieving, creating, and deleting projects.
    /// </summary>
    /// <remarks>This service is designed to handle project-related operations for a specific user, identified
    /// by their Windows username.</remarks>
    public interface IProjectService
    {
        /// <summary>
        /// Retrieves a list of projects associated with the specified Windows user.
        /// </summary>
        /// <param name="windowsUserName">The Windows username for which to retrieve the associated projects.  This value cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains  an enumerable collection of
        /// <see cref="ProjectDto"/> objects representing the projects  associated with the specified user. If no
        /// projects are found, the collection will be empty.</returns>
        Task<IEnumerable<ProjectDto>> ListAsync(string windowsUserName);

        /// <summary>
        /// Retrieves a project by its unique identifier and the associated Windows user name.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to retrieve project details. Ensure
        /// that the caller has appropriate permissions to access the project associated with the provided <paramref
        /// name="windowsUserName"/>.</remarks>
        /// <param name="id">The unique identifier of the project to retrieve.</param>
        /// <param name="windowsUserName">The Windows user name associated with the project.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the project data as a <see
        /// cref="ProjectDto"/> if found; otherwise, <see langword="null"/>.</returns>
        Task<ProjectDto?> GetAsync(Guid id, string windowsUserName);

        /// <summary>
        /// Creates a new project asynchronously based on the specified request.
        /// </summary>
        /// <param name="windowsUserName">The Windows username of the user creating the project. Cannot be null or empty.</param>
        /// <param name="req">The request object containing the details of the project to be created. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ProjectDto"/>
        /// representing the newly created project.</returns>
        Task<ProjectDto> CreateAsync(string windowsUserName, CreateProjectRequest req);

        /// <summary>
        /// Deletes the specified entity by its unique identifier.
        /// </summary>
        /// <remarks>Ensure that the caller has the necessary permissions to perform the delete
        /// operation.</remarks>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <param name="windowsUserName">The name of the Windows user performing the operation. This value cannot be null or empty.</param>
        /// <returns><see langword="true"/> if the entity was successfully deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeleteAsync(Guid id, string windowsUserName);
    }
}
