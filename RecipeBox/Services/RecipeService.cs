using RecipeBox.Common;
using RecipeBox.DTOs.Ingredients;
using RecipeBox.DTOs.Recipe;
using RecipeBox.DTOs.RecipeIngredients;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;
using RecipeBox.Services.Interfaces;

namespace RecipeBox.Services
{
    public class RecipeService: IRecipeService
    {
        private readonly IRecipeRepository _repository;
        private readonly IIngredientRepository _ingredientRepository;

        public RecipeService(IRecipeRepository repository, IIngredientRepository ingredientRepo)
        {
            this._repository = repository;
            this._ingredientRepository = ingredientRepo;
        }

        private RecipeResponse BuildRecipeResponse(Recipe recipe)
        {
            if (recipe == null)
                return null;
            RecipeResponse response = new RecipeResponse();
            response.Id = recipe.Id;
            response.Name = recipe.Name;
            response.Description = recipe.Description;
            response.RecipeURL = recipe.RecipeURL;
            foreach (RecipeIngredient ingred in recipe.RecipeIngredients)
            {
                RecipeIngredientResponse ingredResp = new RecipeIngredientResponse();
                ingredResp.IngredientId = ingred.IngredientId;
                ingredResp.IngredientName = ingred.Ingredient.Name;
                ingredResp.Unit = ingred.Ingredient.Unit;
                ingredResp.Amount = ingred.Amount;
                response.Ingredients.Add(ingredResp);
            }
            return response;
        }

        //TODO: подумать, в каком виде отдавать данные
        public Result<IEnumerable<RecipeResponse>> GetAll()
        {
            List<RecipeResponse> recipeRes = new List<RecipeResponse>();
            IEnumerable<Recipe> recipes = _repository.GetAll();
            foreach (Recipe recipe in recipes)
            {
                recipeRes.Add(BuildRecipeResponse(recipe));
            }
            return Result<IEnumerable<RecipeResponse>>.Success(recipeRes);
        }
        public Result<RecipeResponse> GetById(int id)
        {
            if (id <= 0)
                return Result<RecipeResponse>.Failure(ErrorType.Validation,
                                                      $"Id must be greater than 0.");
            var res = _repository.GetById(id);
            if (res == null)
                return Result<RecipeResponse>.Failure(ErrorType.NotFound,
                                                      $"Recipe with id {id} not found.");
            return Result<RecipeResponse>.Success(BuildRecipeResponse(res));
        }


        private Result<List<RecipeIngredient>> BuildRecipeIngredients(IEnumerable<CreateRecipeIngredientRequest> recipeIngredients)
        {
            var res = new List<RecipeIngredient>();
            foreach (var recipIng in recipeIngredients)
            {
                if (recipIng.IngredientId <= 0)
                    return Result<List<RecipeIngredient>>.Failure(ErrorType.Validation,
                                                          "Ingredient id must be greater than 0.");
                if (recipIng.Amount <= 0)
                    return Result<List<RecipeIngredient>>.Failure(ErrorType.Validation,
                                                          "Amount must be greater than 0.");
                if (res.Any(i => i.IngredientId == recipIng.IngredientId))
                    return Result<List<RecipeIngredient>>.Failure(ErrorType.Validation,
                                                          "Duplicate ingredient in list.");

                var ingredient = _ingredientRepository.GetById(recipIng.IngredientId);
                if (ingredient == null)
                    return Result<List<RecipeIngredient>>.Failure(ErrorType.NotFound,
                               $"Ingredient with id {recipIng.IngredientId} not found.");
                res.Add(new RecipeIngredient(0, null, recipIng.IngredientId, ingredient, recipIng.Amount));
            }
            return Result<List<RecipeIngredient>>.Success(res);
        }

        public Result<RecipeResponse> Create(CreateRecipeRequest recipe)
        {
            if (string.IsNullOrWhiteSpace(recipe.Name))
            {
                return Result<RecipeResponse>.Failure(ErrorType.Validation,
                               "Name must not be empty.");
            }
            var name = recipe.Name.Trim();

            Recipe newRec = new Recipe(name, recipe.Description, recipe.RecipeURL);


            var recipeIngredients = recipe.RecipeIngredients ?? 
                                    Enumerable.Empty<CreateRecipeIngredientRequest>();

            var resBuild = BuildRecipeIngredients(recipeIngredients);
            if (resBuild.IsSuccess)
                newRec.RecipeIngredients.AddRange(resBuild.Value!);
            else
            {
                return Result<RecipeResponse>.Failure(resBuild.ErrorType!.Value, resBuild.ErrorMsg!);
            }
            
            return Result<RecipeResponse>.Success(BuildRecipeResponse(_repository.Create(newRec)));

        }

        public Result Update(int id, UpdateRecipeRequest recipe)
        {
            if (id <= 0)
                return Result.Failure(ErrorType.Validation,
                                                      $"Id must be greater than 0.");
            if (string.IsNullOrWhiteSpace(recipe.Name))
            {
                return Result.Failure(ErrorType.Validation,
                               "Name must not be empty.");
            }
            var name = recipe.Name.Trim();

            Recipe newRec = new Recipe(name, recipe.Description, recipe.RecipeURL);

            var recipeIngredients = recipe.RecipeIngredients ??
                                    Enumerable.Empty<CreateRecipeIngredientRequest>();

            var resBuild = BuildRecipeIngredients(recipeIngredients);
            if (resBuild.IsSuccess)
                newRec.RecipeIngredients.AddRange(resBuild.Value!);
            else
            {
                return Result.Failure(resBuild.ErrorType!.Value, resBuild.ErrorMsg!);
            }

            return _repository.Update(id, newRec) ?
                   Result.Success() :
                   Result.Failure(ErrorType.NotFound,
                                  $"Recipe with id <{id}> was not found.");
        }
        public Result Delete(int id)
        {
            if (id <= 0)
                return Result.Failure(ErrorType.Validation,
                                                      $"Id must be greater than 0.");
            return _repository.Delete(id) ?
                   Result.Success() :
                   Result.Failure(ErrorType.NotFound,
                                  $"Recipe with id <{id}> was not found.");
        }
        public Result<IEnumerable<RecipeResponse>> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<IEnumerable<RecipeResponse>>.Failure
                                                            (ErrorType.Validation,
                                                            "Name must not be empty.");

            name = name.Trim();
            var recipesByNames = _repository.GetByName(name);

            List<RecipeResponse> recipeRes = new List<RecipeResponse>();

            foreach (var recipe in recipesByNames)
            {
                recipeRes.Add(BuildRecipeResponse(recipe));
            }
            return Result<IEnumerable<RecipeResponse>>.Success(recipeRes);
        }
    }
}
