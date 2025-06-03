using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Core.Models
{
    /// <summary>
    /// Represents a request to update the properties of a node instance in a system.
    /// </summary>
    /// <remarks>This request is used to modify the configuration and position of an existing node instance.
    /// The node instance is identified by its unique <see cref="Id"/>.</remarks>
    /// <param name="Id">The unique identifier of the node instance to be updated.</param>
    /// <param name="NodeTypeId">The identifier of the node type associated with the node instance. This value cannot be null or empty.</param>
    /// <param name="ConfigurationJson">A JSON string representing the updated configuration for the node instance.  The format and structure of the
    /// JSON must conform to the expected schema for the node type.</param>
    /// <param name="PositionX">The updated X-coordinate of the node instance's position in the system.</param>
    /// <param name="PositionY">The updated Y-coordinate of the node instance's position in the system.</param>
    public record UpdateNodeInstanceRequest
        (
          Guid Id,
          string NodeTypeId,
          string ConfigurationJson,
          double PositionX,
          double PositionY
        );
}
