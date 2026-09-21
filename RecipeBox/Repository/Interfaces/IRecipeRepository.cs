using RecipeBox.Common;
using RecipeBox.DTOs;
using RecipeBox.DTOs.Recipe;
using RecipeBox.Models;

namespace RecipeBox.Repository.Interfaces
{
    public interface IRecipeRepository
    {
        //GetAll, GetById, Create, Update, Delete, GetByName
        Task<PagedResult<Recipe>> GetAllAsync(CancellationToken cancellationToken, int page, int pageSize, string? name);
        Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Recipe> CreateAsync(Recipe recipe, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(int id, Recipe recipe, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Recipe>> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<IEnumerable<Recipe>> GetByUserAsync(int userId, CancellationToken cancellationToken);


    }
}
