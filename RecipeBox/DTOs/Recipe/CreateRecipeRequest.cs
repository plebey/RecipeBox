using RecipeBox.DTOs.RecipeIngredients;
using RecipeBox.Models;

namespace RecipeBox.DTOs.Recipe
{
    public class CreateRecipeRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? RecipeURL { get; set; }
        public List<CreateRecipeIngredientRequest> RecipeIngredients { get; set; }
    }
}
