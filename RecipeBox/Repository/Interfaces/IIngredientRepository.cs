using RecipeBox.Models;

namespace RecipeBox.Repository.Interfaces
{
    public interface IIngredientRepository
    {
        //GetAll, GetById, Create, Update, Delete, GetByName
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<Ingredient?> GetByIdAsync(int id);
        Task<Ingredient> CreateAsync(Ingredient recipe);
        Task<bool> UpdateAsync(int id, Ingredient recipe);
        Task<bool> DeleteAsync(int id);
        Task<Ingredient?> GetByNameAsync(string name);
    }
}
