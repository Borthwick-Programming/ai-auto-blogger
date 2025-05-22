using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using WorkflowEngine.Domain.Models;
using WorkflowEngine.Runtime.Interfaces;
using WorkflowEngine.Runtime.Services;
using Microsoft.EntityFrameworkCore;
using WorkflowEngine.Infrastructure.Data;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Authentication.Negotiate;
using WorkflowEngine.Core.Interfaces;
using WorkflowEngine.Core.Services;


namespace WorkflowEngine.Api.Configuration
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<INodeInstanceService, NodeInstanceService>();
            services.AddScoped<INodeConnectionService, NodeConnectionService>();
            return services;
        }

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
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WorkflowEngineDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // TODO: register repository interfaces 

            return services;
        }

        /// <summary>
        /// Configures Windows Authentication (Negotiate) as the default auth scheme.
        /// </summary>
        public static IServiceCollection AddAuthenticationServices(
            this IServiceCollection services, IConfiguration config)
        {
            // 1) Set Negotiate as the default scheme for both Authenticate & Challenge
            services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                    .AddNegotiate();

            // 2) Add policy support
            services.AddAuthorization();

            return services;
        }
    }

}
