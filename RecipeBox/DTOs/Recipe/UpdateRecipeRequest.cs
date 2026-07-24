using RecipeBox.DTOs.RecipeIngredients;
using RecipeBox.Models;

namespace RecipeBox.DTOs.Recipe
{
    //id приходит из url
    public class UpdateRecipeRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? RecipeURL { get; set; }

        public List<CreateRecipeIngredientRequest> RecipeIngredients { get; set; }
    }
}
