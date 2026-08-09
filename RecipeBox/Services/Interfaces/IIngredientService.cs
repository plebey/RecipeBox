using RecipeBox.Common;
using RecipeBox.DTOs.Ingredients;
using RecipeBox.Models;

namespace RecipeBox.Services.Interfaces
{
    public interface IIngredientService
    {
        Task<Result<IEnumerable<IngredientResponse>>> GetAllAsync();
        Task<Result<IngredientResponse>> GetByNameAsync(string name);
        Task<Result<IngredientResponse>> GetByIdAsync(int id);
        Task<Result<IngredientResponse>> CreateAsync(CreateIngredientRequest ingredient);
        Task<Result> UpdateAsync(int id, UpdateIngredientRequest ingredient);
        Task<Result> DeleteAsync(int id);
    }
}
