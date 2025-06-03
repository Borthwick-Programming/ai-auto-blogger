using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Core.Models
{
    /// <summary>
    /// Represents a request to update a connection between two nodes in a system.
    /// </summary>
    /// <remarks>This request specifies the details of the connection to be updated, including the unique
    /// identifiers of the nodes and the ports involved in the connection.</remarks>
    /// <param name="Id">The unique identifier of the connection to be updated.</param>
    /// <param name="FromNodeInstanceId">The unique identifier of the source node in the connection.</param>
    /// <param name="FromPortName">The name of the port on the source node that is part of the connection.</param>
    /// <param name="ToNodeInstanceId">The unique identifier of the target node in the connection.</param>
    /// <param name="ToPortName">The name of the port on the target node that is part of the connection.</param>
    public record UpdateNodeConnectionRequest
        (
          Guid Id,
          Guid FromNodeInstanceId,
          string FromPortName,
          Guid ToNodeInstanceId,
          string ToPortName
        );
}
