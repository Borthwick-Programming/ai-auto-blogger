using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Infrastructure.Entities
{
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
    }
}
