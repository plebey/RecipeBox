namespace RecipeBox.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public string? RecipeURL { get; init; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<RecipeIngredient> RecipeIngredients { get; init; } = new List<RecipeIngredient>();

        private Recipe() { }

        public Recipe(int userId, string name, string? description = null, string? recipeURL = null, List<RecipeIngredient>? newRecipeIngredients = null)
        {
            Name = name;
            Description = description;
            RecipeURL = recipeURL;
            RecipeIngredients = newRecipeIngredients ?? new List<RecipeIngredient>();
            UserId = userId;
        }
    }
}
