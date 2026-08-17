using RecipeBox.Common;
using RecipeBox.DTOs;
using RecipeBox.DTOs.Recipe;
using RecipeBox.Models;

namespace RecipeBox.Services.Interfaces
{
    public interface IRecipeService
    {
        Task<Result<UpdateRecipeRequest>> GetForUpdateAsync(int id, CancellationToken cancellationToken);
        Task<Result<PagedResult<RecipeResponse>>> GetAllAsync(CancellationToken cancellationToken, int page, int pageSize, string? name);
        Task<Result<RecipeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result<RecipeResponse>> CreateAsync(CreateRecipeRequest recipe, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(int id, UpdateRecipeRequest recipe, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Result<IEnumerable<RecipeResponse>>> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
