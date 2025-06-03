using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Domain.Models
{
    /// <summary>
    /// Describes a single input or output port for a node, including name and data type.
    /// These are used to connect nodes together in a typed workflow graph.
    /// BTW, instead of writing out properties, constructor, Equals/GetHashCode, and ToString as a class reference type would need, 
    /// in a  'record' reference type, one can declare everything in one line. This was added as of C# 9.0
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Type"></param>
    public record PortDefinition(string Name, string Type); // e.g., "string", "object", "number"
}
