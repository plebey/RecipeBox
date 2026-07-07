using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;
using RecipeBox.Services.Interfaces;

namespace RecipeBox.Services
{
    public class RecipeService: IRecipeService
    {
        private readonly IRecipeRepository _repository;

        public RecipeService(IRecipeRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<Recipe> GetAll()
        {
            return _repository.GetAll();
        }
        public Recipe? GetById(int id)
        {
            return _repository.GetById(id);
        }
        public Recipe? Create(Recipe recipe)
        {
            if (recipe == null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(recipe.Name))
            {
                return null;
            }
            return _repository.Create(recipe);
        }
        public bool Update(int id, Recipe recipe)
        {
            return _repository.Update(id, recipe);
        }
        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
        public IEnumerable<Recipe> GetByName(string name)
        {
            return _repository.GetByName(name);
        }
    }
}
