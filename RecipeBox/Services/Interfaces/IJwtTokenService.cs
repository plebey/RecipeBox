using RecipeBox.Models;

namespace RecipeBox.Services.Interfaces
{
    public interface IJwtTokenService
    {
        (string jwtToken, DateTime expiresAtUtc) GenerateToken(User user);
    }
}
