using RecipeBox.DTOs.RecipeIngredients;

namespace RecipeBox.DTOs.Recipe
{
    public class RecipeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? RecipeURL { get; set; }
        public List<RecipeIngredientResponse> Ingredients { get; set; } = new List<RecipeIngredientResponse>();
    }
}
