namespace RecipeBox.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public string? RecipeURL { get; init; }

        public List<RecipeIngredient> RecipeIngredients { get; init; } = new List<RecipeIngredient>();

        private Recipe() { }

        public Recipe(string name, string? description = null, string? recipeURL = null, List<RecipeIngredient>? newRecipeIngredients = null)
        {
            Name = name;
            Description = description;
            RecipeURL = recipeURL;
            RecipeIngredients = newRecipeIngredients ?? new List<RecipeIngredient>();
        }
    }
}
