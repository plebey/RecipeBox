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

        public async Task<IEnumerable<Ingredient>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Ingredients.ToListAsync(cancellationToken);
        }

        public Task<Ingredient?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Ingredients.FirstOrDefaultAsync(rec => rec.Id == id, cancellationToken);
        }

        public async Task<Ingredient> CreateAsync(Ingredient ingred, CancellationToken cancellationToken)
        {
            ingred.Id = 0;
            _context.Ingredients.Add(ingred);
            await _context.SaveChangesAsync(cancellationToken);

            return ingred;
        }

        public async Task<bool> UpdateAsync(int id, Ingredient newIngred, CancellationToken cancellationToken)
        {
            var ingred = await _context.Ingredients.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            if (ingred == null)
                return false; // не нашли
 
            newIngred.Id = ingred.Id;

            _context.Entry(ingred).CurrentValues.SetValues(newIngred);
            await _context.SaveChangesAsync(cancellationToken);

            return true;

        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {

            var ingred =await _context.Ingredients.FirstOrDefaultAsync(ing => ing.Id == id, cancellationToken);
            if (ingred == null)
                return false;
            _context.Ingredients.Remove(ingred);
            await _context.SaveChangesAsync(cancellationToken);

            return true; 
        }

        public Task<Ingredient?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return _context.Ingredients.FirstOrDefaultAsync(ing => ing.Name == name, cancellationToken);
        }
    }
}
