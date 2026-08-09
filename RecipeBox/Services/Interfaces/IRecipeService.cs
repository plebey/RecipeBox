using RecipeBox.Common;
using RecipeBox.DTOs.Recipe;
using RecipeBox.Models;

namespace RecipeBox.Services.Interfaces
{
    public interface IRecipeService
    {
        Task<Result<IEnumerable<RecipeResponse>>> GetAllAsync();
        Task<Result<RecipeResponse>> GetByIdAsync(int id);
        Task<Result<RecipeResponse>> CreateAsync(CreateRecipeRequest recipe);
        Task<Result> UpdateAsync(int id, UpdateRecipeRequest recipe);
        Task<Result> DeleteAsync(int id);
        Task<Result<IEnumerable<RecipeResponse>>> GetByNameAsync(string name);
    }
}
