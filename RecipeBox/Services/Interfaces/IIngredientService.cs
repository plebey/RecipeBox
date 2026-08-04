using RecipeBox.Common;
using RecipeBox.DTOs.Ingredients;
using RecipeBox.Models;

namespace RecipeBox.Services.Interfaces
{
    public interface IIngredientService
    {
        Result<IEnumerable<IngredientResponse>> GetAll();
        Result<IngredientResponse> GetByName(string name);
        Result<IngredientResponse> GetById(int id);
        Result<IngredientResponse> Create(CreateIngredientRequest ingredient);
        Result Update(int id, UpdateIngredientRequest ingredient);
        Result Delete(int id);
    }
}
