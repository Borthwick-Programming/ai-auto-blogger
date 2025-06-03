using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Infrastructure.Entities
{
    /// <summary>
    /// Represents an instance of a node within a project, including its type, configuration, and position on a canvas.
    /// </summary>
    /// <remarks>A <see cref="NodeInstance"/> is associated with a specific project and is identified by a
    /// unique ID.  It includes metadata such as the node type, configuration settings, and its position on a
    /// canvas.</remarks>
    public class NodeInstance
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = default!;

        // Type identifier from your Domain.NodeDefinition.Id
        public string NodeTypeId { get; set; } = default!;

        // JSON blob storing configured properties
        public string ConfigurationJson { get; set; } = default!;

        // Canvas positioning
        public double PositionX { get; set; }
        public double PositionY { get; set; }

        public ICollection<NodeConnection> OutgoingConnections { get; set; } = new List<NodeConnection>();
        public ICollection<NodeConnection> IncomingConnections { get; set; } = new List<NodeConnection>();
    }
}
