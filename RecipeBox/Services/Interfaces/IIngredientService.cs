using RecipeBox.DTOs.Ingredients;
using RecipeBox.Models;

namespace RecipeBox.Services.Interfaces
{
    public interface IIngredientService
    {
        IEnumerable<IngredientResponse> GetAll();
        IngredientResponse? GetByName(string name);
        IngredientResponse? GetById(int id);
        Ingredient? GetByIdDomain(int id);
        IngredientResponse? Create(CreateIngredientRequest ingredient);
        bool Update(int id, UpdateIngredientRequest ingredient);
        bool Delete(int id);
    }
}
