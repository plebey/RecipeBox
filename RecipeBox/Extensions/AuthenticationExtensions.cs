using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RecipeBox.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication
            (this IServiceCollection services,
            IConfiguration configuration
            )
        {
            var jwtKey = configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT key is not configured.");

            var jwtIssuer = configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT issuer is not configured.");

            var jwtAudience = configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("JWT audience is not configured.");

            services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,

                    ValidateAudience = true,
                    ValidAudience = jwtAudience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
                };
            });
            return services;
        }
    }
}
