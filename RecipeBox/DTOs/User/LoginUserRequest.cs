using System.ComponentModel.DataAnnotations;

namespace RecipeBox.DTOs.User
{
    public class LoginUserRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
