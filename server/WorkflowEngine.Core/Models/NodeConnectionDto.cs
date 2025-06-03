using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Core.Models
{
    /// <summary>
    /// Represents a connection between two nodes, including the source and destination node instances and their
    /// respective ports.
    /// </summary>
    /// <remarks>This data transfer object (DTO) is used to describe the relationship between two nodes in a
    /// system,  specifying the originating and target node instances and the ports involved in the
    /// connection.</remarks>
    /// <param name="Id"></param>
    /// <param name="FromNodeInstanceId"></param>
    /// <param name="FromPortName"></param>
    /// <param name="ToNodeInstanceId"></param>
    /// <param name="ToPortName"></param>
    public record NodeConnectionDto
        (
          Guid Id,
          Guid FromNodeInstanceId,
          string FromPortName,
          Guid ToNodeInstanceId,
          string ToPortName
        );
}
