using RecipeBox.Common;

namespace RecipeBox.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAtUtc { get; init; }
        public List<Recipe> Recipes { get; init; } = new List<Recipe>();

        private User() { }
        public User(string email, string role)
        {
            Email = email;
            Role = role;
            IsBlocked = false;
            CreatedAtUtc = DateTime.UtcNow;
        }
    }
}
