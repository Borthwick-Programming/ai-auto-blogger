using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Infrastructure.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = default!;
        public ICollection<NodeInstance> NodeInstances { get; set; } = new List<NodeInstance>();
    }
}
