using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Infrastructure.Entities
{
/// <summary>
/// Represents a user within the system, including their identity, authentication provider, and associated projects.
/// </summary>
/// <remarks>A user is uniquely identified by their <see cref="Id"/> and is associated with an external
/// authentication provider. The <see cref="Projects"/> property contains the list of projects that the user is
/// associated with.</remarks>
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = default!;
        public string Provider { get; set; } = default!; // e.g. "google"
        public string ProviderId { get; set; } = default!; // e.g. Google’s "sub" claim
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
