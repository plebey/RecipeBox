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
        private readonly IIngredientService _ingredientService;

        public RecipeService(IRecipeRepository repository, IIngredientService ingredientService)
        {
            this._repository = repository;
            this._ingredientService = ingredientService;
        }

        private RecipeResponse? BuildRecipeResponse(Recipe? recipe)
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
        public IEnumerable<RecipeResponse> GetAll()
        {
            List<RecipeResponse> recipeRes = new List<RecipeResponse>();
            IEnumerable<Recipe> recipes = _repository.GetAll();
            foreach (Recipe recipe in recipes)
            {
                recipeRes.Add(BuildRecipeResponse(recipe));
            }
            return recipeRes;
        }
        public RecipeResponse? GetById(int id)
        {
            return BuildRecipeResponse(_repository.GetById(id));
        }
        public RecipeResponse? Create(CreateRecipeRequest recipe)
        {
            if (recipe == null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(recipe.Name))
            {
                return null;
            }
            Recipe newRec = new Recipe(recipe.Name, recipe.Description, recipe.RecipeURL);
            foreach (var recipIng in recipe.RecipeIngredients)
            {
                var ingredient = _ingredientService.GetByIdDomain(recipIng.IngredientId);
                if (ingredient == null)
                    return null;
                newRec.RecipeIngredients.Add(new RecipeIngredient(0, null, recipIng.IngredientId, ingredient, recipIng.Amount));
            }    
            return BuildRecipeResponse(_repository.Create(newRec));
        }

        public bool Update(int id, UpdateRecipeRequest recipe)
        {
            if (recipe == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(recipe.Name))
            {
                return false;
            }

            Recipe newRec = new Recipe(recipe.Name, recipe.Description, recipe.RecipeURL);

            foreach (var recipIng in recipe.RecipeIngredients)
            {
                var ingredient = _ingredientService.GetByIdDomain(recipIng.IngredientId);
                if (ingredient == null)
                    return false;
                newRec.RecipeIngredients.Add(new RecipeIngredient(0, null, recipIng.IngredientId, ingredient, recipIng.Amount));
            }
            return _repository.Update(id, newRec);
        }
        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
        public IEnumerable<RecipeResponse> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<RecipeResponse>();
            var recipesByNames = _repository.GetByName(name);
            if (recipesByNames == null)
                return Enumerable.Empty<RecipeResponse>();
            List<RecipeResponse> recipeRes = new List<RecipeResponse>();

            foreach (Recipe recipe in recipesByNames)
            {
                recipeRes.Add(BuildRecipeResponse(recipe));
            }
            return recipeRes;
        }
    }
}
