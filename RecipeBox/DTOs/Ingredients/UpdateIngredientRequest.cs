using RecipeBox.Models;

namespace RecipeBox.DTOs.Ingredients
{
    public class UpdateIngredientRequest
    {
        public required string Name { get; set; }
        public required string Unit { get; set; }
        public string? PurchaseURL { get; set; }

    }
}
