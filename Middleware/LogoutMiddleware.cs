using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace XRoute.Middleware
{

    public class LogoutMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogoutMiddleware> _logger;

        public LogoutMiddleware(RequestDelegate next, ILogger<LogoutMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null)
            {
                _logger.LogWarning("HttpContext is null.");
                return;  // or throw an appropriate exception
            }

            if (context.User?.Identity == null)
            {
                await _next(context);
                return;
            }

            // Check if the request path is for logout
            if (context.Request.Path.Equals("/Account/Logout"))
            {
                // Perform the logout logic
                if (context.User.Identity.IsAuthenticated)
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Session.Clear();
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }

            await _next(context);
        }


    }

    // Extension method to add the middleware to the HTTP request pipeline.
    public static class LogoutMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogoutMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogoutMiddleware>();
        }
    }
}
