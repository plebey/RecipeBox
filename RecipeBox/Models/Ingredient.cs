namespace RecipeBox.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        //TODO: вывести в отдельную таблицу с хранением синонимов названий
        public string Name { get; init; }
        public string Unit { get; init; }
        public string? PurchaseURL { get; init; }

        public List<RecipeIngredient> RecipeIngredients { get; init; } = new List<RecipeIngredient>();

        private Ingredient() { }
        public Ingredient(string name, string unit, string? purchaseURL = null)
        {
            Name = name;
            Unit = unit;
            PurchaseURL = purchaseURL;
            RecipeIngredients = new List<RecipeIngredient>();
        }
    }
}
