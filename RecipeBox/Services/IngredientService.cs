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

        public Result<IEnumerable<IngredientResponse>> GetAll()
        {
            //TODO: переписать через DTO на выдачу без рецептов?
            List<IngredientResponse> ingResp = new List<IngredientResponse>();
            IEnumerable<Ingredient> ingredients = _repository.GetAll();

            foreach(var ingredient in ingredients)
            {
                ingResp.Add(BuildIngredientResponse(ingredient));
            }

            return Result<IEnumerable<IngredientResponse>>.Success(ingResp);
        }
        public Result<IngredientResponse> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<IngredientResponse>.Failure(ErrorType.Validation,
                                      "Name must not be empty.");
            name = name.Trim();
            var res = _repository.GetByName(name);
            if (res == null)
                return Result<IngredientResponse>.Failure(ErrorType.NotFound,
                                  $"Ingredient with name <{name}> was not found.");
            else
                return Result<IngredientResponse>.Success(BuildIngredientResponse(res));
        }
        public Result<IngredientResponse> GetById(int id)
        {
            if (id <= 0)
            {
                return Result<IngredientResponse>.Failure(
                    ErrorType.Validation,
                    "Ingredient id must be greater than zero.");
            }
            var res = _repository.GetById(id);
            if (res == null)
                return Result<IngredientResponse>.Failure(ErrorType.NotFound,
                                  $"Ingredient with id <{id}> was not found.");
            else
                return Result<IngredientResponse>.Success(BuildIngredientResponse(res));
        }
        public Result<IngredientResponse> Create(CreateIngredientRequest ingredientReq)
        {
            if (string.IsNullOrWhiteSpace(ingredientReq.Name) || string.IsNullOrWhiteSpace(ingredientReq.Unit))
                return Result<IngredientResponse>.Failure(ErrorType.Validation,
                               "Name and Unit must not be empty.");
            var name = ingredientReq.Name.Trim();
            var unit = ingredientReq.Unit.Trim();
            if (_repository.GetByName(name) != null)
                return Result<IngredientResponse>.Failure(ErrorType.Conflict,
                                      $"Ingredient with name \"{name}\" already exists.");
            var newIngr = new Ingredient(name, unit, ingredientReq.PurchaseURL);

            var res = _repository.Create(newIngr);
            return Result<IngredientResponse>.Success(BuildIngredientResponse(res));
        }
        public Result Update(int id, UpdateIngredientRequest ingredientReq)
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

            var withSameName = _repository.GetByName(name);
            if (withSameName != null &&
                withSameName.Id != id)
                return Result.Failure(ErrorType.Conflict,
                                      $"Ingredient with name \"{name}\" already exists.");

            var newIngr = new Ingredient(name, unit, ingredientReq.PurchaseURL);

            return _repository.Update(id, newIngr) ?
                   Result.Success() :
                   Result.Failure(ErrorType.NotFound,
                                  $"Ingredient with id <{id}> was not found.");
        }
        public Result Delete(int id)
        {
            if (id <= 0)
            {
                return Result.Failure(
                    ErrorType.Validation,
                    "Ingredient id must be greater than zero.");
            }
            return _repository.Delete(id) ?
                   Result.Success() :
                   Result.Failure(ErrorType.NotFound,
                                  $"Ingredient with id <{id}> was not found.");
        }
    }
}
