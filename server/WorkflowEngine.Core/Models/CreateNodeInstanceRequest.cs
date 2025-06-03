using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Core.Models
{
    /// <summary>
    /// Represents a request to create a new node instance with specified configuration and position.
    /// </summary>
    /// <param name="NodeTypeId">The unique identifier of the node type to be instantiated. This value cannot be null or empty.</param>
    /// <param name="ConfigurationJson">A JSON string containing the configuration settings for the node instance. This value cannot be null or empty.</param>
    /// <param name="PositionX">The X-coordinate of the node's position in the layout. Must be a valid double value.</param>
    /// <param name="PositionY">The Y-coordinate of the node's position in the layout. Must be a valid double value.</param>
    public record CreateNodeInstanceRequest
        (
          string NodeTypeId,
          string ConfigurationJson,
          double PositionX,
          double PositionY
        );
}
