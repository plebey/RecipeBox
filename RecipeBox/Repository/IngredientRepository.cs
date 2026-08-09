using RecipeBox.Data;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RecipeBox.Repository
{
    public class IngredientRepository: IIngredientRepository
    {
        private readonly DBContextRecipeBox _context;
        public IngredientRepository(DBContextRecipeBox context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public Task<Ingredient?> GetByIdAsync(int id)
        {
            return _context.Ingredients.FirstOrDefaultAsync(rec => rec.Id == id);
        }

        public async Task<Ingredient> CreateAsync(Ingredient ingred)
        {
            ingred.Id = 0;
            _context.Ingredients.Add(ingred);
            await _context.SaveChangesAsync();

            return ingred;
        }

        public async Task<bool> UpdateAsync(int id, Ingredient newIngred)
        {
            var ingred = await _context.Ingredients.FirstOrDefaultAsync(r => r.Id == id);
            if (ingred == null)
                return false; // не нашли
 
            newIngred.Id = ingred.Id;

            _context.Entry(ingred).CurrentValues.SetValues(newIngred);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeleteAsync(int id)
        {

            var ingred =await _context.Ingredients.FirstOrDefaultAsync(ing => ing.Id == id);
            if (ingred == null)
                return false;
            _context.Ingredients.Remove(ingred);
            await _context.SaveChangesAsync();

            return true; 
        }

        public Task<Ingredient?> GetByNameAsync(string name)
        {
            return _context.Ingredients.FirstOrDefaultAsync(ing => ing.Name == name);
        }
    }
}
