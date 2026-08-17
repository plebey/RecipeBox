using RecipeBox.DTOs;
using RecipeBox.Models;

namespace RecipeBox.Repository.Interfaces
{
    public interface IIngredientRepository
    {
        //GetAll, GetById, Create, Update, Delete, GetByName
        Task<PagedResult<Ingredient>> GetAllAsync(CancellationToken cancellationToken, int page, int pageSize, string? name);
        Task<Ingredient?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Ingredient> CreateAsync(Ingredient recipe, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(int id, Ingredient recipe, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Ingredient?> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
