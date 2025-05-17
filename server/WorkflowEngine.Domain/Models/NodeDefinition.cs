using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Domain.Models
{
    /// <summary>
    /// Describes a node's identity, purpose, configuration schema, and I/O ports.
    /// This is the blueprint for what a node is — not a runtime instance.
    /// </summary>
    /// <param name="Id">Unique string identifier (e.g., "http-request")</param>
    /// <param name="Name">Display name (shown in UI)</param>
    /// <param name="Description">What the node does (for documentation)</param>
    /// <param name="NodeType">Category/type (e.g., "action", "transformer")</param>
    /// <param name="ConfigurationSchemaJson">JSON Schema for validating user configuration</param>
    /// <param name="Inputs">Expected input port definitions</param>
    /// <param name="Outputs">Expected output port definitions</param>
    public record NodeDefinition(
    string Id,
    string Name,
    string Description,
    string NodeType,
    string ConfigurationSchemaJson,
    List<PortDefinition> Inputs,
    List<PortDefinition> Outputs
);
}
