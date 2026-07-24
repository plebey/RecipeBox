namespace RecipeBox.DTOs.RecipeIngredients
{
    public class RecipeIngredientResponse
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }  // берём из связанного Ingredient
        public string Unit { get; set; }
        public decimal Amount { get; set; }
    }
}
