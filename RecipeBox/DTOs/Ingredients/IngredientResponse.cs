using RecipeBox.Models;

namespace RecipeBox.DTOs.Ingredients
{
    public class IngredientResponse
    {
        public int Id { get; set; }
        public string Name { get; init; }
        public string Unit { get; init; }
        public string? PurchaseURL { get; init; }

        public IngredientResponse(int id, string name, string unit, string? purchaseURL)
        {
            Id = id;
            Name = name;
            Unit = unit;
            PurchaseURL = purchaseURL;
        }
    }
}
