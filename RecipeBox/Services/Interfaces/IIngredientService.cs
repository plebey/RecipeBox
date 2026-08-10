using RecipeBox.Common;
using RecipeBox.DTOs.Ingredients;
using RecipeBox.Models;

namespace RecipeBox.Services.Interfaces
{
    public interface IIngredientService
    {
        Task<Result<IEnumerable<IngredientResponse>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<IngredientResponse>> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<Result<IngredientResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result<IngredientResponse>> CreateAsync(CreateIngredientRequest ingredient, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(int id, UpdateIngredientRequest ingredient, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
