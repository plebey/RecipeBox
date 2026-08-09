using RecipeBox.Common;
using RecipeBox.DTOs.Ingredients;
using RecipeBox.DTOs.Recipe;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;
using RecipeBox.Services.Interfaces;

namespace RecipeBox.Services
{
    public class IngredientService: IIngredientService
    {
        private readonly IIngredientRepository _repository;
        public IngredientService(IIngredientRepository repository)
        {
            _repository = repository;
        }
        private IngredientResponse BuildIngredientResponse(Ingredient ingredient)
        {
            return new IngredientResponse(ingredient.Id,
                                          ingredient.Name,
                                          ingredient.Unit,
                                          ingredient.PurchaseURL);
        }

        public async Task<Result<IEnumerable<IngredientResponse>>> GetAllAsync()
        {
            //TODO: переписать через DTO на выдачу без рецептов?
            List<IngredientResponse> ingResp = new List<IngredientResponse>();
            IEnumerable<Ingredient> ingredients = await _repository.GetAllAsync();

            foreach(var ingredient in ingredients)
            {
                ingResp.Add(BuildIngredientResponse(ingredient));
            }

            return Result<IEnumerable<IngredientResponse>>.Success(ingResp);
        }
        public async Task<Result<IngredientResponse>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<IngredientResponse>.Failure(ErrorType.Validation,
                                      "Name must not be empty.");
            name = name.Trim();
            var res = await _repository.GetByNameAsync(name);
            if (res == null)
                return Result<IngredientResponse>.Failure(ErrorType.NotFound,
                                  $"Ingredient with name <{name}> was not found.");
            else
                return Result<IngredientResponse>.Success(BuildIngredientResponse(res));
        }
        public async Task<Result<IngredientResponse>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return Result<IngredientResponse>.Failure(
                    ErrorType.Validation,
                    "Ingredient id must be greater than zero.");
            }
            var res = await _repository.GetByIdAsync(id);
            if (res == null)
                return Result<IngredientResponse>.Failure(ErrorType.NotFound,
                                  $"Ingredient with id <{id}> was not found.");
            else
                return Result<IngredientResponse>.Success(BuildIngredientResponse(res));
        }
        public async Task<Result<IngredientResponse>> CreateAsync(CreateIngredientRequest ingredientReq)
        {
            if (string.IsNullOrWhiteSpace(ingredientReq.Name) || string.IsNullOrWhiteSpace(ingredientReq.Unit))
                return Result<IngredientResponse>.Failure(ErrorType.Validation,
                               "Name and Unit must not be empty.");
            var name = ingredientReq.Name.Trim();
            var unit = ingredientReq.Unit.Trim();
            if ((await _repository.GetByNameAsync(name)) != null)
                return Result<IngredientResponse>.Failure(ErrorType.Conflict,
                                      $"Ingredient with name \"{name}\" already exists.");
            var newIngr = new Ingredient(name, unit, ingredientReq.PurchaseURL);

            var res = await _repository.CreateAsync(newIngr);
            return Result<IngredientResponse>.Success(BuildIngredientResponse(res));
        }
        public async Task<Result> UpdateAsync(int id, UpdateIngredientRequest ingredientReq)
        {
            if (id <= 0)
            {
                return Result.Failure(
                    ErrorType.Validation,
                    "Ingredient id must be greater than zero.");
            }
            if (string.IsNullOrWhiteSpace(ingredientReq.Name) || string.IsNullOrWhiteSpace(ingredientReq.Unit))
                return Result.Failure(ErrorType.Validation,
                                      "Name and Unit must not be empty.");
            var name = ingredientReq.Name.Trim();
            var unit = ingredientReq.Unit.Trim();

            var withSameName = await _repository.GetByNameAsync(name);
            if (withSameName != null &&
                withSameName.Id != id)
                return Result.Failure(ErrorType.Conflict,
                                      $"Ingredient with name \"{name}\" already exists.");

            var newIngr = new Ingredient(name, unit, ingredientReq.PurchaseURL);

            return await _repository.UpdateAsync(id, newIngr) ?
                   Result.Success() :
                   Result.Failure(ErrorType.NotFound,
                                  $"Ingredient with id <{id}> was not found.");
        }
        public async Task<Result> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return Result.Failure(
                    ErrorType.Validation,
                    "Ingredient id must be greater than zero.");
            }
            return await _repository.DeleteAsync(id) ?
                   Result.Success() :
                   Result.Failure(ErrorType.NotFound,
                                  $"Ingredient with id <{id}> was not found.");
        }
    }
}
