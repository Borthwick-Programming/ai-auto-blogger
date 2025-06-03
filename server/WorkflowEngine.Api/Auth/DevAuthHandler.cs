using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
namespace WorkflowEngine.Api.Auth
{
    /// <summary>
    /// A development‐only authentication handler that auto‐signs in as the configured dev user.
    /// </summary>
    public class DevAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="DevAuthHandler"/> class.
        /// </summary>
        /// <param name="options">The monitor for <see cref="AuthenticationSchemeOptions"/> used to configure the authentication scheme.</param>
        /// <param name="logger">The factory used to create logger instances for logging operations.</param>
        /// <param name="encoder">The <see cref="UrlEncoder"/> used to encode URLs as part of the authentication process.</param>
        public DevAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IConfiguration configuration
        ) : base(options, logger, encoder)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Handles the authentication process for the current request.
        /// </summary>
        /// <remarks>This method creates an authentication ticket for a predefined development user and
        /// returns a successful authentication result. It is typically used in scenarios where a custom authentication
        /// scheme is implemented.</remarks>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The result contains an <see
        /// cref="AuthenticateResult"/>  indicating a successful authentication with the generated authentication
        /// ticket.</returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var raw = _configuration["DevAuthentication:FallbackUsername"] ?? "defaultUser";
            var devUser = raw.Split('\\').Last().Trim().ToLower(); // strip domain and normalize casing


            var claims = new[] { new Claim(ClaimTypes.Name, devUser) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
