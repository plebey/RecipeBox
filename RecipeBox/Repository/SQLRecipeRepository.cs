using Microsoft.EntityFrameworkCore;
using RecipeBox.Data;
using RecipeBox.Models;
using RecipeBox.Repository.Interfaces;

namespace RecipeBox.Repository
{
    public class SQLRecipeRepository: IRecipeRepository
    {
        private readonly DBContextRecipeBox _context;
        public SQLRecipeRepository(DBContextRecipeBox context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return await _context.Recipes.Include(r => r.RecipeIngredients)
                                   .ThenInclude(i=> i.Ingredient)
                                   .ToListAsync();
        }

        public Task<Recipe?> GetByIdAsync(int id)
        {
            return _context.Recipes.Include(r => r.RecipeIngredients)
                                   .ThenInclude(i => i.Ingredient)
                                   .FirstOrDefaultAsync(rec => rec.Id == id);
        }

        public async Task<IEnumerable<Recipe>> GetByNameAsync(string name)
        {
            return await _context.Recipes.Include(r => r.RecipeIngredients)
                                   .ThenInclude(i => i.Ingredient)
                                   .Where(rec => rec.Name == name).ToListAsync();
        }

        public async Task<Recipe> CreateAsync(Recipe recipe)
        {
            recipe.Id = 0;
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return recipe;
        }

        public async Task<bool> UpdateAsync(int id, Recipe newRecipe)
        {

            var recipe = await _context.Recipes
                        .Include(r=>r.RecipeIngredients)
                        .FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null)
                return false; // не нашли

            newRecipe.Id = recipe.Id;

            _context.Entry(recipe).CurrentValues.SetValues(newRecipe);

            recipe.RecipeIngredients.Clear();
            foreach (var recIng in newRecipe.RecipeIngredients)
            {
                recipe.RecipeIngredients.Add(recIng);
            }

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeleteAsync(int id)
        {

            var recipe = await _context.Recipes.FirstOrDefaultAsync(rec => rec.Id == id);
            if (recipe == null)
                return false;
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return true;

        }

        
    }
}
