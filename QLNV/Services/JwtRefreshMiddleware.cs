using Microsoft.IdentityModel.Tokens;
using QLNV.Models.Entities;
using QLNV.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace QLNV.Services
{
    public class JwtRefreshMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public JwtRefreshMiddleware(RequestDelegate next, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _next = next;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var authenticationService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (!string.IsNullOrEmpty(token))
                {
                    var isTokenValid = !userRepository.IsTokenExpired(token);
                    if (!isTokenValid)
                    {
                        var refreshToken = context.Request.Headers["RefreshToken"].FirstOrDefault();
                        var responseLogin = userRepository.GetUserFromToken(token);
                        var user = userRepository.GetUserById(responseLogin.UserId);

                        if (user != null && !string.IsNullOrEmpty(refreshToken) && user.ResetToken == refreshToken)
                        {
                            var newJwtToken = authenticationService.CreateJwtToken(user);
                            var newRefreshToken = authenticationService.CreateRefreshToken(user);

                            var result = new ResponseLogin
                            {
                                UserId = user.UserId,
                                RoleId = user.RoleId,
                                Token = newJwtToken,
                                ResetToken = newRefreshToken.Token
                            };
                            userRepository.SaveResponsLogin(result);
                            userRepository.SaveRefreshToken(newRefreshToken);

                            // Add new tokens to response headers
                            context.Response.Headers.Add("NewJwtToken", newJwtToken);
                            context.Response.Headers.Add("NewRefreshToken", newRefreshToken.Token);
                        }
                    }
                }

                // Ensure the request continues to the next middleware
                await _next(context);
            }
        }
    }
}
