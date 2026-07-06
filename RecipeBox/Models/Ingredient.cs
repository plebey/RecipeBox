namespace RecipeBox.Models
{
    public class Ingredient
    {
        public string Name { get; init; }
        public string Unit { get; init; }
        public decimal Amount { get; set; }

        public Ingredient(string name, string unit, decimal amount)
        {
            Name = name;
            Unit = unit;
            Amount = amount;
        }
    }
}
