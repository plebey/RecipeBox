using System.ComponentModel.DataAnnotations;
using RecipeBox.Models;

namespace RecipeBox.DTOs.User
{
    public class UpdateEmailUserRequest
    {
        [Required]
        [EmailAddress]
        public required string NewEmail { get; set; }
        [Required]
        public required string Password { get; set; }

    }
}

