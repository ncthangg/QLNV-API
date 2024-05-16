using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QLNV.Repositories;
using System.Linq;
using System.Threading.Tasks;
namespace QLNV.Services
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _next = next;

            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    var _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                    var user = _userRepository.GetUserFromToken(token);
                    if (user != null)
                    {
                        var isTokenActive = _userRepository.IsTokenActive(token);
                        if (!isTokenActive)
                        {

                            context.Response.StatusCode = 401; // Unauthorized
                            await context.Response.WriteAsync("Unauthorized: Token is invalid or inactive.");
                            return;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401; // Unauthorized
                        await context.Response.WriteAsync("Unauthorized: Token is invalid or inactive.");
                        return;
                    }
                }
                await _next(context);
            }
        }
    }

}
