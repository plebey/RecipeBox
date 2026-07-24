namespace RecipeBox.Models
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public decimal Amount { get; set; }

        private RecipeIngredient() { }

        public RecipeIngredient(int recipeId, Recipe recipe, int ingredientId, Ingredient ingredient, decimal amount)
        {
            RecipeId = recipeId;
            this.Recipe = recipe;
            IngredientId = ingredientId;
            this.Ingredient = ingredient;
            Amount = amount;
        }
    }
}
