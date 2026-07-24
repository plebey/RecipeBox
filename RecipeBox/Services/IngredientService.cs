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

        public IEnumerable<IngredientResponse> GetAll()
        {
            //TODO: переписать через DTO на выдачу без рецептов?
            List<IngredientResponse> ingResp = new List<IngredientResponse>();
            IEnumerable<Ingredient> ingredients = _repository.GetAll();

            foreach(var ingredient in ingredients)
            {
                ingResp.Add(BuildIngredientResponse(ingredient));
            }


            return ingResp;
        }
        public IngredientResponse? GetByName(string name)
        {
            var res = _repository.GetByName(name);
            if (res == null)
                return null;
            else
                return BuildIngredientResponse(res);
        }
        public IngredientResponse? GetById(int id)
        {
            var res = _repository.GetById(id);
            if (res == null)
                return null;
            else
                return BuildIngredientResponse(res);
            //TODO: переписать через DTO 2 варианта - с рецептами и без
        }
        public Ingredient? GetByIdDomain(int id)
        {
            return _repository.GetById(id);
        }
        public IngredientResponse? Create(CreateIngredientRequest ingredientReq)
        {
            if (string.IsNullOrWhiteSpace(ingredientReq.Name) || string.IsNullOrWhiteSpace(ingredientReq.Unit))
                return null;
            if (_repository.GetByName(ingredientReq.Name) != null)
                return null;
            var newIngr = new Ingredient(ingredientReq.Name, ingredientReq.Unit, ingredientReq.PurchaseURL);

            var res = _repository.Create(newIngr);
            if (res == null)
                return null;
            else
                return BuildIngredientResponse(res);
        }
        public bool Update(int id, UpdateIngredientRequest ingredientReq)
        {
            if (string.IsNullOrWhiteSpace(ingredientReq.Name) || string.IsNullOrWhiteSpace(ingredientReq.Unit))
                return false;
            var newIngr = new Ingredient(ingredientReq.Name, ingredientReq.Unit, ingredientReq.PurchaseURL);
            return _repository.Update(id, newIngr);
        }
        public bool Delete(int id)
        {
            return (_repository.Delete(id));
        }
    }
}
