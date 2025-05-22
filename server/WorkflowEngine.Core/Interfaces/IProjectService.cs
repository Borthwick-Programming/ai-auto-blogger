using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Core.Models;

namespace WorkflowEngine.Core.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> ListAsync(string windowsUserName);
        Task<ProjectDto?> GetAsync(Guid id, string windowsUserName);
        Task<ProjectDto> CreateAsync(string windowsUserName, CreateProjectRequest req);
        Task<bool> DeleteAsync(Guid id, string windowsUserName);
    }
}
