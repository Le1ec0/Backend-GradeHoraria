using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace GradeHoraria.Helpers

{
    public class TokenMiddleware
    {
        public IConfiguration _configuration { get; }
        private readonly RequestDelegate _next;
        private readonly ConfigurationManager<OpenIdConnectConfiguration> _configManager;

        public TokenMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
            _configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
        $"{_configuration.GetValue<string>("AzureAD:Instance") + _configuration.GetValue<string>("AzureAD:TenantId") + "/v2.0"}/.well-known/openid-configuration",
        new OpenIdConnectConfigurationRetriever());
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var openidConfig = await _configManager.GetConfigurationAsync();

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudiences = new[] {
                    _configuration.GetValue<string>("AzureAD:ClientId"),
                    _configuration.GetValue<string>("AzureAD:ClientAPI"),
                    },

                    ValidateIssuer = true,
                    ValidIssuer = _configuration.GetValue<string>("AzureAD:Instance") + _configuration.GetValue<string>("AzureAD:TenantId") + "/v2.0",

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeys = openidConfig.SigningKeys,
                    TokenDecryptionKey = openidConfig.JsonWebKeySet.Keys.FirstOrDefault(k => k.Kid == tokenHandler.ReadJwtToken(token).Header.Kid)
                };

                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var rawToken);
                context.User = claimsPrincipal;
                var jwtToken = new JwtSecurityToken(token);
                var userClaims = jwtToken.Claims.Where(c => c.Type == "userClaimType").ToList();
                context.Items["AccessToken"] = token;
            }
            try
            {
                await _next(context);
                Console.WriteLine("Token middleware executing...");
            }
            catch (Exception)
            {
            }
        }
        public static async Task<ICollection<SecurityKey>> GetSigningKeys()
        {
            var openidConfigManaged = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"https://login.microsoftonline.com/e182f34b-6958-474c-9143-88349addfba9/v2.0/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever());

            var config = await openidConfigManaged.GetConfigurationAsync();
            return config.SigningKeys;
        }
    }
}