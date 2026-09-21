using Microsoft.IdentityModel.Tokens;
using RecipeBox.Models;
using RecipeBox.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeBox.Services
{
    public class JwtTokenService: IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public (string jwtToken, DateTime expiresAtUtc) GenerateToken(User user)
        {
            var claims = new List<Claim>
            { 
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured.");
            var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT issuer is not configured.");
            var expiresMinutes = _configuration.GetValue<int>("Jwt:ExpiresMinutes");
            if (expiresMinutes <= 0)
            {
                throw new InvalidOperationException(
                    "JWT issuer is not configured.");
            }

            var jwtSecret = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT issuer is not configured.");
            var keyBytes = Encoding.UTF8.GetBytes(jwtSecret);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(
                                securityKey,
                                SecurityAlgorithms.HmacSha256);

            var expiresAtUtc = DateTime.UtcNow.AddMinutes(expiresMinutes);

            var token = new JwtSecurityToken(
                                        issuer: issuer,
                                        audience: audience,
                                        claims: claims,
                                        expires: expiresAtUtc,
                                        signingCredentials: credentials
                                    );

            var handler = new JwtSecurityTokenHandler();

            return (handler.WriteToken(token), expiresAtUtc);
        }

    }
}
