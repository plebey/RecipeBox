using RecipeBox.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.DTOs.Ingredients
{
    public class CreateIngredientRequest
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Unit { get; set; }
        public string? PurchaseURL { get; set; }

    }
}
