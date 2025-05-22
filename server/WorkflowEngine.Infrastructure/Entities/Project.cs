using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Infrastructure.Entities
{
    /// <summary>
    /// Represents a project, including its unique identifier, name, owner, and associated node instances.
    /// </summary>
    /// <remarks>A project is a container for related node instances and is associated with a specific owner.
    /// The <see cref="Owner"/> property provides details about the user who owns the project.</remarks>
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = default!;
        public ICollection<NodeInstance> NodeInstances { get; set; } = new List<NodeInstance>();
    }
}
