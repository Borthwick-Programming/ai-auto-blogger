using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorkflowEngine.Domain.Models
{
    public class ExtendedNodeDefinition
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string NodeType { get; set; } = "";
        public object ConfigurationSchemaJson { get; set; } = new();
        public List<PortDefinition> Inputs { get; set; } = new();
        public List<PortDefinition> Outputs { get; set; } = new();
        public VisualMetadata? Visual { get; set; }

        public NodeDefinition ToNodeDefinition()
        {
            return new NodeDefinition(
                Id,
                Name,
                Description,
                NodeType,
                JsonSerializer.Serialize(ConfigurationSchemaJson),
                Inputs,
                Outputs
            );
        }
    }
}
