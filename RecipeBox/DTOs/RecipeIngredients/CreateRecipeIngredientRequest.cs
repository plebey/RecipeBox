using System.ComponentModel.DataAnnotations;

namespace RecipeBox.DTOs.RecipeIngredients
{
    public class CreateRecipeIngredientRequest
    {
        [Required]
        public int IngredientId { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
