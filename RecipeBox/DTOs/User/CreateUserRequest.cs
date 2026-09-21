using System.ComponentModel.DataAnnotations;
using RecipeBox.Models;

namespace RecipeBox.DTOs.User
{
    public class CreateUserRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }

    }
}

