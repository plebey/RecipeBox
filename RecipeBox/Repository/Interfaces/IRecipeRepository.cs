using RecipeBox.Models;

namespace RecipeBox.Repository.Interfaces
{
    public interface IRecipeRepository
    {
        //GetAll, GetById, Create, Update, Delete, GetByName
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<Recipe?> GetByIdAsync(int id);
        Task<Recipe> CreateAsync(Recipe recipe);
        Task<bool> UpdateAsync(int id, Recipe recipe);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Recipe>> GetByNameAsync(string name);

    }
}
