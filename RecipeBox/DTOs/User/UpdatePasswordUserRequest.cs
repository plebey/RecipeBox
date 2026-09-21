using System.ComponentModel.DataAnnotations;
using RecipeBox.Models;

namespace RecipeBox.DTOs.User
{
    public class UpdatePasswordUserRequest
    {
        [Required]
        public required string newPassword { get; set; }
        [Required]
        public required string oldPassword { get; set; }

    }
}

