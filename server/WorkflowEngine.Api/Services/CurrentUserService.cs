using WorkflowEngine.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WorkflowEngine.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string Username { get; }
        public CurrentUserService(
        IHttpContextAccessor httpCtx,
        IWebHostEnvironment env,
        IConfiguration config)
        {
            var name = httpCtx.HttpContext?.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(name))
            {
                Username = name;
            }
            else if (env.IsDevelopment())
            {
                // read from appsettings.Development.json
                Username = config.GetValue<string>("DevAuthentication:FallbackUsername")!;
            }
            else
            {
                throw new InvalidOperationException("User must be authenticated");
            }
            Console.WriteLine($"(CurrentUserService) Current dev user is: {Username}");
        }
    }
}
