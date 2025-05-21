namespace WorkflowEngine.Api.Models
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!; //Can ignore the possible null assignment warning
    }
}
