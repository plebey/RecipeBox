using RecipeBox.Common;
using RecipeBox.DTOs;
using RecipeBox.DTOs.Ingredients;
using RecipeBox.DTOs.Recipe;
using RecipeBox.DTOs.RecipeIngredients;
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

        public async Task<Result<UpdateIngredientRequest>> GetForUpdateAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return Result<UpdateIngredientRequest>.Failure(
                    ErrorType.Validation,
                    "Id must be greater than 0.");
            }

            var ingredient = await _repository.GetByIdAsync(id, cancellationToken);

            if (ingredient == null)
                return Result<UpdateIngredientRequest>.Failure(ErrorType.NotFound, $"Ingredient with id {id} not found.");

            return Result<UpdateIngredientRequest>.Success(new UpdateIngredientRequest()
            {
                Name = ingredient.Name,
                PurchaseURL = ingredient.PurchaseURL,
                Unit = ingredient.Unit
            });
        }

        public async Task<Result<PagedResult<IngredientResponse>>> GetAllAsync(CancellationToken cancellationToken, int page, int pageSize, string? name)
        {
            //TODO: переписать через DTO на выдачу без рецептов?
            List<IngredientResponse> ingResp = new List<IngredientResponse>();
            if (page <= 0)
            {
                return Result<PagedResult<IngredientResponse>>.Failure(
                    ErrorType.Validation,
                    "Page must be greater than 0.");
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                return Result<PagedResult<IngredientResponse>>.Failure(
                    ErrorType.Validation,
                    "Page size must be between 1 and 100.");
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
            }

            var pagedIngreds = await _repository.GetAllAsync(cancellationToken, page, pageSize, name);

            foreach(var ingredient in pagedIngreds.Items)
            {
                ingResp.Add(BuildIngredientResponse(ingredient));
            }

            var ingRespPaged = new PagedResult<IngredientResponse>()
            {
                Items = ingResp,
                Page = pagedIngreds.Page,
                PageSize = pagedIngreds.PageSize,
                TotalCount = pagedIngreds.TotalCount
            };

            return Result<PagedResult<IngredientResponse>>.Success(ingRespPaged);
        }
        public async Task<Result<IngredientResponse>> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<IngredientResponse>.Failure(ErrorType.Validation,
                                      "Name must not be empty.");
            name = name.Trim();
            var res = await _repository.GetByNameAsync(name, cancellationToken);
            if (res == null)
                return Result<IngredientResponse>.Failure(ErrorType.NotFound,
                                  $"Ingredient with name <{name}> was not found.");
            else
                return Result<IngredientResponse>.Success(BuildIngredientResponse(res));
        }


        public async Task<Result<IngredientResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return Result<IngredientResponse>.Failure(
                    ErrorType.Validation,
                    "Ingredient id must be greater than zero.");
            }
            var res = await _repository.GetByIdAsync(id, cancellationToken);
            if (res == null)
                return Result<IngredientResponse>.Failure(ErrorType.NotFound,
                                  $"Ingredient with id <{id}> was not found.");
            else
                return Result<IngredientResponse>.Success(BuildIngredientResponse(res));
        }


        private bool ValidateIngredNameUnitURLLength(string name, string unit, string? purchaseURL)
        {
            if (purchaseURL != null) 
                if (purchaseURL.Length > Constraints.IngredientURLMaxLength)
                    return false;
            if (name.Length > Constraints.IngredientNameMaxLength || unit.Length > Constraints.IngredientUnitMaxLength)
                return false;
            return true;
        }

        public async Task<Result<IngredientResponse>> CreateAsync(CreateIngredientRequest ingredientReq, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(ingredientReq.Name) || string.IsNullOrWhiteSpace(ingredientReq.Unit))
                return Result<IngredientResponse>.Failure(ErrorType.Validation,
                               "Name and Unit must not be empty.");
            var name = ingredientReq.Name.Trim();
            var unit = ingredientReq.Unit.Trim();
            var purchaseURL = ingredientReq.PurchaseURL != null ? ingredientReq.PurchaseURL.Trim() : null;

            if (!ValidateIngredNameUnitURLLength(name, unit, purchaseURL))
                return Result<IngredientResponse>.Failure(ErrorType.Validation,
                               "One or more parameters are too long.");

            if ((await _repository.GetByNameAsync(name, cancellationToken)) != null)
                return Result<IngredientResponse>.Failure(ErrorType.Conflict,
                                      $"Ingredient with name \"{name}\" already exists.");
            var newIngr = new Ingredient(name, unit, purchaseURL);

            var res = await _repository.CreateAsync(newIngr, cancellationToken);
            return Result<IngredientResponse>.Success(BuildIngredientResponse(res));
        }
        public async Task<Result> UpdateAsync(int id, UpdateIngredientRequest ingredientReq, CancellationToken cancellationToken)
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
            var purchaseURL = ingredientReq.PurchaseURL != null ? ingredientReq.PurchaseURL.Trim() : null;

            if (!ValidateIngredNameUnitURLLength(name, unit, purchaseURL))
                return Result.Failure(ErrorType.Validation,
                               "One or more parameters are too long.");

            var withSameName = await _repository.GetByNameAsync(name, cancellationToken);
            if (withSameName != null &&
                withSameName.Id != id)
                return Result.Failure(ErrorType.Conflict,
                                      $"Ingredient with name \"{name}\" already exists.");

            var newIngr = new Ingredient(name, unit, ingredientReq.PurchaseURL);

            return await _repository.UpdateAsync(id, newIngr, cancellationToken) ?
                   Result.Success() :
                   Result.Failure(ErrorType.NotFound,
                                  $"Ingredient with id <{id}> was not found.");
        }
        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return Result.Failure(
                    ErrorType.Validation,
                    "Ingredient id must be greater than zero.");
            }
            return await _repository.DeleteAsync(id, cancellationToken) ?
                   Result.Success() :
                   Result.Failure(ErrorType.NotFound,
                                  $"Ingredient with id <{id}> was not found.");
        }
    }
}
