namespace RecipeBox.Common
{
    public static class Constraints
    {
        public const int RecipeNameMaxLength = 50;
        public const int RecipeDescriptionMaxLength = 10000;
        public const int RecipeURLMaxLength = 200;

        public const int IngredientNameMaxLength = 50;
        public const int IngredientUnitMaxLength = 20;
        public const int IngredientURLMaxLength = 200;

        public const int IngredientAmountDigitsLength = 10;
        public const int IngredientAmountAfterComma = 3;

    }
}
