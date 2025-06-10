using System.Net;
using System.Text;
using System.Threading.Tasks;
using BuddyMatch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuddyMatch.API.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BasicAuthMiddleware> _logger;

        public BasicAuthMiddleware(RequestDelegate next, ILogger<BasicAuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            // Skip authentication for login route and OPTIONS requests
            if (context.Request.Path.StartsWithSegments("/api/user/login") || 
                context.Request.Method == "OPTIONS")
            {
                await _next(context);
                return;
            }

            string? authHeader = context.Request.Headers["Authorization"]; // CS8600: Handle possible null
            
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Extract credentials from Basic Auth header
                var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                var credentials = decodedCredentials.Split(':', 2);
                
                if (credentials.Length == 2)
                {
                    var username = credentials[0];
                    var password = credentials[1];
                    
                    // Verify credentials with the service
                    var isValid = await authService.ValidateCredentialsAsync(username, password);
                    
                    if (isValid)
                    {
                        // Set authenticated user on the context
                        context.Items["Username"] = username;
                        await _next(context);
                        return;
                    }
                }
            }

            // If authentication failed
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.Headers.Append("WWW-Authenticate", "Basic"); // ASP0019: Use Append
        }
    }
}