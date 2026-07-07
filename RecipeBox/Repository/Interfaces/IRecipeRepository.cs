using RecipeBox.Models;

namespace RecipeBox.Repository.Interfaces
{
    public interface IRecipeRepository
    {
        //GetAll, GetById, Create, Update, Delete, GetByName
        IEnumerable<Recipe> GetAll();
        Recipe? GetById(int id);
        Recipe? Create(Recipe recipe);
        bool Update(int id, Recipe recipe);
        bool Delete(int id);
        IEnumerable<Recipe> GetByName(string name);

    }
}
