using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
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
            string nodePath = Path.Combine(AppContext.BaseDirectory, "Configuration", "NodeDefinitions");
            string[] files = Directory.GetFiles(nodePath, "*.json");
            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var extNode = JsonSerializer.Deserialize<ExtendedNodeDefinition>(json);

                if (extNode != null)
                {
                    registry.Register(extNode.ToNodeDefinition());
                }
            }

            return services;
        }
    }
}
