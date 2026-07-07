using RecipeBox.Models;

namespace RecipeBox.Services.Interfaces
{
    public interface IRecipeService
    {
        IEnumerable<Recipe> GetAll();
        Recipe? GetById(int id);
        Recipe? Create(Recipe recipe);
        bool Update(int id, Recipe recipe);
        bool Delete(int id);
        IEnumerable<Recipe> GetByName(string name);
    }
}
