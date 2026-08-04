using RecipeBox.Common;
using RecipeBox.DTOs.Recipe;
using RecipeBox.Models;

namespace RecipeBox.Services.Interfaces
{
    public interface IRecipeService
    {
        Result<IEnumerable<RecipeResponse>> GetAll();
        Result<RecipeResponse> GetById(int id);
        Result<RecipeResponse> Create(CreateRecipeRequest recipe);
        Result Update(int id, UpdateRecipeRequest recipe);
        Result Delete(int id);
        Result<IEnumerable<RecipeResponse>> GetByName(string name);
    }
}
