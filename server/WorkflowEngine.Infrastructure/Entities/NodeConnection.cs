using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Infrastructure.Entities
{
    /// <summary>
    /// Represents a connection; a directed edge connecting two node instances in a workflow, including the source and destination
    /// </summary>
    public class NodeConnection
    {
        public Guid Id { get; set; }

        // Source node instance and port
        public Guid FromNodeInstanceId { get; set; }
        public string FromPortName { get; set; } = default!;

        // Destination node instance and port
        public Guid ToNodeInstanceId { get; set; }
        public string ToPortName { get; set; } = default!;

        // Navigation properties
        public NodeInstance FromNode { get; set; } = default!;
        public NodeInstance ToNode { get; set; } = default!;
    }
}
