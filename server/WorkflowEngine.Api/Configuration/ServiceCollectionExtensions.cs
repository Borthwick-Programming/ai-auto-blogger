using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using WorkflowEngine.Domain.Models;
using WorkflowEngine.Runtime.Interfaces;
using WorkflowEngine.Runtime.Services;
using static System.Net.Mime.MediaTypeNames;

namespace WorkflowEngine.Api.Configuration
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Registers an in-memory <see cref="INodeRegistry"/> implementation and
        /// loads all node definitions from JSON files in the “Configuration/NodeDefinitions” folder.
        /// </summary>
        /// <param name="services">The DI service collection.</param>
        /// <param name="config">
        /// The application configuration (can be used to customize the definitions folder path).
        /// </param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddNodeDefinitions(
            this IServiceCollection services, IConfiguration config, IHostEnvironment env)
        {
            // Load definitions first
            var folder = Path.Combine(env.ContentRootPath, "Configuration", "NodeDefinitions");
            var registry = new InMemoryNodeRegistry();

            foreach (var file in Directory.EnumerateFiles(folder, "*.json"))
            {
                var json = File.ReadAllText(file);
                var def = JsonSerializer.Deserialize<ExtendedNodeDefinition>(json,
                             new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                          ?? throw new InvalidOperationException($"Could not parse {file}");
                registry.Register(def.ToNodeDefinition());
            }

            // Register the populated instance
            services.AddSingleton<INodeRegistry>(registry);
            return services;
        }
    }

}
