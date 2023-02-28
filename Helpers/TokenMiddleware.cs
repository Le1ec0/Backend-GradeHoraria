using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GradeHoraria.Helpers

{
    public class TokenMiddleware
    {
        public IConfiguration Configuration { get; }
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = Configuration.GetValue<string>("AzureAd:ClientId"),

                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetValue<string>("AzureAd:Instance") + Configuration.GetValue<string>("AzureAd:TenantId") + "/v2.0",

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
                };
                try
                {
                    var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var rawToken);
                    context.User = claimsPrincipal;
                }
                catch (Exception)
                {
                    // Handle token validation errors
                }
            }
            await _next(context);
            {
                Console.WriteLine("Token middleware executing...");
                // rest of the code
                await _next(context);
            }
        }
    }
}