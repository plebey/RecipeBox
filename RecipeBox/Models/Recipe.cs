namespace RecipeBox.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }

        public List<Ingredient> Ingredients { get; init; }

        public Recipe(string name, string description = "", List<Ingredient>? ingredients = null)
        {
            Name = name;
            Description = description;
            Ingredients = ingredients ?? new List<Ingredient>();
        }
    }
}
