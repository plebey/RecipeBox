using RecipeBox.DTOs.RecipeIngredients;
using RecipeBox.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.DTOs.Recipe
{
    public class CreateRecipeRequest
    {
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? RecipeURL { get; set; }
        public List<CreateRecipeIngredientRequest>? RecipeIngredients { get; set; }
    }
}
