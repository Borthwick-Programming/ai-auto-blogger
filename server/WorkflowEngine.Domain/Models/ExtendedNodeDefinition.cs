using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorkflowEngine.Domain.Models
{
    /// <summary>
    /// Represents an extended definition of a node, including metadata, configuration schema,  input and output ports,
    /// and optional visual information.
    /// </summary>
    /// <remarks>This class is used to define the structure and metadata of a node in a system, including its 
    /// unique identifier, name, description, type, configuration schema, and port definitions.  It also supports
    /// optional visual metadata for rendering purposes.</remarks>
    public class ExtendedNodeDefinition
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string NodeType { get; set; } = "";
        public JsonElement ConfigurationSchemaJson { get; set; } = new();
        public List<PortDefinition> Inputs { get; set; } = new();
        public List<PortDefinition> Outputs { get; set; } = new();
        public VisualMetadata? Visual { get; set; }

        /// <summary>
        /// Converts the current object to a <see cref="NodeDefinition"/> instance.
        /// </summary>
        /// <returns>A <see cref="NodeDefinition"/> object that represents the current state of the instance,  including its ID,
        /// name, description, type, configuration schema, inputs, and outputs.</returns>
        public NodeDefinition ToNodeDefinition()
        {
            return new NodeDefinition(
                Id,
                Name,
                Description,
                NodeType,
                ConfigurationSchemaJson,
                //JsonSerializer.Serialize(ConfigurationSchemaJson),
                Inputs,
                Outputs
            );
        }
    }
}
