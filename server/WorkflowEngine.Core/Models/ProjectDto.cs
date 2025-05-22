namespace WorkflowEngine.Core.Models
{
    /// <summary>
    /// Represents a project with an identifier and a name.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    public record ProjectDto(Guid Id, string Name);
       
}
