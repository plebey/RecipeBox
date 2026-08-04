using RecipeBox.Data;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;

namespace RecipeBox.Repository
{
    public class IngredientRepository: IIngredientRepository
    {
        private readonly DBContextRecipeBox _context;
        public IngredientRepository(DBContextRecipeBox context)
        {
            _context = context;
        }

        public IEnumerable<Ingredient> GetAll()
        {
            return _context.Ingredients.ToList();
        }

        public Ingredient? GetById(int id)
        {
            return _context.Ingredients.FirstOrDefault(rec => rec.Id == id);
        }

        public Ingredient Create(Ingredient ingred)
        {
            ingred.Id = 0;
            _context.Ingredients.Add(ingred);
            _context.SaveChanges();

            return ingred;
        }

        public bool Update(int id, Ingredient newIngred)
        {
            var ingred = _context.Ingredients.FirstOrDefault(r => r.Id == id);
            if (ingred == null)
                return false; // не нашли
 
            newIngred.Id = ingred.Id;

            _context.Entry(ingred).CurrentValues.SetValues(newIngred);
            _context.SaveChanges();

            return true;

        }

        public bool Delete(int id)
        {

            var ingred = _context.Ingredients.FirstOrDefault(ing => ing.Id == id);
            if (ingred == null)
                return false;
            _context.Ingredients.Remove(ingred);
            _context.SaveChanges();
            return true; 

        }

        public Ingredient? GetByName(string name)
        {
            return _context.Ingredients.Where(ing => ing.Name == name).FirstOrDefault();
        }
    }
}
