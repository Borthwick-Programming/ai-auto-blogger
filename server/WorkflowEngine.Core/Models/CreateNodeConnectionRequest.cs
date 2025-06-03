using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Core.Models
{
    /// <summary>
    /// Represents a request to create a connection between two nodes in a system.
    /// </summary>
    /// <param name="FromNodeInstanceId">The unique identifier of the source node instance.</param>
    /// <param name="FromPortName">The name of the port on the source node to establish the connection from.</param>
    /// <param name="ToNodeInstanceId">The unique identifier of the target node instance.</param>
    /// <param name="ToPortName">The name of the port on the target node to establish the connection to.</param>
    public record CreateNodeConnectionRequest
        (
          Guid FromNodeInstanceId,
          string FromPortName,
          Guid ToNodeInstanceId,
          string ToPortName
        );
}
