using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Core.Models
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a node instance within a project.
    /// </summary>
    /// <remarks>This DTO encapsulates the essential information about a node instance, including its unique
    /// identifier, associated project, node type, configuration, and position within a coordinate system.</remarks>
    /// <param name="Id"></param>
    /// <param name="ProjectId"></param>
    /// <param name="NodeTypeId"></param>
    /// <param name="ConfigurationJson"></param>
    /// <param name="PositionX"></param>
    /// <param name="PositionY"></param>
    public record NodeInstanceDto(
    Guid Id,
    Guid ProjectId,
    string NodeTypeId,
    string ConfigurationJson,
    double PositionX,
    double PositionY
);
}
