using RecipeBox.Models;

namespace RecipeBox.Repository.Interfaces
{
    public interface IIngredientRepository
    {
        //GetAll, GetById, Create, Update, Delete, GetByName
        IEnumerable<Ingredient> GetAll();
        Ingredient? GetById(int id);
        Ingredient Create(Ingredient recipe);
        bool Update(int id, Ingredient recipe);
        bool Delete(int id);
        Ingredient? GetByName(string name);
    }
}
