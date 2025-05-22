using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Infrastructure.Entities
{
    /// <summary>
    /// Represents a user in the system, including their unique identifier, username, and associated projects.
    /// </summary>
    /// <remarks>This class is used to model a user entity, including their credentials and the projects they
    /// are associated with. The <see cref="PasswordHash"/> property is nullable to support scenarios where password
    /// information is not required, like when using AD in localhost and the currently logged in user does not need to authenticate.</remarks>
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = default!;
        public string? PasswordHash { get; set; } 
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
