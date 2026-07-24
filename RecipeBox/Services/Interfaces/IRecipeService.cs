using RecipeBox.DTOs.Recipe;
using RecipeBox.Models;

namespace RecipeBox.Services.Interfaces
{
    public interface IRecipeService
    {
        IEnumerable<RecipeResponse> GetAll();
        RecipeResponse? GetById(int id);
        RecipeResponse? Create(CreateRecipeRequest recipe);
        bool Update(int id, UpdateRecipeRequest recipe);
        bool Delete(int id);
        IEnumerable<RecipeResponse> GetByName(string name);
    }
}
