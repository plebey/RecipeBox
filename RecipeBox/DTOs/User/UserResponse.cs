using RecipeBox.DTOs.Recipe;
using RecipeBox.Models;

namespace RecipeBox.DTOs.User
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
