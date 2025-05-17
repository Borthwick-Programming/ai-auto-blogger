using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Domain.Models
{
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
