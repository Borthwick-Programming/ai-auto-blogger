using Microsoft.Extensions.DependencyInjection;
using WorkflowEngine.Domain.Models;
using WorkflowEngine.Runtime.Interfaces;
using WorkflowEngine.Runtime.Services;
namespace WorkflowEngine.Api.Configuration
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the in-memory node registry and all core node definitions.
        /// </summary>
        public static IServiceCollection AddNodeRegistry(this IServiceCollection services)
        {
            var registry = new InMemoryNodeRegistry();
            string basePath = AppContext.BaseDirectory;
            string schemaPath = Path.Combine(basePath, "Configuration", "NodeSchemas");

            string httpSchema = File.ReadAllText(Path.Combine(schemaPath, "http-request.schema.json"));
            string conditionalSchema = File.ReadAllText(Path.Combine(schemaPath, "conditional-branch.schema.json"));


            // Core Node: HTTP Request
            NodeDefinition httpNodeDef = new NodeDefinition
                (
                     Id: "http-request"
                   , Name: "HTTP Request"
                   , Description: "Calls an external API."
                   , NodeType: "http-request"
                   , ConfigurationSchemaJson: httpSchema
                   , Inputs: new() { }
                   , Outputs: new() { new PortDefinition("response", "object") }
                );
            registry.Register(httpNodeDef);

            // Core Node: Conditional Branch
            NodeDefinition conditionalBranchDef = new NodeDefinition
                (
                     Id: "conditional-branch"
                   , Name: "Conditional Branch"
                   , Description: "Routes data to one of two paths based on a condition."
                   , NodeType: "conditional-branch"
                   , ConfigurationSchemaJson: conditionalSchema
                   , Inputs: new() { new PortDefinition("input", "object") }
                   , Outputs : new() { new PortDefinition("output", "object" )}
                );
            registry.Register(conditionalBranchDef);

            services.AddSingleton<INodeRegistry>(registry);
            return services;
        }
    }
}
