namespace WorkflowEngine.Core.Models
{
    /// <summary>
    /// Represents a request to create a new project.
    /// </summary>
    /// <remarks>This class is used to encapsulate the data required to create a project.  The <see
    /// cref="Name"/> property must be provided and cannot be null or empty.</remarks>
    public class CreateProjectRequest
    {
        public string Name { get; set; } = default!; //Can ignore the possible null assignment warning
    }
}
